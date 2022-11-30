using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;

    public Transform player;
    private Vector3 startPos;

    private void Start()
    {
        instance = this;

        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x + startPos.x, player.position.y + startPos.y, player.position.z + startPos.z);
    }

    //public IEnumerator CameraShake(float duration, float magnitude)
    //{

    //    Vector3 originalPos = transform.position;

    //    float elapsed = 0f;

    //    while (elapsed < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, 0) + originalPos;

    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    transform.localPosition = originalPos;
    //}
}
