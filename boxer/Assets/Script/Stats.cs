using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private GameObject player;
    public Text bc;
    private int bombCount;
    public int bossKilled;
    public int throwerKilled;
    public Text score;
    private int totalUnit;
    public bool showStats;
    public Button button;
    public GameObject title;
    public GameObject text;
    public GameObject inst;

    private void Start()
    {
        Time.timeScale = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        showStats = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            title.SetActive(false);
            text.SetActive(false);
            inst.SetActive(false);
            bc.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
        bombCount = player.GetComponent<Player>().bombCount;
        bc.text = bombCount.ToString() + "X grenade";
        totalUnit = throwerKilled + bossKilled;
        if (showStats)
        {
            score.text = bossKilled + " boss killed\n" + throwerKilled + " thrower killed\n" +"Total "+ totalUnit + " unit killed";
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                restart();
            }
        }
    }
   
     void restart()
    {
        SceneManager.LoadScene(0);
    }
    
}
