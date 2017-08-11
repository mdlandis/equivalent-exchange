using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoEffect : MonoBehaviour
{

    public GameObject tornado;
    GameObject animation;
    public GameObject windCircle;
    GameObject effect;

    public float tickRate;
    public float duration;
    public float cDuration;
    public float radius;
    public int damage;

    Collider[] thingsHit;
    Goblin_ro_ctrl currentEnemy;


    Input_Listeners IPL;

    Color c;

    // Use this for initialization
    void Start()
    {

        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        CreateTornadoEffect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateTornadoEffect()
    {
        animation = Instantiate(tornado);
        animation.transform.position = transform.position;
        animation.transform.localScale = new Vector3(radius * .5f, 1, radius * .5f);
        effect = Instantiate(windCircle);
        effect.transform.position = transform.position;
        effect.transform.localScale *= radius * 2;
        c = effect.GetComponent<SpriteRenderer>().color;
        effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, 0.0f);
        StartCoroutine(PerformFadeWindCircle(.25f, 1));
        StartCoroutine("PerformTornado");
    }


    IEnumerator PerformTornado()
    {
        cDuration = 0;
        while(cDuration < duration)
        {
            thingsHit = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in thingsHit)
            {
                if (c.CompareTag("Enemy"))
                {
                    currentEnemy = c.GetComponent<Goblin_ro_ctrl>();
                    if(!currentEnemy.brute)
                    {
                        currentEnemy.ChangeWalkState(0);
                        currentEnemy.beingPulled = true;
                        currentEnemy.pullLocation = transform.position + new Vector3(0, 1, 0);
                        currentEnemy.TakeDamage(damage);
                    }
                    
                }
            }
            cDuration += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
        thingsHit = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in thingsHit)
        {
            if (c.CompareTag("Enemy"))
            {
                if(!currentEnemy.dead)
                {
                    currentEnemy = c.GetComponent<Goblin_ro_ctrl>();
                    currentEnemy.ChangeWalkState(2);
                    currentEnemy.beingPulled = false;
                }
                
            }
        }
        StartCoroutine(PerformFadeWindCircle(1, -1));

    }

    IEnumerator PerformFadeWindCircle(float duration, int direction)
    {
        c = effect.GetComponent<SpriteRenderer>().color;
        for (int i = 1; i <= 100; i++)
        {
            yield return new WaitForSeconds(duration / 100);
            if (direction == 1)
            {
                effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, i / 100.0f);
            }
            else
            {
                effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, 1 - (i / 100.0f));
            }
        }
        if (direction == -1)
        {
            RemoveWindCircle();
        }

    }

    public void RemoveWindCircle()
    {
        Destroy(effect);
        Destroy(gameObject);
        Destroy(animation);
    }


}
