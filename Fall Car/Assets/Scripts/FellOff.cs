using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public class FellOff : MonoBehaviour
{
    public static FellOff instance;

    public GameObject player;
    public CarMovement playerScript;
    private Vector3 lastPlayerPos;

    public GameObject fracturedCarPrefab;
    public GameObject explosionPrefab;

    private Rigidbody[] pieces;

    [HideInInspector] public bool exploded;

    private Vector3 posLastFrame;

    private bool checkingMovement;
    private float difference;

    public Slider slider;
    public Animator sliderAnim;

    public Animator UIanim;

    private void Start()
    {
        instance = this;
        lastPlayerPos = player.transform.position;
        posLastFrame = player.transform.position;
        difference = 0f;
        //StartCoroutine(MovementCheck());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Die();
        }
    }

    public void Die()
    {
        playerScript.fellOff = true;

        GameObject fracturedCar = Instantiate(fracturedCarPrefab);
        fracturedCar.transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);


        pieces = fracturedCar.GetComponentsInChildren<Rigidbody>();

        StartCoroutine(Explode(fracturedCar.transform.position, player.GetComponent<Rigidbody>().velocity));

        sliderAnim.enabled = false;
        UIanim.SetTrigger("dead");
    }

    private IEnumerator Explode(Vector3 pos, Vector3 vel)
    {
        if (!exploded)
        {
            exploded = true;

            player.gameObject.SetActive(false);

            CameraShaker.Instance.ShakeOnce(5f, 3f, 0f, 5f);

            yield return new WaitForSeconds(0.02f);

            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = pos;

            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].velocity = new Vector3(vel.x, 0, vel.z) / 3f;
                pieces[i].AddExplosionForce(15f, pos, 6f, 5f, ForceMode.Impulse);
            }

            Score.instance.SetHighScore();
        }
    }

    private IEnumerator MovementCheck()
    {
        float timer = 0f;
        Vector3 startPos = posLastFrame;
        float distance = 0f;
        sliderAnim.SetBool("fill", true);
        while (timer < 2f && distance < 5f)
        {
            distance = Mathf.Abs(Vector3.Magnitude(startPos - posLastFrame));
            if (difference > 0.004f)
            {
                distance = 6f;
            }


            slider.value = timer;

            timer += Time.deltaTime;
            yield return null;
        }

        //Vector3 currentPlayerPos = player.transform.position;
        //float difference = Mathf.Abs(Vector3.Magnitude(currentPlayerPos - lastPlayerPos));

        //Debug.Log(difference);

        if (distance < 5f)
        {
            Die();
        }
        else
        {
            slider.value = 0f;
            sliderAnim.SetBool("fill", false);
        }
        checkingMovement = false;


        //lastPlayerPos = player.transform.position;

        //if (!playerScript.fellOff)
        //    StartCoroutine(MovementCheck());
    }


    private void FixedUpdate()
    {
        if (Input.GetKey("r"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (playerScript.fellOff)
            return;

        Vector3 posThisFrame = player.transform.position;
        difference = Mathf.Abs(Vector3.Magnitude(posThisFrame - posLastFrame)) * Time.fixedDeltaTime;
        
        if (difference < 0.001f && !checkingMovement)
        {
            checkingMovement = true;
            StartCoroutine(MovementCheck());
        }

        posLastFrame = posThisFrame;
    }
}
