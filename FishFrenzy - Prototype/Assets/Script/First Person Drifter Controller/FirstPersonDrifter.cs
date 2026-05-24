// original by Eric Haines (Eric5h5)
// adapted by @torahhorse
// http://wiki.unity3d.com/index.php/FPSWalkerEnhanced

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonDrifter : MonoBehaviour
{
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;

    private bool limitDiagonalSpeed = true;
    public bool enableRunning = false;

    public Transform playerCamera;

    public float idleBobAmount = 0.15f;
    public float idleBobSpeed = 2.0f;

    [Header("Water Drag")]
    public float acceleration = 4f;
    public float drag = 1.2f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    private CharacterController controller;
    private Transform myTransform;
    private float speed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;

        if (enableRunning)
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        }

        bool isMoving = Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f;

        if (isMoving)
        {
            Vector3 forwardMove = playerCamera.forward * inputY;
            Vector3 sideMove = playerCamera.right * inputX;

            moveDirection = (forwardMove + sideMove).normalized * speed * inputModifyFactor;

            currentVelocity = Vector3.Lerp(
                currentVelocity,
                moveDirection,
                acceleration * Time.deltaTime
            );
        }
        else
        {
           currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero,
                drag * Time.deltaTime
            );
            float bob = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;
            currentVelocity += new Vector3(0f, bob, 0f) * Time.deltaTime;
            //moveDirection = new Vector3(0f, bob, 0f);
        }

        controller.Move(currentVelocity * Time.deltaTime);
    }
}