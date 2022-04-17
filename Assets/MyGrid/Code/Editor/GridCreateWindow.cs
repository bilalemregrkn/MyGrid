using UnityEditor;
using UnityEngine;

namespace MyGrid
{
	public class GridCreateWindow : EditorWindow
	{
		[MenuItem("MyGrid/Display Window")]
		static void Initialize()
		{
			//Get Old Gameobject
			var old = Selection.activeObject;

			//Select
			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/MyGrid/Prefabs/GridCreator.prefab");
			EditorGUIUtility.PingObject(Selection.activeObject);

			//Create Inspector Panel
			// Retrieve the existing Inspector tab, or create a new one if none is open
			EditorWindow inspectorWindow = EditorWindow.GetWindow(typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow"));
			// Get the size of the currently window
			Vector2 size = new Vector2(inspectorWindow.position.width, inspectorWindow.position.height);
			// Clone the inspector tab (optionnal step)
			inspectorWindow = Instantiate(inspectorWindow);
			// Set min size, and focus the window
			inspectorWindow.minSize = size;
			inspectorWindow.Show();
			inspectorWindow.Focus();

			//Lock
			ActiveEditorTracker.sharedTracker.isLocked = true;
			ActiveEditorTracker.sharedTracker.ForceRebuild();

			//Set old
			Selection.activeObject = old;
		}
	}
}