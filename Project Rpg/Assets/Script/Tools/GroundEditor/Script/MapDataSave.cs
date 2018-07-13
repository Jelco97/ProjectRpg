using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Save", menuName = "MapDataSave")][SerializeField]
public class MapDataSave //: ScriptableObject 
{
    public string NameMap;
    public int CheckerOnTheLenght;
    public int CheckerOnTheHeight;
    public int CellByLenghtChecker;
    public int Density;
    //public GameObject GroundFolder;
    //public List<GameObject> Checker;
    public List<HeightGround> Height;
}
