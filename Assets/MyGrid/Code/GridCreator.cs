using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyGrid
{
    public enum GridType
    {
        Rectangle,
        Hexagon,
        Custom
    }

    public enum AxisType
    {
        XZ,
        XY
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }


    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private GridType gridType;
        [SerializeField] private AxisType axisType;
        [SerializeField] private GridSetting gridSetting;
        [SerializeField] private Vector2Int size = new(3, 3);

        [ContextMenu(nameof(CreateGrid))]
        public void CreateGrid()
        {
            Create(GetPrefab(), false);
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(CreateGridAsPrefab))]
        public void CreateGridAsPrefab()
        {
            Create(GetPrefab(), true);
        }
#endif
        
        private void Create(TileController prefab, bool createAsPrefab)
        {
            //Create GridManager Object
            var gridManager = new GameObject
            {
                transform =
                {
                    name = "Grid"
                }
            }.AddComponent<GridManager>();

            //Create Tiles
            var tiles = new List<TileController>();
            var tilePosition = Vector3.zero;

            for (int vertical = 0; vertical < size.y; vertical++)
            {
                tilePosition = GetTilePosition(tilePosition, vertical, axisType);

                for (int horizontal = 0; horizontal < size.x; horizontal++)
                {
                    var coordinate = new Vector2Int(horizontal, vertical);
                    if (!CanSpawn(gridType, coordinate))
                        continue;

                    tilePosition.x = horizontal * GetDistance().x;

                    TileController tile = null;
                    if (createAsPrefab)
                    {
#if UNITY_EDITOR
                        var prefabGameObject = prefab.gameObject;
                        var tileGameObject = PrefabUtility.InstantiatePrefab(prefabGameObject) as GameObject;
                        if (tileGameObject != null) tile = tileGameObject.GetComponent<TileController>();
                        if (tile != null) tile.transform.position = tilePosition;
#endif
                    }
                    else
                    {
                        tile = Instantiate(prefab, tilePosition, Quaternion.identity);
                    }


                    if (tile == null) continue;
                    tile.transform.name = $"Tile [{horizontal},{vertical}]";
                    tile.coordinate = coordinate;
                    tile.transform.SetParent(gridManager.transform);
                    tiles.Add(tile);
                }
            }

            gridManager.SetTiles(tiles);
            SetNeighbor(gridManager);
            SetCenter(gridManager.gameObject);
        }
        
        private TileController GetPrefab()
        {
            return gridType switch
            {
                GridType.Rectangle => gridSetting.rectangle.prefab,
                GridType.Hexagon => gridSetting.hexagon.prefab,
                GridType.Custom => gridSetting.custom.prefab,
                _ => null
            };
        }

        private Vector2 GetDistance()
        {
            return gridType switch
            {
                GridType.Rectangle => gridSetting.rectangle.distance,
                GridType.Hexagon => gridSetting.hexagon.distance,
                GridType.Custom => gridSetting.custom.distance,
                _ => Vector2Int.zero
            };
        }
        
        private Vector3 GetTilePosition(Vector3 position, int count, AxisType type)
        {
            var axis = count * GetDistance().y;
            switch (type)
            {
                case AxisType.XY:
                    position.y = axis;
                    break;
                case AxisType.XZ:
                    position.z = axis;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return position;
        }

        private bool CanSpawn(GridType type, Vector2Int coordinate)
        {
            var isPairHorizontal = coordinate.x % 2 == 0;
            var isPairVertical = coordinate.y % 2 == 0;
            return type switch
            {
                GridType.Rectangle => true,
                GridType.Hexagon => isPairHorizontal == isPairVertical,
                GridType.Custom => true,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        private static void SetCenter(GameObject parent)
        {
            var pivotHelper = parent.AddComponent<SetCenterPosition>();
            pivotHelper.SetCenter();
            parent.transform.position = Vector3.zero;
        }
        
        private void SetNeighbor(GridManager manager)
        {
            var length = Enum.GetNames(typeof(Direction)).Length;
            foreach (var tile in manager.Tiles)
            {
                var data = new List<TileData>();
                for (int i = 0; i < length; i++)
                {
                    var direction = (Direction)i;
                    var coordinate = tile.coordinate.GetCoordinate(direction);

                    //One step more if hexagon and vertical
                    if (gridType == GridType.Hexagon)
                    {
                        if (direction is Direction.Down or Direction.Up)
                            coordinate = coordinate.GetCoordinate(direction);
                    }

                    var neighbour = manager.GetTile(coordinate);
                    if (!neighbour) continue;
                    data.Add(new TileData()
                    {
                        tile = manager.GetTile(coordinate),
                        direction = direction
                    });
                }

                tile.PrepareNeighbour(data);
            }
        }
    }
}