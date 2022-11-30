using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpikeBall : MonoBehaviour
{
    public Rigidbody spikeBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RandomDelay());
        }
    }

    private IEnumerator RandomDelay()
    {
        float delay = Random.Range(0f, 1.3f);

        yield return new WaitForSeconds(delay);

        spikeBall.isKinematic = false;
    }
}
