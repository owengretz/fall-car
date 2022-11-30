using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChunk : MonoBehaviour
{
    [Range(1, 5)]
    public int difficulty;
    public Transform start;
    public Transform end;
    public bool flippable;
    public float minDistance;
    public List<GameObject> cantSpawnAfter = new List<GameObject>();
}
