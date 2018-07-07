using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save", menuName = "MapDataSave")]
public class MapDataSave : ScriptableObject {

    public string NameMap;
    public GameObject GroundFolder;
    public int CheckerOnTheLenght;
    public int CheckerOnTheHeight;
    public int CellBychecker;
    public GameObject[] Checker;
}
