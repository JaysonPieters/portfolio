using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float startFOV;
    float multiplier = 50f;
    int index = 0;
    int sampleCount = 100;
    float[] velocities;
    Vector3 lastPosition;

    [SerializeField] private Transform Player;
    [SerializeField] private float smoothtime = 0.25f;

    Vector3 offset;
    Vector3 velocity = Vector3.zero;

    private CarController[] cars;
    private GameObject car;
    private Camera cam;
    [SerializeField] int carID;


    void Start()
    {
        cars = FindObjectsOfType<CarController>();
        foreach (CarController Car in cars)
        {
            if (Car.carID == carID)
            {
                cam = GetComponentInChildren<Camera>();
            }
        }
        startFOV = cam.fieldOfView;
        velocities = new float[sampleCount];
        lastPosition = transform.position;
        offset = transform.localToWorldMatrix.GetPosition();
    }


    void Update()
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector3 pos = transform.position;

            //Calculate average velocity
            float averageVelocity = 0;
            velocities[index] = (lastPosition - pos).magnitude;
            foreach (float vel in velocities)
            {
                averageVelocity += vel;
            }
            averageVelocity /= sampleCount;
            index = (index + 1) % sampleCount;


            lastPosition = pos;

            cam.fieldOfView = startFOV + multiplier * averageVelocity;
        }
    }
}
