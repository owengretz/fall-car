using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            RoadGenerator.instance.SpawnRoad();
    }
}
