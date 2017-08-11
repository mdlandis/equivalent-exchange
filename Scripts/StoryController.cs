using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour {

    public TextMeshPro text;

    public float delay;
    ScreenFader fader;

	// Use this for initialization
	void Start () {
        fader = GetComponent<ScreenFader>();
        fader.faded = true;
        fader.fadeTime = 2;
        fader.fadeIn = true;
        DetermineStoryText();
        Invoke("MoveToNextScene", fader.fadeTime + delay);

    }
	void MoveToNextScene()
    {
        fader.fadeIn = false;
        Invoke("SceneTransition", fader.fadeTime);
    }
    void SceneTransition()
    {
        SceneManager.LoadScene("Main");
    }

    void DetermineStoryText()
    {
        int level = (PlayerPrefs.GetInt("Level") % 10);
        if(level == 1)
        {
            text.text = "There once was an Alchemist...";
        }
        else if(level == 2)
        {
            text.text = "...who angered the elements.";
        }
        else if(level == 3)
        {
            text.text = "For the sake of his village...";
        }
        else if(level == 4)
        {
            text.text = "...he meddled with magic.";
        }
        else if(level == 5)
        {
            text.text = "When the rain was scarce, \n his potions were plentiful.";
        }
        else if(level == 6)
        {
            text.text = "When the harvest was poor, \n his potions were bountiful.";
        }
        else if(level == 7)
        {
            text.text = "But the rulers of nature would not be usurped.";
        }
        else if(level == 8)
        {
            text.text = "They sent legions of creatures raised from the dirt.";
        }
        else if(level == 9)
        {
            text.text = "When the alchemist saw his townspeople hurt...";
        }
        else if(level == 0)
        {
            text.text = "He once again meddled with magic.";
        }
    }

    
}
