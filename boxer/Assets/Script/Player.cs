using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    private float speed = 0f;
    private Plane playerPlane;
    private Ray ray;
    private float hitdist;
    private Vector3 targetPoint;
    private Quaternion targetRotation;
    public Animator myAnim;
    public float dashForce = 1f;
    private float turnSpeed;

    public bool dash5 = true;
    public Renderer r;
    public Color defaultColor;
    private float speedWhenGettingHit =1f;
    private float speedWhenKicking = 0f;
    private RaycastHit hit;
    public int health;
    public int maxHealth;
    public healthBar hb;
    public GameObject m4;
    public GameObject m4position;
    public GameObject toChild;
    public GameObject crossbow;
    public GameObject crossbowPosition;
    public GameObject grenade;
    public GameObject handgun;
    public GameObject hgposition;
    public GameObject uzi;
    public GameObject uziposition;
    public int bombCount;
    private AudioSource stoneHit;
    private GameObject cv;
    private bool canKick;

    void Start()
    {
        turnSpeed = 20f;
        canKick = true;
        cv = GameObject.FindGameObjectWithTag("scores");
        stoneHit = GetComponent<AudioSource>();
        bombCount = 3;
        health = 100;
        health = maxHealth;
        hb.setMaxHealth(maxHealth);
        r = GetComponentInChildren<Renderer>();
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        defaultColor = r.materials[2].color;
    }

    void FixedUpdate()
    {
        hb.setHealth(health);


        if (transform.position.y < -15)
        {
            gameOver();
        }
        playerPlane = new Plane(Vector3.up, transform.position);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hitdist = 0;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            targetPoint = ray.GetPoint(hitdist);
            targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * turnSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * speedWhenGettingHit * Time.deltaTime;
            transform.position += transform.forward * speedWhenKicking * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            speed = 20f;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            speed = 0f;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (canKick && !myAnim.GetBool("firing") && !myAnim.GetBool("hgfiring"))
            {
                StartCoroutine(kickTimer());
                myAnim.SetTrigger("kick");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (bombCount <= 8 && bombCount > 0)
            {
                Instantiate(grenade, crossbowPosition.transform.position, transform.rotation);
                bombCount -= 1;
            }
        }
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsTag("hit"))
        {
            speedWhenGettingHit = 0f;
        }
        else
        {
            speedWhenGettingHit = 1f;
        }
        myAnim.SetFloat("karakterHizi", speed);
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            speedWhenKicking = 50f;
            if (dash5)
            {
                dash5 = false;
            }
        }
        else
        {
            speedWhenKicking = 0f;
            dash5 = true;
        }
        if (health <= 0)
        {
            gameOver();
        }
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            speed = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "gun" && myAnim.GetBool("firing")==false && !myAnim.GetBool("hgfiring"))
        {
            StartCoroutine(gun(m4,m4position));
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "bow" && myAnim.GetBool("firing") == false && !myAnim.GetBool("hgfiring") )
        {
            StartCoroutine(gun(crossbow,crossbowPosition));
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "handgun" && myAnim.GetBool("firing") == false && !myAnim.GetBool("hgfiring") )
        {
            StartCoroutine(handguncr(handgun,hgposition));
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "uzi" && myAnim.GetBool("firing") == false && !myAnim.GetBool("hgfiring"))
        {
            StartCoroutine(handguncr(uzi,uziposition));
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag== "bomb")
        {
            if (bombCount <8)
            {
                bombCount += 1;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Stone" && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            StartCoroutine(playerGetHit(5,10f,other.gameObject));
        }
        if (other.gameObject.tag == "heart")
        {
            if (health >= 80)
            {
                health = 100;
            }
            else
            {
                health += 20;
            }
            Destroy(other.gameObject);
        }
    }
    IEnumerator gun(GameObject gun2, GameObject position)
    {
        GameObject gun = Instantiate(gun2, position.transform.position,position.transform.rotation) as GameObject;
        gun.transform.parent = toChild.transform;
        myAnim.SetBool("firing", true);
        yield return new WaitForSeconds(20f);
        myAnim.SetBool("firing", false);
        Destroy(gun);
    }
    IEnumerator handguncr(GameObject gun2,GameObject position)
    {
        GameObject gun = Instantiate(gun2, position.transform.position, position.transform.rotation) as GameObject;
        gun.transform.parent = toChild.transform;
        myAnim.SetBool("hgfiring", true);
        yield return new WaitForSeconds(20f);
        myAnim.SetBool("hgfiring", false);
        Destroy(gun);
    }
    public IEnumerator playerGetHit(int amount,float amount2,GameObject other)
    {
        stoneHit.Play();
        transform.forward = -other.gameObject.transform.forward;
        rb.AddForce(-transform.forward * amount2, ForceMode.Impulse);
        health -= amount;
        r.materials[2].color = Color.red;
        myAnim.SetBool("getHit", true);
        yield return new WaitForSeconds(0.1f);
        myAnim.SetBool("getHit", false);
        r.materials[2].color = defaultColor;
    }
    void gameOver()
    {
        cv.GetComponent<Stats>().showStats = true;
        Time.timeScale = 0;
        gameObject.SetActive(false);
    }
    IEnumerator kickTimer()
    {
        canKick = false;
        yield return new WaitForSeconds(3.5f);
        canKick = true;
    }
}