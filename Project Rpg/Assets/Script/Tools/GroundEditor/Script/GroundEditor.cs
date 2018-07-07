using UnityEditor;
using UnityEngine;

 public class GroundEditor : EditorWindow
{
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

    ///New Ground
    string NameMap = "New Map";
    int CheckerOnTheLenght;
    int CheckerOnTheHeight;
    int CellByLenghtChecker;
    int CellDensity;
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

        if(skinNewGround)
        {
            NewGroundEditor();
        }
    }

    #region Tool Bar
    void ToolBarButton()
    {
        Rect buttonRect = new Rect(3, 0, 50, 18);
        if(GUI.Button(buttonRect, "File",EditorStyles.toolbarDropDown))
        {
            GenericMenu toolsMenu = new GenericMenu();
            toolsMenu.AddItem(new GUIContent("New"),false, NewGround);
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
        GUI.Label(headerRect, "New Ground",header);

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
        GUI.Label(labelRec, "Map Name",field);
        Rect fieldRect = fieldBackgroundRect;/////
        fieldRect.x += 3;
        NameMap = EditorGUI.TextField(fieldRect, NameMap, field);
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
        CheckerOnTheLenght = EditorGUI.IntField(fieldRect, CheckerOnTheLenght, field);
        CheckerOnTheLenght = Mathf.Min(CheckerOnTheLenght, 40);
        CheckerOnTheLenght = Mathf.Max(CheckerOnTheLenght, 1);

        fieldBorderBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 30;
        GUI.Label(labelRec, "Checker on the height", field);
        fieldRect.y += 30;
        CheckerOnTheHeight = EditorGUI.IntField(fieldRect, CheckerOnTheHeight, field);
        CheckerOnTheHeight = Mathf.Min(CheckerOnTheHeight, 40);
        CheckerOnTheHeight = Mathf.Max(CheckerOnTheHeight, 1);

        fieldBorderBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBorderBackgroundRect, borderColor);
        fieldBackgroundRect.y += 30;
        EditorGUI.DrawRect(fieldBackgroundRect, backgroundColor);

        labelRec.y += 30;
        GUI.Label(labelRec, "Cell by checker on the lenght", field);
        fieldRect.y += 30;
        CellByLenghtChecker = EditorGUI.IntField(fieldRect, CellByLenghtChecker, field);
        CellByLenghtChecker = Mathf.Min(CellByLenghtChecker, 50);
        CellByLenghtChecker = Mathf.Max(CellByLenghtChecker, 5);
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
        CellDensity = EditorGUI.IntField(fieldRect, CellDensity, field);
        CellDensity = Mathf.Min(CheckerOnTheLenght, 10);
        CellDensity = Mathf.Max(CheckerOnTheLenght, 1);
        #endregion

        #region Validation
        Rect validationButtonRect = new Rect(backgroundRect.x + 20, backgroundRect.y + backgroundRect.size.y - 50, 120, 30);/////
        GUI.Button(validationButtonRect, "Cancel");
        validationButtonRect.x += 290;
        GUI.Button(validationButtonRect, "Creat");
        #endregion

    }

    void CancelNewGround()
    {
        skinNewGround = false;
    }
    #endregion

    #region Skin Constructor
    Vector2 MidlePos(Vector2 SizeRect,bool yBorder = true)
    {
        float xPos = (position.size.x / 2) - (SizeRect.x /2);
        float yPos;

        if (yBorder)
            yPos = ((position.size.y +18) / 2) - (SizeRect.y / 2);
        else
            yPos = (position.size.y / 2) - (SizeRect.y /2);

        return new Vector2(xPos, yPos);
    }
    #endregion

    #region Ground

    #endregion
}