using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private bool activeOrNot;
    public Light flashlight;

    void Start()
    {
        flashlight.GetComponent<Light>();
        activeOrNot = flashlight.enabled;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            activeOrNot = !activeOrNot;
            flashlight.enabled = activeOrNot;
        }
    }
}
