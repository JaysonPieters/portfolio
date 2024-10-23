using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene("_MainMenu");

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("_MainScene");
    }

    public void Quit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

}

