// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WeaponHolder : MonoBehaviour
// {
//     public Camera fpsCam;
//     [Header("---AimingDownSights Setting---")]
//     // zoomSpeed should be between 1~0, 1 is the fastest
//     public float zoomSpeed;
//     public float cameraZoomValue;
//     private float cameraFOV;
//     private float cameraZoomFOV;
//     private Vector3 originalPosition;
//     private Quaternion originalRotation;
//     public Vector3 aimingDownSightPosition;
//     public Quaternion aimDownSightsRotation;
//     public float adsSpeed = 3f;

//     void Start()
//     {
//         cameraFOV = fpsCam.fieldOfView;
//         cameraZoomFOV = fpsCam.fieldOfView - cameraZoomValue;
//     }

//     void Update()
//     {
//         aimDownSights();
//     }

//     private void aimDownSights()
//     {
//         if(Input.GetButton("Fire2"))
//         {
//         transform.localPosition = Vector3.Lerp(transform.localPosition, aimingDownSightPosition, adsSpeed * Time.deltaTime);
//         transform.localRotation = Quaternion.Slerp(transform.localRotation, aimDownSightsRotation, adsSpeed * Time.deltaTime);
//         fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView ,cameraZoomFOV, zoomSpeed);
//         }
//         else
//         {
//         transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, adsSpeed * Time.deltaTime);
//         transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, adsSpeed * Time.deltaTime);
//         fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, cameraFOV, zoomSpeed);
//         }
//     }
// }
