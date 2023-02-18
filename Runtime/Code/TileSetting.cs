using System;
using UnityEngine;

namespace MyGrid.Code
{
	[CreateAssetMenu(menuName = "ScriptableObject/Create TileSetting", fileName = "TileSetting", order = 0)]
	public class TileSetting : ScriptableObject
	{
		public TileSettingData rectangle;
		public TileSettingData hexagon;
		public TileSettingData custom;
	}

	[Serializable]
	public struct TileSettingData
	{
		public TileController prefab;
		public Vector2 distance;
	}
}