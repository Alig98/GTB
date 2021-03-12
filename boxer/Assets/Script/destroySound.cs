using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySound : MonoBehaviour
{
    private float destroyTime;
    void Start()
    {
        destroyTime = Time.time + 2.2f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
