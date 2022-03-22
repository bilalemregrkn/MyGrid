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
			// changeDistance = false;
			var tiles = new List<TileController>();
			var gridPosition = Vector3.zero;
			var gridManager = new GameObject
			{
				transform =
				{
					name = "Grid"
				}
			}.AddComponent<GridManager>();

			bool canSpawn = false;
			for (int i = 0; i < size.y; i++)
			{
				switch (axisType)
				{
					case AxisType.XY:
						gridPosition.y = i * GetDistance().y;
						break;
					case AxisType.XZ:
						gridPosition.z = i * GetDistance().y;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (gridType == GridType.Hexagon)
					canSpawn = i % 2 != 0;

				for (int j = 0; j < size.x; j++)
				{
					if (gridType == GridType.Hexagon)
					{
						canSpawn = !canSpawn;

						if (!canSpawn)
							continue;
					}

					gridPosition.x = j * GetDistance().x;

					TileController tile = null;
					if (createAsPrefab)
					{
#if UNITY_EDITOR
						var prefabGameObject = prefab.gameObject;
						var tileGameObject = PrefabUtility.InstantiatePrefab(prefabGameObject) as GameObject;
						if (tileGameObject != null) tile = tileGameObject.GetComponent<TileController>();
						if (tile != null) tile.transform.position = gridPosition;
#endif
					}
					else
					{
						tile = Instantiate(prefab, gridPosition, Quaternion.identity);
					}


					if (tile == null) continue;
					tile.transform.name = $"Tile [{j},{i}]";
					tile.coordinate = new Vector2Int(j, i);
					tile.transform.SetParent(gridManager.transform);
					tiles.Add(tile);
				}
			}

			gridManager.SetTiles(tiles);
			SetNeighbor(gridManager);
			SetCenter(gridManager.gameObject);
		}

		private static void SetCenter(GameObject parent)
		{
			var pivotHelper = parent.AddComponent<SetCenterPosition>();
			pivotHelper.SetCenter();
			parent.transform.position = Vector3.zero;
		}

		// private void OnValidate()
		// {
		// 	if (changeDistance)
		// 		SetDistance(distance);
		// }

		// private void SetDistance(float newDistance)
		// {
		// 	if (listTile != null && listTile.Count != 0)
		// 	{
		// 		var origin = Vector3.zero;
		// 		var tilePosition = Vector3.zero;
		//
		// 		origin.x = -newDistance * (((float)size.x - 1) / 2);
		// 		origin.y = -newDistance * (((float)size.y - 1) / 2);
		//
		// 		switch (axisType)
		// 		{
		// 			case AxisType.XY:
		// 				tilePosition.y = origin.y;
		// 				break;
		// 			case AxisType.XZ:
		// 				tilePosition.z = origin.y;
		// 				break;
		// 		}
		//
		// 		for (int i = 0; i < size.y; i++)
		// 		{
		// 			tilePosition.x = origin.x;
		// 			for (int j = 0; j < size.x; j++)
		// 			{
		// 				var index = (i * size.x) + j;
		// 				listTile[index].transform.position = tilePosition;
		// 				tilePosition.x += newDistance;
		// 			}
		//
		// 			switch (axisType)
		// 			{
		// 				case AxisType.XY:
		// 					tilePosition.y += newDistance;
		// 					break;
		// 				case AxisType.XZ:
		// 					tilePosition.z += newDistance;
		// 					break;
		// 			}
		// 		}
		// 	}
		// }


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

	public static class Extension
	{
		public static Vector2Int GetCoordinate(this Vector2Int coordinate, Direction direction)
		{
			return direction switch
			{
				Direction.Up => new Vector2Int(coordinate.x, coordinate.y + 1),
				Direction.Down => new Vector2Int(coordinate.x, coordinate.y - 1),
				Direction.Right => new Vector2Int(coordinate.x + 1, coordinate.y),
				Direction.Left => new Vector2Int(coordinate.x - 1, coordinate.y),
				Direction.UpRight => new Vector2Int(coordinate.x + 1, coordinate.y + 1),
				Direction.UpLeft => new Vector2Int(coordinate.x - 1, coordinate.y + 1),
				Direction.DownRight => new Vector2Int(coordinate.x + 1, coordinate.y - 1),
				Direction.DownLeft => new Vector2Int(coordinate.x - 1, coordinate.y - 1),
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}
	}
}