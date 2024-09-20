using UnityEngine;
using UnityEngine.UI;

public class gg : MonoBehaviour
{
    [Header("Movement")] 
    public ArduinoController ac;

    public BackgroundScroller bgScroller;
    public float moveAccel;
    public float maxSpeed;
    public float fastForwardMult = 2;

    private bool _isPaused;
    private bool _isRewinding;
    
    private bool _isOnPlatform;
    private bool _isOnGround;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    [Header("Coin")]
    public Text coinText;
    public int coinValue;

    [Header("Spawn")]
    public SpawnScript sp;
    
    private float _lastPositionX;
    private Rigidbody2D _rig;
    private Animator _anim;
    
    private float originalMoveAccel;
    private float originalMaxSpeed;

    private float _camOffset;
    
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        
        originalMoveAccel = moveAccel;
        originalMaxSpeed = maxSpeed;

        if (ac != null)
        {
            ac.FastForwardChanged += delegate
            {
                if (ac.FastForwardPlayer)
                {
                    moveAccel = originalMoveAccel * fastForwardMult;
                    maxSpeed = originalMaxSpeed * fastForwardMult;
                }
                else
                {
                    moveAccel = originalMoveAccel;
                    maxSpeed = originalMaxSpeed;
                }
            };
            
            ac.RewindChanged += delegate
            {
                _isRewinding = ac.RewindPlayer;
            };
        }
    }

    void Update()
    {
        if (ac != null)
        {
            if (ac.movePlayer && !_isPaused)
            {
                MovePlayer();
            }
            else if (!ac.movePlayer && !_isPaused)
            {
                StopPlayer();
            }

            if (_isPaused)
            {
                StopPlayer();
            }
        }

        /*if (ac != null && ac.movePlayer)
        {
            _anim.SetBool("isMoving", true);
            
            // calculate score
            int distancePassed = Mathf.FloorToInt(transform.position.x - _lastPositionX);
            int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);
            
            if (scoreIncrement > 0)
            {
                score.IncreaseCurrentScore(scoreIncrement);
                _lastPositionX += distancePassed;
            }

            // game over
            if (transform.position.y < fallPositionY)
            {
                GameOver();
            }

            _camOffset = gameCamera.horizontalOffset;
        }
        else if (ac != null && !ac.movePlayer)
        {
            _anim.SetBool("isMoving", false);
            gameCamera.horizontalOffset = 0;
            bgScroller.speed = 0;
        }
        else if (ac == null)
        {
            _anim.SetBool("isOnGround", true);
            
            if (Input.GetKey(KeyCode.Space))
            {
                if (_isOnGround)
                {
                    _anim.SetBool("isOnGround", false);
                    _isOnPlatform = true;
                }
            }
            
            // calculate score
            int distancePassed = Mathf.FloorToInt(transform.position.x - _lastPositionX);
            int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);
            
            if (scoreIncrement > 0)
            {
                score.IncreaseCurrentScore(scoreIncrement);
                _lastPositionX += distancePassed;
            }

            // game over
            if (transform.position.y < fallPositionY)
            {
                GameOver();
            }
        }*/

    }
    
    private void MovePlayer()
    {
        _anim.SetBool("isMoving", true);

        // Calculate score based on distance passed
        int distancePassed = Mathf.FloorToInt(transform.position.x - _lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            _lastPositionX += distancePassed;
        }

        // Move player forward or backward
        Vector2 velocityVector = _rig.velocity;
        float accel = _isRewinding ? -moveAccel : moveAccel;  // Invert acceleration if rewinding
        velocityVector.x = Mathf.Clamp(velocityVector.x + accel * Time.deltaTime, -maxSpeed, maxSpeed);
        _rig.velocity = velocityVector;

        // Background and camera scrolling
        bgScroller.speed = Mathf.Abs(velocityVector.x) / maxSpeed;  // Adjust background speed
        gameCamera.horizontalOffset = Mathf.Sign(velocityVector.x) * Mathf.Abs(gameCamera.horizontalOffset);
    }

    private void StopPlayer()
    {
        _anim.SetBool("isMoving", false);
        _rig.velocity = Vector2.zero;
        bgScroller.speed = 0;
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameOver();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            coinValue++;
            coinText.text = coinValue.ToString();
            Destroy(other.gameObject);
        }
    }

    private void GameOver()
    {
        // set high score
        score.FinishScoring();

        // stop camera movement
        gameCamera.enabled = false;
        bgScroller.speed = 0;

        // show game over
        gameOverScreen.SetActive(true);

        // disable this too
        enabled = false;
        gameObject.SetActive(false);
        sp.enabled = false;
    }

    private void FixedUpdate() 
    {
        /*if (ac == null)
        {
            // raycast ground
            RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                Vector2.down, groundRaycastDistance, groundLayerMask);
        
            if (hit)
            {
                if (!_isOnGround && _rig.velocity.y <= 0)
                {
                    _isOnGround = true;
                }
            }
            else
            {
                _isOnGround = false;
            }

            Vector2 velocityVector = _rig.velocity;

            if (_isOnPlatform)
            {
                velocityVector.y += 5f;
                _isOnPlatform = false;
            }

            velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

            _rig.velocity = velocityVector;
        }
        else if (ac != null && ac.movePlayer)
        {
            Vector2 velocityVector = _rig.velocity;
            velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
            _rig.velocity = velocityVector;
        }*/
        
        if (ac != null && ac.movePlayer)
        {
            Vector2 velocityVector = _rig.velocity;
            velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
            _rig.velocity = velocityVector;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
