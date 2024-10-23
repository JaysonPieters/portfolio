using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    Rigidbody body;
    public Wheel[] Wheels;
    //[SerializeField] ParticleSystem particle;

    //Car "stats"
    public float Power;
    public float MaxAngle;

    //Internal variables
    private Vector3 movement;
    public float Forward;
    private float Angle;


    private float Brake;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Forward = Input.GetAxis("Vertical");
        Angle = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            body.drag = 0.1f;
            body.mass = 900;
            body.angularDrag = 0;
            //particle.Play();
            Forward = 0.5f;
            MaxAngle = 21;
        }
        else
        {
            body.drag = 0.3f;
            body.mass = 1500;
            body.angularDrag = 0.4f;
            //particle.Pause();
            MaxAngle = 13;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 1, 0);
            transform.Rotate(0, 0, 0);
        }
        //Debug.Log(body.velocity.normalized);
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in Wheels)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                wheel.Accelerate(Forward * Power);
                wheel.Brake(0);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                wheel.Brake(2000);
                wheel.Accelerate(0);
            }

            if (!Input.GetKey(KeyCode.W))
            {
                wheel.Accelerate(0);
            }
            wheel.Turn(Angle * MaxAngle);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 direction = ((transform.position + transform.forward) - transform.position).normalized;
        Vector3 reflect = Vector3.Reflect(direction, collision.transform.forward); ;
        Debug.Log(reflect);
    }
}
