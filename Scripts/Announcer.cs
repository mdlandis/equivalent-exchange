using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcer : MonoBehaviour {

    public AudioClip _25secSpawn;
    public AudioClip _20secSpawn;
    public AudioClip _15secSpawn;
    public AudioClip _10secSpawn;

    public AudioClip firstWave;
    public AudioClip lastWave;

    public AudioClip[] newWave = new AudioClip[3];

    public AudioClip damaged;
    public AudioClip damaged2;
    public AudioClip damaged3;

    public AudioClip lose;

    AudioSource source;


    private void Awake()
    {
        source = GetComponent<AudioSource>();        
    }

    public void PlaySound(int index)
    {
        switch(index)
        {
            case 1:
                source.clip = _25secSpawn;
                break;
            case 2:
                source.clip = _20secSpawn;
                break;
            case 3:
                source.clip = _15secSpawn;
                break;
            case 4:
                source.clip = _10secSpawn;
                break;
            case 5:
                source.clip = firstWave;
                break;
            case 6:
                source.clip = lastWave;
                break;
            case 7:
                source.clip = newWave[Random.Range(0, 3)];
                break;
            case 8:
                source.clip = damaged;
                break;
            case 9:
                source.clip = damaged2;
                break;
            case 10:
                source.clip = damaged3;
                break;
            case 11:
                source.clip = lose;
                break;
        }
        source.Play();
        
    }
	
}
