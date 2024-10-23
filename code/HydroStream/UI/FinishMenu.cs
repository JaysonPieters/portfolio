using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadScene("_MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("_MainScene");
    }
}
