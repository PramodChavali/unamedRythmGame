using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    // InputAction move = InputSystem.actions.FindAction("Player/Move");

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private InputActionReference move;
    [SerializeField] Rigidbody2D rb;
    Vector2 movementDirection;

    void Start()
    {
        
    }

    void Update()
    {
        movementDirection = move.action.ReadValue<Vector2>();
        Debug.Log(movementDirection);
        movePlayer();
    }

    void movePlayer()
    {
        rb.linearVelocity = movementDirection * movementSpeed;
    }
}
