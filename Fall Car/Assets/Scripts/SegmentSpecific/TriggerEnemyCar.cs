using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyCar : MonoBehaviour
{
    public EnemyCar[] cars;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach (EnemyCar car in cars)
                car.moving = true;
        }
    }
}
