using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour {

    public GameObject smoke;

    GameObject mySmoke;


	// Use this for initialization
	void Start () {
        mySmoke = Instantiate(smoke);
        mySmoke.transform.position = transform.position;
	}
	
}
