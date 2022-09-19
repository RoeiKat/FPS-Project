using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePause = false;
    public AudioMixer audioMixer;
    public GameObject pauseMenuUI;
    private float currentAudioSet;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePause)
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
        audioMixer.SetFloat("masterVolume", currentAudioSet);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePause = false;
    }

    void Pause()
    {
        audioMixer.GetFloat("masterVolume", out float value);
        currentAudioSet = value;
        audioMixer.SetFloat("masterVolume", -80);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePause = true;
    }

    public void loadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
