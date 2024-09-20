using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Move();
    }

    public void Move()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    }
}
