using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternRecoil : MonoBehaviour
{   
    private MouseLook camControl;
    private CameraShake cameraShake;
    
    public Vector2[] recoilPattern;

    float verticalRecoil;
    float horizontalRecoil;

    public float duration;
    
    float time;
    int index;

    public float shakeDuration = 0.05f;
    public float shakeMagnitude = 0.4f;

    void Start()
    {
        cameraShake = gameObject.GetComponent<CameraShake>();
        camControl = gameObject.GetComponent<MouseLook>();
    }

    void Update()
    {
        if(time > 0)
        {
        camControl.xRotation -= (verticalRecoil * Time.deltaTime) / duration;
        // camControl.mouseX += (horizontalRecoil * Time.deltaTime) / duration;
        camControl.playerBody.Rotate(0f, camControl.mouseX + ((horizontalRecoil * Time.deltaTime) / duration), 0f);
        time -= Time.deltaTime;
        }
    }

    public void startRecoil()
    {
        time = duration;
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = nextIndex(index);
    }
    
    int nextIndex(int index)
    {
        return (index + 1) % recoilPattern.Length;
    }

    public void resetIndex()
    {
        index = 0;
    }
}
