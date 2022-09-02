using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float xShakeRange = 1f;
    public float yShakeRange = 1f;

    public IEnumerator Shake(float dur, float mag) 
    {
        Vector3 originalPosition = transform.localPosition;
        
        float elapsed = 0.0f;

        while (elapsed < dur)
        {
            float x = Random.Range(-xShakeRange, xShakeRange) * mag;
            float y = Random.Range(-yShakeRange, yShakeRange) * mag;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
