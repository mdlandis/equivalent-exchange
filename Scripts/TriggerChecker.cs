using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerChecker : MonoBehaviour {

    Input_Listeners IPL;

	// Use this for initialization
	void Start () {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        GetComponent<ScreenFader>().fadeIn = true;
    }
	
	// Update is called once per frame
	void Update () {
		if(IPL.leftTriggerInteractive)
        {
            GetComponent<ScreenFader>().fadeIn = false;
            Invoke("GoToMenu", 1);
        }
        else if(IPL.rightTriggerInteractive)
        {
            GetComponent<ScreenFader>().fadeIn = false;
            Invoke("GoToMain", 1);
        }
        else if(IPL.rightGripInteractive && IPL.leftGripInteractive)
        {
            Application.Quit();
        }
	}
   
    void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    void GoToMain()
    {
        PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene("StoryScene");
    }
}
