using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool MyTurn = false;
    public bool Move = false;

    #region Deplacement
    public float Speed = 5f;
    public Vector3[] Path;
    private Vector3 destination;
    private int indexPath = 0;
    #endregion

    public void FixedUpdate()
    {
        if (!MyTurn)
            return;

        if (Move)
        {
            //Move to the destination
            float distanceBetwenTwoPoint = Vector3.Distance(this.transform.position, destination);
            transform.position = Vector3.Lerp(this.transform.position, destination, (Speed * Time.deltaTime) / distanceBetwenTwoPoint);

            //The player is on the case ?
            if (Mathf.Abs(Vector3.Distance(this.transform.position, destination)) < .05f)
            {
                this.transform.position = destination;
                if(indexPath < Path.Length)
                    destination = Path[indexPath++];
                else
                    Move = false;
            }
        }
    }
}
