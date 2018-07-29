using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#pragma warning disable 0108

[ExecuteInEditMode]
public class MeshAnalyzer : MonoBehaviour
{
    #region inspector
    public bool GrassData = false;
    public bool CountraintOption = false;
    public bool GreenColorCondition = true;
    public bool DebugMode = false;
    #endregion

    public int NumberGrass = 8;
    public float GrassHeight = 1f;
    public float GrassWidth = 1f;
    public Material GrassMat;
    public int MaxTryDeepPos = 15;
    public float ErrorPositionGeometryTerrain = .3f;
    public bool OneGrassPerVertex = false;
    public Vector2 RandomPosPerVertex = new Vector2(0, 0);

    private Mesh meshAnalyzed;
    private Mesh grassMesh;
    private GameObject grass;
    private Vector3[] vertexPos;
    private Vector3[] centerPosGrass;
    private List<Color> vertexColor;
    private Collider collider;
    private bool noCollision = false;

    public void Analyze()
    {
        #region Initialisation
        Vector3[] posGrass;
        int[] indexGreenVertex;

        #endregion

        #region MeshData
        if (!meshAnalyzed)
            meshAnalyzed = this.gameObject.GetComponent<MeshFilter>().sharedMesh;

        vertexPos = meshAnalyzed.vertices;
        vertexColor = new List<Color>(meshAnalyzed.colors);
        #endregion

        #region Analyze Data
        bool greenVertexColorFound = false;
        int lenghtNewTab = 0;

        for (int i = vertexColor.Count - 1; 0 <= i; i--)
        {
            if (vertexColor[i] == Color.green)
            {
                lenghtNewTab++;
                greenVertexColorFound = true;
            }
        }
        indexGreenVertex = new int[lenghtNewTab];

        if (greenVertexColorFound)
        {
            int x = 0;
            for (int i = vertexColor.Count - 1; 0 <= i; i--)
            {
                if (vertexColor[i] == Color.green)
                {
                    indexGreenVertex[x] = i;
                    x++;
                }
            }
            centerPosGrass = new Vector3[indexGreenVertex.Length];
            for (int i = 0; i < indexGreenVertex.Length; i++)
            {
                centerPosGrass[i] = vertexPos[indexGreenVertex[i]];
            }
        }
        else
        {
            if (DebugMode) ;
            Debug.Log("No Vertex Color Green found, no grass build");
            return;
        }
        #endregion

        #region Grass Object
        if (!grass)
        {
            if (transform.Find("grassPross"))
                DestroyImmediate(transform.Find("grassPross").gameObject);
            grass = new GameObject();
            grass.name = "grassPross";

        }
        else
        {
            DestroyImmediate(grass);
            grass = new GameObject();
            grass.name = "grassPross";
        }
        grass.transform.parent = this.gameObject.transform;
        grass.transform.localPosition = Vector3.zero;

        MeshFilter meshFilterGrass = grass.AddComponent<MeshFilter>();
        if (meshFilterGrass == null)
            meshFilterGrass = grass.AddComponent<MeshFilter>();

        MeshRenderer meshRenderGrass = grass.AddComponent<MeshRenderer>();
        if (meshRenderGrass == null)
            meshRenderGrass = grass.AddComponent<MeshRenderer>();

        MeshCollider meshColiderGrass = grass.AddComponent<MeshCollider>();
        if (meshColiderGrass == null)
            meshColiderGrass = grass.AddComponent<MeshCollider>();

        grassMesh = new Mesh();
        collider = this.gameObject.GetComponent<Collider>();
        if (collider == null)
            collider = this.gameObject.AddComponent<MeshCollider>();
        #endregion

        #region Analyze Var
        if (OneGrassPerVertex)//clamp Grass Number By number of green vertex
            NumberGrass = Mathf.Clamp(NumberGrass, 0, centerPosGrass.Length);
        GrassHeight = Mathf.Max(GrassHeight, .2f);
        GrassWidth = Mathf.Max(GrassWidth, .2f);
        #endregion

        #region Generation Grass Data
        int verticeGrass = NumberGrass * 6;
        int triangleGrass = NumberGrass * 2;

        Vector3[] vertice = new Vector3[verticeGrass];
        Vector2[] uvs = new Vector2[verticeGrass];
        int[] triangle = new int[triangleGrass * 3];
        #endregion

        #region Generation Center Grass Pos
        posGrass = new Vector3[NumberGrass];

        for (int y = 0; y < NumberGrass; y++)
        {
            if (OneGrassPerVertex)//Only green vertex increas by RandomPosPerVertex
            {
                posGrass[y] = centerPosGrass[y];
                posGrass[y].x += UnityEngine.Random.Range((float)RandomPosPerVertex.x, (float)RandomPosPerVertex.y);
                posGrass[y].y += UnityEngine.Random.Range((float)RandomPosPerVertex.x, (float)RandomPosPerVertex.y);
            }
            else
            {
                int randomPosIndex = UnityEngine.Random.Range(0, centerPosGrass.Length);
                Vector4 randomPos = ScanProximityVertex(centerPosGrass[randomPosIndex]);

                Vector3 pos = centerPosGrass[randomPosIndex];
                pos.x += UnityEngine.Random.Range(randomPos.y, randomPos.x);
                pos.z += UnityEngine.Random.Range(randomPos.w, randomPos.z);

                posGrass[y] = pos;
            }
        }
        #endregion

        #region Generate mesh Grass
        for (int i = 0; i < NumberGrass; i++)
        {
            Vector3 positionGrass = posGrass[i];
            float yPos1;
            float yPos2;

            int x = 0;
            do
            {
                noCollision = false;
                float randomRotation = UnityEngine.Random.Range(0, 360);
                vertice[i * 4] = new Vector3((positionGrass.x) - GrassWidth * Mathf.Cos(randomRotation), positionGrass.y, positionGrass.z - GrassWidth * Mathf.Sin(randomRotation));
                yPos1 = vertice[i * 4].y = HitByRay(this.transform.TransformPoint(vertice[i * 4]));
                vertice[i * 4 + 1] = new Vector3((positionGrass.x) + GrassWidth * Mathf.Cos(randomRotation), positionGrass.y, positionGrass.z + GrassWidth * Mathf.Sin(randomRotation));
                yPos2 = vertice[i * 4 + 1].y = HitByRay(this.transform.TransformPoint(vertice[i * 4 + 1]));
                vertice[i * 4 + 2] = new Vector3((positionGrass.x) - GrassWidth * Mathf.Cos(randomRotation), yPos1 + GrassHeight, positionGrass.z - GrassWidth * Mathf.Sin(randomRotation));
                vertice[i * 4 + 3] = new Vector3((positionGrass.x) + GrassWidth * Mathf.Cos(randomRotation), yPos2 + GrassHeight, positionGrass.z + GrassWidth * Mathf.Sin(randomRotation));

                x++;
            }
            while (noCollision && x < MaxTryDeepPos);

            if (x == MaxTryDeepPos && DebugMode)
                Debug.Log("Can't find pos for grass, ''Flying Grass''");
            noCollision = noCollision == true ? false : false;

            uvs[i * 4] = new Vector2(0, 0);
            uvs[i * 4 + 1] = new Vector2(1, 0);
            uvs[i * 4 + 2] = new Vector2(0, 1);
            uvs[i * 4 + 3] = new Vector2(1, 1);

            triangle[i * 6] = i * 4;
            triangle[i * 6 + 1] = i * 4 + 1;
            triangle[i * 6 + 2] = i * 4 + 2;

            triangle[i * 6 + 3] = i * 4 + 2;
            triangle[i * 6 + 4] = i * 4 + 1;
            triangle[i * 6 + 5] = i * 4 + 3;
        }

        grassMesh.vertices = vertice;
        grassMesh.triangles = triangle;
        grassMesh.uv = uvs;
        #endregion

        #region Build mesh
        meshFilterGrass.sharedMesh = grassMesh;
        meshColiderGrass.sharedMesh = grassMesh;
        if (GrassMat)
            meshRenderGrass.material = GrassMat;
        #endregion

        #region Placement Mesh
        grass.transform.parent = this.gameObject.transform;
        grass.transform.localEulerAngles = Vector3.zero;
        #endregion
    }

    public void CleanUp()
    {
        if (!grass)
            grass = this.transform.Find("grassPross").gameObject;
        if (grass)
            DestroyImmediate(grass);
        DestroyImmediate(grassMesh);
    }

    public Vector4 ScanProximityVertex(Vector3 pos)
    {
        ///Initialisation Vector2
        Vector2 posFactorX = new Vector2(10, -10);//(+,-)
        Vector2 posFactorZ = new Vector2(10, -10);//(+,-)

        #region No Green Contraint
        if (!GreenColorCondition)
        {
            foreach (Vector3 vertex in vertexPos)
                if (pos.z - ErrorPositionGeometryTerrain <= vertex.z && pos.z + ErrorPositionGeometryTerrain >= vertex.z)//Aligné en Z ??
                    if (pos.x < vertex.x && (vertex.x - pos.x) < posFactorX.x)//Plus petit que vertex ? && distance ?
                        posFactorX.x = vertex.x - pos.x;
                    else if (pos.x > vertex.x && (vertex.x - pos.x) > posFactorX.y)//Plus grand que vertex ? && distance ?
                        posFactorX.y = vertex.x - pos.x;

            foreach (Vector3 vertex in vertexPos)
                if (pos.x - ErrorPositionGeometryTerrain <= vertex.x && pos.x + ErrorPositionGeometryTerrain >= vertex.x)//Aligné en X ?
                    if (pos.z < vertex.z && (vertex.z - pos.z) < posFactorZ.x)//Plus petit que vertex ? && distance ?
                        posFactorZ.x = vertex.z - pos.z;
                    else if (pos.z > vertex.z && (vertex.z - pos.z) > posFactorZ.y)//Plus grand ? && distance ?
                        posFactorZ.y = vertex.z - pos.z;
        }
        #endregion

        #region Green Contraint
        else
        {
            foreach (Vector3 vertex in centerPosGrass)
                if (pos.z - ErrorPositionGeometryTerrain <= vertex.z && pos.z + ErrorPositionGeometryTerrain >= vertex.z)//Aligné en Z ??
                {
                    if (pos.x < vertex.x && (vertex.x - pos.x) < posFactorX.x)//Plus petit que vertex ? && distance ?
                        posFactorX.x = vertex.x - pos.x;
                    else if (pos.x > vertex.x && (vertex.x - pos.x) > posFactorX.y)//Plus grand que vertex ? && distance ?
                        posFactorX.y = vertex.x - pos.x;
                }

            foreach (Vector3 vertex in centerPosGrass)
                if (pos.x - ErrorPositionGeometryTerrain <= vertex.x && pos.x + ErrorPositionGeometryTerrain >= vertex.x)//Aligné en X ?
                {
                    if (pos.z < vertex.z && (vertex.z - pos.z) < posFactorZ.x)//Plus petit que vertex ? && distance ?
                        posFactorZ.x = vertex.z - pos.z;
                    else if (pos.z > vertex.z && (vertex.z - pos.z) > posFactorZ.y)//Plus grand ? && distance
                        posFactorZ.y = vertex.z - pos.z;
                }
        }
        #endregion

        #region PosFactor
        if (posFactorX.x == 10)
            if (OneGrassPerVertex)
                posFactorX.x = UnityEngine.Random.Range(RandomPosPerVertex.x, RandomPosPerVertex.y);
            else
                posFactorX.x = 0;

        if (posFactorX.y == -10)
            if (OneGrassPerVertex)
                posFactorX.y = UnityEngine.Random.Range(RandomPosPerVertex.x, RandomPosPerVertex.y);
            else
                posFactorX.y = 0;


        if (posFactorZ.x == 10)
            if (OneGrassPerVertex)
                posFactorZ.x = UnityEngine.Random.Range(RandomPosPerVertex.x, RandomPosPerVertex.y);
            else
                posFactorZ.x = 0;

        if (posFactorZ.y == -10)
            if (OneGrassPerVertex)
                posFactorZ.y = UnityEngine.Random.Range(RandomPosPerVertex.x, RandomPosPerVertex.y);
            else
                posFactorZ.y = 0;
        #endregion

        return new Vector4(posFactorX.x, posFactorX.y, posFactorZ.x, posFactorZ.y);
    }

    public float HitByRay(Vector3 initialPos)
    {
        RaycastHit hit;
        Vector3 origineRay = initialPos;
        origineRay.y += 5;
        Ray ray = new Ray(origineRay, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point.y;
        }
        else
        {
            noCollision = true;
            return initialPos.y;
        }
    }
}
