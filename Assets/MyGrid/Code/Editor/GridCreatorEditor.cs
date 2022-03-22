using UnityEditor;
using UnityEngine;

namespace MyGrid
{
	[CustomEditor(typeof(GridCreator))]
	public class GridCreatorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			GridCreator myScript = (GridCreator)target;
			if (GUILayout.Button(nameof(myScript.CreateGrid)))
			{
				myScript.CreateGrid();
			}

			if (GUILayout.Button(nameof(myScript.CreateGridAsPrefab)))
			{
				myScript.CreateGridAsPrefab();
			}
		}
	}
}