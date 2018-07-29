using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MergeMesh : Editor
{
    [MenuItem("CustomTools/Merge %#g",false,10),ContextMenu("Merge")]
    static void MergeMeshFunction()
    {
        #region Initialisation
        GameObject[] goMeshSelected = new GameObject[Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable).Length]; // | SelectionMode.TopLevel
        List <Vector3> vertexNewMesh = new List<Vector3>();
        List<Vector2> uvNewMesh = new List<Vector2>();
        List<int> triNewMesh = new List<int>();
        List<Vector3> normalNewMesh = new List<Vector3>();

        int numberTri = 0;
        int index = 0;

        Mesh meshObjMerge = new Mesh();
        #endregion

        #region Analyze Selection
        foreach (GameObject g in Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel))
        {
            if (g.GetComponent<MeshFilter>().sharedMesh)
            {
                goMeshSelected[index] = g;
                index++;
            }
        }
        #endregion

        #region Creation new mesh
        GameObject objMerge = new GameObject();
        objMerge.name = goMeshSelected[0].name + "_Combine";
        objMerge.transform.parent = goMeshSelected[0].transform.parent;

        MeshFilter meshFilterObjMerge = objMerge.AddComponent<MeshFilter>();
        MeshCollider meshColliderObjMerge = objMerge.AddComponent<MeshCollider>();
        objMerge.AddComponent< MeshRenderer>().material = goMeshSelected[0].GetComponent<MeshRenderer>().sharedMaterial;
        #endregion

        #region Merge data
        foreach (GameObject g in goMeshSelected)
        {
            Mesh gMesh = g.GetComponent<MeshFilter>().sharedMesh;
            if (!gMesh)
            {
                Debug.Log("No Mesh");
                break;
            }

            foreach(Vector3 v in gMesh.vertices)
            {
                Vector3 wPosV = g.transform.TransformPoint(v);
                vertexNewMesh.Add(wPosV);
            }

            foreach(int tri in gMesh.triangles)///Multiple index 0....1....2....3 ect
            {
                int nTri = tri + numberTri;
                triNewMesh.Add(nTri);
            }
            numberTri += gMesh.vertices.Length;

            foreach (Vector2 uv in gMesh.uv)
            {
                uvNewMesh.Add(uv);
            }

            
            foreach(Vector3 n in gMesh.normals)
            {
                normalNewMesh.Add(n);
            }
        }
        #endregion

        #region Undo
        Undo.RecordObjects(goMeshSelected, "Disable Mesh");
        foreach (GameObject g in goMeshSelected)
            g.SetActive(false);//DestroyImmediate(g);
        #endregion

        #region Build Mesh
        meshObjMerge.vertices = vertexNewMesh.ToArray();
        meshObjMerge.triangles = triNewMesh.ToArray();
        meshObjMerge.uv = uvNewMesh.ToArray();
        meshObjMerge.normals = normalNewMesh.ToArray();

        meshFilterObjMerge.sharedMesh = meshObjMerge;
        meshColliderObjMerge.sharedMesh = meshObjMerge;
        GameObject[] newSelection = new GameObject[1] { objMerge };
        Selection.objects = newSelection;
        #endregion
    }
}
