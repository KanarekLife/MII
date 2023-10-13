using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    
    [Space(10)]
    [Range(0.01f, 20.0f)] [SerializeField] private float jumpForce = 6f;

    private Rigidbody2D rigidBody;

    public LayerMask GroundLayer;

    private const float rayLength = 1.5f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, GroundLayer.value);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumping");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);            
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);            
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white);
    }
}
