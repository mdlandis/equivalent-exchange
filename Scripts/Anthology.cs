using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Anthology : MonoBehaviour {

    public GameObject hand;
    public TextMeshPro text;
    public Canvas canvas;
    public bool active;

    Right_VR_Cont right;
    Left_VR_Cont left;
    Input_Listeners IPL;

    GameObject NVRGrippedLastFrame;


	// Use this for initialization
	void Start () {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        active = true;
        text.text = "Hellooo!!";
        activate(false);
        right = hand.GetComponent<Right_VR_Cont>();
        left = hand.GetComponent<Left_VR_Cont>();
	}
	
	// Update is called once per frame
	void Update () {
		if(active)
        {
            transform.position = hand.transform.position;
            canvas.transform.LookAt(Camera.main.transform.position);
            if(right)
            {
                if(IPL.rightHand.CurrentlyInteracting)
                {
                    if (!IPL.rightHand.CurrentlyInteracting.Equals(NVRGrippedLastFrame))
                    {
                        activate(true);
                    }
                    NVRGrippedLastFrame = IPL.rightHand.CurrentlyInteracting.gameObject;
                }
                else
                {
                    if(NVRGrippedLastFrame)
                    {
                        activate(true);
                    }
                    NVRGrippedLastFrame = null;
                }               
                
            }
            if(left)
            {
                if (IPL.leftHand.CurrentlyInteracting)
                {
                    if (!IPL.leftHand.CurrentlyInteracting.Equals(NVRGrippedLastFrame))
                    {
                        activate(true);
                    }
                    NVRGrippedLastFrame = IPL.leftHand.CurrentlyInteracting.gameObject;
                }
                else
                {
                    if (NVRGrippedLastFrame)
                    {
                        activate(true);
                    }
                    NVRGrippedLastFrame = null;
                }
            }
        }
	}

    public void activate(bool val)
    {
        active = val;
        canvas.enabled = active;
        text.enabled = active;
        text.transform.GetChild(0).gameObject.SetActive(val);
        if (!active)
        {
            
        }
        else
        {
            transform.position = hand.transform.position;
            canvas.transform.LookAt(Camera.main.transform.position);            
            if(right)
            {
                text.text = DetermineText(right.DetermineHolding());
            }
            else
            {
                text.text = DetermineText(left.DetermineHolding());
            }
            
        }
    }

    public void refresh()
    {
        if(active)
        {
            activate(true);
        }
    }

    public string DetermineText(int id)
    {
        if(id == 0)
        {
            return "You're not holding anything.";
        }
        else if(id == 1)
        {
            return "An uncorked potion. It won't do anything without a cork.";
        }
        else if(id == 2)
        {
            return "An unboiled potion. It won't do anything unless it's fully boiled.";
        }
        else if(id == 3)
        {
            return "A potion boiled with incorrect ingredients. It won't do anything when you throw it.";
        }
        else if(id == 4)
        {
            return "A fireball potion. Deals massive damage in a small area.";
        }
        else if(id == 5)
        {
            return "A tornado potion. Pulls enemies together and holds them there for a few seconds.";
        }
        else if(id == 6)
        {
            return "A blizzard potion. Creates a long-lasting field that slows enemies down.";
        }
        else if(id == 7)
        {
            return "A lightning potion. After a delay, deals massive damage to the closest enemy.";
        }
        else if(id == 8)
        {
            return "An arcane potion. Increases the radius of any potion thrown into it.";
        }
        else if(id == 9)
        {
            return "A poison potion. Creates a medium duration field that damages enemies over time.";
        }
        else if(id == 10)
        {
            return "Yellow Essence. Used to make potions.";
        }
        else if(id == 11)
        {
            return "Magenta Essence. Used to make potions.";
        }
        else if(id == 12)
        {
            return "Cyan Essence. Used to make potions.";
        }
        else if(id == 13)
        {
            return "A cork. Put it on a potion before you boil it.";
        }

        return "";
    }
}
