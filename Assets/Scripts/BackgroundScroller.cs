using System;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(0,1)] public float speed;

    public float offset = 4;
    public Transform player;

    private bool _isPaused;
    private Vector2 _currentOffset;

    private Renderer _renderer;
    private float _ogSpeed;
    
    private void Start()
    {
        _ogSpeed = speed;
        _renderer = GetComponent<Renderer>();
    }

    void Update() 
    { 
        _renderer.material.mainTextureOffset = new Vector2(player.position.x / offset, 0f); 
        
        //_renderer.material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }

    public void Pause()
    {
        // if (!_isPaused)
        // {
        //     _isPaused = true;
        //     _currentOffset = _renderer.material.mainTextureOffset;
        //     speed = 0;
        //     _renderer.material.mainTextureOffset = _currentOffset;
        // }
    }

    public void Resume()
    {
        // if (_isPaused)
        // {
        //     _isPaused = false;
        //     speed = _ogSpeed;
        //     _renderer.material.mainTextureOffset = _currentOffset;
        // }
    }
}
