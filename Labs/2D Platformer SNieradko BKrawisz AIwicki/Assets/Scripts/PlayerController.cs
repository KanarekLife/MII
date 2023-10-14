using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    
    [Space(10)]
    [Range(0.01f, 20.0f)] [SerializeField] private float jumpForce = 6f;

    [FormerlySerializedAs("GroundLayer")] public LayerMask groundLayer;

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    
    private const float RayLength = 1.5f;
    
    private bool _isWalking;
    private bool _isFacingRight = true;
    private int _score;
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            _score++;
            Debug.Log($"Score: {_score}");
            other.gameObject.SetActive(false);
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, RayLength, groundLayer.value);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumping");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _isWalking = false;
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _isWalking = true;
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if (!_isFacingRight) Flip();
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _isWalking = true;
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);      
            if (_isFacingRight) Flip();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        Debug.DrawRay(transform.position, RayLength * Vector3.down, Color.white);
        
        _animator.SetBool(Grounded, IsGrounded());
        _animator.SetBool(IsWalking, _isWalking);
    }
}
