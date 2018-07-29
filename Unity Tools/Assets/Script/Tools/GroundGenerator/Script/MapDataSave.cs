using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Save", menuName = "MapDataSave")]
[SerializeField]
public class MapDataSave //: ScriptableObject 
{
    public string NameMap;
    public int CheckerOnTheLenght;
    public int CheckerOnTheHeight;
    public int CellByLenghtChecker;
    public int Density;

    public float vertexColorRedValue;
    public float vertexColorGreenValue;
    public float vertexColorBlueValue;

    public List<HeightGround> Height;
}
