using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public PauseMenu pauseMenu;
    public GameObject deathMenuUI;

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pauseMenu.disablePauseMenu = true;
        deathMenuUI.SetActive(true);
    }
}
