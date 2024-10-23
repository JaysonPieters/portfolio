using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
    private Transform heatBar;
    private CarController[] cars;
    private CarController car;
    [SerializeField] int carID;

    // Start is called before the first frame update
    void Start()
    {
        heatBar = GetComponent<Transform>();
        cars = FindObjectsOfType<CarController>();
        foreach (CarController Car in cars)
        {
            if (Car.carID == carID)
            {
                car = Car;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        heatBar.transform.rotation = Quaternion.Euler(0, 0, -270 / car.maxHeat * car.heatLevel);
    }
}
