using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    private LapCount lapCount;
    private CheckpointRegistor checkpoint;
    private Timer time;

    private LapCount[] cars;
    [SerializeField] int carID;

    private void Start()
    {
        lapCount = GameObject.FindObjectOfType<LapCount>();
        time = GameObject.FindObjectOfType<Timer>();

        cars = FindObjectsOfType<LapCount>();
        foreach (LapCount Car in cars)
        {
            if (Car.carID == carID)
            {
                lapCount = Car;
            }
        }
    }

    private void Update()
    {

        if (lapCount.currentLap > lapCount.totalLapAmount)
        {
            SaveTime();
            SceneManager.LoadScene("_WinScreen");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        checkpoint = GetComponent<CheckpointRegistor>();
        try
        {
            if (checkpoint.currentCheckpoint >= 7 && other.gameObject.CompareTag("finish"))
            {
                lapCount.currentLap++;
                checkpoint.currentCheckpoint = 0;
            }
        }
        catch (System.Exception)
        {

        }

    }
    private void SaveTime()
    {
        PlayerPrefs.SetFloat("Minutes", time.minutes);
        PlayerPrefs.SetFloat("Seconds", time.seconds);
        PlayerPrefs.Save();
    }
}
