using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Color color;
    public int type;
    //yellow = 1
    //magenta = 2
    //cyan = 3

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Shelf"))
        {
            //PlaySound(GetComponent<AudioSource>(), GetComponent<AudioSource>().clip, true);
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
}
