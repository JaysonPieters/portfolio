using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool IsPowered;
    public bool CanTurn;

    //public GameObject Visuals;

    private WheelCollider wheelCollider;

    public void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        Vector3 Position;
        Quaternion Rotation;
        wheelCollider.GetWorldPose(out Position,out Rotation);
        /*Visuals.transform.position = Position;
        Visuals.transform.rotation = Rotation;*/
        Debug.Log(wheelCollider.motorTorque);

        //Detect when drifting
        WheelHit hit = new WheelHit();
        if (wheelCollider.GetGroundHit(out hit))
        {
            if (hit.sidewaysSlip > .15)
                Debug.Log("drifting");
        }

        Debug.Log(wheelCollider.rpm);
    }

    public void Accelerate(float power)
    {
        if (IsPowered)
        {
            wheelCollider.motorTorque = power;
        }
    }

    public void Turn(float Angle)
    {
        if (CanTurn)
        {
            wheelCollider.steerAngle = Angle;
        }
    }

    public void Brake(float power)
    {
        wheelCollider.brakeTorque = power;
    }
}