using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private ShipTile[] tilePrefabs;

    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [SerializeField] private float tileSize;

    private enum TileType
    {
        TLCorner,
        TRCorner,
        BLCorner,
        BRCorner,
        TopWall,
        BottomWall,
        LeftWall,
        RightWall
    }

    private ShipTile[,] map;

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
}
