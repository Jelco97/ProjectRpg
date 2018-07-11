using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundEditor : EditorWindow
{
    #region Material
    public Material CheckerMaterial;
    #endregion

    #region GUIStyle
    GUIStyle header;
    GUIStyle subtitle;
    GUIStyle field;
    #endregion

    #region Color
    Color borderColor = new Color(.9f, .9f, .9f, 1);
    Color backgroundColor = new Color(.8f, .8f, .8f, 1);
    #endregion

    #region Var
    ///Mouse
    bool mouseClicked;

    ///ToolBar
    bool groundCreat;

    ///Editor Skin
    private bool skinNewGround;
    private bool skinCheckerVue;
    private bool skinCellVue;
    private bool skinDataGround;

    ///New Ground
    string nameMap = "New Map";
    int checkerOnTheLenght = 4;
    int checkerOnTheHeight = 4;
    int cellByLenghtChecker = 10;
    int cellDensity = 4;

    ///Save
    GameObject GroundFolder;
    List<GameObject> checker = new List<GameObject>();
    HeightGround currentHeightGround;
    string initNameMap;
    int initCheckerOnTheLenght;
    int initCheckerOnTheHeight;
    int initCellByLenghtChecker;
    int initCellDensity;

    ///Tile vue
    GameObject currentGround;
    List<Vector2> cellPaint = new List<Vector2>();
    bool paintMode;
    bool valueChoice;
    float paintValue;

    #endregion

    void Awake()
    {
        header = new GUIStyle();
        header.fontStyle = FontStyle.Bold;
        header.fontSize = 18;

        subtitle = new GUIStyle();
        subtitle.fontSize = 14;

        field = new GUIStyle();
        field.fontSize = 10;

    }

    [MenuItem("Window/GroundEditor")]
    static void Init()
    {
        GroundEditor window = (GroundEditor)EditorWindow.GetWindow(typeof(GroundEditor));
        window.minSize = new Vector2(500, 400);
    }

    void OnGUI()
    {
        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0 && mouseClicked)
            mouseClicked = false;
        else if (!mouseClicked && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            mouseClicked = true;

        GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
        ToolBarButton();
        GUILayout.EndHorizontal();

        if (skinNewGround || skinDataGround)
            NewGroundEditor();

        else if (skinCheckerVue)
            CheckerEditor();

        else if (skinCellVue)
            CellEditor();

    }

    #region Tool Bar
    void ToolBarButton()
    {
        Rect buttonRect = new Rect(3, 0, 50, 18);
        if (GUI.Button(buttonRect, "File", EditorStyles.toolbarDropDown))
        {
            GenericMenu toolsMenu = new GenericMenu();
            toolsMenu.AddItem(new GUIContent("New"), false, NewGround);
            toolsMenu.AddItem(new GUIContent("Load"), false, LoadGround);
            toolsMenu.AddSeparator("");
            toolsMenu.AddItem(new GUIContent("Save"), false, SaveGround);

            Rect dropDownRect = new Rect(3, 3, 0, 16);
            toolsMenu.DropDown(dropDownRect);

            EditorGUIUtility.ExitGUI();
        }

        EditorGUI.BeginDisabledGroup(!groundCreat);
        buttonRect.x += 50;
        if (GUI.Button(buttonRect, "Checker", EditorStyles.toolbarDropDown))
        {
            GenericMenu toolsMenu = new GenericMenu();
            toolsMenu.AddItem(new GUIContent("Ground Data"), false, GroundData);

            Rect dropDownRect = new Rect(53, 3, 0, 16);
            toolsMenu.DropDown(dropDownRect);

            EditorGUIUtility.ExitGUI();
        }

        buttonRect.x += 50;
        if (GUI.Button(buttonRect, "Cell", EditorStyles.toolbarDropDown))
        {
            GenericMenu toolsMenu = new GenericMenu();
            toolsMenu.AddItem(new GUIContent("Clean"), false, CleanCurrentCell);

            Rect dropDownRect = new Rect(103, 3, 0, 16);
            toolsMenu.DropDown(dropDownRect);

            EditorGUIUtility.ExitGUI();
        }
        EditorGUI.EndDisabledGroup();
    }

    #region File
    void NewGround()
    {
        skinNewGround = true;
    }

    void LoadGround()
    {
        Debug.Log(checker.Count);
    }

    void SaveGround()
    {

    }
    #endregion

    #region Checker
    void GroundData()
    {
        initNameMap = nameMap;
        initCheckerOnTheLenght = checkerOnTheLenght;
        initCheckerOnTheHeight = checkerOnTheHeight;
        initCellByLenghtChecker = cellByLenghtChecker;
        initCellDensity = cellDensity;

        skinDataGround = true;
    }

    void CancelGroundData()
    {
        nameMap = initNameMap;
        checkerOnTheLenght = initCheckerOnTheLenght;
        checkerOnTheHeight = initCheckerOnTheHeight;
        cellByLenghtChecker = initCellByLenghtChecker;
        cellDensity = initCellDensity;

        skinDataGround = false;
    }
    #endregion

    #endregion

    #region Skin
    void NewGroundEditor()
    {
        Rect backgroundBorderRect = new Rect(MidlePos(new Vector2(456, 356)), new Vector2(456, 356));/////
        EditorGUI.DrawRect(backgroundBorderRect, borderColor);
        Rect backgroundRect = new Rect(MidlePos(new Vector2(450, 350)), new Vector2(450, 350));/////
        EditorGUI.DrawRect(backgroundRect, backgroundColor);

        Rect headerRect = new Rect(backgroundRect.x + 10, backgroundRect.y + 10, 430, 20);/////
        if (skinDataGround)
            GUI.Label(headerRect, "Ground Data", header);
        else
            GUI.Label(headerRect, "New Ground", header);

        #region Map
        Rect subtitleRect = new Rect(headerRect.x + 20, headerRect.y + 30, 410, 20);/////
        GUI.Label(subtitleRect, "Map", subtitle);

        Rect fieldBorderBackgroundRect = new Rect(subtitleRect.x + 10, subtitleRect.y + 30, 390, 20);/////
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        Rect fieldBackgroundRect = new Rect(subtitleRect.x + 200, subtitleRect.y + 33, 197, 14);/////
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        Rect labelRec = fieldBorderBackgroundRect;/////
        labelRec.y += 3;
        labelRec.x += 3;
        GUI.Label(labelRec, "Map Name", field);
        Rect fieldRect = fieldBackgroundRect;/////
        fieldRect.x += 3;
        nameMap = EditorGUI.TextField(fieldRect, nameMap, field);
        #endregion

        #region Checker
        subtitleRect.y += 60;//20 Per Box +10 per line
        GUI.Label(subtitleRect, "Checker", subtitle);

        fieldBorderBackgroundRect.y += 60;///60 = Last box + new Laber + 2* border (10)
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 60;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 60;
        GUI.Label(labelRec, "Checker on the lenght", field);
        fieldRect.y += 60;
        //if (skinDataGround)
        //    GUI.Label(fieldRect, "" + checkerOnTheLenght, field);
        //else
        checkerOnTheLenght = EditorGUI.IntField(fieldRect, checkerOnTheLenght, field);
        checkerOnTheLenght = Mathf.Min(checkerOnTheLenght, 40);
        checkerOnTheLenght = Mathf.Max(checkerOnTheLenght, 1);

        fieldBorderBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 30;
        GUI.Label(labelRec, "Checker on the height", field);
        fieldRect.y += 30;
        //if (skinDataGround)
        //    GUI.Label(fieldRect, "" + checkerOnTheHeight, field);
        //else
        checkerOnTheHeight = EditorGUI.IntField(fieldRect, checkerOnTheHeight, field);
        checkerOnTheHeight = Mathf.Min(checkerOnTheHeight, 40);
        checkerOnTheHeight = Mathf.Max(checkerOnTheHeight, 1);

        fieldBorderBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 30;
        GUI.Label(labelRec, "Cell by checker on the lenght", field);
        fieldRect.y += 30;
        cellByLenghtChecker = EditorGUI.IntField(fieldRect, cellByLenghtChecker, field);
        cellByLenghtChecker = Mathf.Min(cellByLenghtChecker, 50);
        cellByLenghtChecker = Mathf.Max(cellByLenghtChecker, 5);
        #endregion

        #region Cell
        subtitleRect.y += 120;//20 Per Box +10 per line
        GUI.Label(subtitleRect, "Mesh", subtitle);

        fieldBorderBackgroundRect.y += 60;
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 60;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 60;
        GUI.Label(labelRec, "Cell Density", field);
        fieldRect.y += 60;
        cellDensity = EditorGUI.IntField(fieldRect, cellDensity, field);
        cellDensity = Mathf.Min(cellDensity, 10);
        cellDensity = Mathf.Max(cellDensity, 1);
        #endregion

        #region Validation
        Rect validationButtonRect = new Rect(backgroundRect.x + 20, backgroundRect.y + backgroundRect.size.y - 50, 120, 30);/////

        if (!skinDataGround)
        {
            if (GUI.Button(validationButtonRect, "Cancel"))
                skinNewGround = false;
            validationButtonRect.x += 290;
            if (GUI.Button(validationButtonRect, "Creat"))
            {
                CreatGround();
                skinNewGround = false;
                skinCheckerVue = true;
            }
        }
        else
        {
            if (GUI.Button(validationButtonRect, "Cancel"))
                CancelGroundData();
            validationButtonRect.x += 290;
            if (GUI.Button(validationButtonRect, "Rebuild"))
            {
                RebuildGround();
                skinNewGround = false;
                skinDataGround = false;
                skinCheckerVue = true;
            }
        }
        #endregion

    }

    void CheckerEditor()
    {
        Rect buttonRect = ButtonRect(20, checkerOnTheLenght, position.size.y - 28, checkerOnTheHeight, 20);
        buttonRect.y -= buttonRect.size.y;
        int index = 0;

        for (int y = checkerOnTheHeight; y > 0; y--)
        {
            if (y != checkerOnTheHeight)
                buttonRect.y -= buttonRect.size.y + 5;

            buttonRect.x = 20;

            for (int x = 0; x < checkerOnTheLenght; x++)
            {
                if (x != 0)
                    buttonRect.x += buttonRect.size.x + 5;
                if (GUI.Button(buttonRect, "" + index))
                {
                    skinCellVue = true;
                    currentGround = checker[index];
                    currentHeightGround = currentGround.GetComponent<GroundBaseGenerator>().HeightChecker;
                    skinCheckerVue = false;
                }
                index++;
            }
        }
    }

    void CellEditor()
    {
        Rect verticalToolBar = new Rect(0, 18, 53, position.size.y);//////
        EditorGUI.DrawRect(verticalToolBar, borderColor);
        verticalToolBar = new Rect(0, 18, 50, position.size.y);
        EditorGUI.DrawRect(verticalToolBar, backgroundColor);

        Rect verticalToolBarButton = new Rect(5, 23, 40, 40);//////
        if (GUI.Button(verticalToolBarButton, "Back"))
        {
            paintMode = false;
            skinCheckerVue = true;
            skinCellVue = false;
        }

        verticalToolBarButton.y += 45;
        if (GUI.Button(verticalToolBarButton, "Build"))
        {
            RebuildCurrentChecker();
        }

        verticalToolBarButton.y += 45;
        if (!paintMode)
        {
            if (GUI.Button(verticalToolBarButton, "Paint"))
            {
                paintMode = true;
            }
        }
        else if (GUI.Button(verticalToolBarButton, "P"))
        {
            paintMode = false;
        }


        #region Cell
        Rect cellButtonRect = ButtonRect(70, cellByLenghtChecker, (position.size.y - 20), cellByLenghtChecker, 5, position.size.x - 70, position.size.y - 18);
        cellButtonRect.y -= cellButtonRect.size.y;
        Rect cellFieldRect = CenterPosRect(cellButtonRect);
        float initialPosXCellFieldRect = cellFieldRect.x;

        for (int y = cellByLenghtChecker; y > 0; y--)
        {
            cellButtonRect.x = 70;
            cellFieldRect.x = initialPosXCellFieldRect;

            if (y != cellByLenghtChecker)
            {
                cellButtonRect.y -= cellButtonRect.size.y + 5;
                cellFieldRect.y -= cellButtonRect.size.y + 5;
            }

            for (int x = 0; x < cellByLenghtChecker; x++)
            {

                if (!paintMode)
                {
                    EditorGUI.DrawRect(cellButtonRect, backgroundColor);
                    currentHeightGround.HeightGroundData[Mathf.Abs(y - cellByLenghtChecker)].Row[x] =
                        EditorGUI.FloatField(cellFieldRect, currentHeightGround.HeightGroundData[Mathf.Abs(y - cellByLenghtChecker)].Row[x], field);
                }

                if (paintMode)
                {
                    if (cellPaint.Contains(new Vector2(y, x)))
                        EditorGUI.DrawRect(cellButtonRect, borderColor);
                    else
                        EditorGUI.DrawRect(cellButtonRect, backgroundColor);

                    GUI.Label(cellFieldRect, "" + currentHeightGround.HeightGroundData[Mathf.Abs(y - cellByLenghtChecker)].Row[x], field);

                    if (mouseClicked)
                    {
                        if (cellButtonRect.Contains(Event.current.mousePosition))
                        {
                            if (!valueChoice)
                            {
                                mouseClicked = true;
                                valueChoice = true;
                                paintValue = currentHeightGround.HeightGroundData[Mathf.Abs(y - cellByLenghtChecker)].Row[x];
                                Debug.Log(paintValue);
                            }
                            else if (valueChoice && !cellPaint.Contains(new Vector2(y, x)))
                            {
                                cellPaint.Add(new Vector2(y, x));
                                currentHeightGround.HeightGroundData[Mathf.Abs(y - cellByLenghtChecker)].Row[x] = paintValue;
                                Repaint();
                            }
                        }
                    }
                    else if (!mouseClicked && valueChoice)
                    {
                        cellPaint.Clear();
                        Repaint();
                        valueChoice = false;
                    }

                }

                cellButtonRect.x += cellButtonRect.size.x + 5;
                cellFieldRect.x += cellButtonRect.size.x + 5;
            }
        }
        #endregion

    }
    #endregion

    #region Skin Constructor
    Vector2 MidlePos(Vector2 SizeRect, bool yBorder = true)
    {
        float xPos = (position.size.x / 2) - (SizeRect.x / 2);
        float yPos;

        if (yBorder)
            yPos = ((position.size.y + 18) / 2) - (SizeRect.y / 2);
        else
            yPos = (position.size.y / 2) - (SizeRect.y / 2);

        return new Vector2(xPos, yPos);
    }

    /// <summary>
    /// Creat a Rect with correct proportion
    /// </summary>
    /// <param name="XPos">X Position of the rect</param>
    /// <param name="numberXCell"></param>
    /// <param name="YPos">Y Position of the rect</param>
    /// <param name="numberYCell"></param>
    /// <param name="Margin">Margin in right and left</param>
    /// <returns></returns>
    Rect ButtonRect(float XPos, int numberXCell, float YPos, int numberYCell, float Margin, float SizeX = 0, float SizeY = 0)
    {
        if (SizeX == 0)
        {
            SizeX = position.size.x;
            SizeY = position.size.y;
        }

        float xSize = (SizeX - (Margin * 2) - ((numberXCell + 1) * 5)) / numberXCell;
        float ySize = (SizeY - 18 - (Margin * 2) - ((numberYCell + 1) * 5)) / numberYCell;
        return new Rect(XPos, YPos, xSize, ySize);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="XPos">Current x Pos</param>
    /// <param name="YPos">Current y Pos</param>
    /// <param name="XSize">Current x Size</param>
    /// <param name="YSize">Current y Size</param>
    /// <param name="NewXSize">New x Size (10)</param>
    /// <param name="NewYSize">New y Size (10)</param>
    /// <returns></returns>
    Rect CenterPosRect(float XPos, float YPos, float XSize, float YSize, float NewXSize = 10, float NewYSize = 10)
    {
        float xPos = ((XSize / 2) + XPos) - (NewXSize / 2);
        float yPos = ((YSize / 2) + YPos) - (NewYSize / 2);

        return new Rect(xPos, yPos, NewXSize, NewYSize);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="CurrentRect"></param>
    /// <param name="NewXSize">New x Size (10)</param>
    /// <param name="NewYSize">New y Size (10)</param>
    /// <returns></returns>
    Rect CenterPosRect(Rect CurrentRect, float NewXSize = 10, float NewYSize = 10)
    {
        float xPos = ((CurrentRect.size.x / 2) + CurrentRect.position.x) - (NewXSize / 2);
        float yPos = ((CurrentRect.size.y / 2) + CurrentRect.position.y) - (NewYSize / 2);

        return new Rect(xPos, yPos, NewXSize, NewYSize);
    }
    #endregion

    #region Ground
    void CreatGround()
    {
        if (!GroundFolder)
        {
            GroundFolder = new GameObject();
            GroundFolder.transform.position = Vector3.zero;
            GroundFolder.name = "GroundFolder";
        }

        int cell = cellByLenghtChecker + 1;
        int index = 0;
        for (int y = 0; y < checkerOnTheHeight; y++)
            for (int x = 0; x < checkerOnTheLenght; x++)
            {
                checker.Add(new GameObject());
                checker[index].transform.parent = GroundFolder.transform;
                checker[index].name = "" + nameMap + " " + index;
                GroundBaseGenerator groundScript = checker[index].AddComponent<GroundBaseGenerator>();

                groundScript.NumberCellByLenght = cell;
                groundScript.Density = cellDensity;
                groundScript.HeightChecker = new HeightGround();
                groundScript.HeightChecker.InitialisationRowArray(cell);

                groundScript.GenerateGroundBase();

                checker[index].transform.position = new Vector3(x * cellByLenghtChecker, 0, y * cellByLenghtChecker);
                checker[index].GetComponent<MeshRenderer>().material = CheckerMaterial;
                index++;
            }

        groundCreat = true;
    }

    void RebuildCurrentChecker()
    {
        currentGround.GetComponent<GroundBaseGenerator>().GenerateGroundBase();
    }

    void CleanCurrentCell()
    {
        currentHeightGround.CleanHeight();
        RebuildCurrentChecker();
        Repaint();
    }

    void RebuildGround()
    {
        #region Cell
        foreach (GameObject obj in checker)
        {
            GroundBaseGenerator script = obj.GetComponent<GroundBaseGenerator>();
            script.NumberCellByLenght = cellByLenghtChecker;
            script.Density = cellDensity;
            script.HeightChecker.NewRowArray(cellByLenghtChecker);
            script.GenerateGroundBase();
        }
        #endregion

        int newNumberCheckerIncreaseOnTheLenght = checkerOnTheLenght - initCheckerOnTheLenght;//2-4

        if (newNumberCheckerIncreaseOnTheLenght > 0)
        {
            Debug.Log("Lenght");
            int indexCheckerOnTheLenghtPos = initCheckerOnTheLenght;//2

            for (int y = 0; y < initCheckerOnTheHeight; y++)
            {
                if (y != 0)
                    indexCheckerOnTheLenghtPos += initCheckerOnTheLenght + newNumberCheckerIncreaseOnTheLenght;

                for (int x = 0; x < newNumberCheckerIncreaseOnTheLenght; x++)
                {
                    checker.Insert(indexCheckerOnTheLenghtPos + x, new GameObject());
                    GameObject newChecker = checker[indexCheckerOnTheLenghtPos + x];
                    newChecker.transform.parent = GroundFolder.transform;
                    newChecker.name = "Extended Checker" + (indexCheckerOnTheLenghtPos + x);

                    GroundBaseGenerator script = newChecker.AddComponent<GroundBaseGenerator>();
                    script.NumberCellByLenght = cellByLenghtChecker;
                    script.Density = cellDensity;
                    script.HeightChecker = new HeightGround();
                    script.HeightChecker.InitialisationRowArray(cellByLenghtChecker);
                    script.GenerateGroundBase();

                    newChecker.GetComponent<MeshRenderer>().material = CheckerMaterial;
                }
            }
        }

        else
        {
            int absNewNumberCheckerIncreaseOnTheLenght = Mathf.Abs(newNumberCheckerIncreaseOnTheLenght);
            int initialSizeArrayChecker = checker.Count;

            for (int y = 0; y < initCheckerOnTheHeight; y++)
            {
                if(y !=0)
                {
                    initialSizeArrayChecker -= checkerOnTheLenght;
                }

                for(int x = 0; x < absNewNumberCheckerIncreaseOnTheLenght; x++)
                {
                    initialSizeArrayChecker -= 1;
                    DestroyImmediate(checker[initialSizeArrayChecker]);
                    checker.RemoveAt(initialSizeArrayChecker);

                }
            }
        }

        int newNumberCheckerIncreaseOnTheHeight = checkerOnTheHeight - initCheckerOnTheHeight;//2-4

        if (newNumberCheckerIncreaseOnTheHeight > 0)
        {
            Debug.Log("Height");
            for (int y = 0; y < newNumberCheckerIncreaseOnTheHeight; y++)
            {
                for (int x = 0; x < checkerOnTheLenght; x++)
                {
                    checker.Add(new GameObject());

                    GameObject newChecker = checker[checker.Count - 1];
                    newChecker.transform.parent = GroundFolder.transform;
                    newChecker.name = "Extended Checker" + checker.Count;

                    GroundBaseGenerator script = newChecker.AddComponent<GroundBaseGenerator>();
                    script.NumberCellByLenght = cellByLenghtChecker;
                    script.Density = cellDensity;
                    script.HeightChecker = new HeightGround();
                    script.HeightChecker.InitialisationRowArray(cellByLenghtChecker);
                    script.GenerateGroundBase();

                    newChecker.GetComponent<MeshRenderer>().material = CheckerMaterial;
                }
            }
        }

        else
        {
            int absNewNumberCheckerIncreaseOnTheHeight = Mathf.Abs(newNumberCheckerIncreaseOnTheHeight);
            for (int y = 0; y < absNewNumberCheckerIncreaseOnTheHeight; y++)
            {
                for(int x = 0; x < checkerOnTheLenght;x++)
                {
                    DestroyImmediate(checker[checker.Count - 1]);
                    checker.RemoveAt(checker.Count - 1);
                }
            }
        }
        

        #region Placement
        int index = 0;
        for (int y = 0; y < checkerOnTheHeight; y++)
        {
            for (int x = 0; x < checkerOnTheLenght; x++)
            {
                checker[index].transform.position = new Vector3(x * (cellByLenghtChecker - 1), 0, y * (cellByLenghtChecker - 1));
                index++;
            }
        }
        #endregion
    }

    #endregion
}
