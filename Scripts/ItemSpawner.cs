using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public TimerBar timer;
    public TimerBar myTimer;
    public GameObject whatToSpawn;
    public GameObject holding;
    public float leashRange;
    private float checkInterval = .1f;
    public float cooldown;

    bool started;

	// Use this for initialization
	void Start ()
    {
        ConfigureTimer();
        SpawnNew();
        StartCoroutine("SlowUpdate");
	}
	
	IEnumerator SlowUpdate()
    {
        while(true)
        {
            if(Vector3.Distance(holding.transform.position, transform.position) > leashRange)
            {
                if (!started)
                {
                    myTimer.StartTimer(cooldown);
                    started = true;
                }
                
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void SpawnNew()
    {
        holding = Instantiate(whatToSpawn) as GameObject;
        holding.transform.localPosition = transform.position;
    }

    public void FinishTimer()
    {
        SpawnNew();
        started = false;
    }

    void ConfigureTimer()
    {
        myTimer = Instantiate(timer) as TimerBar;
        myTimer.transform.position = transform.position + new Vector3(0, .2f, -.03f);
        myTimer.transform.rotation = transform.rotation;
        myTimer.transform.Rotate(new Vector3(0, 180, 0));
        myTimer.belongs = this;
    }
}
