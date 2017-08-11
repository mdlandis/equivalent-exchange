using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEffect : MonoBehaviour {

    public GameObject flamestrike;
    GameObject animation;
    public GameObject fireCircle;
    GameObject effect;
    public float delay;
    public float radius;
    public int damage;

    Input_Listeners IPL;

    Color c;

	// Use this for initialization
	void Start () {

        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        CreateFireballEffect(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateFireballEffect()
    {
        animation = Instantiate(flamestrike);
        animation.transform.position = transform.position;
        effect = Instantiate(fireCircle);
        effect.transform.position = transform.position;
        effect.transform.localScale *= radius * 2;
        c = effect.GetComponent<SpriteRenderer>().color;
        effect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.b, c.g, 0.0f);
        StartCoroutine(PerformFadeFireCircle(.25f, 1));
        StartCoroutine("DelayForDamage");
    }


    IEnumerator DelayForDamage()
    {
        yield return new WaitForSeconds(delay);
        Collider[] thingsHit = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in thingsHit)
        {
            if (c.CompareTag("Enemy"))
            {
                c.GetComponent<Goblin_ro_ctrl>().TakeDamage(damage);
            }
        }
        StartCoroutine(PerformFadeFireCircle(2, -1));

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
        if(direction == -1)
        {
            RemoveFireCircle();
        }
        
    }

    public void RemoveFireCircle()
    {
        Destroy(effect);
        Destroy(gameObject);
        Destroy(animation);
    }


}
