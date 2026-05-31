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
    public bool enableRunning = false;

    [Header("Energy")]
    public EnergySystem energySystem;

    [Header("Camera Follow")]
    public Transform playerCamera;
    public float cameraFollowSpeed = 8f;
    private Vector3 cameraOffset;
    public Transform cameraPivot;

    [Header("Vertical Movement")]
    public float verticalSpeed = 5f;

    [Header("Idle Floating")]
    public float idleBobAmount = 0.15f;
    public float idleBobSpeed = 2.0f;

    [Header("Water Drag")]
    public float acceleration = 4f;
    public float drag = 1.2f;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    private CharacterController controller;
    private Vector3 currentVelocity = Vector3.zero;
    private float speed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = walkSpeed;

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        if (playerCamera != null)
            cameraOffset = playerCamera.position - transform.position;
    }

    void FixedUpdate()
    {
        float inputX = 0f;
        float inputZ = 0f;
        float inputY = 0f;

        if (Input.GetKey(KeyCode.A)) inputX = -1f;
        else if (Input.GetKey(KeyCode.D)) inputX = 1f;

        if (Input.GetKey(KeyCode.W)) inputZ = 1f;
        else if (Input.GetKey(KeyCode.S)) inputZ = -1f;

        if (Input.GetKey(KeyCode.UpArrow)) inputY = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) inputY = -1f;

        bool isTryingToRun = enableRunning && Input.GetKey(KeyCode.LeftShift);
        bool canRun = energySystem != null && energySystem.HasEnergy();

        if (isTryingToRun && canRun)
        {
            speed = runSpeed;
            energySystem.UseRunningEnergy(Time.deltaTime);
        }
        else
        {
            speed = walkSpeed;
        }

        bool isMoving =
            Mathf.Abs(inputX) > 0.1f ||
            Mathf.Abs(inputZ) > 0.1f ||
            Mathf.Abs(inputY) > 0.1f;

        Vector3 targetVelocity = Vector3.zero;

        if (isMoving)
        {
            Vector3 horizontalMove =
                (transform.forward * inputZ) +
                (transform.right * inputX);

            if (horizontalMove.magnitude > 1f)
                horizontalMove.Normalize();

            Vector3 verticalMove = Vector3.up * inputY * verticalSpeed;

            targetVelocity = (horizontalMove * speed) + verticalMove;

            currentVelocity = Vector3.Lerp(
                currentVelocity,
                targetVelocity,
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
            currentVelocity += Vector3.up * bob * Time.deltaTime;
        }

        if (currentVelocity.sqrMagnitude > 0.1f)
        {
            Vector3 flatDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

            if (flatDirection.sqrMagnitude > 0.01f)
            {
                Quaternion yawRotation = Quaternion.LookRotation(flatDirection);

                float targetPitch = (-currentVelocity.y / verticalSpeed) * 20f;
                targetPitch = Mathf.Clamp(targetPitch, -20f, 20f);

                Quaternion pitchRotation = Quaternion.Euler(targetPitch, 0f, 0f);
                Quaternion targetRotation = yawRotation * pitchRotation;

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        controller.Move(currentVelocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (playerCamera == null || cameraPivot == null) return;

        cameraPivot.position = Vector3.Lerp(
            cameraPivot.position,
            transform.position,
            cameraFollowSpeed * Time.deltaTime
        );

        Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z);

        if (flatForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatForward);

            cameraPivot.rotation = Quaternion.Slerp(
                cameraPivot.rotation,
                targetRotation,
                2f * Time.deltaTime
            );
        }
    }
}