using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyGrid
{
	public enum AxisType
	{
		XZ,
		XY
	}
	
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		UpRight,
		UpLeft,
		DownRight,
		DownLeft
	}

	public class GridManager : MonoBehaviour
	{
		public static GridManager Instance { get; private set; }

		[SerializeField] private AxisType axisType;

		[SerializeField] private TileController tilePrefab;

		[SerializeField] private Vector2Int size = new(3, 3);

		[SerializeField, Range(0, 5f)] private float distance = 1.25f;
		[SerializeField] private bool changeDistance;

		[SerializeField] private List<TileController> listTile;

		private void Awake()
		{
			Instance = this;
		}

		[ContextMenu(nameof(CreateGrid))]
		public void CreateGrid()
		{
			Create(tilePrefab, false);
		}

#if UNITY_EDITOR
		[ContextMenu(nameof(CreateGridAsPrefab))]
		public void CreateGridAsPrefab()
		{
			Create(tilePrefab, true);
		}
#endif

		private void Create(TileController prefab, bool createAsPrefab)
		{
			changeDistance = false;
			listTile = new List<TileController>();
			var gridPosition = Vector3.zero;
			var parent = new GameObject
			{
				transform =
				{
					name = "Grid"
				}
			};

			for (int i = 0; i < size.y; i++)
			{
				switch (axisType)
				{
					case AxisType.XY:
						gridPosition.y = i * distance;
						break;
					case AxisType.XZ:
						gridPosition.z = i * distance;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}


				for (int j = 0; j < size.x; j++)
				{
					gridPosition.x = j * distance;

					TileController tile;
					if (createAsPrefab)
					{
#if UNITY_EDITOR
						var prefabGameObject = prefab.gameObject;
						var tileGameObject = PrefabUtility.InstantiatePrefab(prefabGameObject) as GameObject;
						tile = tileGameObject.GetComponent<TileController>();
#endif
					}
					else
					{
						tile = Instantiate(prefab, gridPosition, Quaternion.identity);
					}


					tile.transform.name = $"Tile [{j},{i}]";
					tile.coordinate = new Vector2(j, i);
					tile.transform.SetParent(parent.transform);
					listTile.Add(tile);
				}
			}

			SetNeighbor();

			if (createAsPrefab)
				SetDistance(distance);

			SetCenter(parent);
		}

		private static void SetCenter(GameObject parent)
		{
			var pivotHelper = parent.AddComponent<SetCenterPosition>();
			pivotHelper.SetCenter();
			parent.transform.position = Vector3.zero;
		}

		private void OnValidate()
		{
			if (changeDistance)
				SetDistance(distance);
		}

		private void SetDistance(float newDistance)
		{
			if (listTile != null && listTile.Count != 0)
			{
				var origin = Vector3.zero;
				var tilePosition = Vector3.zero;

				origin.x = -newDistance * (((float)size.x - 1) / 2);
				origin.y = -newDistance * (((float)size.y - 1) / 2);

				switch (axisType)
				{
					case AxisType.XY:
						tilePosition.y = origin.y;
						break;
					case AxisType.XZ:
						tilePosition.z = origin.y;
						break;
				}

				for (int i = 0; i < size.y; i++)
				{
					tilePosition.x = origin.x;
					for (int j = 0; j < size.x; j++)
					{
						var index = (i * size.x) + j;
						listTile[index].transform.position = tilePosition;
						tilePosition.x += newDistance;
					}

					switch (axisType)
					{
						case AxisType.XY:
							tilePosition.y += newDistance;
							break;
						case AxisType.XZ:
							tilePosition.z += newDistance;
							break;
					}
				}
			}
		}

		public TileController GetGrid(Vector2 coordinate)
		{
			return listTile.Find(item => item.coordinate == coordinate);
		}

		private void SetNeighbor()
		{
			foreach (var tile in listTile)
			{
				var coordinate = tile.coordinate;

				var upCoordinate = coordinate;
				var downCoordinate = coordinate;
				var rightCoordinate = coordinate;
				var leftCoordinate = coordinate;

				upCoordinate.y++;
				downCoordinate.y--;
				rightCoordinate.x++;
				leftCoordinate.x--;

				tile.PrepareNeighbour(
					GetGrid(upCoordinate),
					GetGrid(downCoordinate),
					GetGrid(leftCoordinate),
					GetGrid(rightCoordinate)
				);
			}
		}
	}
}