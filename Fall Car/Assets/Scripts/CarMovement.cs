using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{

    public static CarMovement instance;

    [HideInInspector] public bool fellOff;

    public GameObject centerOfMass;

    public float maxSpeed;
    public float moveSpeed;
    public float turnSpeed;

    public Vector3 direction;


    private float turnInputValue;
    private string turnInputName;


    public Rigidbody rb;
    private float distToGround;
    public Collider bodyCollider;

    //public WheelCollider wheelFL, wheelFR, wheelBL, wheelBR;
    public WheelCollider[] wheels;

    public bool canBoost;
    private bool boosting;
    public Slider boostBar;
    public Animator boostBarAnim;
    private bool barShaking;

    public ParticleSystem[] trails;
    //public Collider wheelFL;
    //public Collider wheelFR;
    //bool velSet = false;


    //private void OnCollisionEnter(Collision collision)
    //{
    //    fellOff = true;

    //    GameObject fracturedCar = Instantiate(fracturedCarPrefab);
    //    fracturedCar.transform.SetPositionAndRotation(transform.position, transform.rotation);

    //    gameObject.SetActive(false);
    //}

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();

        //moveAxisName = "Vertical";
        turnInputName = "Horizontal";

        distToGround = bodyCollider.bounds.extents.y;

        rb.centerOfMass = centerOfMass.transform.position;
        //rb.centerOfMass = new Vector3(0, -0.15f, 0);
        foreach (ParticleSystem trail in trails)
            trail.Pause();
    }


    private void Update()
    {
        //moveAxisValue = Input.GetAxis(moveAxisName);
        turnInputValue = Input.GetAxis(turnInputName);

        if (boostBar.value < boostBar.maxValue)
        {
            if (!boosting)
                boostBar.value += Time.deltaTime;
            if (canBoost == true)
            {
                canBoost = false;
                boostBarAnim.SetBool("full", false);
            }
        }
        else if (canBoost == false)
        {
            canBoost = true;
            boostBarAnim.SetBool("full", true);
        }


        if (Input.GetButtonDown("Jump"))
        {
            if (canBoost)
                StartCoroutine(Boost());
            else if (!barShaking && !boosting)
                StartCoroutine(BoostBarShake());
        }
    }

    private IEnumerator Boost()
    {
        boosting = true;
        foreach (ParticleSystem trail in trails)
            trail.Play();
        float timer = 3f;
        while (timer > 1f)
        {
            maxSpeed = 20;

            timer -= Time.deltaTime;
            boostBar.value -= ((Time.deltaTime * boostBar.maxValue) / 3f) * (1f / 5f + 1f);
            yield return null;
        }
        while (timer > 0f)
        {
            maxSpeed = (timer + 1f) * 10f;

            timer -= Time.deltaTime;
            boostBar.value -= ((Time.deltaTime * boostBar.maxValue) / 3f) * (0.6f);
            yield return null;
        }
        boostBarAnim.SetTrigger("done boosting");
        maxSpeed = 10f;
        boosting = false;
        foreach (ParticleSystem trail in trails)
            trail.Pause();
    }

    private IEnumerator BoostBarShake()
    {
        barShaking = true;
        Vector2 originalPos = boostBar.transform.position;
        float timer = 0.5f;
        float timerAmount = timer;
        float shakeAmount = 300f;
        while (timer > timerAmount * 4f / 5f)
        {
            boostBar.transform.Translate(new Vector2(shakeAmount/2, 0) * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        while (timer > timerAmount * 3f / 5f)
        {
            boostBar.transform.Translate(new Vector2(-shakeAmount, 0) * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        while (timer > timerAmount * 2f / 5f)
        {
            boostBar.transform.Translate(new Vector2(shakeAmount, 0) * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        while (timer > timerAmount * 1f / 5f)
        {
            boostBar.transform.Translate(new Vector2(-shakeAmount, 0) * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        while (timer > 0f)
        {
            boostBar.transform.Translate(new Vector2(shakeAmount/2, 0) * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        boostBar.transform.position = originalPos;
        barShaking = false;
    }

    private void FixedUpdate()
    {
        if (!fellOff)
        {
            Turn();
            Move();
        }
        else
        {
            //rb.velocity = Vector3.zero;
        }
        
    }

    private bool IsGrounded() 
    {
        Debug.Log(Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f));
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void Turn()
    {
        float turn = turnInputValue * turnSpeed /** Time.deltaTime*/;

        Vector3 rotationVector = new Vector3(0, turn, 0);

        direction = transform.forward;
        //if (IsGrounded())
        //{
        //    direction = transform.forward;
        //}
        //else if (!velSet)
        //{
        //    rb.velocity = transform.forward * moveSpeed * Time.deltaTime;
        //    velSet = true;
        //}
        foreach (WheelCollider wheel in wheels)
        {
            wheel.steerAngle = turn;
        }
        //transform.Rotate(rotationVector);
    }

    private void Move()
    {
        //rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        //rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        //rb.position += /*Vector3.forward*/direction * moveSpeed * Time.deltaTime;
        //Debug.Log(rb.velocity.magnitude);
        if (rb.velocity.sqrMagnitude < maxSpeed)
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = moveSpeed /** Time.deltaTime*/;
                
            }
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void OnDisable()
    {
        if (boostBarAnim != null)
            boostBarAnim.SetTrigger("dead");
    }
}
