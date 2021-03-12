using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    private float destroyTime;
    private Rigidbody myrb;

    void Start()
    {
        myrb = GetComponent<Rigidbody>();
        myrb.AddForce(transform.forward * 150f, ForceMode.Impulse);
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
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBoss")
        {
            Destroy(gameObject);
        }

    }
}