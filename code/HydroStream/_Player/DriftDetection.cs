using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftDetection : MonoBehaviour
{
    private WheelCollider wheelCollider;

    public void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        //Debug.Log(wheelCollider.motorTorque);

        //Detect when drifting
        WheelHit hit = new WheelHit();
        if (wheelCollider.GetGroundHit(out hit))
        {
            if (hit.sidewaysSlip > .15)
                Debug.Log("drifting");
        }

        //Debug.Log(wheelCollider.rpm);
    }
}
