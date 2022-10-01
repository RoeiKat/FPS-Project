using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool disablePauseMenu = false;
    public bool gamePause = false;
    public GameObject pauseMenuUI;
    private float currentAudioSet;
    public ActiveWeapon activeWeapon;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!disablePauseMenu)
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
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePause = false;
        Gun weapon = activeWeapon.getActiveWeapon();
        if( weapon != null )
        {
            activeWeapon.canShoot = true;
        }
        AudioListener.pause = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePause = true;
        if(activeWeapon.canShoot)
        {
        activeWeapon.canShoot = false;
        }
        AudioListener.pause = true;
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
