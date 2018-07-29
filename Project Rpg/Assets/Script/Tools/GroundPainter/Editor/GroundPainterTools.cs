using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundPainterTools : EditorWindow
{
    #region Window Var
    private GameObject cameraTools;
    private Camera cam;
    private static GroundPainterTools windowTools;

    private Rect borderArea;
    private Rect borderAreaMenu;
    private Rect buttonArea;
    private Rect menuButton;
    private Rect extraMenuArea;
    private Rect extraMenuWindowArea;
    private Rect speedSelectionArea;
    private Rect brushMenuArea;

    private bool menuActivedBool = false;
    private bool importMenuBool = false;
    private bool settingMenuBool = false;
    private bool brushMenuBool = false;

    private Texture2D objSelectedTexture2D;

    private int i = 0;
    private int lenghtTabPrefabs = 5;

    private Vector2 posMouseRightClick;
    #endregion

    #region BuildEditor Var
    private GameObject objInHierachie;
    private GameObject objSelected;
    private GameObject folderElements;

    private List<GameObject> lastObjInstantiate = new List<GameObject>();
    private GameObject[] lastObjInstantiateBrush = new GameObject[8];

    private Object[] prefabs;

    private Ray rayTools;
    private RaycastHit hit;

    private Event currentEvent;

    private Vector2 windowSize;
    private Vector2 lastPosMouseDeplacement;
    private Vector2 lastPosMouseBrush;

    private Vector2 randomRotationMesh;
    private Vector2 randomSizeXMesh;
    private Vector2 randomSizeYMesh;
    private Vector2 randomSizeZMesh;
    private Vector2 randomSizeAllMesh;

    private Vector3 directionVectorNormal;
    private Vector3 rotateAngleValue;

    private int baseLayer;

    private bool randomRotationMeshBool = false;
    private bool randomSizeMeshXBool = false;
    private bool randomSizeMeshYBool = false;
    private bool randomSizeMeshZBool = false;
    private bool randomSizeAllMeshBool = false;
    private bool useBrush = false;

    private float speedCam = 1;
    private float randomRotationMeshValue;
    private float randomSizeMeshAllValue;
    private float sizeBrush = 1f;
    private float lastSizeBrush = 1f;
    private float deplacementBySize;

    private int densityBrush = 8;
    #endregion

    #region Cursor
    public GameObject ObjCursor;
    private Transform objTransform;
    #endregion

    #region window
    [MenuItem("CustomTools/GroundPainter")]
    public static void ShowWindow()
    {
        windowTools = (GroundPainterTools)EditorWindow.GetWindow(typeof(GroundPainterTools));
        windowTools.titleContent.text = "GroundEditor";
        windowTools.creationCam();
        windowTools.RefresCam();
        windowTools.InitialisationRect();
        windowTools.InitialisationPrefs();
        windowTools.windowSize = windowTools.position.size;

    }
    #endregion

    void OnGUI()
    {
        currentEvent = Event.current;

        if (!windowTools)
        {
            ShowWindow();
        }

        #region Cam
        if (windowSize != windowTools.position.size)
            RefresCam();
        GUI.DrawTexture(new Rect(0, 0, position.width, position.height), cam.targetTexture);
        #endregion

        #region Rect
        InitialisationRect();
        #endregion

        #region Menu

        #region Button Menu
        GUI.Box(menuActivedBool == true ? borderArea : borderAreaMenu, "");
        GUILayout.BeginArea(menuButton);
        if (GUILayout.Button("Menu"))
        {
            menuActivedBool = menuActivedBool == true ? false : true;
        }
        GUILayout.EndArea();
        #endregion

        #region Base Menu
        if (menuActivedBool)
        {
            GUILayout.BeginArea(buttonArea);

            if (GUILayout.Button("Setting"))
            {
                settingMenuBool = settingMenuBool == true ? false : true;
                importMenuBool = false;
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Import"))
            {
                importMenuBool = importMenuBool == true ? false : true;
                settingMenuBool = false;
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Brush"))
            {
                useBrush = useBrush == true ? false : true;
            }
            EditorGUILayout.Space();

            GUILayout.EndArea();
        }
        #endregion

        #region Setting Menu
        if (settingMenuBool && menuActivedBool)
        {
            GUI.Box(extraMenuArea, "Setting Menu");

            GUILayout.BeginArea(extraMenuWindowArea);

            #region Tab
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Number of prefabs use, max 15 prefabs", MessageType.Info);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            lenghtTabPrefabs = EditorGUILayout.IntField("Number of ellements", lenghtTabPrefabs);
            lenghtTabPrefabs = Mathf.Clamp(lenghtTabPrefabs, 1, 15);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            #endregion

            #region Cam
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Use mouse Whell to move the camera", MessageType.Info);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            speedCam = EditorGUILayout.FloatField("SpeedCam", speedCam);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            #endregion

            #region Rotation
            GUILayout.BeginHorizontal();
            randomRotationMeshBool = EditorGUILayout.Toggle("Use random Rotation", randomRotationMeshBool);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            randomRotationMesh = EditorGUILayout.Vector2Field("Random Rotation Mesh betwen two constante", randomRotationMesh);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            #endregion

            #region Size
            GUILayout.BeginHorizontal();
            randomSizeMeshXBool = EditorGUILayout.Toggle("randomSizeMeshXBool", randomSizeMeshXBool);
            GUILayout.EndHorizontal();
            if (randomSizeMeshXBool && !randomSizeAllMeshBool)
            {
                GUILayout.BeginHorizontal();
                randomSizeXMesh = EditorGUILayout.Vector2Field("Random Size X", randomSizeXMesh);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            randomSizeMeshYBool = EditorGUILayout.Toggle("randomSizeMeshYBool", randomSizeMeshYBool);
            GUILayout.EndHorizontal();
            if (randomSizeMeshYBool && !randomSizeAllMeshBool)
            {
                GUILayout.BeginHorizontal();
                randomSizeYMesh = EditorGUILayout.Vector2Field("Random Size Y", randomSizeYMesh);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            randomSizeMeshZBool = EditorGUILayout.Toggle("randomSizeMeshZBool", randomSizeMeshZBool);
            GUILayout.EndHorizontal();
            if (randomSizeMeshZBool && !randomSizeAllMeshBool)
            {
                GUILayout.BeginHorizontal();
                randomSizeZMesh = EditorGUILayout.Vector2Field("Random Size X", randomSizeZMesh);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            randomSizeAllMeshBool = EditorGUILayout.Toggle("randomSizeAllMeshBool", randomSizeAllMeshBool);
            GUILayout.EndHorizontal();
            if (randomSizeAllMeshBool)
            {
                GUILayout.BeginHorizontal();
                randomSizeAllMesh = EditorGUILayout.Vector2Field("randomSize", randomSizeAllMesh);
                GUILayout.EndHorizontal();
            }
            #endregion

            #region Brush
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Use right click when this toggle is actived for display brush window. Use B tutch to active the brush.", MessageType.Info);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            useBrush = EditorGUILayout.Toggle("Use Brush", useBrush);
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndArea();
        }
        #endregion

        #region Import Menu
        if (importMenuBool && menuActivedBool)
        {
            GUI.Box(extraMenuArea, "Import Menu");

            GUILayout.BeginArea(extraMenuWindowArea);

            for (i = 0; i < lenghtTabPrefabs; i++)
            {
                GUILayout.BeginHorizontal();

                if (prefabs[i])
                    GUILayout.Box(AssetPreview.GetAssetPreview(prefabs[i]), GUILayout.Width(64), GUILayout.Height(64));
                else
                    GUILayout.Box(Texture2D.blackTexture, GUILayout.Width(64), GUILayout.Height(64));

                prefabs[i] = EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), true);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }
        #endregion

        #region SpeedSelection

        GUI.Box(speedSelectionArea, "");

        GUILayout.BeginArea(speedSelectionArea);
        for (i = 0; i < lenghtTabPrefabs; i++)
        {
            GUILayout.BeginHorizontal();

            if (!prefabs[i])
                GUILayout.Box(Texture2D.blackTexture, GUILayout.Width(64), GUILayout.Height(64));
            else
            {
                EditorGUI.BeginDisabledGroup(objSelected == prefabs[i]);
                if (GUILayout.Button(AssetPreview.GetAssetPreview(prefabs[i]), GUILayout.Width(64), GUILayout.Height(64)))
                {
                    objSelected = (GameObject)prefabs[i];
                    SelectFolder(prefabs[i]);
                    GUI.enabled = true;
                }
                EditorGUI.EndDisabledGroup();
            }


            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
        #endregion

        #region Brush Menu (Right Click)

        #region MenuBrush
        if (brushMenuBool)
        {
            GUI.Box(brushMenuArea, "");
            GUILayout.BeginArea(brushMenuArea);

            sizeBrush = EditorGUILayout.FloatField("Size", sizeBrush);
            if (sizeBrush != lastSizeBrush)
            {
                objTransform.localScale = new Vector3(sizeBrush, sizeBrush, 1);
                lastSizeBrush = sizeBrush;
            }

            densityBrush = EditorGUILayout.IntField("Density", densityBrush);
            if (densityBrush != lastObjInstantiateBrush.Length)
            {
                densityBrush = Mathf.Max(densityBrush, 1);
                lastObjInstantiateBrush = new GameObject[densityBrush];
            }

            randomRotationMesh = EditorGUILayout.Vector2Field("Random Rotation", randomRotationMesh);

            GUILayout.EndArea();
        }
        #endregion

        #region AparitionMenuBrush
        if (useBrush && currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && brushMenuBool)
        {
            brushMenuBool = false;
            Repaint();
        }
        else if (useBrush && currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && !brushMenuBool)
        {
            brushMenuBool = true;
            posMouseRightClick = currentEvent.mousePosition;
        }
        #endregion

        #endregion

        #endregion

        #region ray and left click and Curso

        #region Ray and Cursor
        Handles.SetCamera(cam);
        rayTools = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
        if (Physics.Raycast(rayTools, out hit, Mathf.Infinity) && useBrush)
        {
            if (!ObjCursor.activeSelf)
                ObjCursor.SetActive(true);
            Vector3 posCursor = hit.point + (Vector3.up * .05f);
            ObjCursor.transform.position = posCursor;
        }
        else
        {
            if (ObjCursor.activeSelf)
            {
                ObjCursor.SetActive(false);
            }
        }
        #endregion

        if (cam && currentEvent.type == EventType.MouseDown && !menuActivedBool && objSelected)
        {
            #region left Click
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                #region Size
                Vector3 initialScale = objSelected.transform.localScale;

                if (randomSizeAllMeshBool)
                {
                    randomSizeMeshAllValue = Random.Range(randomSizeAllMesh.x, randomSizeAllMesh.y);
                    objSelected.transform.localScale = Vector3.one * randomSizeMeshAllValue;
                }
                else
                {
                    if (randomSizeMeshXBool)
                    {
                        objSelected.transform.localScale = new Vector3(Random.Range(randomSizeXMesh.x, randomSizeXMesh.y),
                            objSelected.transform.localScale.y, objSelected.transform.localScale.z);
                    }
                    if (randomSizeMeshYBool)
                    {
                        objSelected.transform.localScale = new Vector3(objSelected.transform.localScale.x,
                            Random.Range(randomSizeYMesh.x, randomSizeYMesh.y), objSelected.transform.localScale.z);
                    }
                    if (randomSizeMeshZBool)
                    {
                        objSelected.transform.localScale = new Vector3(objSelected.transform.localScale.x,
                            objSelected.transform.localScale.y, Random.Range(randomSizeYMesh.x, randomSizeYMesh.y));
                    }
                }
                #endregion

                #region Spawn

                if (Physics.Raycast(rayTools, out hit, Mathf.Infinity))
                {

                    GameObject objInstantiate;

                    #region No Brush
                    if (!useBrush)
                    {
                        if (!randomRotationMeshBool)
                        {
                            TestNormalVecor(hit.normal);

                            Quaternion rootVal = Quaternion.LookRotation(directionVectorNormal, hit.normal);

                            objInstantiate = GameObject.Instantiate((objSelected), hit.point, rootVal);

                            if (objInstantiate.GetComponent<MeshFilter>())
                            {
                                deplacementBySize = objInstantiate.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
                                objInstantiate.transform.position += (Vector3.up * deplacementBySize * .5f);
                            }
                            objInstantiate.transform.parent = folderElements.transform;
                        }
                        else
                        {
                            TestNormalVecor(hit.normal);

                            Quaternion rootVal = Quaternion.LookRotation(directionVectorNormal, hit.normal);

                            objInstantiate = GameObject.Instantiate(objSelected, hit.point, rootVal);

                            objInstantiate.transform.rotation *= Quaternion.Euler(Vector3.up * randomRotationMeshValue);

                            if (objInstantiate.GetComponent<MeshFilter>())
                            {
                                deplacementBySize = objInstantiate.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
                                objInstantiate.transform.position += (Vector3.up * deplacementBySize * .5f);
                            }
                        }

                        objInstantiate.transform.parent = folderElements.transform;
                        lastObjInstantiate.Add(objInstantiate);

                    }
                    #endregion

                    #region Brush
                    else
                    {
                        Vector3 initialPosHit = hit.point;

                        for (int i = 0; i < densityBrush; i++)
                        {
                            ///Brush
                            Vector2 randomPosHit = Random.insideUnitCircle * sizeBrush;
                            Vector3 directionRayPoint = (new Vector3(randomPosHit.x, 0, randomPosHit.y) + initialPosHit) - cam.transform.position;

                            Ray rayRandomPos = new Ray(cam.transform.position, directionRayPoint);
                            if (Physics.Raycast(rayRandomPos, out hit, Mathf.Infinity))
                            {
                                ///Rotation
                                TestNormalVecor(hit.normal);
                                Quaternion rootVal = Quaternion.LookRotation(directionVectorNormal, hit.normal);

                                objInstantiate = GameObject.Instantiate(objSelected, hit.point, rootVal);

                                objInstantiate.transform.rotation *= Quaternion.Euler(Vector3.up * randomRotationMeshValue);

                                if (objInstantiate.GetComponent<MeshFilter>())
                                {
                                    deplacementBySize = objInstantiate.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
                                    objInstantiate.transform.position += (Vector3.up * deplacementBySize * .5f);
                                }

                                objInstantiate.transform.parent = folderElements.transform;
                                lastObjInstantiateBrush[i] = objInstantiate;
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                objSelected.transform.localScale = initialScale;
            }
            #endregion
        }
        else if (menuActivedBool && currentEvent.type == EventType.MouseDown || brushMenuBool && currentEvent.type == EventType.MouseDown)
        {
            menuActivedBool = false;
            brushMenuBool = false;
        }
        #endregion

        #region InputUser
        InputUser();
        #endregion

        #region End
        if (currentEvent.type == EventType.MouseMove)
            windowTools.Repaint();

        windowTools.Repaint();
        #endregion
    }

    #region Initialisation
    private void InitialisationRect()
    {
        borderArea = new Rect(0, 0, 90f, windowTools.position.size.y); //Global Rect when button is actived
        borderAreaMenu = new Rect(0, 0, 90, 30); //Background Buton Menu

        buttonArea = new Rect(5, 40, 80f, windowTools.position.size.y); //Are button
        menuButton = new Rect(5, 5, 80f, 20);//Are menu

        extraMenuArea = new Rect(110, 20, windowTools.position.size.x * .5f, position.height - 40); //Global Rect when button is clicked
        extraMenuWindowArea = new Rect(130, 40, windowTools.position.size.x * .5f - 40, position.height - 80);//area wher button is xrite in extra menu

        speedSelectionArea = new Rect(windowTools.position.width - 64, 0, 64, windowTools.position.height);//Right area

        if (useBrush && brushMenuBool)
            brushMenuArea = new Rect(posMouseRightClick.x, posMouseRightClick.y * .5f, 250f, 130f);//windowTools.position.width * .5f

        Resources.UnloadUnusedAssets();
    }

    private void InitialisationPrefs()
    {
        #region Tab
        prefabs = new Object[15];

        if (EditorPrefs.GetInt("lenghtTabPrefabs") == 0)
            lenghtTabPrefabs = 5;
        else
            lenghtTabPrefabs = EditorPrefs.GetInt("lenghtTabPrefabs");

        for (i = 0; i < lenghtTabPrefabs; i++)
        {
            if (AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("prefabs" + i), typeof(Object)))
            {
                prefabs[i] = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("prefabs" + i), typeof(Object));
            }
            else
            {
                prefabs[i] = null;
            }
        }
        #endregion

        #region Cursor
        if (ObjCursor)
        {
            ObjCursor = Instantiate(ObjCursor, Vector3.zero, Quaternion.Euler(90, 0, 0));
            ObjCursor.layer = 2;
            ObjCursor.transform.parent = cameraTools.transform;
            objTransform = ObjCursor.GetComponent<Transform>();
            ObjCursor.SetActive(false);
        }
        else
            Debug.Log("Generic Cursor is Missing");
        #endregion

        #region Value Random
        randomRotationMeshBool = EditorPrefs.GetBool("randomRotationMeshBool");
        randomRotationMesh.x = EditorPrefs.GetFloat("RRValueX");
        randomRotationMesh.y = EditorPrefs.GetFloat("RRValueY");
        #endregion
    }
    #endregion

    #region Camera
    private void RefresCam()
    {
        cam.targetTexture = new RenderTexture((int)windowTools.position.width, (int)windowTools.position.height, 24, RenderTextureFormat.ARGB32);
        cam.Render();
        windowSize = windowTools.position.size;
    }

    public void creationCam()
    {
        if (GameObject.Find("CameraTools"))
            cameraTools = GameObject.Find("CameraTools");

        if (!cameraTools)
        {
            objInHierachie = new GameObject("ToolsObj");

            cameraTools = new GameObject("CameraTools");
            cameraTools.transform.position = new Vector3(0, 30, 0);
            cameraTools.transform.rotation = Quaternion.Euler(Vector3.right * 75f);
            cam = cameraTools.AddComponent<Camera>();

            cameraTools.transform.parent = objInHierachie.transform;

            cam.backgroundColor = Color.white;
            cam.orthographic = false;
            cam.orthographicSize = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize;
            cam.farClipPlane = 100f;
            cam.farClipPlane = 300f;
        }
    }
    #endregion

    #region Input User
    private void InputUser()
    {
        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.M)
        {
            menuActivedBool = menuActivedBool == true ? false : true;
        }

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.B)
        {
            useBrush = true;
        }

        if (currentEvent.type == EventType.ScrollWheel)
        {
            cameraTools.transform.position += Vector3.up * currentEvent.delta.y;
        }
        if (currentEvent.button == 2)
        {
            Vector2 dirCam;
            if (lastPosMouseDeplacement.x < currentEvent.mousePosition.x)
                dirCam.x = -speedCam;
            else if (lastPosMouseDeplacement.x > currentEvent.mousePosition.x)
                dirCam.x = speedCam;
            else
                dirCam.x = 0;

            if (lastPosMouseDeplacement.y < currentEvent.mousePosition.y)
                dirCam.y = speedCam;
            else if (lastPosMouseDeplacement.y > currentEvent.mousePosition.y)
                dirCam.y = -speedCam;
            else
                dirCam.y = 0;

            lastPosMouseDeplacement = currentEvent.mousePosition;

            cameraTools.transform.Translate(Vector3.right * dirCam.x + Vector3.up * dirCam.y);

        }
        else lastPosMouseDeplacement = currentEvent.mousePosition;

        if (currentEvent.keyCode == KeyCode.Z && currentEvent.type == EventType.KeyDown)
        {
            if (lastObjInstantiate.Count - 1 > 0)
            {
                DestroyImmediate(lastObjInstantiate[lastObjInstantiate.Count - 1]);
                lastObjInstantiate.RemoveAt(lastObjInstantiate.Count - 1);
            }
            else if (lastObjInstantiate[0] != null)
            {
                DestroyImmediate(lastObjInstantiate[0]);
            }
        }

        if (currentEvent.keyCode == KeyCode.G && currentEvent.type == EventType.KeyDown)
        {
            for (int i = 0; i < lastObjInstantiateBrush.Length; i++)
            {
                if (lastObjInstantiateBrush[i])
                {
                    DestroyImmediate(lastObjInstantiateBrush[i]);
                    lastObjInstantiateBrush[i] = null;
                }
            }
        }

    }
    #endregion

    #region Hierarchy
    private void SelectFolder(Object obj)
    {
        if (!GameObject.Find(obj.name + "_Folder"))
        {
            folderElements = new GameObject(obj.name + "_Folder");
        }
        else
            folderElements = GameObject.Find(obj.name + "_Folder");
    }
    #endregion

    #region Function
    private void TestNormalVecor(Vector3 normal)
    {
        if (randomRotationMeshBool || useBrush)
            randomRotationMeshValue = Random.Range(randomRotationMesh.x, randomRotationMesh.y);

        if (normal.x < .01f && normal.x > -.01f)
        {
            directionVectorNormal = Vector3.right;
        }
        else if (normal.z < .01f && normal.z > -.01f)
        {
            directionVectorNormal = Vector3.forward;
        }
        else
        {
            directionVectorNormal = Vector3.Cross(normal, Vector3.up);
        }
    }
    #endregion

    private void OnDestroy()
    {
        EditorPrefs.SetInt("lenghtTabPrefabs", lenghtTabPrefabs);
        EditorPrefs.SetFloat("RRValueX", randomRotationMesh.x);
        EditorPrefs.SetFloat("RRValueY", randomRotationMesh.y);
        EditorPrefs.SetBool("randomRotationMeshBool", randomRotationMeshBool);
        for (i = 0; i < lenghtTabPrefabs; i++)
        {
            EditorPrefs.SetString("prefabs" + i, AssetDatabase.GetAssetPath(prefabs[i]));
        }
        Resources.UnloadUnusedAssets();

        DestroyImmediate(objInHierachie);
    }
}
