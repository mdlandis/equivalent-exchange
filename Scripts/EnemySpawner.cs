using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour {

    public TextMeshPro timerText;
    public TextMeshPro levelText;
    public TextMeshPro highscoreText;


    public int enemiesToSpawnScalar;
    public int enemiesThisLevel;
    public int enemiesSoFar;
    int wave;

    public int spawnDelay;
    public int cDelay;
    public GameObject enemy;
    public int groupMin;
    public int groupMax;
    public List<Transform> possibleSpawnPoints = new List<Transform>();

    GameObject newEnemy;

    public List<Goblin_ro_ctrl> enemiesRemaining = new List<Goblin_ro_ctrl>();

    int level;
    int levelAdjust;
    int bruteThreshhold;

    int rand;
    int numToSpawn;

    GameManager GM;

	// Use this for initialization
	void Start () {
        wave = 0;
        
        //PlayerPrefs.SetInt("HighScore", 1);
        GM = Singleton_Service.GetSingleton<GameManager>();
        possibleSpawnPoints.Add(this.transform);
        StartCoroutine("SpawnEnemies");
        level = PlayerPrefs.GetInt("Level");
        if(level > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", level);
        }
        bruteThreshhold = level * 1;
        levelAdjust = level - 1;
        levelText.text = "Level " + level;
        highscoreText.text = "High Score: Level " + PlayerPrefs.GetInt("HighScore");

        enemiesThisLevel += enemiesToSpawnScalar * levelAdjust;
        spawnDelay = 25 - levelAdjust * 5;
        if(spawnDelay < 10)
        {
            spawnDelay = 10;
        }
        GM.announcer.PlaySound((30 - spawnDelay)/5);
        


	}
	
	IEnumerator SpawnEnemies()
    {
        while(enemiesSoFar < enemiesThisLevel)
        {
            yield return new WaitForSeconds(1);
            cDelay++;
            timerText.text = "Next Wave: " + (spawnDelay - cDelay);
            if(cDelay == spawnDelay)
            {
                
                numToSpawn = Random.Range(groupMin, groupMax + 1);
                enemiesSoFar += numToSpawn;
                wave++;
                if (wave == 1)
                {
                    GM.announcer.PlaySound(5);
                }
                else if(enemiesSoFar >= enemiesThisLevel)
                {
                    GM.announcer.PlaySound(6);
                }
                else
                {
                    GM.announcer.PlaySound(7);
                }

                for (int i = 0; i < numToSpawn; i++)
                {
                    rand = Random.Range(0, possibleSpawnPoints.Count);
                    newEnemy = Instantiate(enemy) as GameObject;
                    newEnemy.transform.position = possibleSpawnPoints[rand].position;
                    if(Random.Range(0, 101) >= 96 - bruteThreshhold && level > 2)
                    {
                        newEnemy.GetComponent<Goblin_ro_ctrl>().brute = true;
                        GetComponent<AudioSource>().Play();
                    }
                    yield return new WaitForSeconds(.2f);
                    
                }

                cDelay = 0;
            }
                       
        }
        timerText.text = "No more waves!";
        timerText.color = Color.magenta;
        
        while(true)
        {           
            if(SeeIfAllEnemiesDead())
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }
        timerText.text = "You Win!";
        timerText.color = Color.green;
        GM.win();

    }

    bool SeeIfAllEnemiesDead()
    {
        enemiesRemaining.Clear();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemiesRemaining.Add(enemy.GetComponent<Goblin_ro_ctrl>());
        }
        foreach (Goblin_ro_ctrl enemy in enemiesRemaining)
        {
            if(!enemy.dead)
            {
                return false;
            }
        }
        return true;
    }
}
