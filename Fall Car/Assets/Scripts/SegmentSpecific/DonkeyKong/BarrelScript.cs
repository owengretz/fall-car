using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{

    public Vector3 velocity;
    public bool dontSetVel;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FellOff.instance.Die();
        }
    }

    private void FixedUpdate()
    {
        if (!dontSetVel)
            GetComponent<Rigidbody>().velocity = new Vector3(0f, GetComponent<Rigidbody>().velocity.y, velocity.z);
        //Debug.Log(GetComponent<Rigidbody>().velocity);
    }
}
