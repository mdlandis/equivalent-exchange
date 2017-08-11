using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public BlizzardEffect blizzard;
    public TornadoEffect tornado;
    public FireballEffect fireball;
    public SmokeEffect smokepuff;
    public LightningEffect lightning;
    public PoisonEffect poison;
    public ArcaneEffect arcane;

    public TextMeshPro healthText;

    public Announcer announcer;
    

    public bool tutorial;

    public float loseDelay;

    public int health;
    public int startingHealth;
    public int healthIncrement;

    bool firstdamage;
    bool firstBigDamage;

    public AudioSource audioSource;

    Input_Listeners IPL;

    bool lost;

    public AudioClip[] sounds = new AudioClip[3];

    // Use this for initialization
    void Start()
    {
        if(PlayerPrefs.GetInt("Level") == 1)
        {
            health = startingHealth;
        }
        else
        {
            health = PlayerPrefs.GetInt("Health");
            health += healthIncrement;
        }
       
       
        if(healthText)
        {
            healthText.text = "HP: " + health;
        }
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        //Time.timeScale = 0.5f;
        GetComponent<ScreenFader>().fadeIn = true;
    }

    public void PlaySound(AudioSource source, AudioClip clip, bool modulate)
    {
        source.clip = clip;
        if(modulate)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.9f, 1.1f);
        }        
        source.Play();
    }
    public void dealDamage(int amount)
    {
        if(!lost)
        {
            if(!firstdamage)
            {
                announcer.PlaySound(8);
                firstdamage = true;
            }
            health -= amount;
            /*
            IPL.rightHand.LongHapticPulse(1, NewtonVR.NVRButtons.Trigger);
            IPL.leftHand.TriggerHapticPulse(1, NewtonVR.NVRButtons.Trigger);
            */
            SteamVR_Controller.Input(3).TriggerHapticPulse(2000);
            SteamVR_Controller.Input(4).TriggerHapticPulse(2000);
            PlaySound(GetComponent<AudioSource>(), sounds[0], true);
            healthText.text = "HP: " + health;
            if (health <= 5 && !firstBigDamage)
            {
                firstBigDamage = true;
                healthText.color = Color.red;
                announcer.PlaySound(9);
            }
            if(health == 1)
            {
                announcer.PlaySound(10);
            }
            if (health <= 0)
            {
                announcer.PlaySound(11);
                //Time.timeScale = .1f;
                StartCoroutine("FadeMusic");
                StartCoroutine("DelayForLose");
                GetComponent<ScreenFader>().fadeIn = false;
            }
        }
        
    }

    IEnumerator DelayForLose()
    {
        PlaySound(GetComponent<AudioSource>(), sounds[1], false);
        lost = true;
        yield return new WaitForSeconds(loseDelay);
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Goblin_ro_ctrl>().runSpeed = 0;
            enemy.GetComponent<Goblin_ro_ctrl>().slowedSpeed = 0;
            enemy.GetComponent<Goblin_ro_ctrl>().ChangeWalkState(0);
        }
        SceneManager.LoadScene("GameOver");

    }

    public void win()
    {
        PlaySound(GetComponent<AudioSource>(), sounds[2], false);
        PlayerPrefs.SetInt("Health", health);
        StartCoroutine("FadeMusic");
        StartCoroutine("DelayForWin");
        GetComponent<ScreenFader>().fadeIn = false;
    }

    IEnumerator DelayForWin()
    {
        
        yield return new WaitForSeconds(loseDelay);       
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        
        SceneManager.LoadScene("StoryScene");
    }

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance(this);
    }
    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance(this);
    }

    IEnumerator FadeMusic()
    {
        while(audioSource.volume > 0)
        {
            audioSource.volume -= .05f;
            yield return new WaitForSeconds(0.05f);
        }
        //perfect opportunity to insert an on complete hook here before the coroutine exits.
    }
}
