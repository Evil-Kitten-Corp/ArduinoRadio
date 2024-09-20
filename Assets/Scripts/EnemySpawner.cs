using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Camera gameCamera;
    public float timer;

    [Header("enemy")]
    public GameObject enemy;
    public Enemy scriptEnemy;
    public float spawnRateEnemy = 20f;
    public float timerSpawnEnemy;

    [Header("Coin")]
    public GameObject coin;
    public float spawnRateCoin = 2f;
    public float timerSpawnCoin;

    private bool _canSpawnEnemy;
    private Vector2 _screenDisplay;

    void Update()
    {
        _screenDisplay.x = gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + -5f;

        if (enemy.transform.position.x < _screenDisplay.x)
        {
            enemy.SetActive(false);
            _canSpawnEnemy = true;
            timer += Time.deltaTime;
        }
        
        if (timer > spawnRateEnemy && _canSpawnEnemy)
        {
            timerSpawnEnemy = Random.Range(5, 15);
            spawnRateEnemy += timerSpawnEnemy;
            Spawn();
        }
    }

    private void Spawn()
    {
        enemy.SetActive(true);
        enemy.transform.position = transform.position;
        scriptEnemy.Move();
        _canSpawnEnemy = false;
    }

    private void OnDrawGizmos() 
    {
        _screenDisplay.x = gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + -5f;

        Debug.DrawLine(_screenDisplay + Vector2.up * 10f / 2, _screenDisplay + 
                                                              Vector2.down * 10f / 2, Color.blue);
        
        Debug.DrawLine(_screenDisplay + Vector2.up * 10f / 2, _screenDisplay + 
                                                              Vector2.down * 10f / 2, Color.blue);
    }
}
