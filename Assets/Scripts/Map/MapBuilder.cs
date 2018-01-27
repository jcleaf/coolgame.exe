using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class MapBuilder : MonoBehaviour
{
    private const int MAX_NUM_TRIES = 20;

#pragma warning disable 0649
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject exit;

    [SerializeField] private Transform parent;

    [SerializeField] private float tileSize;

    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [SerializeField] private int idealNumExits;

    [Tooltip("Index corresponds to number of branches.")]
    [SerializeField]
    private int[] branchWeights;

    [Tooltip("Minimum percentage of tiles that are floors not walls.")]
    [Range(0.4f, 0.6f)]
    [SerializeField]
    private float minFloorRatio;

    [Tooltip("Maximum percentage of tiles that are floors not walls.")]
    [Range(0.6f, 1f)]
    [SerializeField]
    private float maxFloorRatio;

    [SerializeField] private bool rebuild;
#pragma warning restore 0649

    private class Tile
    {
        public bool visited;
        public bool cleanVisited;
        public GameObject go;
        public TilePos pos;
        public bool isWall;

        public Tile(GameObject go, int x, int y)
        {
            this.go = go;
            visited = false;
            pos = new TilePos(x, y);
            isWall = true;
        }
    }

    private struct TilePos
    {
        public int x;
        public int y;

        public TilePos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private Tile[,] map;
    private System.Random random;
    private int floorCount;
    private int maxFloorCount;
    private int numTries;

    private List<Tile> potentialExits;

    void Start()
    {
        random = new System.Random();
        potentialExits = new List<Tile>();
        Generate();
        CleanEdges();
    }

    void Update()
    {
        if (rebuild)
        {
            numTries = 0;
            Rebuild();
            CleanEdges();
            rebuild = false;
        }
    }

    private void Rebuild()
    {
        if (numTries > MAX_NUM_TRIES)
        {
            Debug.LogError("Failed to create an adequate level!");
            return;
        }

        foreach (Tile tile in map)
        {
            Destroy(tile.go);
        }

        Generate();
    }

    private void Generate()
    {
        map = new Tile[columns, rows];

        floorCount = 0;
        ++numTries;

        int numTiles = rows * columns;
        int minFloorCount = (int)(minFloorRatio * numTiles);
        maxFloorCount = (int)(maxFloorRatio * numTiles);

        Assert.IsTrue(maxFloorCount > minFloorCount);

        //make grid of walls
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                Spawn(wall, j, i);
            }
        }

        int startingX = Random.Range(2, columns - 2);
        int startingY = Random.Range(2, rows - 2);

        Visit(new List<Tile>() { map[startingX, startingY] }, 4);

        if (floorCount < minFloorCount)
        {
            Rebuild();
        }
    }

    private void Visit(List<Tile> tiles, int numBranchesOverride = -1)
    {
        List<Tile> toVisit = new List<Tile>();

        foreach (Tile tile in tiles)
        {
            int x = tile.pos.x;
            int y = tile.pos.y;
            tile.visited = true;

            Destroy(tile.go);
            ReplaceGameObject(floor, tile, x, y);
            ++floorCount;

            //reached our max size; early out
            if (floorCount >= maxFloorCount)
            {
                return;
            }

            //determine number of desired branches
            int totalWeight = 0;
            for (int i = 0; i < branchWeights.Length; ++i)
            {
                totalWeight += branchWeights[i];
            }

            int branches = numBranchesOverride;
            if (branches < 0)
            {
                int rnd = Random.Range(0, totalWeight);

                for (int i = 0; i < branchWeights.Length; ++i)
                {
                    int branchWeight = branchWeights[i];
                    if (rnd < branchWeight)
                    {
                        branches = i;
                        break;
                    }

                    rnd -= branchWeight;
                }
            }


            List<TilePos> neighbors = new List<TilePos>() { new TilePos(x - 1, y), new TilePos(x + 1, y), new TilePos(x, y - 1), new TilePos(x, y + 1) };
            List<int> neighborIndeces = Enumerable.Range(0, neighbors.Count).OrderBy(n => random.Next()).ToList();


            for (int i = 0; i < neighbors.Count; ++i)
            {
                TilePos neighborPos = neighbors[neighborIndeces[i]];

                if (branches > 0 && IsBranchable(neighborPos.x, neighborPos.y))
                {
                    toVisit.Add(map[neighborPos.x, neighborPos.y]);
                    --branches;
                }
                else if (IsValid(neighborPos.x, neighborPos.y))
                {
                    map[neighborPos.x, neighborPos.y].visited = true;
                }
            }
        }

        if (toVisit.Count > 0)
        {
            Visit(toVisit);
        }
    }

    private bool IsBranchable(int x, int y)
    {
        return IsValid(x, y) && !map[x, y].visited && !IsEdge(x, y);
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < columns && y < rows;
    }

    private bool IsEdge(int x, int y)
    {
        return x == 0 || y == 0 || x == columns - 1 || y == rows - 1;
    }

    private void Spawn(GameObject prefab, int x, int y)
    {
        GameObject tile = Instantiate(
                    prefab,
                    transform.position + (Vector3.right * x * tileSize) + (-Vector3.forward * y * tileSize),
                    Quaternion.identity,
                    parent);

        map[x, y] = new Tile(tile, x, y);
    }

    private void ReplaceGameObject(GameObject prefab, Tile tile, int x, int y, bool isWall = false)
    {
        tile.isWall = isWall;
        tile.go = Instantiate(
                    prefab,
                    transform.position + (Vector3.right * x * tileSize) + (-Vector3.forward * y * tileSize),
                    Quaternion.identity,
                    parent);
    }

    private void CleanEdges()
    {
        Queue<Tile> edges = new Queue<Tile>();
        potentialExits.Clear();

        for (int row = 0; row < rows; ++row)
        {
            Tile tile = map[0, row];
            if (tile.isWall) { edges.Enqueue(tile); tile.cleanVisited = true;}

            tile = map[columns - 1, row];
            if (tile.isWall) { edges.Enqueue(tile); tile.cleanVisited = true; }
        }

        for (int column = 1; column < columns - 1; ++column)
        {
            Tile tile = map[column, 0];
            if (tile.isWall) { edges.Enqueue(tile); tile.cleanVisited = true;}

            tile = map[column, rows - 1];
            if (tile.isWall) { edges.Enqueue(tile); tile.cleanVisited = true;}
        }

        while (edges.Count > 0)
        {
            Tile edgeTile = edges.Dequeue();
            List<Tile> neighbors = CheckNeighbors(edgeTile, edgeTile.pos.x, edgeTile.pos.y);

            if (neighbors != null)
            {
                Destroy(edgeTile.go);
                edgeTile.go = null;
                potentialExits.Remove(edgeTile);

                foreach (Tile tile in neighbors)
                {
                    edges.Enqueue(tile);
                }
            }
        }

        AddExits();
    }

    private List<Tile> CheckNeighbors(Tile from, int x, int y, bool diagonals = true)
    {
        List<Tile> neighbors = new List<Tile>();
        bool shouldRemove = true;

        shouldRemove &= AddToNeighbors(from, x - 1, y, ref neighbors);
        if (!shouldRemove) { return null; }

        shouldRemove &= AddToNeighbors(from, x + 1, y, ref neighbors);
        if (!shouldRemove) { return null; }

        shouldRemove &= AddToNeighbors(from, x, y - 1, ref neighbors);
        if (!shouldRemove) { return null; }

        shouldRemove &= AddToNeighbors(from, x, y + 1, ref neighbors);
        if (!shouldRemove) { return null; }

        if (diagonals)
        {
            shouldRemove &= AddToNeighbors(from, x - 1, y - 1, ref neighbors, true);
            if (!shouldRemove) { return null; }

            shouldRemove &= AddToNeighbors(from, x - 1, y + 1, ref neighbors, true);
            if (!shouldRemove) { return null; }

            shouldRemove &= AddToNeighbors(from, x + 1, y - 1, ref neighbors, true);
            if (!shouldRemove) { return null; }

            shouldRemove &= AddToNeighbors(from, x + 1, y + 1, ref neighbors, true);
            if (!shouldRemove) { return null; }
        }

        return neighbors;
    }

    private bool AddToNeighbors(Tile from, int x, int y, ref List<Tile> neighbors, bool isDiagonal = false)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            Tile tile = map[x, y];
            if (tile.isWall)
            {
                if (tile.go != null && !tile.cleanVisited)
                {
                    neighbors.Add(tile);
                    tile.cleanVisited = true;
                }
                
                return true;
            }

            if (!isDiagonal && IsValidExit(from))
            {
                potentialExits.Add(from);
            }
            
            return false;
        }

        return true;
    }

    private bool IsValidExit(Tile tile)
    {
        if (potentialExits.Contains(tile))
        {
            return false;
        }

        int x = tile.pos.x;
        int y = tile.pos.y;

        return x - 1 < 0 || map[x - 1, y].go == null ||
            x + 1 >= columns || map[x + 1, y].go == null ||
            y - 1 < 0 || map[x, y - 1].go == null ||
            y + 1 >= rows || map[x, y + 1].go == null;
    }

    private void AddExits()
    {
        Debug.LogFormat("{0} potential exits.", potentialExits.Count);
        List<int> exitIndeces = Enumerable.Range(0, potentialExits.Count).OrderBy(n => random.Next()).ToList();

        int exits = 0;
        while (exits < exitIndeces.Count && exits < idealNumExits)
        {
            Tile tile = potentialExits[exitIndeces[exits]];
            Destroy(tile.go);
            ReplaceGameObject(exit, tile, tile.pos.x, tile.pos.y, true);
            ++exits;
        }
    }
}
