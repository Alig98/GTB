using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject enemy;
    public GameObject stone;
    public bool throwRock = false;
    void Update()
    {
        if (enemy.GetComponent<Enemy>().myAnim.GetCurrentAnimatorStateInfo(0).IsTag("throw"))
        {
            if(throwRock == false)
            {
                StartCoroutine("instRock");
                throwRock = true;
            }
        }
        else
        {
            StopCoroutine("instRock");
            throwRock = false;
        }
    }
    IEnumerator instRock()
    {
        yield return new WaitForSeconds(2.12f);
        Instantiate(stone, transform.position, enemy.transform.rotation);
    }
}
