using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionStack : MonoBehaviour {

    public List<Ingredient> contents = new List<Ingredient>();

    public List<GameObject> potentialAdditions = new List<GameObject>();
    public List<GameObject> itemsToRemove = new List<GameObject>();

    public GameObject liquid;
    public GameObject boilDisplayMeter;
    public GameObject boilDisplay;
    public ParticleSystem boilBubbles;

    public bool corked;
    public bool activated;

    public float displayYStart = .52f;
    public float displayYEnd = .655f;

    public float boil;
    public float maxBoil;
    public float maxOverBoil;
    public float errorMargin;
    public float acceleration;

    public AudioClip[] sounds = new AudioClip[3];


    float percentBoiled;
    float percentOverBoiled;

    Color fic;
    public bool dinged;
    float[] newColorVals = new float[3];

    AudioSource source;

    public bool enhancedRadius;
    bool tempr;
    bool templ;

    public int potionId;
    int tempId;
    //0 = smoke
    //1 = fireball
    //2 = tornado
    //3 = blizzard

    Input_Listeners IPL;
    GameManager GM;

    bool exploded;

	// Use this for initialization
	void Start () {
        liquid.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .2f);

        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        GM = Singleton_Service.GetSingleton<GameManager>();

        StartCoroutine("LookForAdditions");

        boilDisplayMeter.SetActive(false);
        boilDisplay.SetActive(false);

        potionId = 0;
        exploded = false;
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy")) && !exploded)//activated
        {
            potionId = DeterminePotionType();
            CreatePotionEffect(new Vector3(transform.position.x, 0.51f, transform.position.z));
            activated = false;
            exploded = true;
        }       
    }

    private void OnTriggerEnter(Collider col)
    {
        if(!corked)
        {
            if (col.CompareTag("Ingredient") || col.CompareTag("Cork"))
            {
                potentialAdditions.Add(col.gameObject);
            }
        }
        if(col.CompareTag("Activator"))
        {
            activated = true;
            GetComponent<Rigidbody>().velocity *= acceleration;
        }
        
    }
    private void OnTriggerExit(Collider col)
    {
        if(!corked)
        {
            if (col.CompareTag("Ingredient") || col.CompareTag("Cork"))
            {
                if (potentialAdditions.Contains(col.gameObject))
                {
                    potentialAdditions.Remove(col.gameObject);
                }

            }
        }        
    }

    public void PlaySound(AudioSource source, AudioClip clip, bool modulate)
    {
        source.clip = clip;
        if (modulate)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.9f, 1.1f);
        }
        source.Play();
    }

    IEnumerator LookForAdditions()
    {
        while(true)
        {
            itemsToRemove.Clear();
            foreach(GameObject pi in potentialAdditions)
            {
                if(IPL.rightHand.CurrentlyInteracting)
                {
                    tempr = !IPL.rightHand.CurrentlyInteracting.Equals(pi.GetComponent<NVRInteractableItem>());
                }
                else
                {
                    tempr = true;
                }

                if(IPL.leftHand.CurrentlyInteracting)
                {
                    templ = !IPL.leftHand.CurrentlyInteracting.Equals(pi.GetComponent<NVRInteractableItem>());
                }
                else
                {
                    templ = true;
                }
         
                if (true)//tempr && templ)
                {
                    if(pi.CompareTag("Ingredient"))
                    {
                        AddToContents(pi.GetComponent<Ingredient>());
                        itemsToRemove.Add(pi);
                    }
                    else if(pi.CompareTag("Cork"))
                    {
                        CorkPotion(pi);
                        itemsToRemove.Add(pi);
                    }
                              
                }
            }
            foreach(GameObject r in itemsToRemove)
            {
                potentialAdditions.Remove(r);
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    void AddToContents(Ingredient i)
    {
        IPL.rightHand.CurrentlyHoveringOver.Clear();
        IPL.leftHand.CurrentlyHoveringOver.Clear();
        contents.Add(i);
        i.gameObject.SetActive(false);
        PlaySound(source, sounds[2], true);
        if (contents.Count == 1)
        {
            liquid.GetComponent<Renderer>().material.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        }
        else if(contents.Count == 2)
        {
            fic = contents[0].color;
            if(fic.r == i.color.r && fic.g == i.color.g && fic.b == i.color.b)
            {
                liquid.GetComponent<Renderer>().material.color = new Color(newColorVals[0], newColorVals[1], newColorVals[2]);
            }
            else
            {
                newColorVals[0] = fic.r + i.color.r;
                newColorVals[1] = fic.g + i.color.g;
                newColorVals[2] = fic.b + i.color.b;
                for (int j = 0; j < newColorVals.Length; j++)
                {
                    if (newColorVals[j] > 1)
                    {
                        newColorVals[j] = 1;
                    }
                    else
                    {
                        newColorVals[j] = 0;
                    }
                }
                liquid.GetComponent<Renderer>().material.color = new Color(newColorVals[0], newColorVals[1], newColorVals[2]);
            }
            
        }
        else
        {
            liquid.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    void CorkPotion(GameObject cork)
    {
        
        corked = true;
        PlaySound(source, sounds[2], true);
        Destroy(cork.GetComponent<Rigidbody>());
        Destroy(cork.GetComponent<BoxCollider>());
        Destroy(cork.GetComponent<NVRInteractableItem>());
        cork.transform.SetParent(transform);
        cork.transform.localPosition = new Vector3(0, .3f, 0);
        cork.transform.rotation = transform.rotation;
        cork.transform.Rotate(new Vector3(180, 0, 0));
        if(contents.Count == 1)
        {
            maxBoil *= 0.5f;
        }
        IPL.leftHand.GetComponent<Left_VR_Cont>().anthology.refresh();
        IPL.rightHand.GetComponent<Right_VR_Cont>().anthology.refresh();
        IPL.rightHand.CurrentlyHoveringOver.Clear();
        IPL.leftHand.CurrentlyHoveringOver.Clear();

    }

    public int DeterminePotionType()
    {
        if(boil < maxBoil || contents.Count == 0 || contents.Count > 2)
        {
            return 0;
        }
        if(contents.Count == 1)
        {
            tempId = contents[0].type;
            if (tempId == 1) //yellow + magenta
            {
                return 4;
            }
            if (tempId == 2) //yellow + cyan
            {
                return 5;
            }
            if (tempId == 3)
            {
                return 6;
            }
        }
        else
        {
            tempId = contents[0].type * 10 + contents[1].type;

            if (tempId == 12 || tempId == 21) //yellow + magenta
            {
                return 1;
            }
            if (tempId == 13 || tempId == 31) //yellow + cyan
            {
                return 2;
            }
            if (tempId == 23 || tempId == 32)
            {
                return 3;
            }
        }       
        return 0;
    }

    public void Boil(float amount)
    {      
        boil += amount;
        percentBoiled = boil / maxBoil;
        percentBoiled = Mathf.Clamp(percentBoiled, 0.0f, 1.0f);
        percentOverBoiled = (boil-maxBoil) / maxOverBoil;
        percentOverBoiled = Mathf.Clamp(percentOverBoiled, 0.0f, 1.0f);
        
        //.655 at 2
        //.52 at 0
        boilDisplay.transform.localPosition = new Vector3(0, displayYStart + percentBoiled * (displayYEnd - displayYStart), 0);
        boilDisplay.transform.localScale = new Vector3(.11f, .15f * percentBoiled, .11f);
        if(boil < maxBoil)
        {
            boilDisplay.GetComponent<Renderer>().material.color = new Color((1 - percentBoiled), percentBoiled, 0);
        }
        else if(boil > maxBoil && !dinged)
        {
            dinged = true;
            PlaySound(source, sounds[0], false);
        }
        
        ParticleSystem bubbles = Instantiate(boilBubbles) as ParticleSystem;
        bubbles.transform.position = transform.position;
        //bubbles.transform.position -= new Vector3(0, -.2f, 0);
    }

    public void revealBoilDiplsay(bool val)
    {
        boilDisplay.SetActive(val);
        boilDisplayMeter.SetActive(val);
        
    }

    void CreatePotionEffect(Vector3 position)
    {
        if(potionId == 0)
        {
            SmokeEffect smokepuff = Instantiate(GM.smokepuff) as SmokeEffect;
            smokepuff.transform.position = position;
        }
        if(potionId == 1)
        {
            FireballEffect fireball = Instantiate(GM.fireball) as FireballEffect;
            fireball.transform.position = position;
            if (enhancedRadius)
            {
                fireball.radius *= 2;
            }
        }
        if(potionId == 2)
        {
            TornadoEffect tornado = Instantiate(GM.tornado) as TornadoEffect;
            tornado.transform.position = position;
            if (enhancedRadius)
            {
                tornado.radius *= 2;
            }
        }
        if(potionId == 3)
        {
            BlizzardEffect blizzard = Instantiate(GM.blizzard) as BlizzardEffect;
            blizzard.transform.position = position;
            if (enhancedRadius)
            {
                blizzard.radius *= 2;
            }
        }
        if(potionId == 4)
        {
            LightningEffect lightning = Instantiate(GM.lightning) as LightningEffect;
            lightning.transform.position = position;
            if (enhancedRadius)
            {
                lightning.radius *= 2;
            }
        }
        if(potionId == 5)
        {
            ArcaneEffect arcane = Instantiate(GM.arcane) as ArcaneEffect;
            arcane.transform.position = position;
            if (enhancedRadius)
            {
                arcane.radius *= 2;
            }
        }
        if(potionId == 6)
        {
            PoisonEffect poison = Instantiate(GM.poison) as PoisonEffect;
            poison.transform.position = position;
            if (enhancedRadius)
            {
                poison.radius *= 2;
            }
        }

        PlaySound(source, sounds[1], true);
        StartCoroutine("DelayForDestroySelf");
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Renderer>())
            {
                child.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    IEnumerator DelayForDestroySelf()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
