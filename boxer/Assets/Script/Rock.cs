using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody myrb;
    private float destroyTime;

    void Start()
    {
        myrb = GetComponent<Rigidbody>();
        myrb.AddForce(transform.forward *100f, ForceMode.Impulse);
        destroyTime = Time.time + 3f;
    }
     void Update()
    {
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Zemin")
        {
            Destroy(gameObject);
        }
    }


}
