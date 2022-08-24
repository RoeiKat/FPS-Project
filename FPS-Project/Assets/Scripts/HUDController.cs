using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Health playerHealth;
    public Gun playerGun;

    public TextMeshProUGUI ammoHUD;

    public TextMeshProUGUI healthHUD;
    public Slider healthSlider;
    public Image fill;

    void Start()
    {
        initalizeSlider();
    }

    void Update()
    {
        ammoHUD.text = (playerGun.currentBullets.ToString() + "/" + playerGun.bulletsLeft.ToString());
        healthHUD.text = playerHealth.health.ToString();
        damageTest();
        setSlider();
    }

    public void initalizeSlider()
    {
        healthSlider.value = playerHealth.health;
        healthSlider.maxValue = playerHealth.health;
    }
    public void setSlider()
    {
        healthSlider.value = playerHealth.health;
        if(playerHealth.health <= 10)
        {
            fill.color = new Color32(250, 135 ,131, 255);
        } 
        else
        {
            fill.color = new Color32(132, 250, 131, 255);
        }
    }

    public void damageTest()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            playerHealth.TakeDamage(10);
        }
    }
}
