using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)][SerializeField] private float moveRange = 10.0f;

    private bool _isFacingRight = false;
    private bool _isMovingRight = false;
    private float _startPositionX;
    private Animator _animator;
    private static readonly int IsDead = Animator.StringToHash("isDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _startPositionX = transform.position.x;
    }
    
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!_isFacingRight) Flip();
        if (!_isMovingRight) _isMovingRight = true;
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);      
        if (_isFacingRight) Flip();
        if (_isMovingRight) _isMovingRight = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
        }else if (other.CompareTag("Player"))
        {
            if (transform.position.y < other.gameObject.transform.position.y)
            {
                _animator.SetBool(IsDead, true);
                StartCoroutine(KillOnAnimationEnd());
                GameManager.instance.AddDefeatedEnemy(1);
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
