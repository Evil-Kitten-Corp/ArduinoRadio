using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float waktuDestroy;
    private float waktuBerjalan;

    // Update is called once per frame
    void Update()
    {
        waktuBerjalan += Time.deltaTime;
        if(waktuBerjalan >= waktuDestroy){
            Destroy(this.gameObject);
        }
    }

    public void Hancurkan(){
        Destroy(this.gameObject);
    }
}
