using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject floor;

    [SerializeField] private float tileSize;

    [SerializeField] private int columns;
    [SerializeField] private int rows;

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

    private List<Tile> walls = new List<Tile>();

    private System.Random random;

    void Start()
    {
        random = new System.Random();
        Generate();
    }

    void Update()
    {
        if (rebuild)
        {
            foreach (Tile tile in map)
            {
                Destroy(tile.go);
            }

            Generate();
            rebuild = false;
        }
    }

    void Generate()
    {
        map = new Tile[columns, rows];

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

        Visit(new List<Tile>() { map[startingX, startingY] });
    }

    void Visit(List<Tile> tiles)
    {
        List<Tile> toVisit = new List<Tile>();

        foreach (Tile tile in tiles)
        {
            int x = tile.pos.x;
            int y = tile.pos.y;
            tile.visited = true;

            Destroy(tile.go);
            ReplaceGameObject(floor, tile, x, y);

            int branches = Random.Range(1, 3); //1-3 branches
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
                else if(IsValid(neighborPos.x, neighborPos.y))
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
                    transform.position +(Vector3.right * x * tileSize) + (-Vector3.forward * y * tileSize),
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

    /*
    void Start()
    {
        map = new ShipTile[columns, rows];

        //build outsides
        SpawnTile(tilePrefabs[(int)TileType.TLCorner], 0, 0);
        SpawnTile(tilePrefabs[(int)TileType.TRCorner], columns - 1, 0);
        SpawnTile(tilePrefabs[(int)TileType.BLCorner], 0, rows - 1);
        SpawnTile(tilePrefabs[(int)TileType.BRCorner], columns - 1, rows - 1);

        BuildOuterColumn(tilePrefabs[(int)TileType.LeftWall], 0);
        BuildOuterColumn(tilePrefabs[(int)TileType.RightWall], columns - 1);
        BuildOuterRow(tilePrefabs[(int)TileType.TopWall], 0);
        BuildOuterRow(tilePrefabs[(int)TileType.BottomWall], rows - 1);

        //build inside
        System.Random random = new System.Random();
        for (int row = 1; row < rows - 1; ++row)
        {
            for (int column = 1; column < columns - 1; ++column)
            {

                List<int> randomNumbers = Enumerable.Range(0, tilePrefabs.Length).OrderBy(x => random.Next()).ToList();

                for (int k = 0; k < tilePrefabs.Length; ++k)
                {
                    ShipTile tile = SpawnTile(tilePrefabs[randomNumbers[k]], row, column);

                    if ((!tile.hasTopWall || map[column, row - 1] == null || !map[column, row - 1].hasBottomWall) &&
                        (!tile.hasBottomWall || map[column, row + 1] == null || !map[column, row + 1].hasTopWall) &&
                        (!tile.hasLeftWall || map[column - 1, row] == null || !map[column - 1, row].hasRightWall) &&
                        (!tile.hasRightWall || map[column + 1, row] == null || !map[column + 1, row].hasLeftWall))
                    {
                        break;
                    }

                    Destroy(tile.gameObject);

                    if (k == tilePrefabs.Length - 1)
                    {
                        Debug.LogErrorFormat("Couldn't find a valid tile type at {0}, {1}!", column, row);
                    }
                }

                if (map[column, row] == null)
                {
                    Debug.LogErrorFormat("Failed to spawn tile at {0}, {1}!", column, row);
                }
            }
        }
    }

    void BuildOuterColumn(ShipTile tile, int column)
    {
        for (int row = 1; row < rows - 1; ++row)
        {
            SpawnTile(tile, column, row);
        }
    }

    void BuildOuterRow(ShipTile tile, int row)
    {
        for (int column = 1; column < columns - 1; ++column)
        {
            SpawnTile(tile, column, row);
        }
    }

    private ShipTile SpawnTile(ShipTile tile, int column, int row)
    {
        ShipTile newTile = Instantiate(
            tile,
            transform.position + (Vector3.right * column * tileSize) + (-Vector3.forward * row * tileSize),
            Quaternion.identity);

        map[row, column] = newTile;

        return newTile;
    }
    */
}
