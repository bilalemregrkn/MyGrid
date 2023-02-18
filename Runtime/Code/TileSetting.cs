using System;
using UnityEngine;

namespace MyGrid.Code
{
	[CreateAssetMenu(menuName = "ScriptableObject/My Grid/Create TileSetting", fileName = "TileSetting", order = 0)]
	public class TileSetting : ScriptableObject
	{
		public TileSettingData rectangle;
		public TileSettingData hexagon;
	}

	[Serializable]
	public struct TileSettingData
	{
		public TileController prefab;
		public Vector2 distance;
	}
}