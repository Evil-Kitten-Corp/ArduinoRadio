using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(0,1)] public float speed;
    
    void Update() 
    { 
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
