using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("_MainScene");
    }

    public void CreditButton()
    {
        SceneManager.LoadScene("_CreditsScene");
    }

    public void MurderButton()
    {
        Application.Quit();
    }
}
