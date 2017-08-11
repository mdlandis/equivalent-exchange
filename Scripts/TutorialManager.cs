using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    public TextMeshPro tutorialText;
    public int stage;
    public Boiler boiler;
    private float delay = 2.0f;

    Input_Listeners IPL;


    public Goblin_ro_ctrl enemy;
    public Goblin_ro_ctrl newEnemy;

    public Transform boundary1;
    public Transform boundary2;

    public GameObject indicator1;
    public GameObject indicator2;

    private void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        StartCoroutine("CheckTutorialProgress");
        PlayerPrefs.SetInt("Level", 1);
    }

    IEnumerator CheckTutorialProgress()
    {
        while(true)
        {
            yield return new WaitForSeconds(.25f);
            if (stage == 0)
            {
                if(IPL.leftHand && IPL.rightHand)
                {
                    if (IPL.leftHand.GetComponent<Left_VR_Cont>().pullPotion || IPL.rightHand.GetComponent<Right_VR_Cont>().pullPotion)
                    {
                        ProgressTutorial();
                    }
                }
                
            }
            else if(stage == 1)
            {
                foreach(GameObject pot in GameObject.FindGameObjectsWithTag("Potion"))
                {
                    
                    if(pot.GetComponent<PotionStack>())
                    {
                        PotionStack p = pot.GetComponent<PotionStack>();
                        if(p.contents.Count == 2)
                        {
                            if (p.contents[0].type * 10 + p.contents[1].type == 12 || p.contents[0].type * 10 + p.contents[1].type == 21) //yellow + magenta
                            {
                                ProgressTutorial();
                                break;
                            }
                        }                     
                    }
                }
            }
            else if(stage == 2)
            {
                foreach(PotionStack p in boiler.potionsOnBoiler)
                {
                    if(p.DeterminePotionType() == 1)
                    {
                        ProgressTutorial();
                        break;
                    }
                }
            }
            else if(stage == 3)
            {
                if((IPL.GetRightButtonInteracting() && (IPL.rightHand.CurrentlyInteracting || (IPL.rightHand.GetComponent<Right_VR_Cont>().objectHovering && IPL.GetRightTriggerInteracting()))) || (IPL.GetLeftButtonInteracting() && (IPL.leftHand.CurrentlyInteracting || (IPL.leftHand.GetComponent<Left_VR_Cont>().objectHovering && IPL.GetLeftTriggerInteracting()))))
                {
                    ProgressTutorial();
                }
            }
            else if(stage == 4)
            {
                if(GameObject.FindGameObjectWithTag("Enemy").GetComponent<Goblin_ro_ctrl>().dead)
                {
                    ProgressTutorial();
                }
            }
            else if(stage == 5)
            {
                if(allEnemiesDead())
                {
                    ProgressTutorial();
                }                
            }
        }       
    }

    private void ProgressTutorial()
    {
        GetComponent<AudioSource>().Play();
        stage++;
        if(stage == 1)
        {
            tutorialText.text = "Add a Yellow Essence and a Magenta Essence to your potion to make it a Fireball potion.";
            indicator1.transform.localPosition = new Vector3(-0.015f, -.42f, -1.26f);
            indicator2.transform.localPosition = new Vector3(0.485f, -.42f, -1.26f);
            indicator1.GetComponent<ParticleSystem>().time = 0;
            indicator2.GetComponent<ParticleSystem>().time = 0;

        }
        if(stage == 2)
        {
            tutorialText.text = "Put a cork on your potion and place it on the surface in front of you to boil it.";
            indicator1.transform.localPosition = new Vector3(-.283f, .47f, -1.205f);
            indicator2.transform.position = new Vector3(0, 0, 0);
        }
        if(stage == 3)
        {
            tutorialText.text = "Pick up the potion and press any button on top of that controller to read about what it does.";
            indicator1.transform.position = new Vector3(0, 0, 0);
        }
        if(stage == 4)
        {
            tutorialText.text = "Throw your potion at the enemy to your right. \n\nYou can also use the Grip button to shoot your potion out.";
        }
        if(stage == 5)
        {
            tutorialText.text = "Try combining different ingredients and reading about what each potion does. \nKill all 10 enemies to start the game!";
            for(int i = 0; i < 10; i++)
            {
                newEnemy = Instantiate(enemy) as Goblin_ro_ctrl;
                newEnemy.runSpeed = 0;
                newEnemy.slowedSpeed = 0;
                float randx = Random.Range(boundary1.position.x, boundary2.position.x);
                float randz = Random.Range(boundary1.position.z, boundary2.position.z);
                newEnemy.transform.position = new Vector3(randx, 0.3f, randz);
            }
        }
        if(stage == 6)
        {
            tutorialText.text = "Complete";
            StartCoroutine("DelaySceneTransition");
        }
    }

    public bool allEnemiesDead()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(!enemy.GetComponent<Goblin_ro_ctrl>().dead)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator DelaySceneTransition()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("StoryScene");
    }


}
