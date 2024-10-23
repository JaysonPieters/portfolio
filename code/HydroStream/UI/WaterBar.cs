using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    private Image waterBar;
    private CarController[] cars;
    private CarController car;
    [SerializeField] int carID;

    // Start is called before the first frame update
    void Start()
    {
        waterBar = GetComponent<Image>();
        cars = FindObjectsOfType<CarController>();
        foreach(CarController Car in cars)
        {
            if(Car.carID == carID)
            {
                car = Car;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        waterBar.fillAmount = car.waterLevel / car.maxWater;
    }
}
