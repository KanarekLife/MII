using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2f;
    [Range(0.01f, 20.0f)][SerializeField] private float moveRange = 10.0f;

    private bool _isMovingRight = false;
    private float _startPositionX;

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!_isMovingRight) _isMovingRight = true;
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (_isMovingRight) _isMovingRight = false;
    }

    private void Awake()
    {
        _startPositionX = transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (_isMovingRight)
        {
            if (transform.position.x < _startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }
        else
        {
            if (transform.position.x > _startPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
    }
}
