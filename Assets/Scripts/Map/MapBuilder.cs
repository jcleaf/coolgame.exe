using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class MapBuilder : MonoBehaviour
{
    private const int MAX_NUM_TRIES = 20;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject floor;

    [SerializeField] private float tileSize;

    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [Tooltip("Index corresponds to number of branches.")]
    [SerializeField]
    private int[] branchWeights;

    [Tooltip("Minimum percentage of tiles that are floors not walls.")]
    [Range(0.1f, 0.6f)]
    [SerializeField]
    private float minFloorRatio;

    [Tooltip("Maximum percentage of tiles that are floors not walls.")]
    [Range(0.1f, 1f)]
    [SerializeField]
    private float maxFloorRatio;

    [SerializeField] private bool rebuild;

    private class Tile
    {
        public bool visited;
        public GameObject go;
        public TilePos pos;

        public Tile(GameObject go, int x, int y)
        {
            this.go = go;
            visited = false;
            pos = new TilePos(x, y);
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

    void Start()
    {
        random = new System.Random();
        Generate();
    }

    void Update()
    {
        if (rebuild)
        {
            numTries = 0;
            Rebuild();
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

        int startingX = Random.Range(0, columns);
        int startingY = Random.Range(0, rows);

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

                if (branches > 0)
                {
                    if (IsUnvisited(neighborPos.x, neighborPos.y))
                    {
                        toVisit.Add(map[neighborPos.x, neighborPos.y]);
                        --branches;
                    }
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

    private bool IsUnvisited(int x, int y)
    {
        return IsValid(x, y) && !map[x, y].visited;
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < columns && y < rows;
    }

    private void Spawn(GameObject prefab, int x, int y)
    {
        GameObject tile = Instantiate(
                    prefab,
                    transform.position + (Vector3.right * x * tileSize) + (-Vector3.forward * y * tileSize),
                    Quaternion.identity);

        map[x, y] = new Tile(tile, x, y);
    }

    private void ReplaceGameObject(GameObject prefab, Tile tile, int x, int y)
    {
        tile.go = Instantiate(
                    prefab,
                    transform.position + (Vector3.right * x * tileSize) + (-Vector3.forward * y * tileSize),
                    Quaternion.identity);
    }

    private void CleanEdges()
    {

    }
}
