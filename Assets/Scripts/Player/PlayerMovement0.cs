using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement0 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed=5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private CharacterController _controller;

    private Vector3 velocity;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current!=null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
        }

        Vector3 moveDirection = transform.right * input.x + transform.forward * input.y;

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (_controller.isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }
}
