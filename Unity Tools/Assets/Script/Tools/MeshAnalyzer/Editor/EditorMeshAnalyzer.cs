using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshAnalyzer))]
[CanEditMultipleObjects]
public class EditorMeshAnalyzer : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        MeshAnalyzer myScript = (MeshAnalyzer)target;
        if (GUILayout.Button("Generate Grass"))
        {
            myScript.Analyze();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("CleanUp"))
        {
            myScript.CleanUp();
        }
        EditorGUILayout.Space();

        #region Grass Data
        if (GUILayout.Button("Grass Data"))
        {
            myScript.GrassData = myScript.GrassData == true ? false : true;
        }
        if (myScript.GrassData)
        {
            myScript.NumberGrass = EditorGUILayout.IntField("NumberGrass", myScript.NumberGrass);
            myScript.GrassHeight = EditorGUILayout.FloatField("GrassHeight", myScript.GrassHeight);
            myScript.GrassWidth = EditorGUILayout.FloatField("GrassWidth", myScript.GrassWidth);
            myScript.GrassMat = (Material)EditorGUILayout.ObjectField("Grass Material", myScript.GrassMat, typeof(Material), true);
            EditorGUILayout.Space();

            if (GUILayout.Button("Contraint Option"))
            {
                myScript.CountraintOption = myScript.CountraintOption == true ? false : true;
            }
            if (myScript.CountraintOption)
            {
                myScript.MaxTryDeepPos = EditorGUILayout.IntField("Number Test for 'Grass flying'", myScript.MaxTryDeepPos);
                myScript.OneGrassPerVertex = EditorGUILayout.Toggle("One Grass per vertex", myScript.OneGrassPerVertex);
                if (myScript.OneGrassPerVertex)
                {
                    myScript.RandomPosPerVertex = EditorGUILayout.Vector2Field("Random Pos Add", myScript.RandomPosPerVertex);
                    EditorGUILayout.Space();
                }
                myScript.GreenColorCondition = EditorGUILayout.Toggle("Creat only one green Vertex", myScript.GreenColorCondition);
                myScript.ErrorPositionGeometryTerrain = EditorGUILayout.FloatField("Error Position Geometry Terrain", myScript.ErrorPositionGeometryTerrain);
            }
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("DebugMode"))
        {
            myScript.DebugMode = myScript.DebugMode == true ? false : true;
        }
        #endregion
    }
}
