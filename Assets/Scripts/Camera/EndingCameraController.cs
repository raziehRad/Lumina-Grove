using System.Collections;
using UnityEngine;

public class EndingCameraController : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform endingCameraPoint;

    [Header("Camera Controls")]
   // [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerLook playerLook;

    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 2.5f;

    private bool isMoving;

    public IEnumerator MoveToEndingPosition()
    {
        if (isMoving)
            yield break;

        if (cameraTransform == null || endingCameraPoint == null)
        {
            Debug.LogError("Camera references are missing!", this);
            yield break;
        }

        isMoving = true;

       // if (cameraFollow != null)
            GameEvents.RaiseStopCameraControl();
           // cameraFollow.StopCameraControl();

        if (playerLook != null)
            playerLook.enabled = false;

        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        Vector3 targetPosition = endingCameraPoint.position;
        Quaternion targetRotation = endingCameraPoint.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            cameraTransform.position =
                Vector3.Lerp(startPosition, targetPosition, t);

            cameraTransform.rotation =
                Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;

        isMoving = false;
    }
}