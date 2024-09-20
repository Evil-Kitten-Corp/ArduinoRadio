using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public Camera gameCamera;
    public float waktu;

    [Header("Musuh")]
    public GameObject musuh;
    public Enemy scriptMusuh;
    public float spawnRateMusuh = 5f;
    public float waktuSpawnMusuh = 5f;

    [Header("Coin")]
    public GameObject coin;
    public float spawnRateCoin = 2f;
    public float waktuSpawnCoin = 1f;
    

    private float waktuCoin;
    private bool bisaSpawn;
    private Vector2 lebarLayar;

    void Update()
    {
        waktuCoin += Time.deltaTime;
        lebarLayar.x = gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + -5f;

        if(musuh.transform.position.x < lebarLayar.x){
            musuh.SetActive(false);
            bisaSpawn = true;
            waktu += Time.deltaTime;
        }
        
        // if(waktu > spawnRateMusuh && bisaSpawn){
        //     waktuSpawnMusuh = Random.Range(5, 15);
        //     spawnRateMusuh += waktuSpawnMusuh;
        //     Spawn();
        // }

        if(waktuCoin > spawnRateCoin){
            waktuSpawnCoin = Random.Range(3, 8);
            spawnRateCoin += waktuSpawnCoin;
            GameObject a = Instantiate(coin);
            a.transform.position = new Vector3(transform.position.x, Random.Range(-3.21f, -1.22f), transform.position.z);

        }
    }

    private void Spawn(){
        musuh.SetActive(true);
        musuh.transform.position = this.transform.position;
        scriptMusuh.Move();
        bisaSpawn = false;
    }

    private void OnDrawGizmos() {
        lebarLayar.x = gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + -5f;

        Debug.DrawLine(lebarLayar + Vector2.up * 10f / 2, lebarLayar + Vector2.down * 10f / 2, Color.blue);
        Debug.DrawLine(lebarLayar + Vector2.up * 10f / 2, lebarLayar + Vector2.down * 10f / 2, Color.blue);
    }
}
