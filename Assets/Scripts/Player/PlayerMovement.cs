using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatingJoystick joystick;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;
    
    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = joystick.InputDirection;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDirection =
            forward * input.y +
            right * input.x;
        
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.fixedDeltaTime;

        Vector3 movement = moveDirection * moveSpeed;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.fixedDeltaTime);
    }
}