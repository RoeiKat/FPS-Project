using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public GameObject deathMenuUI;

    public void playAgain()
    {
        SceneManager.LoadScene("MainScene");
        pauseMenu.disablePauseMenu = false;
        deathMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
