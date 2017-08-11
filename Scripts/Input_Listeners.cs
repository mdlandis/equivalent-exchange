using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Listeners : MonoBehaviour
{

    public bool leftTriggerInteractive = false;
    public bool rightTriggerInteractive = false;
    public bool leftGripInteractive = false;
    public bool rightGripInteractive = false;
    public bool leftButtonInteractive = false;
    public bool rightButtonInteractive = false;

    public float pullSpeed;
    public Vector3 halfExtents;
    public float range;

    public SpriteRenderer rightRingDisplay;
    public SpriteRenderer leftRingDisplay;

    public NVRHand rightHand;
    public NVRHand leftHand;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetLeftTriggerInteracting()
    {
        return leftTriggerInteractive;
    }
    public bool GetRightTriggerInteracting()
    {
        return rightTriggerInteractive;
    }
    public void SetLeftTriggerInteracting(bool val)
    {
        leftTriggerInteractive = val;
    }
    public void SetRightTriggerInteracting(bool val)
    {
        rightTriggerInteractive = val;
    }

    public void SetLeftGripped(bool val)
    {
        leftGripInteractive = val;
    }
    public void SetRightGripped(bool val)
    {
        rightGripInteractive = val;
    }

    public bool GetLeftButtonInteracting()
    {
        return leftButtonInteractive;
    }
    public bool GetRightButtonInteracting()
    {
        return rightButtonInteractive;
    }
    public void SetLeftButtonInteracting(bool val)
    {
        leftButtonInteractive = val;
    }
    public void SetRightButtonInteracting(bool val)
    {
        rightButtonInteractive = val;
    }


    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance(this);
    }
    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance(this);
    }

    public NVRInteractableItem CheckForObject(Collider[] hovering, Transform raycastOrigin, bool right)
    {
        if(right && rightHand.CurrentlyHoveringOver.Count > 0) 
        {
            return null;
        }
        else if(!right && leftHand.CurrentlyHoveringOver.Count > 0)
        {
            return null;
        }
            
        foreach (Collider c in hovering)
        {
            if (c.GetComponent<NVRInteractableItem>())
            {
                return c.GetComponent<NVRInteractableItem>();
                /*
                Vector3 dir = (c.transform.position - raycastOrigin.position);
                RaycastHit hit;
                if (Physics.Raycast(raycastOrigin.position, dir, out hit))
                {
                    if(hit.collider)
                    {
                        //Debug.Log(hit.collider.tag);// + " " + hit.collider.transform.parent.name);
                        if (hit.collider.gameObject.GetComponent<NVRInteractableItem>())
                        {
                            //Debug.Log(hit.collider.name + " compared to " + c.name);
                            if (hit.collider.name.Equals(c.name))
                            {
                                
                                
                            }
                        }
                    }                   
                } 
                */
            }
        }
        return null;
    }

    /*
    public void FadeObject(float duration, string methodOnFinish, GameObject self, int direction)
    {
        StartCoroutine(PerformFade(duration, methodOnFinish, self, direction));
    }

    
    IEnumerator PerformFade(float duration, string methodOnFinish, GameObject self, int direction)
    {
        Color c = self.GetComponent<Renderer>().material.color;
        for (int i = 1; i <= 100; i++)
        {
            yield return new WaitForSeconds(duration / 100);
            if (direction == 1)
            {
                self.GetComponent<Renderer>().material.color = new Color(c.r, c.b, c.g, i / 100);
            }
            else
            {
                self.GetComponent<Renderer>().material.color = new Color(c.r, c.b, c.g, 1 - (i / 100));
            }
        }
        self.SendMessage(methodOnFinish);
    }
    */
}