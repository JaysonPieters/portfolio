using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeLoad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalTime;
    public float savedMinutes;
    public float savedSeconds;

    private void Start()
    {
        finalTime = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        LoadTime();
        finalTime.text = $"{savedMinutes}:{savedSeconds}";
    }

    
    private void LoadTime()
    {
        savedMinutes = PlayerPrefs.GetFloat("Minutes");
        savedSeconds = PlayerPrefs.GetFloat("Seconds");
    }
}
