using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody myrb;
    private GameObject player;
    private Vector3 lookVector;
    public int currentHealth;
    public int maxHealth;
    public Animator myAnim;
    public float dashForce;
    private float speed = 10f;
    private Color defaultColor;
    public Renderer r;
    public GameObject bomb;
    private float speedWhenPunching=1f;
    public bool dashEnemy = true;
    public healthBar healthbar1;
    public GameObject gun;
    public GameObject heart;
    public GameObject crossbowIcon;

    public Gradient skin;
    public Gradient pants;

    private bool canDead = true;
    public GameObject hb;
    public GameObject handGunicon;
    public GameObject uziicon;
    public GameObject grenadeicon;
    private AudioSource dieAudio;
    public AudioSource kickSound;
    private GameObject cv;

    void Start()
    {
        cv = GameObject.FindGameObjectWithTag("scores");
        dieAudio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        healthbar1.setMaxHealth(maxHealth);
        r = GetComponentInChildren<Renderer>();
        myAnim = GetComponent<Animator>();
        myrb = GetComponent<Rigidbody>();

        float x = Random.Range(0, 100);
        float y = Random.Range(0, 100);

        r.materials[2].color = skin.Evaluate(x / 100);
        r.materials[0].color = pants.Evaluate(y / 100);

        defaultColor = r.materials[2].color;

    }
    void FixedUpdate()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        healthbar1.setHealth(currentHealth);
        lookVector = transform.position - player.transform.position;
        myAnim.SetFloat("hiz", speed);
        if (!myAnim.GetCurrentAnimatorStateInfo(0).IsTag("1") && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("2") 
            && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("3") && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("4")
            && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("throw") && (player.transform.position - transform.position).sqrMagnitude >500f)
        {
            transform.position += transform.forward * speed*speedWhenPunching * Time.deltaTime;
            transform.forward = new Vector3(-lookVector.x, 0, -lookVector.z);
            myAnim.SetBool("throwing", false);
        }
        else
        {
            myAnim.SetBool("throwing", true);
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsTag("throw"))
            {
                transform.forward = new Vector3(-lookVector.x, 0, -lookVector.z);
            }
        }
        if (currentHealth <= 0 && canDead==true)
        {
            cv.GetComponent<Stats>().throwerKilled += 1;
            hb.SetActive(false);
            StartCoroutine(deadAnim());

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bigbullet")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(20));
        }
        if (other.gameObject.tag == "smallbullet")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(10));
        }
        if (other.gameObject.tag == "Stone")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(20));
        }
        if (other.gameObject.tag == "arrow")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit3());
        }
        if (other.gameObject.tag == "Player" && player.GetComponent<Player>().myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            Instantiate(kickSound, transform.position, transform.rotation);
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && player.GetComponent<Player>().myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {

            if (player.GetComponent<Player>().dash5 == false)
            {
                StartCoroutine(hitMat());
                StartCoroutine(hit4());
            }
        }
    }


    public IEnumerator hitMat()
    {
        r.materials[2].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        r.materials[2].color = defaultColor;
    }

    public IEnumerator hit1(int number)
    {
        currentHealth -= number;
        myAnim.SetBool("hit1", true);
        myrb.AddForce(-transform.forward * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        myAnim.SetBool("hit1", false);

    }
    IEnumerator hit3()
    {
        currentHealth -= 50;
        myAnim.SetBool("hit3", true);
        myrb.AddForce(-transform.forward * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        myAnim.SetBool("hit3", false);
    }
    public IEnumerator hit4()
    {
        currentHealth -= 10;
        myAnim.SetBool("hit4", true);
        myrb.AddForce(-transform.forward * 6 * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        myAnim.SetBool("hit4", false);
    }
    IEnumerator deadAnim()
    {
        dieAudio.Play();
        speed = 0f;
        canDead = false;
        myAnim.SetBool("die", true);
        yield return new WaitForSeconds(2.3f);
        int x = Random.Range(1, 20);
        if (x == 1)
        {
            Instantiate(gun, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
        }
        if (x == 2)
        {
            Instantiate(heart, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        }
        if (x == 3)
        {
            Instantiate(crossbowIcon, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        }
        if (x == 4)
        {
            Instantiate(handGunicon, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        }
        if (x == 5)
        {
            Instantiate(uziicon, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        }
        if (x == 6)
        {
            Instantiate(grenadeicon, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        }
        Destroy(gameObject);
    }


}
