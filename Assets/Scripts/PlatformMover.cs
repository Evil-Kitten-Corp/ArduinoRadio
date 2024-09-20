using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformMover : MonoBehaviour
{
    public bool moveRight = true;
    [Range(0, 1)] public float moveSpeed;
    private bool _canMove;

    private void Update()
    {
        if (_canMove)
        {
            Vector3 newPosition = transform.position;

            if (moveRight)
            {
                newPosition.x += Time.time * moveSpeed;
            }
            else
            {
                newPosition.x -= Time.time * moveSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _canMove = false;
        }
    }
}
