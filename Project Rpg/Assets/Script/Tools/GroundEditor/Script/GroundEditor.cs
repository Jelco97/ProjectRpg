using UnityEditor;
using UnityEngine;

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
    ///Editor Skin
    private bool skinNewGround;
    private bool skinCheckerVue;
    private bool skinCellVue;

    ///New Ground
    string nameMap = "New Map";
    int checkerOnTheLenght = 4;
    int checkerOnTheHeight = 4;
    int cellByLenghtChecker = 10;
    int cellDensity = 4;

    ///Save
    GameObject GroundFolder;
    GameObject[] checker;

    ///Tile vue
    GameObject currentGround;

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
        GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
        ToolBarButton();
        GUILayout.EndHorizontal();

        if (skinNewGround)
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
    }

    #region File
    void NewGround()
    {
        skinNewGround = true;
    }

    void LoadGround()
    {

    }

    void SaveGround()
    {

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
        cellDensity = Mathf.Min(checkerOnTheLenght, 10);
        cellDensity = Mathf.Max(checkerOnTheLenght, 1);
        #endregion

        #region Validation
        Rect validationButtonRect = new Rect(backgroundRect.x + 20, backgroundRect.y + backgroundRect.size.y - 50, 120, 30);/////
        if (GUI.Button(validationButtonRect, "Cancel"))
            skinNewGround = false;
        validationButtonRect.x += 290;
        if (GUI.Button(validationButtonRect, "Creat"))
        {
            CreatGround();
            skinNewGround = false;
            skinCheckerVue = true;
        }
        #endregion

    }

    void CheckerEditor()
    {
        Rect buttonRect = ButtonRect(20, position.size.y - 28, 20);
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
                if(GUI.Button(buttonRect, "" + index))
                {
                    skinCellVue = true;
                    currentGround = checker[index];
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

        Rect verticalToolBarButton = new Rect(5, 23, 40, 40);
        if(GUI.Button(verticalToolBarButton, "Back"))
        {
            skinCheckerVue = true;
            skinCellVue = false;
        }

        #region Cell
        Rect cellButtonRect = new Rect();584654
        #endregion

    }
    #endregion

    #region Cell
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

    Rect ButtonRect(float XPos, float YPos, float Margin)
    {
        float xSize = (position.size.x - (Margin * 2) - ((checkerOnTheLenght + 1) * 5)) / checkerOnTheLenght;
        float ySize = (position.size.y - 18 - (Margin * 2) - ((checkerOnTheHeight + 1) * 5)) / checkerOnTheHeight;
        return new Rect(XPos, YPos, xSize, ySize);
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
        checker = new GameObject[checkerOnTheHeight * checkerOnTheLenght];

        int  cell = cellByLenghtChecker +1;
        int index = 0;
        for (int y = 0; y < checkerOnTheHeight; y++)
            for (int x = 0; x < checkerOnTheLenght; x++)
            {
                checker[index] = new GameObject();
                checker[index].transform.parent = GroundFolder.transform;
                checker[index].name = "" + nameMap + " " + index;
                GroundBaseGenerator groundScript = checker[index].AddComponent<GroundBaseGenerator>();

                groundScript.NumberTile = cell;
                groundScript.Density = cellDensity;
                groundScript.HeightChecker = new HeightGround();
                groundScript.HeightChecker.InitialisationRowArray(cell);

                groundScript.GenerateGroundBase();

                checker[index].transform.position = new Vector3(x * cellByLenghtChecker, 0, y * cellByLenghtChecker);
                checker[index].GetComponent<MeshRenderer>().material = CheckerMaterial;
                index++;
            }
    }
    #endregion
}