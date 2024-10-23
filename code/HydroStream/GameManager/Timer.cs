using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timerStart = 3;
    private float time;
    private TextMeshProUGUI timerText;
    public float minutes;
    public float seconds;
    [SerializeField] private TextMeshProUGUI countDown;
    private CarController[] cars;
    private CarController car;
    [SerializeField] int carID;

    void Start()
    {
        timerText = gameObject.GetComponent<TextMeshProUGUI>();

        cars = FindObjectsOfType<CarController>();
        foreach (CarController Car in cars)
        {
            if (Car.carID == carID)
            {
                car = Car;
            }
        }
    }

    void Update()
    {
        if (timerStart >= 0)
        {
            timerStart -= Time.deltaTime;
            timerStart = Mathf.Round(timerStart * 100f) / 100f;
            countDown.text = $"{timerStart}";
            car.canDrive = false;
        }
        else if (timerStart <= 0)
        {
            time += Time.deltaTime;
            countDown.gameObject.SetActive(false);
            car.canDrive = true;
        }
        seconds = time;
        if (time >= 60)
        {
            minutes++;
            time -= 60;
        }
        seconds = Mathf.Round(seconds * 100f) / 100f;
        timerText.text = $"{minutes}:{seconds}";
    }
}
