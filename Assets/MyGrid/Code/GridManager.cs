using System.Collections.Generic;
using UnityEngine;

namespace MyGrid
{
	public class GridManager : MonoBehaviour
	{
		public static GridManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
		}

		public List<TileController> Tiles => listTile;
		[SerializeField] private List<TileController> listTile;

		public TileController GetTile(Vector2Int coordinate)
		{
			return listTile.Find(item => item.coordinate == coordinate);
		}

		public void SetTiles(List<TileController> tiles)
		{
			listTile = tiles;
		}
	}
}