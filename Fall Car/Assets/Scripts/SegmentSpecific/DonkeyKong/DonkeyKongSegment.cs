using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonkeyKongSegment : MonoBehaviour
{
    [HideInInspector] public bool throwingBarrels;
    [HideInInspector] public bool doneThrowingBarrels;

    public GameObject barrelPrefab;
    private GameObject player;
    public GameObject barrelSpawner;
    public Transform[] barrelSpawnPoints;
    public float barrelInterval;

    private int lastSpawnIndex;

    private void Start()
    {
        player = CarMovement.instance.gameObject;
    }


    public void StartThrowing()
    {
        

        Debug.Log("starting to throw");

        throwingBarrels = true;
        StartCoroutine(ThrowBarrel());
    }

    public void StopThrowing()
    {
        

        Debug.Log("stopping throwing");

        throwingBarrels = false;
        doneThrowingBarrels = true;
    }

    private IEnumerator ThrowBarrel()
    {
        yield return new WaitForSeconds(barrelInterval);


        int spawnIndex = Random.Range(0, barrelSpawnPoints.Length);
        while (spawnIndex == lastSpawnIndex)
        {
            spawnIndex = Random.Range(0, barrelSpawnPoints.Length);
        }
        lastSpawnIndex = spawnIndex;

        Vector3 spawnPos = barrelSpawnPoints[spawnIndex].position;

        GameObject barrel = Instantiate(barrelPrefab);
        barrel.transform.position = spawnPos;

        if (throwingBarrels && !doneThrowingBarrels && !CarMovement.instance.fellOff)
            StartCoroutine(ThrowBarrel());
    }

    private void FixedUpdate()
    {
        barrelSpawner.transform.position = new Vector3(transform.position.x, player.transform.position.y + 10, player.transform.position.z + 40);
    }
}
