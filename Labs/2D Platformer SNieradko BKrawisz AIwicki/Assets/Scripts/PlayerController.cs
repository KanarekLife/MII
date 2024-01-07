using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    
    [Space(10)]
    [Range(0.01f, 100.0f)] [SerializeField] private float jumpForce = 6f;

    [FormerlySerializedAs("GroundLayer")] public LayerMask groundLayer;

    [SerializeField] public AudioClip bonusCollectedSound;
    [SerializeField] public AudioClip keyCollectedSound;
    [SerializeField] public AudioClip enemyDefeatedSound;
    [SerializeField] public AudioClip potionCollectedSound;
    [SerializeField] public AudioClip deathSound;
    
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private AudioSource _audioSource;
    
    private const float RayLength = 1.5f;
    
    private bool _isWalking;
    private bool _isFacingRight = true;
    private Vector2 _startPosition;
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    public GameObject collectionObject;
    public GameObject specialCollectionObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(1);
            other.gameObject.SetActive(false);
            Instantiate(collectionObject, other.gameObject.transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(bonusCollectedSound, AudioListener.volume);
        } else if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.instance.AddPoints(1);
                Debug.Log("Killed an enemy");
                _audioSource.PlayOneShot(enemyDefeatedSound, AudioListener.volume);
            }
            else
            {
               Death();
            }
        } else if (other.CompareTag("Key"))
        {
            GameManager.instance.AddKey();
            other.gameObject.SetActive(false);
            Instantiate(specialCollectionObject, other.gameObject.transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(keyCollectedSound, AudioListener.volume);
        } else if (other.CompareTag("EndOfStage"))
        {
            GameManager.instance.PlayerReachedEnd();
        } else if (other.CompareTag("FallLevel"))
        {
            Death();
        } else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        } else if (other.CompareTag("Potion"))
        {
            _audioSource.PlayOneShot(potionCollectedSound, AudioListener.volume);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    public void IncreaseNumberOfLives()
    {
        GameManager.instance.AddLives(1);
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
        _audioSource = GetComponent<AudioSource>();
        _startPosition = transform.position;
    }

    private bool IsGrounded()
    {
        Vector2 position1 = transform.position;
        position1.x -= 0.6f;
        Vector2 position2 = transform.position;
        position2.x += 0.6f;
        return Physics2D.Raycast(position1    , Vector2.down, RayLength, groundLayer.value) ||
               Physics2D.Raycast(position2, Vector2.down, RayLength, groundLayer.value);
    }

    public void Death()
    {
        GameManager.instance.AddLives(-1);
        _audioSource.PlayOneShot(deathSound, AudioListener.volume);
        transform.position = _startPosition;
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
        if (GameManager.instance.currentGameState != GameState.GS_GAME) return;

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
