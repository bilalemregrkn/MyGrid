using System;
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

			GUILayout.Space(10);
			GridCreator myScript = (GridCreator)target;
			if (GUILayout.Button(nameof(myScript.CreateGrid)))
			{
				myScript.CreateGrid();
			}

			GUILayout.Space(3);
			// GUI.backgroundColor = new Color(127 / 255f, 214 / 255f, 253 / 255f, 255 / 255f);
			// GUI.backgroundColor = new Color32(127, 214, 253, 255);
			// GUI.backgroundColor = Color.red;
			if (GUILayout.Button(nameof(myScript.CreateGridAsPrefab), GUILayout.Height(50)))
			{
				myScript.CreateGridAsPrefab();
			}
		}
	}

	// Simple script that creates a new non-dockable window
}