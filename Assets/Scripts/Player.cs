using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    private Transform tr;

    private Lane currentLane;

    [SerializeField]
    [Tooltip("The speed at which the player moves sideways when switching lanes")]
    private float speed = 5f;

    [SerializeField]
    [Tooltip("The upwards force applied to the player when he jumps")]
    private float jumpForce = 5f;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        currentLane = Lane.Middle;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set to grounded when touching the ground
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset grounded status if the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Set to not grounded
        }
    }
}
