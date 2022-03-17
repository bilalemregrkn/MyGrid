using System;
using UnityEngine;

namespace MyGrid
{
	[CreateAssetMenu(menuName = "Create GridSetting", fileName = "GridSetting", order = 0)]
	public class GridSetting : ScriptableObject
	{
		public GridSettingData rectangle;
		public GridSettingData hexagon;
		public GridSettingData custom;
	}

	[Serializable]
	public struct GridSettingData
	{
		public TileController prefab;
		public Vector2 distance;
	}
}