using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using NewtonVR;
using TMPro;

public class Right_VR_Cont : MonoBehaviour
{

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId aButton = Valve.VR.EVRButtonId.k_EButton_A;
    private Valve.VR.EVRButtonId bButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId joystickPress = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    public Collider[] hovering;
    public NVRInteractableItem objectHovering;
    public NVRInteractableItem delayedHold;

    public GameObject raycastOrigin;

    private SpriteRenderer ringDisplay;
    public bool pullPotion;
    public bool pullSomething;

    public GameObject whoosh;
    

    Input_Listeners IPL;

    bool shouldPull;

    bool playedWhoosh;

    public Anthology anthology;

    GameObject itemOfInterest;

    public GameObject aimer;



    public AudioClip[] sounds = new AudioClip[2];
    AudioSource source;
    // Use this for initialization
    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        IPL.rightHand = GetComponent<NVRHand>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        ringDisplay = IPL.rightRingDisplay;
        source = GetComponent<AudioSource>();
        Invoke("LateStart", 2f);
    }

    void LateStart()
    {
        GameObject hand = transform.Find("Render Model for RightHand").Find("grip").Find("attach").gameObject;
        if(hand.GetComponents<SphereCollider>().Length > 1)
        {
            for(int i = 0; i < hand.GetComponents<SphereCollider>().Length-1; i++)
            {
                Destroy(hand.GetComponents<SphereCollider>()[i]);
            }
        }
        SphereCollider handCollider = hand.GetComponent<SphereCollider>();
        handCollider.radius = 0.02f;
        handCollider.center = new Vector3(0.07f, -0.02f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetPressDown(triggerButton))
        //IPL.Set_Left_Interacting(true); IPL.Set_Player_Interacting(true);
        {
            IPL.SetRightTriggerInteracting(true);
            if (anthology)
            {
                if (anthology.active)
                {
                    anthology.activate(true);
                }
            }
        }
        if (controller.GetPressUp(triggerButton))
        //IPL.Set_Left_Interacting(false); IPL.Set_Player_Interacting(false);
        {
            IPL.SetRightTriggerInteracting(false);
            if (objectHovering)
            {
                objectHovering.GetComponent<Rigidbody>().isKinematic = false;
                objectHovering.GetComponent<Rigidbody>().velocity = controller.velocity + new Vector3(controller.velocity.x * -2, 0, controller.velocity.z * -2);
                objectHovering.GetComponent<Rigidbody>().angularVelocity = controller.angularVelocity;
            }
            Invoke("ClearDelayedHold", .25f);

            if(anthology)
            {
                if (anthology.active)
                {
                    anthology.activate(true);
                }
            }
            
        }
        if (controller.GetPressDown(gripButton))
        //IPL.Set_Left_Interacting(true); IPL.Set_Player_Interacting(true);
        {
            IPL.SetRightGripped(true);
            if (aimer)
            {
                aimer.SetActive(true);
                if (IPL.rightHand.CurrentlyInteracting)
                {
                    delayedHold = IPL.rightHand.CurrentlyInteracting.GetComponent<NVRInteractableItem>();
                }
                else if (objectHovering && IPL.rightTriggerInteractive)
                {
                    delayedHold = objectHovering;
                }
            }
        }
        if (controller.GetPressUp(gripButton))
        //IPL.Set_Left_Interacting(false); IPL.Set_Player_Interacting(false);
        {
            if(aimer)
            {
                aimer.SetActive(false);
            }
            
            IPL.SetRightGripped(false);
            /*
            if (IPL.rightHand.CurrentlyInteracting)
            {

                NVRInteractable test = IPL.rightHand.CurrentlyInteracting;
                StartCoroutine(delayForReactivate(test));
                test.GetComponent<NVRInteractableItem>().enabled = false;
                IPL.rightHand.EndInteraction(test);
                test.GetComponent<Rigidbody>().AddForce(raycastOrigin.transform.forward * 500);
                StartCoroutine("vibrateRight", .1f);
                PlaySound(source, sounds[1], true);
            }
            else if (objectHovering && IPL.rightTriggerInteractive)
            {
                NVRInteractableItem test = objectHovering;
                objectHovering = null;
                test.GetComponent<Rigidbody>().isKinematic = false;
                test.GetComponent<Rigidbody>().AddForce(raycastOrigin.transform.forward * 500);
                StartCoroutine("vibrateRight", .1f);
                PlaySound(source, sounds[1], true);
            }
            */
            if(delayedHold)
            {
                if(IPL.rightHand.CurrentlyInteracting)
                {
                    IPL.rightHand.EndInteraction(delayedHold);
                }
                else if(objectHovering && IPL.rightTriggerInteractive)
                {
                    objectHovering = null;
                }
                delayedHold.GetComponent<NVRInteractableItem>().enabled = false;
                delayedHold.GetComponent<Rigidbody>().isKinematic = false;
                delayedHold.GetComponent<Rigidbody>().AddForce(raycastOrigin.transform.forward * 500);
                StartCoroutine("vibrateRight", .1f);
                PlaySound(source, sounds[1], true);
            }
        }

        if(controller.GetPressDown(aButton) || controller.GetPressDown(bButton) || controller.GetPressDown(joystickPress))
        {
            IPL.SetRightButtonInteracting(true);
            anthology.activate(true);
        }
        if(controller.GetPressUp(aButton) || controller.GetPressUp(bButton) || controller.GetPressUp(joystickPress))
        {
            IPL.SetRightButtonInteracting(false);
            anthology.activate(false);
        }


        if(!IPL.GetRightTriggerInteracting())
        {
            hovering = Physics.OverlapBox(raycastOrigin.transform.position + raycastOrigin.transform.forward * IPL.range, IPL.halfExtents, raycastOrigin.transform.rotation);
            objectHovering = IPL.CheckForObject(hovering, raycastOrigin.transform, true);
                           
            //indicator.transform.position = hovering.transform.position;
            if(objectHovering)
            {              
                ringDisplay.transform.position = objectHovering.transform.position;
                ringDisplay.transform.LookAt(Camera.main.transform);
            }
            playedWhoosh = false;
        }
        else
        {           
            if (objectHovering) //&& !IPL.rightHand.CurrentlyInteracting)
            {
                if(!playedWhoosh)
                {
                    StartCoroutine("vibrateRight", .1f);
                    PlaySound(source, sounds[0], true);
                    playedWhoosh = true;
                }
                objectHovering.GetComponent<Rigidbody>().isKinematic = true;
                objectHovering.transform.position = Vector3.MoveTowards(objectHovering.transform.position, raycastOrigin.transform.position, IPL.pullSpeed * Time.deltaTime);
            }
        }
        if (objectHovering && IPL.GetRightTriggerInteracting())
        {
            if(objectHovering.GetComponent<PotionStack>())
            {
                pullPotion = true;
            }
            else
            {
                pullPotion = false;
            }
            pullSomething = true;
        }
        else
        {
            pullSomething = false;
        }
        if (objectHovering && !IPL.GetRightTriggerInteracting())
        {
            ringDisplay.enabled = true;
        }
        else
        {
            ringDisplay.enabled = false;
        }

    }


    public string GetHoldingText()
    {
        if(objectHovering && IPL.GetRightTriggerInteracting())
        {
            return "Holding with pull";
        }
        else if(IPL.rightHand.CurrentlyInteracting)
        {
            return "Holding with NVR";
        }
        else
        {
            return "You're not holding anything.";
        }
    }

    public IEnumerator vibrateRight(float length)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            controller.TriggerHapticPulse((ushort)2000f);
            yield return null;
        }
    }

    public IEnumerator delayForReactivate(NVRInteractable item)
    {
        yield return new WaitForSeconds(.5f);
        item.GetComponent<NVRInteractableItem>().enabled = true;
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

    //0 = nothing
    //1 = uncorked potion
    //2 = unboiled potion
    //3 = boiled but incorrect potion
    //4 = fireball
    //5 = tornado
    //6 = blizzard
    //7 = lightning
    //8 = arcane
    //9 = poison
    //10 = yellow essence
    //11 = magenta essence
    //12 = cyan essence
    //13 = cork
    public int DetermineHolding()
    {
        if (objectHovering && IPL.GetRightTriggerInteracting())
        {
            itemOfInterest = objectHovering.gameObject;
        }
        else if (IPL.rightHand.CurrentlyInteracting)
        {
            itemOfInterest = IPL.rightHand.CurrentlyInteracting.gameObject;
        }
        else
        {
            return 0;
        }       
        if(itemOfInterest.GetComponent<PotionStack>())
        {
            PotionStack potionInHand = itemOfInterest.GetComponent<PotionStack>();
            if (!potionInHand.corked)
            {
                return 1;
            }
            else
            {
                if(!potionInHand.dinged)
                {
                    return 2;
                }
                else
                {
                    return potionInHand.DeterminePotionType() + 3;
                }
            }
        }
        else if(itemOfInterest.GetComponent<Ingredient>())
        {
            return itemOfInterest.GetComponent<Ingredient>().type + 9;
        }
        else
        {
            return 13;
        }

        return 0;
    }

    void ClearDelayedHold()
    {
        delayedHold = null;
    }


}