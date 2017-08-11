using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneEffect : MonoBehaviour
{

    public GameObject blizzard;
    GameObject animation;
    public GameObject iceCircle;
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
        CreateBlizzardEffect();
    }

    public void PotionHit(PotionStack potion)
    {
        potion.enhancedRadius = true;
    }

    void CreateBlizzardEffect()
    {
        animation = Instantiate(blizzard);
        animation.transform.position = transform.position;
        animation.transform.Translate(new Vector3(0, 0.25f, 0));
        //animation.GetComponent<ParticleSystem>().startSize = radius;
        //animation.transform.localScale = new Vector3(radius * .6f, 1, radius * .6f);
        effect = Instantiate(iceCircle);
        effect.transform.position = transform.position;
        effect.transform.localScale *= radius * 2;
        c = effect.GetComponent<SpriteRenderer>().color;
        effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, 0.0f);
        effect.transform.parent = transform;
        StartCoroutine(PerformFadeIceCircle(.25f, 1));
        StartCoroutine("PerformBlizzard");
    }


    IEnumerator PerformBlizzard()
    {
        
        yield return new WaitForSeconds(duration);
        
        thingsHit = Physics.OverlapSphere(transform.position, radius);
        StartCoroutine(PerformFadeIceCircle(1, -1));

    }

    IEnumerator PerformFadeIceCircle(float duration, int direction)
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
            RemoveIceCircle();
        }

    }

    public void RemoveIceCircle()
    {
        Destroy(effect);
        Destroy(gameObject);
        Destroy(animation);
    }


}
