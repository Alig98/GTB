using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject enemy;
    public GameObject boss;
    private float nextSpawnTime;
    void Start()
    {

        nextSpawnTime = Time.time;
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Time.time > nextSpawnTime)
        {
            int x = Random.Range(1, 10);
            if (x == 1)
            {
                Instantiate(boss, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(enemy, transform.position, transform.rotation);
            }
            nextSpawnTime += 10f;
        }

    }
}
