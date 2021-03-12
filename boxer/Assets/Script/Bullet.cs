using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float destroyTime;
    void Start()
    {
        destroyTime = Time.time + 3f;
    }
    void Update()
    {
        transform.position += transform.up * 200f * Time.deltaTime;
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
