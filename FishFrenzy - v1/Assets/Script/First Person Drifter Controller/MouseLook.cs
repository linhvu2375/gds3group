// original by asteins
// adapted by @torahhorse
// http://wiki.unity3d.com/index.php/SmoothMouseLook

// Instructions:
// There should be one MouseLook script on the Player itself, and another on the camera
// player's MouseLook should use MouseX, camera's MouseLook should use MouseY

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseLook : MonoBehaviour
{

    public float sensitivityX = 10f;
    public float sensitivityY = 9f;

    public float minY = -85f;
    public float maxY = 85f;

    public float smoothSpeed = 5f;

    //float rotationY = 0f;
    float currentYaw;
    float currentPitch;

    float targetYaw;
    float targetPitch;

    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        currentYaw = transform.eulerAngles.y;
        currentPitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // Left-right (yaw)
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;

        // Up-down (pitch)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        targetYaw += mouseX;
        targetPitch -= mouseY;

        targetPitch = Mathf.Clamp(targetPitch, minY, maxY);

        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * smoothSpeed);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * smoothSpeed);

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        //// Rotate fish horizontally
        //transform.Rotate(0f, mouseX, 0f);

        //// Accumulate vertical rotation
        //rotationY -= mouseY;
        //rotationY = Mathf.Clamp(rotationY, minY, maxY);

        //// Apply vertical rotation
        //Vector3 currentEuler = transform.localEulerAngles;
        //transform.localEulerAngles = new Vector3(rotationY, currentEuler.y, 0f);
    }
}