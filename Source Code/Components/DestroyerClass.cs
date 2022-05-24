using UnityEngine;
using System.Collections;

public class DestroyerClass : MonoBehaviour
{
    float timer = 10f;


    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}