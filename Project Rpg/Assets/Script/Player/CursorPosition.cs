using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CursorPosition : MonoBehaviour
{
    public GameObject ClickParticle;

    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    public Character CharacterScript;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            Vector2Int cursorPosition = new Vector2Int((int)Mathf.Ceil(hit.point.x), (int)Mathf.Ceil(hit.point.z));
            Debug.Log("Round Pos = " + cursorPosition);
            Instantiate(ClickParticle, new Vector3(cursorPosition.x - .5f, .1f, cursorPosition.y - .5f), Quaternion.identity);
            
            if (CharacterScript)
            {
                CharacterScript.Move = true;
                //CharacterScript.destination = new Vector3(cursorPosition.x - .5f, .5f, cursorPosition.y - .5f);
            }
        }
    }
}
