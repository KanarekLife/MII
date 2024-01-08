using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController_193319 : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2f;
    [Range(0.0f, 20.0f)][SerializeField] private float moveLeftRange = 10.0f;
    [Range(0.0f, 20.0f)][SerializeField] private float moveRightRange = 10.0f;

    public bool waitingForPlayer = false;

    public bool isMovingRight = false;
    private float _startPositionX;
    private bool activatedByPlayer = false;

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!isMovingRight) isMovingRight = true;
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (isMovingRight) isMovingRight = false;
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
        if (waitingForPlayer && !activatedByPlayer) {
            if(gameObject.transform.childCount > 0) {
                activatedByPlayer = true;
                Debug.Log("activ");
            }
            if(!activatedByPlayer) {
                return;
            }
        }

        if(activatedByPlayer && gameObject.transform.childCount == 0 && Mathf.Abs(_startPositionX - transform.position.x) < 0.1f) {
            activatedByPlayer = false;
            Debug.Log("inactiv");
            return;
        }

        if (isMovingRight)
        {
            if (transform.position.x < _startPositionX + moveRightRange)
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
            if (transform.position.x > _startPositionX - moveLeftRange)
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
