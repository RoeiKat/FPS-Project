using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        if(Physics.Raycast(ray, out hitInfo, 1000f, ~playerLayer))
        {
            transform.position = hitInfo.point; 
        }
        else 
        {
            transform.localPosition = mainCamera.transform.localPosition + new Vector3 (0,0,10);
        }
    }
}
