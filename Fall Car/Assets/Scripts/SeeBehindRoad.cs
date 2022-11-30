using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeBehindRoad : MonoBehaviour
{
    //Public variable to store a reference to the player game object
    public GameObject[] raycastCheckers;
    //The current wall
    private TransparentRoad currentTransparentWall;

    private void FixedUpdate()
    {
        //Calculate the Vector direction 
        Vector3 direction0 = raycastCheckers[0].transform.position - transform.position;
        Vector3 direction1 = raycastCheckers[1].transform.position - transform.position;
        //Calculate the length
        float length0 = Vector3.Distance(raycastCheckers[0].transform.position, transform.position);
        float length1 = Vector3.Distance(raycastCheckers[1].transform.position, transform.position);
        //Draw the ray in the debug
        Debug.DrawRay(transform.position, direction0 * length0, Color.red);
        Debug.DrawRay(transform.position, direction1 * length1, Color.red);
        //The first object hit reference
        RaycastHit currentHit;
        //Cast the ray and report the firt object hit filtering by "Wall" layer mask
        if (Physics.Raycast(transform.position, direction0, out currentHit, length0, LayerMask.GetMask("TransparentRoad")) || Physics.Raycast(transform.position, direction1, out currentHit, length1, LayerMask.GetMask("TransparentRoad")))
        {
            //Getting the script to change transparency of the hit object
            TransparentRoad transparentWall = currentHit.transform.GetComponent<TransparentRoad>();
            //If the object is not null
            if (transparentWall)
            {
                //If there is a previous wall hit and it's different from this one
                if (currentTransparentWall && currentTransparentWall.gameObject != transparentWall.gameObject)
                {
                    //Restore its transparency setting it not transparent
                    currentTransparentWall.ChangeTransparency(false);
                }
                //Change the object transparency in transparent.
                transparentWall.ChangeTransparency(true);
                currentTransparentWall = transparentWall;
            }
        }
        else
        {
            //Debug.Log("not behind wall");
            //If nothing is hit and there is a previous object hit
            if (currentTransparentWall)
            {
                //Restore its transparency setting it not transparent
                currentTransparentWall.ChangeTransparency(false);
            }
        }
    }
}
