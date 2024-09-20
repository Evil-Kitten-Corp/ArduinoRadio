using UnityEngine;
using UnityEngine.UI;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")] 
    public ArduinoController ac;

    public BackgroundScroller bgScroller;
    public float moveAccel;
    public float maxSpeed;
    public float fastForwardMult = 2;

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

    private float _camOffset;
    
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        if (ac != null)
        {
            ac.FastForwardChanged += delegate
            {
                if (ac.FastForwardPlayer)
                {
                    moveAccel *= fastForwardMult;
                    maxSpeed *= fastForwardMult;
                }
                else
                {
                    moveAccel /= fastForwardMult;
                    maxSpeed /= fastForwardMult;
                }
            };
            
            ac.RewindChanged += delegate
            {
                if (ac.RewindPlayer)
                {
                    moveAccel = -moveAccel;
                    gameCamera.horizontalOffset = -gameCamera.horizontalOffset;
                }
                else
                {
                    moveAccel = Mathf.Abs(moveAccel);
                    gameCamera.horizontalOffset = Mathf.Abs(gameCamera.horizontalOffset);
                }
            };
        }
    }

    void Update()
    {
        if (ac != null && ac.movePlayer)
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
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            if (_isOnGround)
            {
                _isJumping = true;
                sound.PlayJump();
            }
        }

        _anim.SetBool("isOnGround", _isOnGround);
        
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
        }*/

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
        if (ac == null)
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
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
