using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : MonoBehaviour {

    public List<PotionStack> potionsOnBoiler = new List<PotionStack>();
    public float boilDelay;
    public float boilAmount;

    PotionStack potionToMove;
    bool anyBoil;

	// Use this for initialization
	void Start () {
        StartCoroutine("boilPotions");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Potion"))
        {
            potionToMove = collision.collider.GetComponent<PotionStack>();
            potionsOnBoiler.Add(potionToMove);
            if (potionToMove.corked)
            {               
                potionToMove.revealBoilDiplsay(true);
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Potion") && potionsOnBoiler.Contains(collision.collider.GetComponent<PotionStack>()))
        {
            potionToMove = collision.collider.GetComponent<PotionStack>();
            potionsOnBoiler.Remove(potionToMove);                     
            potionToMove.revealBoilDiplsay(false);
                          
        }
    }

    IEnumerator boilPotions()
    {

        while (true)
        {
            
            anyBoil = false;
            foreach(PotionStack p in potionsOnBoiler)
            {
                if(p.corked)
                {
                    if(p.boil < p.maxBoil)
                    {
                        anyBoil = true;
                    }
                    p.Boil(boilAmount);
                    p.revealBoilDiplsay(true);
                }                
            }
            if (!anyBoil && GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
            else if (anyBoil && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(boilDelay);
        }
    }
}
