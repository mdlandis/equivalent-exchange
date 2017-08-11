using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{

    public GameObject lightningBolt;
    GameObject animation;
    public GameObject lightningCircle;
    GameObject effect;
    public float delay;
    public float radius;
    public int damage;

    Input_Listeners IPL;

    Color c;
    float minDistance;
    Goblin_ro_ctrl cEnemy;

    // Use this for initialization
    void Start()
    {

        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        CreateFireballEffect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateFireballEffect()
    {
        effect = Instantiate(lightningCircle);
        effect.transform.position = transform.position;
        effect.transform.localScale *= radius * 2;
        c = effect.GetComponent<SpriteRenderer>().color;
        effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, 0.0f);
        StartCoroutine(PerformFadeFireCircle(.25f, 1));
        StartCoroutine("DelayForDamage");
    }


    IEnumerator DelayForDamage()
    {
        cEnemy = null;
        minDistance = 500;
        Collider[] thingsHit = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in thingsHit)
        {
            if (c.CompareTag("Enemy"))
            {
                if(Vector3.Distance(transform.position, c.transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, c.transform.position);
                    cEnemy = c.GetComponent<Goblin_ro_ctrl>();
                }
            }
        }
        animation = Instantiate(lightningBolt) as GameObject;
        animation.GetComponent<ParticleSystem>().time = 1.9f;
        if (cEnemy != null)
        {
            animation.transform.position = cEnemy.transform.position;
        }
        else
        {
            animation.transform.position = transform.position;
        }
        yield return new WaitForSeconds(delay);

        GetComponent<AudioSource>().Play();
        if(cEnemy != null && !cEnemy.dead)
        {
            cEnemy.TakeDamage(damage);
        }


        StartCoroutine(PerformFadeFireCircle(1, -1));

    }

    IEnumerator PerformFadeFireCircle(float duration, int direction)
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
            RemoveFireCircle();
            Destroy(animation);
            Destroy(effect);
            StartCoroutine("RemoveFireCircle");
        }

    }

    IEnumerator RemoveFireCircle()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        
    }


}
