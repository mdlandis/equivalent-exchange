using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBar : MonoBehaviour {

    public ItemSpawner belongs;   
    public GameObject bar;

    public float displayStart = -.183f;
    public float displayEnd = .325f;
    public float scaleEnd = 0.3f;

    float refreshRate = .1f;
    float duration;
    float cDuration;
    float percentComplete;

    //-.183, 0
    //-0.325, 0.3

    // Use this for initialization
    void Start () {
		
	}
	
	IEnumerator DoTimer()
    {
        while(cDuration <= duration)
        {
            yield return new WaitForSeconds(refreshRate);
            cDuration += refreshRate;
            UpdateBar();
            
        }
        UpdateBar();
        belongs.FinishTimer();
        
    }

    public void StartTimer(float length)
    {
        duration = length;
        cDuration = 0;
        StartCoroutine("DoTimer");
        UpdateBar();
    }

    void UpdateBar()
    {
        
        percentComplete = cDuration / duration;
        percentComplete = Mathf.Clamp(percentComplete, 0.0f, 1.0f);
        bar.GetComponent<Renderer>().material.color = new Color((1 - percentComplete), percentComplete, 0);
        bar.transform.localPosition = new Vector3(displayStart - percentComplete * (displayEnd - displayStart), bar.transform.localPosition.y, bar.transform.localPosition.z);
        bar.transform.localScale = new Vector3(scaleEnd * percentComplete, bar.transform.localScale.y, bar.transform.localScale.z);
    }
}
