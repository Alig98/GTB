using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public Rigidbody myrb;
    private GameObject player;
    private Vector3 lookVector;
    public int currentHealth;
    public int maxHealth;
    public Animator myAnim;
    public float dashForce;
    private float speed = 7f;
    private Color defaultColor;
    public Renderer r;
    public GameObject bomb;
    private float speedWhenPunching = 1f;
    public bool dashEnemy = true;
    public healthBar healthbar1;
    public GameObject gun;
    public GameObject heart;
    public GameObject crossbowIcon;
    public GameObject handGunicon;

    public Gradient skin;
    public Gradient pants;

    private bool canDead = true;
    public GameObject hb;
    private bool punched = false;

    private AudioSource dieAudio;
    public AudioSource attackAudio;
    public AudioSource hitAudio;
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

        healthbar1.setHealth(currentHealth);
        lookVector = transform.position - player.transform.position;
        if (!myAnim.GetCurrentAnimatorStateInfo(0).IsTag("1") && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("2")
            && !myAnim.GetCurrentAnimatorStateInfo(0).IsTag("3")&&(player.transform.position - transform.position).sqrMagnitude > 10f)
        {
            transform.position += transform.forward * speed * speedWhenPunching * Time.deltaTime;
            transform.forward = new Vector3(-lookVector.x, 0, -lookVector.z);
        }
        if (currentHealth <= 0 && canDead == true)
        {
            cv.GetComponent<Stats>().bossKilled += 1;
            hb.SetActive(false);
            StartCoroutine(deadAnim());

        }
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsTag("2"))
        {
            if (punched == false )
            {
                StartCoroutine(punch(player));
            }
        }
        else
        {
            punched = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !player.GetComponent<Player>().myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            myAnim.SetTrigger("Attack");

        }
        if (other.gameObject.tag == "bigbullet")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(10));
        }
        if (other.gameObject.tag == "smallbullet")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(5));
        }
        if (other.gameObject.tag == "arrow")
        {
            StartCoroutine(hitMat());
            StartCoroutine(hit1(20));
        }
        if (other.gameObject.tag == "Player" && player.GetComponent<Player>().myAnim.GetCurrentAnimatorStateInfo(0).IsTag("5"))
        {
            Instantiate(kickSound, transform.position, transform.rotation);
            if (player.GetComponent<Player>().dash5 == false)
            {
                StartCoroutine(hitMat());
                StartCoroutine(hit1(5));
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
    IEnumerator punch(GameObject other)
    {
        Instantiate(attackAudio, transform.position, transform.rotation);
        punched = true;
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(other.GetComponent<Player>().playerGetHit(20, 50f,gameObject));
    }
    IEnumerator deadAnim()
    {
        dieAudio.Play();
        speed = 0f;
        canDead = false;
        myAnim.SetBool("die", true);
        yield return new WaitForSeconds(5f);
        int x = Random.Range(1, 10);
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
        Destroy(gameObject);
    }
}
