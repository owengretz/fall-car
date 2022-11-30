using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public bool mainCar;

    private Rigidbody rb;
    private float speed = 800f;

    [HideInInspector] public bool moving;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!mainCar)
        {
            float variation = Random.Range(-100f, 100f);
            speed += variation;
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.velocity = transform.forward * Time.fixedDeltaTime * speed;
        }
    }
}
