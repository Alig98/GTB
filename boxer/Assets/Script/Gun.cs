using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    private float fireTime;
    public float fireRate;
    public AudioSource ses;
    void Start()
    {
        fireTime = 1f + Time.time;
    }

    void Update()
    {
        if (Time.time > fireTime )
        {
            Instantiate(ses, transform.position, transform.rotation);
            Instantiate(bullet, transform.position, transform.rotation);
            fireTime += fireRate;
        }
    }
}
