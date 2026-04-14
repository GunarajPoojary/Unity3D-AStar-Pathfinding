using UnityEditor;
using UnityEngine;

/// <summary>
/// An Editor Window to generate obstacle data for grid map
/// </summary>
public class ObstacleGenerator : EditorWindow
{
    private SOObstacleData _obstacleData;
    private const int HEADER_LAYOUT_HEIGHT = 40;
    private const int HEADER_FONT_SIZE = 36;
    private const int TOGGLE_LAYOUT_MIN_WIDTH = 30;

    [MenuItem("Tools/ObstacleGenerator")]
    public static void ShowWindow() => GetWindow<ObstacleGenerator>("ObstacleGenerator");

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUIStyle headerText = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = HEADER_FONT_SIZE
        };

        EditorGUILayout.LabelField("Obstacle Generator", headerText, GUILayout.Height(HEADER_LAYOUT_HEIGHT));

        GUILayout.Space(10);

        _obstacleData = (SOObstacleData)EditorGUILayout.ObjectField(
            "Obstacle Data",
            _obstacleData,
            typeof(SOObstacleData),
            false
        );

        GUILayout.Space(10);

        if (_obstacleData == null) return;

        // Ensure array is created if needed
        _obstacleData.Init();

        EditorGUILayout.LabelField("Obstacle Grid (X, Z)", EditorStyles.boldLabel);

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal(); // Create layout in horizontally for each obstacle in row 

        for (int gridX = 0; gridX < _obstacleData.GridWidth; gridX++)
        {
            EditorGUILayout.BeginVertical(); // Create layout in vertically for each obstacle in column

            for (int gridZ = 0; gridZ < _obstacleData.GridHeight; gridZ++)
            {
                bool isObstacle = _obstacleData.IsObstacleAt(gridX, gridZ);

                // Create toggle checkbox
                bool isChecked = GUILayout.Toggle(isObstacle, $"{gridX},{gridZ}", GUILayout.MinWidth(TOGGLE_LAYOUT_MIN_WIDTH));

                // Avoid modifying value if there's no change
                if (isChecked != isObstacle)
                {
                    _obstacleData.SetObstacleAt(gridX, gridZ, isChecked);
                    EditorUtility.SetDirty(_obstacleData); //Save changes
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();
    }
}