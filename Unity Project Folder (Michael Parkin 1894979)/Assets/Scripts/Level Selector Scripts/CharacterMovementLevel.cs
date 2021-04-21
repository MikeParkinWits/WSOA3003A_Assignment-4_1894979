using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovementLevel : MonoBehaviour
{

    [Header("Character Information")]
    public float speed = 1f;
    public bool isMoving { get; set; }

    public LevelLocationsScript currentPoint { get; set; }
    private LevelLocationsScript nextPoint;
    private LevelManager levelManager;

    private void Start()
    {
        //currentPoint.dialogueManager.dialogueActive = false;
        //HERE IF ERROR OF DIALOGUE ACTIVE (dialogueActive) OCCURS!!!

        currentPoint.dialogueManager.canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextPoint != null)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = nextPoint.transform.position;

            if (Vector3.Distance(currentPosition, targetPosition) > 0.0001f)
            {
                transform.position = Vector3.MoveTowards( currentPosition, targetPosition, Time.deltaTime * speed);
                PlayerPrefs.SetInt("Just Battled", 0);
                PlayerPrefs.SetInt("Just In Dojo", 0);
            }
            else
            {
                if (nextPoint.isAutomatic)
                {
                    LevelLocationsScript point = nextPoint.GetNextPoint(currentPoint);
                    MoveToPoint(point);
                }
                else
                {
                    SetCurrentPoint(nextPoint);
                }
            }
        }
    }

    internal void Instantiate(LevelManager levelManager, LevelLocationsScript pointToSpawn)
    {
        this.levelManager = levelManager;
        SetCurrentPoint(pointToSpawn);

    }

    public void TrySetDirection(Directions direction)
    {
        LevelLocationsScript point = currentPoint.GetLevelDirections(direction);

        if (point != null)
        {
            MoveToPoint(point);
        }
    }

    private void MoveToPoint(LevelLocationsScript point)
    {
        nextPoint = point;
        isMoving = true;
    }

    public void SetCurrentPoint(LevelLocationsScript point)
    {
        currentPoint = point;
        nextPoint = null;
        transform.position = point.transform.position;
        isMoving = false;
        
        levelManager.UpdateUI();

        float chance = UnityEngine.Random.Range(0f, 1f);

        if (currentPoint.name == "Point 3" && PlayerPrefs.GetInt("Point 2 Locked") == 1 && PlayerPrefs.GetInt("Just Battled") != 1)
        {

            if (PlayerPrefs.GetInt("New Partner Gained") == 0)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoad);
            }
            else if (PlayerPrefs.GetInt("New Partner Gained") == 1)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoadTwoPlayers);
            }
        }

        else if (currentPoint.name == "Point 11" && PlayerPrefs.GetInt("Point 6 Locked") == 1 && PlayerPrefs.GetInt("Just Battled") != 1)
        {

            if (PlayerPrefs.GetInt("New Partner Gained") == 0)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoad);
            }
            else if (PlayerPrefs.GetInt("New Partner Gained") == 1)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoadTwoPlayers);
            }
        }

        else if (currentPoint.name == "Point 9" && PlayerPrefs.GetInt("Point 10 Locked") == 1 && PlayerPrefs.GetInt("Just Battled") != 1)
        {

            if (PlayerPrefs.GetInt("New Partner Gained") == 0)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoad);
            }
            else if (PlayerPrefs.GetInt("New Partner Gained") == 1)
            {
                levelManager.LoadSceneButton(currentPoint.sceneToLoadTwoPlayers);
            }
        }

        else if (!currentPoint.hideIcon && !currentPoint.hasDialogue && currentPoint.name != "Point 1" && !currentPoint.onlyTreasure)
        {

            float battleSceneChance = 0.5f;
            float coinChance = 0.8f;
            
            if (PlayerPrefs.GetInt("LosingStreak") == 2)
            {
                battleSceneChance = 0.4f;
                coinChance = 0.75f;
            }
            else if (PlayerPrefs.GetInt("LosingStreak") >= 3)
            {
                battleSceneChance = 0.4f;
                coinChance = 0.85f;
            }
            
            if (PlayerPrefs.GetInt("WinningStreak") == 2)
            {
                battleSceneChance = 0.55f;
                coinChance = 0.7f;
            }
            else if (PlayerPrefs.GetInt("WinningStreak") >= 3)
            {
                battleSceneChance = 0.65f;
                coinChance = 0.75f;
            }
            else if (PlayerPrefs.GetInt("WinningStreak") >= 5)
            {
                battleSceneChance = 0.7f;
                coinChance = 0.8f;
            }


            if (chance <= battleSceneChance && PlayerPrefs.GetInt("Just Battled") != 1)
            {       
                if (PlayerPrefs.GetInt("New Partner Gained") == 0)
                {
                    levelManager.LoadSceneButton(currentPoint.sceneToLoad);
                }
                else if (PlayerPrefs.GetInt("New Partner Gained") == 1)
                {
                    levelManager.LoadSceneButton(currentPoint.sceneToLoadTwoPlayers);
                }

            }
            else if (chance > battleSceneChance && chance <= coinChance && PlayerPrefs.GetInt("Just Battled") != 1)
            {
                int coins = UnityEngine.Random.Range(10, 20);
                currentPoint.dialogueLines[0] = "Found " + coins + " coins!";
                coins += PlayerPrefs.GetInt("TotalCoins");
                PlayerPrefs.SetInt("TotalCoins", coins);

                levelManager.UpdateDialogueBox();

            }
            else if (chance > coinChance && chance <= 1f)
            {

            }
        }

        if (currentPoint.hasDialogue)
        {
            if (point.name == "Point 1")
            {
                if (PlayerPrefs.GetInt("BeginningTextDone") == 0)
                {
                    levelManager.UpdateDialogueBox();
                    PlayerPrefs.SetInt("BeginningTextDone", 1);
                }
            }
            else
            {
                levelManager.UpdateDialogueBox();
            }

        }

        if (currentPoint.onlyTreasure && (PlayerPrefs.GetInt("Teasure Pickup") == 0))
        {

            PlayerPrefs.SetInt("Treasure Pickup", 1);

            int coins = UnityEngine.Random.Range(30, 40);
            currentPoint.dialogueLines[0] = "You found a chest with " + coins + " coins!";
            coins += PlayerPrefs.GetInt("TotalCoins");
            PlayerPrefs.SetInt("TotalCoins", coins);

            levelManager.UpdateDialogueBox();
        }
    }
}
