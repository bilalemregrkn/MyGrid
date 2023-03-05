using UnityEditor;
using UnityEngine;

namespace MyGrid.Code.Editor
{
    public class GridCreateWindow : EditorWindow
    {
        private Vector2Int _size = new Vector2Int(5, 5);
        private GridType _gridType;
        private AxisType _axisType;
        private static TileSetting _setting;
        private GridCreator _creator;

        private const int MinGridSize = 1;
        private const int MaxGridSize = 100;

        private const string PathSetting =
            "Packages/com.bilalemregrkn.MyGrid/Runtime/Setting/TileSetting-Default.asset";

        [MenuItem("Tools/My Grid")]
        public static void ShowWindow()
        {
            GetWindow<GridCreateWindow>("My Grid");
            if (_setting == null)
                _setting = AssetDatabase.LoadAssetAtPath<TileSetting>(PathSetting);
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space(20);

            _size = EditorGUILayout.Vector2IntField("Grid Size", _size);
            _size.x = Mathf.Clamp(_size.x, MinGridSize, MaxGridSize);
            _size.y = Mathf.Clamp(_size.y, MinGridSize, MaxGridSize);
            EditorGUILayout.Space(10);

            _gridType = (GridType)EditorGUILayout.EnumPopup("Grid Type", _gridType);
            _axisType = (AxisType)EditorGUILayout.EnumPopup("Axis Type", _axisType);
            EditorGUILayout.Space(10);

            _setting = (TileSetting)EditorGUILayout.ObjectField("Setting", _setting, typeof(TileSetting));
            EditorGUILayout.Space(30);

            if (GUILayout.Button("Create"))
            {
                _creator = _creator ?? new GridCreator();
                _creator.Create(_gridType, _axisType, _setting, _size, true);
            }

            if (GUILayout.Button("Create as Prefab"))
            {
                _creator = _creator ?? new GridCreator();
                _creator.Create(_gridType, _axisType, _setting, _size, true);
            }
        }
    }
}