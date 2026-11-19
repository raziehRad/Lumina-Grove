using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("look Settings")] 
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float minLookAngle = -80f;
    [SerializeField] private float maxLookAngle = 80f;

    private float verticalRotation;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
       // Cursor.visible = false;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        if (Mouse.current==null)return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseY = mouseDelta.y * mouseSensitivity;
        float mouseX = mouseDelta.x * mouseSensitivity;
        
        transform.Rotate(Vector3.up*mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minLookAngle, maxLookAngle);
        cameraTransform.localRotation=Quaternion.Euler(verticalRotation,0,0);
    }
}
