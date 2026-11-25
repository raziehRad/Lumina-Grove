using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform cameraPivot;
    [SerializeField] private FloatingJoystick cameraJoystick;

    [Header("Camera Settings")] [SerializeField]
    private float moveSensitivity = 90f;

    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;
    private float sensitivityMultiplier = 1f;
    private float pitch;
    private bool isEnding;

    private void OnEnable()
    {
        GameEvents.OnStopCameraControl += StopCameraControl;
    }

    private void OnDisable()
    {
        GameEvents.OnStopCameraControl -= StopCameraControl;
    }

    private void Awake()
    {
        if (cameraPivot == null || cameraJoystick == null)
        {
            Debug.LogError("Camera references are missing!", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (isEnding)
            return;

        Look();
    }

    private void Look()
    {
        Vector2 input = cameraJoystick.InputDirection;

        float currentSensitivity = moveSensitivity * sensitivityMultiplier;

        float mouseX = input.x * currentSensitivity * Time.deltaTime;
        float mouseY = input.y * currentSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    public void StopCameraControl()
    {
        isEnding = true;
    }
    public void SetSensitivity(float value)
    {
        sensitivityMultiplier = Mathf.Clamp(value, 0.5f, 2f);
    }
}