using System.Collections.Generic;
using UnityEngine;

namespace MyGrid.Code
{
    public class GridManager : Singleton<GridManager>
    {
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