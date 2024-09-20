using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCharacterController : MonoBehaviour
{
    public Animator anim;
    public BackgroundScroller bgScroller;
    public float normalSpeed = 5f;
    public float fastForwardSpeed = 10f;
    public float rewindSpeed = 5f;
    public bool isPaused = true;
    public bool isFastForwarding = false;
    public bool isRewinding = false;

    [Header("Score")]
    public int score = 0;
    public Text scoreText;

    [Header("GameOver")]
    public GameObject gameOverScreen;

    private float _ogSpeed;
    
    private void Start()
    {
        //_ogSpeed = bgScroller.speed;
        //bgScroller.speed = 0;
    }

    void Update()
    {
        if (!isPaused)
        {
            if (isFastForwarding)
            {
                transform.Translate(Vector2.right * fastForwardSpeed * Time.deltaTime);
                //bgScroller.speed = _ogSpeed * 2;
            }
            else if (isRewinding)
            {
                transform.Translate(Vector2.left * rewindSpeed * Time.deltaTime);
                //bgScroller.speed = _ogSpeed / 2;
            }
            else
            {
                transform.Translate(Vector2.right * normalSpeed * Time.deltaTime);
                //bgScroller.Resume();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            // Increase score when colliding with a coin
            Debug.Log("HIT A COIN YAY");
            score++;
            scoreText.text = "Score: " + score;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // Trigger game over when colliding with an enemy
            GameOver();
        }
    }

    public void Play()
    {
        isPaused = false;
        anim.SetBool("Pause", false);
        //bgScroller.Resume();
    }

    public void Pause()
    {
        isPaused = true;
        anim.SetBool("Pause", true);
        //bgScroller.Pause();
    }

    public void FastForward(bool state)
    {
        isFastForwarding = state;
        isRewinding = false;
    }

    public void Rewind(bool state)
    {
        isRewinding = state;
        isFastForwarding = false;
    }

    void GameOver()
    {
        //bgScroller.Pause(); 
        isPaused = true; // Stop movement
        gameOverScreen.SetActive(true); // Show game over screen
    }
}