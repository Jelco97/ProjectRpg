﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBaseGenerator : MonoBehaviour
{
    public HeightGround HeightChecker;
    public int Density = 1;
    public int NumberCellByLenght = 10;
    public int IndexInTheCheckboard;

    public bool TopChecker;
    public bool RightChecker;
    public HeightGround TopHeight;
    public HeightGround RightHeight;

    public float RedColorByHeight;
    public float GreenColorByHeight;
    public float BlueColorByHeight;

    private bool topChecker;
    private bool rightChecker;
    private Mesh mapMesh;

    public void GenerateGroundBase()
    {
        #region Init
        if (mapMesh != null)
            DestroyImmediate(mapMesh);
        mapMesh = new Mesh();

        int numSideQuad = (Density * NumberCellByLenght) - Density;
        int numVertice = (numSideQuad + 1) * (numSideQuad + 1);
        int numTriangles = numSideQuad * numSideQuad * 2;

        Vector3[] vertices = new Vector3[numVertice];
        int[] triangle = new int[numTriangles * 3];
        Vector2[] uv = new Vector2[numVertice];
        Color[] vertexColor = new Color[numVertice];
        #endregion

        int index = 0;
        for (float zPos = 0f; zPos < numSideQuad + 1; zPos++)
            for (float xPos = 0f; xPos < numSideQuad + 1; xPos++)
            {
                Vector3 positionVertex = new Vector3();
                positionVertex.z = (zPos / Density);
                positionVertex.x = (xPos / Density);

                int Xindex = Mathf.FloorToInt(xPos / Density);
                if (Xindex == NumberCellByLenght - 1)
                {
                    if (RightChecker)
                        rightChecker = true;
                    Xindex--;
                }

                int Zindex = Mathf.FloorToInt(zPos / Density);
                if (Zindex == NumberCellByLenght - 1)
                {
                    if (TopChecker)
                        topChecker = true;
                    Zindex--;
                }

                float height = 0;
                if (!topChecker && !rightChecker)
                    height = HeightChecker.HeightGroundData[Zindex].Row[Xindex];

                else if (topChecker)
                    height = TopHeight.HeightGroundData[0].Row[Xindex];

                else if (rightChecker)
                    height = RightHeight.HeightGroundData[Zindex].Row[0];

                topChecker = false;
                rightChecker = false;

                positionVertex.y = height;

                vertexColor[index] = VertexColorByHeight(height);
                uv[index] = new Vector2(xPos / (numSideQuad + 1), zPos / (numSideQuad + 1));
                vertices[index++] = positionVertex;
            }

        index = 0;
        for (int z = 0; z < numSideQuad; z++)
            for (int x = 0; x < numSideQuad; x++)
            {
                int vertexCounterPerLine = numSideQuad + 1;

                triangle[index++] = x + z * vertexCounterPerLine;
                triangle[index++] = x + (z + 1) * vertexCounterPerLine;
                triangle[index++] = x + 1 + z * vertexCounterPerLine;

                triangle[index++] = x + (z + 1) * vertexCounterPerLine;
                triangle[index++] = x + 1 + (z + 1) * vertexCounterPerLine;
                triangle[index++] = x + 1 + z * vertexCounterPerLine;
            }

        #region Mesh
        mapMesh.vertices = vertices;
        mapMesh.triangles = triangle;
        mapMesh.uv = uv;
        mapMesh.colors = vertexColor;

        mapMesh.RecalculateNormals();
        mapMesh.RecalculateBounds();

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mapMesh;

        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
            meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mapMesh;

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        #endregion
    }

    public void CleanHeightTab()
    {
        HeightChecker.CleanHeight();
    }

    /// <summary>
    /// Return Color if height is behind Value
    /// </summary>
    /// <param name="Height"></param>
    /// <returns></returns>
    public Color VertexColorByHeight(float Height)
    {
        if (Height <= RedColorByHeight)
            return Color.red;
        else if (Height <= GreenColorByHeight)
            return Color.green;
        else
            return Color.blue;
    }
}
