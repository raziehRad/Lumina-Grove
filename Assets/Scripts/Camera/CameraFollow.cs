using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform cameraPivot;
    [SerializeField] private FloatingJoystick cameraJoystick;

    [Header("Camera Settings")] [SerializeField]
    private float moveSensitivity = 150f;

    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    private float pitch;
    private bool isEnding;
    private void LateUpdate()
    {
        if (isEnding || cameraPivot == null || cameraJoystick == null)
            return;

        Look();
    }

    private void Look()
    {
        Vector2 input = cameraJoystick.InputDirection;

        float mouseX = input.x * moveSensitivity * Time.deltaTime;
        float mouseY = input.y * moveSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    public void StopCameraControl()
    {
        isEnding = true;
    }
}