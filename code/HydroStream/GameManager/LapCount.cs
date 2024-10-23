using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapCount : MonoBehaviour
{
    public int totalLapAmount;

    public int currentLap = 1;
    [SerializeField] private TextMeshProUGUI lapCount;
    public int carID;
    
    void Start()
    {
        lapCount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        lapCount.text = $"Lap: {currentLap}/{totalLapAmount}";
    }
}
