using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombDrag : MonoBehaviour
{
    private Rigidbody myrb;
    float timer=1f;
    float countdown;
    bool hasExploded;
    public ParticleSystem ps;
    private float Force;
    private Vector3 posit;
    public AudioSource ses;

    private void Start()
    {
        Force = 10000;
        hasExploded=false;
        myrb = GetComponent<Rigidbody>();
        myrb.AddForce((transform.forward*50f)+(transform.up* 10f), ForceMode.Impulse);
        countdown = timer;

    }
    void Update()
    {
        posit = transform.position;
        countdown -= Time.deltaTime;
        if(countdown<=0 && !hasExploded)
        {
            explode();
        }
        
    }
    private void explode()
    {
        Instantiate(ses, transform.position, transform.rotation);
        Instantiate(ps, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if ( rb != null)
            {
                rb.AddExplosionForce(Force, transform.position, 20f);
                nearbyObject.gameObject.transform.forward = new Vector3((posit- nearbyObject.gameObject.transform.position).x
                    ,0, (posit - nearbyObject.gameObject.transform.position).z);
                if(nearbyObject.gameObject.tag == "Enemy")
                {
                    nearbyObject.GetComponent<Enemy>().myAnim.SetTrigger("boom");
                    nearbyObject.GetComponent<Enemy>().currentHealth -= 80;
                }
                else if (nearbyObject.gameObject.tag == "EnemyBoss")
                {
                    Instantiate(nearbyObject.GetComponent<EnemyBoss>().hitAudio, nearbyObject.transform.position, nearbyObject.transform.rotation);
                    nearbyObject.GetComponent<EnemyBoss>().myAnim.SetTrigger("boom");
                    nearbyObject.GetComponent<EnemyBoss>().currentHealth -= 40;
                }
            }

        }
        hasExploded = true;
        Destroy(gameObject);
    }
}
