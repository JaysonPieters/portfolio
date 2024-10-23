using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    //DEBUG
    private Vector3 spawn;


    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] public bool canDrive = true;
    [SerializeField] public int carID;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private WheelCollider engineWheel;

    //Lights
    [SerializeField] private GameObject leftBackLight, rightBackLight;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform engineWheelTransform;

    //Car Body
    [SerializeField] Transform carBody;

    //Effects
    [SerializeField] GameObject speedVFX;
    [SerializeField] AudioSource boostSFX;
    [SerializeField] AudioSource engineSFX;
    [SerializeField] AudioSource overheatSFX;
    [SerializeField] AudioSource hornSFX;

    //Stats
    [SerializeField] public float waterLevel;
    [SerializeField] public float maxWater = 100;

    [SerializeField] public float heatLevel;
    [SerializeField] public float maxHeat = 100;

    [SerializeField] float heatGain = 1;

    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float boostTime = 5;
    [SerializeField] float heatToWaterRatio = 2;

    [SerializeField] bool overheated = false;
    float baseSpeed;

    bool boosting = false;

    [SerializeField] bool inOil;
    [SerializeField] float oilTime = 5;
    [SerializeField] float currentOilTime;

    [SerializeField] bool inSpike;
    [SerializeField] float spikeTime = 5;
    [SerializeField] float currentSpikeTime;

    //Pole collision shit
    public float radius = 5.0F;
    public float power = 10.0F;

    int index = 0;
    int sampleCount = 100;
    float[] velocities;
    Vector3 lastPosition;
    float avarageVelocity;

    private void Start()
    {
        baseSpeed = motorForce;
        velocities = new float[sampleCount];
        lastPosition = transform.position;
        spawn = transform.position;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        //Calculate average velocity
        float AverageVelocity = 0;
        velocities[index] = (lastPosition - pos).magnitude;
        foreach (float vel in velocities)
        {
            AverageVelocity += vel;
        }
        AverageVelocity /= sampleCount;
        index = (index + 1) % sampleCount;


        lastPosition = pos;
        avarageVelocity = AverageVelocity;

    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (!overheated)
        {
            if(canDrive) verticalInput = Input.GetAxis("Vertical");
        }

        isBreaking = Input.GetKey(KeyCode.Space);

        //Horn
        if (Input.GetKeyDown(KeyCode.E))
        {
            hornSFX.Play();
        }

        //Heat Gain
        if (verticalInput != 0)
        {
            heatLevel += heatGain * Time.deltaTime;
            if(!engineSFX.isPlaying) engineSFX.Play();
        }

        //Boosting
        if (Input.GetMouseButton(1) && waterLevel > 0 && heatLevel > 0 && canDrive)
        {
            if (!boosting)
            {
                boosting = true;
                motorForce *= speedMultiplier;
            }
            waterLevel = waterLevel - Time.deltaTime * (100 / boostTime);
            heatLevel = heatLevel - Time.deltaTime * (100 / boostTime) / heatToWaterRatio;
            verticalInput = 1;

            speedVFX.SetActive(true);
            if (!boostSFX.isPlaying) boostSFX.Play();

        }
        if (!Input.GetMouseButton(1) || waterLevel <= 0 || heatLevel <= 0)
        {
            motorForce = baseSpeed;
            boosting = false;
            speedVFX.SetActive(false);
        }

        //Overheat
        if (heatLevel >= 100)
        {
            overheated = true;
            if (!overheatSFX.isPlaying) overheatSFX.Play();
        }
        if (overheated)
        {
            heatLevel -= heatGain * Time.deltaTime * 25;
            motorForce = 0;
        }
        if (heatLevel <= 5)
        {
            overheated = false;
        }

        //drifting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            maxSteerAngle = 45;

            WheelFrictionCurve frictionCurve = frontRightWheelCollider.sidewaysFriction;
            frictionCurve.stiffness = 1f;
            frontRightWheelCollider.sidewaysFriction = frictionCurve;
            frontLeftWheelCollider.sidewaysFriction = frictionCurve;
            rearLeftWheelCollider.sidewaysFriction = frictionCurve;
            rearRightWheelCollider.sidewaysFriction = frictionCurve;
        }
        else
        {
            WheelFrictionCurve frictionCurve = frontRightWheelCollider.sidewaysFriction;
            frictionCurve.stiffness = 2.6f;
            frontRightWheelCollider.sidewaysFriction = frictionCurve;
            frontLeftWheelCollider.sidewaysFriction = frictionCurve;
            rearLeftWheelCollider.sidewaysFriction = frictionCurve;
            rearRightWheelCollider.sidewaysFriction = frictionCurve;

            maxSteerAngle = 15;
        }

/*        if(currentOilTime >= 0)
        {
            inOil = true;
            currentOilTime -= Time.deltaTime;
            WheelFrictionCurve frictionCurve = frontRightWheelCollider.forwardFriction;
            frictionCurve.stiffness = 1f;
            frontRightWheelCollider.forwardFriction = frictionCurve;
            frontLeftWheelCollider.forwardFriction = frictionCurve;
            rearLeftWheelCollider.forwardFriction = frictionCurve;
            rearRightWheelCollider.forwardFriction = frictionCurve;
        }
        else
        {
            inOil = false;
            WheelFrictionCurve frictionCurve = frontRightWheelCollider.forwardFriction;
            frictionCurve.stiffness = 8f;
            frontRightWheelCollider.forwardFriction = frictionCurve;
            frontLeftWheelCollider.forwardFriction = frictionCurve;
            rearLeftWheelCollider.forwardFriction = frictionCurve;
            rearRightWheelCollider.forwardFriction = frictionCurve;
        }

        if (currentSpikeTime >= 0)
        {
            inSpike = true;
            currentSpikeTime -= Time.deltaTime;
        }
        else
        {
            inSpike = false;
        }*/




        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        engineWheel.motorTorque = verticalInput * motorForce / 100;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        engineWheel.brakeTorque = currentbreakForce;
        if (isBreaking || verticalInput < 0)
        {
            leftBackLight.SetActive(true);
            rightBackLight.SetActive(true);
        }
        else
        {
            leftBackLight.SetActive(false);
            rightBackLight.SetActive(false);
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(engineWheel, engineWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterPickup"))
        {
            waterLevel = 100;
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y - 100, other.gameObject.transform.position.z);
        }

        if (other.CompareTag("oil"))
        {
            currentOilTime = oilTime;
        }

        if (other.CompareTag("spike"))
        {
            currentSpikeTime = spikeTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            try
            {
                if (hit.CompareTag("pole"))
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(power * avarageVelocity, explosionPos, radius, 3.0F);
                        rb.useGravity = true;

                        collision.rigidbody.excludeLayers = 1 << 3;
                    }
                }
            }
            catch(System.Exception e)
            {

            }
            
        }
    }
}
