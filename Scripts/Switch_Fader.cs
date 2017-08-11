using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Switch_Fader : MonoBehaviour {

    Image fader;

	// Use this for initialization
	void Start () {
        fader = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchFade()
    {
        Debug.Log("heello");
    }
}
