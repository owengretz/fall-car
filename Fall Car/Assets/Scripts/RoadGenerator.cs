using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator instance;

    public GameObject firstRoad;
    public GameObject[] roadPrefabs;

    private GameObject recentSegment;

    [HideInInspector] public Vector3 spawnPosition;

    public bool testMode;
    public GameObject testPrefab;
    public bool multiSegmentTest;
    public bool twoInARowPossible;
    public int beginningRoadsToSpawn = 3;

    private void Start()
    {
        if (testMode)
            twoInARowPossible = true;

        instance = this;

        spawnPosition = firstRoad.GetComponent<RoadChunk>().end.position;

        recentSegment = firstRoad;

        for (int i = 0; i < beginningRoadsToSpawn; i++)
        {
            SpawnRoad();
        }
    }

    public void SpawnRoad()
    {
        GameObject road;
        if (!testMode)
        {
            road = Instantiate(CalculateSegment());
        }
        else
        {
            if (multiSegmentTest)
            {
                int randomIndex = Random.Range(0, roadPrefabs.Length);

                road = Instantiate(roadPrefabs[randomIndex]);
            }
            else
            {
                road = Instantiate(testPrefab);
            }
        }

        if (road.GetComponent<RoadChunk>().flippable)
        {
            int zScale = Random.Range(0, 2);
            if (zScale == 0)
                zScale = -1;
            MeshCollider[] roadColliders = road.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider col in roadColliders)
                col.gameObject.transform.localScale = new Vector3(1, zScale, 1);
        }

        road.transform.position = spawnPosition;
        spawnPosition = road.GetComponent<RoadChunk>().end.position;
    }

    private GameObject CalculateSegment()
    {
        int score = Score.instance.score;

        int level = (score + 20) / 20;
        if (level > 5)
            level = 5;
        
        //Debug.Log(level);


        List<GameObject> chooseFrom = new List<GameObject>();

        foreach (GameObject segment in roadPrefabs)
        {
            int chance = 0;

            int difficulty = segment.GetComponent<RoadChunk>().difficulty;

            int difference = Mathf.Abs(level - difficulty);

            //Debug.Log(segment + " - difference: " + difference);

            if (difference < 5)
            {
                chance = Random.Range(1, 1 + difference);
            }

            //Debug.Log(segment + ": chance = " + chance);

            if (chance == 1)
            {
                chooseFrom.Add(segment);
            }
        }

        int randomIndex = Random.Range(0, chooseFrom.Count);
        while (!twoInARowPossible && chooseFrom[randomIndex] == recentSegment || spawnPosition.z < chooseFrom[randomIndex].GetComponent<RoadChunk>().minDistance || recentSegment.GetComponent<RoadChunk>().cantSpawnAfter.FindIndex(x => x == roadPrefabs[randomIndex]) != -1)
        {
            randomIndex = Random.Range(0, chooseFrom.Count);
        }

        recentSegment = chooseFrom[randomIndex];
        return chooseFrom[randomIndex];
    }
}
