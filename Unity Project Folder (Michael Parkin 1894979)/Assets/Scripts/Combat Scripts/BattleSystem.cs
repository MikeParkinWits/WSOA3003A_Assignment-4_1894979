using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, UPNEXT, PLAYERTURN, ENEMYTURN, WON, LOST  }

public class BattleSystem : MonoBehaviour
{
    [Header("UI References")]
    public Text dialogueText;
    public GameObject actionsUI;

    [Header("UI Turn Order")]
    public Text[] turnsUI = new Text[4];

    [Header("Current State")]
    public BattleState state;

    [Header("Instance References")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleUI playerUI;
    public BattleUI enemyUI;

    [Header("Unit UI References")]
    public GameObject[] characterUnitsUI = new GameObject[3];
    public GameObject[] enemyUnitsUI = new GameObject[3];

    [Header("Unit References")]
    public Unit[] playerUnit;
    public Unit[] enemyUnit;
    private int currentPlayerNum = 0;
    private int currentEnemyNum = 0;

    [Header("Code Variables")]
    public float textDelay = 2f;
    public List<Unit> turnOrder = new List<Unit>();

    [Header("Objects to Instantiate")]
    public GameObject[] playerCharacters;
    public GameObject[] enemyCharacters;
    public Transform[] unitSpawnPoints = new Transform[6];
    public Transform[] enemySpawnPoints = new Transform[6];
    public int unitSpawnOffset = 0;
    public int enemySpawnOffset = 0;

    public Actions actions;

    public int currentAttackDamage;
    public int currentAttackSpeed;
    public float currentSpecialBonus;
    public float currentAccuracy;
    public float specialAccuracyBuffer = 0f;
    public float specialMoveMultiplier = 1f;
    public float currentActionNum;

    public GameObject enemySelectPanel;
    public int enemyAttackSelection;

    public GameObject enemyOneSprite;
    public GameObject enemyOneUISprite;

    public GameObject enemyTwoUISprite;

    public GameObject unitOneUISprite;
    public GameObject unitTwoUISprite;

    public GameObject actionButtonsUI;

    public bool specialInputActive = false;
    public bool specialInputAchieved = false;
    public float specialInputTimer;
    public GameObject specialTimeIndicator;

    public int damageCalculated;
    public float damageCalculatedFloat;

    public Sprite[] turnOrderSpriteToSwap;
    public Image[] turnOrderImage;

    public GameObject[] turnOrderTiles;

    public Slider[] playerHealthSliders;
    public Slider[] enemyHealthSliders;

    public Animator[] playerAnimator;

    public Animator[] enemyAnimator;

    public Text[] playerDamageAnimationText;
    public Text[] enemyDamageAnimationText;

    public GameObject middleOfScene;

    public bool hasStarPos = false;

    Vector3 startPos = Vector3.zero;

    public EventScript eventScript;

    public bool specialAllowed = true;


    // Start is called before the first frame update
    void Start()
    {

        //actionsUI.SetActive(false);

        PlayerPrefs.SetInt("SpecialStreak", 0);

        hasStarPos = false;

        state = BattleState.START;

        StartCoroutine(BattleSetup());

        eventScript = GameObject.FindGameObjectWithTag("Player 1").GetComponent<EventScript>();

        turnOrder = turnOrder.OrderBy(w => w.unitSpeed).ToList();

        int startSpeedBuffer = turnOrder.First().unitSpeed;

        if (turnOrder.First().unitSpeed > 0)
        {
            foreach (var x in turnOrder)
            {
                x.unitSpeed -= startSpeedBuffer;
                //Debug.Log(x.ToString() + " " + x.unitSpeed);
            }
        }

        if (playerCharacters.Length >= 1)
        {
            if (playerUnit[0].unitSpeed == 0)
            {
                currentPlayerNum = 0;
            }

            if (playerCharacters.Length >= 2)
            {
                if (playerUnit[1].unitSpeed == 0)
                {
                    currentPlayerNum = 1;
                }

                if (playerCharacters.Length == 3)
                {
                    
                    if (playerUnit[2] != null && playerUnit[2].unitSpeed == 0)
                    {
                        currentPlayerNum = 2;
                    }
                }
            }
        }



        int count = 0;

        foreach (var x in turnOrder)
        {
            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitName.ToString() + " " + x.unitSpeed;
            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
            count++;
        }

        int newCount = 0;

        //Debug.Log("Animator: " + playerAnimator[0]);

        int otherNewCount = 0;


    }

    void UnitBattleSpawnLocation()
    {
        if (playerCharacters.Length == 1)
        {
            unitSpawnOffset = 0;
        }
        else if (playerCharacters.Length == 2)
        {
            unitSpawnOffset = 1;
        }
        else if (playerCharacters.Length == 3)
        {
            unitSpawnOffset = 3;
        }
    }

    void EnemyBattleSpawnLocation()
    {
        if (enemyCharacters.Length == 1)
        {
            enemySpawnOffset = 0;
        }
        else if (enemyCharacters.Length == 2)
        {
            enemySpawnOffset = 1;
        }
        else if (enemyCharacters.Length == 3)
        {
            enemySpawnOffset = 3;
        }
    }

    void Update()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 0)
        {
            PlayerPrefs.SetInt("TotalCoins", 0);
        }

        //actionsUI.SetActive(false);

        new Vector3(0.1f * Time.deltaTime, 0, 0);

        //playerUI.currentTurnIndicator.SetActive(false);

        if (PlayerPrefs.GetInt("Special Bonus Learnt") == 1)
        {

            if (PlayerPrefs.GetInt("Special Start") == 1 && specialAllowed == true)
            {
                //Debug.Log("Special SPEEECCCIIIAAALLLLL");

                if (Input.GetKeyDown(KeyCode.Space))
                {

                    specialInputAchieved = true;
                    PlayerPrefs.SetInt("SpecialBonusAchieved", 1);
                }
            }

            if (PlayerPrefs.GetInt("Special Start") == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    specialAllowed = false;
                }
            }

        }

        int countSliders = 0;

        foreach (var x in playerHealthSliders)
        {

            //Debug.Log("Slider: " + countSliders);

            x.maxValue = playerUnit[countSliders].unitMaxHP;
            x.value = playerUnit[countSliders].unitCurrentHP;
            countSliders++;

            
        }

        countSliders = 0;

        foreach (var x in enemyHealthSliders)
        {

            //Debug.Log("Slider: " + countSliders);

            x.maxValue = enemyUnit[countSliders].unitMaxHP;
            x.value = enemyUnit[countSliders].unitCurrentHP;
            countSliders++;


        }

        int count = 0;

        foreach (var x in turnOrder)
        {
            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitSpeed.ToString();

                if (x.uniqueNum == 1)
                {
                    turnOrderImage[count].sprite = turnOrderSpriteToSwap[0];
                }
                else if (x.uniqueNum == 2)
                {
                    turnOrderImage[count].sprite = turnOrderSpriteToSwap[1];
                }
                else if (x.uniqueNum == 3)
                {
                    turnOrderImage[count].sprite = turnOrderSpriteToSwap[2];
                }

                if (turnOrder.Count == 3)
                {
                    turnOrderTiles[3].SetActive(false);
                    //turnsUI[3].text = " ";
                }

                if (turnOrder.Count == 2)
                {
                    turnOrderTiles[3].SetActive(false);
                    turnOrderTiles[2].SetActive(false);
                    //turnsUI[3].text = " ";
                    //turnsUI[2].text = " ";
                }

                //Debug.Log("Hello: " + x.uniqueNum);

            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
            count++;
        }

        //Debug.Log("Current Player: " + currentPlayerNum);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*
            turnOrder.First().unitSpeed += 10;

            Debug.Log("New Order");

            foreach (var x in turnOrder)
            {
                Debug.Log(x.ToString() + " " + x.unitSpeed);
            }
            */


            //Debug.Log("Player Unit 1: " + playerUnit[0].unitSpeed);

            SceneManager.LoadScene("Test Menu");

        }

    }

    private IEnumerator BattleSetup()
    {

        specialTimeIndicator.SetActive(false);
        specialInputActive = false;
        specialInputTimer = 2f;


        actionsUI.SetActive(false);
        enemySelectPanel.SetActive(false);

        enemyOneSprite.SetActive(true);
        enemyOneUISprite.SetActive(true);

        enemyTwoUISprite.SetActive(true);

        unitOneUISprite.SetActive(true);
        unitTwoUISprite.SetActive(true);

        actionButtonsUI.SetActive(false);

        UnitBattleSpawnLocation();

        foreach (var x in turnOrderTiles)
        {
            x.SetActive(true);
        }

        playerUI.SetUnitUI();

        GameObject playerGameObject;

        int playerUnitIndex= 0;

        playerUnit = new Unit[playerCharacters.Length];
        enemyUnit = new Unit[enemyCharacters.Length];

        foreach (var x in playerCharacters)
        {
            playerGameObject = Instantiate(x, unitSpawnPoints[unitSpawnOffset + playerUnitIndex]);
            //Debug.Log("Count: " + playerGameObject.GetComponent<Unit>());
            playerUnit[playerUnitIndex] = playerGameObject.GetComponent<Unit>();
            playerAnimator[playerUnitIndex] = playerGameObject.GetComponent<Animator>();
            playerDamageAnimationText[playerUnitIndex] = playerGameObject.GetComponentInChildren<Text>();

            if (PlayerPrefs.GetInt("Stat 1 Upgraded") == 1)
            {
                if (PlayerPrefs.GetInt("Stat 2 Upgraded") == 1)
                {
                    float health = playerUnit[playerUnitIndex].unitMaxHP * 2f;
                    playerUnit[playerUnitIndex].unitMaxHP = (int)health;
                    playerUnit[playerUnitIndex].unitCurrentHP = enemyUnit[playerUnitIndex].unitMaxHP;
                }
                else if (PlayerPrefs.GetInt("Stat 2 Upgraded") == 0)
                {
                    float health = playerUnit[playerUnitIndex].unitMaxHP * 1.5f;
                    playerUnit[playerUnitIndex].unitMaxHP = (int) health;
                    playerUnit[playerUnitIndex].unitCurrentHP = playerUnit[playerUnitIndex].unitMaxHP;
                }

            }

            playerUnitIndex++;
            //Debug.Log("Count: " + count);

        }

        //GameObject playerGameObject = Instantiate(playerPrefab, playerBattleStation);

        EnemyBattleSpawnLocation();

        enemyUI.SetEnemyUI();

        GameObject enemyGameObject;

        int enemyUnitIndex = 0;

        foreach (var x in enemyCharacters)
        {
            enemyGameObject = Instantiate(x, enemySpawnPoints[enemySpawnOffset + enemyUnitIndex]);
            enemyUnit[enemyUnitIndex] = enemyGameObject.GetComponent<Unit>();
            enemyAnimator[enemyUnitIndex] = enemyGameObject.GetComponent<Animator>();
            enemyDamageAnimationText[enemyUnitIndex] = enemyGameObject.GetComponentInChildren<Text>();


            enemyUnitIndex++;
        }

        //GameObject enemyGameObject = Instantiate(enemyPrefab, enemyBattleStation);

        if (enemyUnit.Length == 1)
        {
            dialogueText.text = "An Enemy approaches...";
        }
        else if (enemyUnit.Length >= 1)
        {
            dialogueText.text = "Enemies are approaching...";
        }

        if (playerCharacters.Length >= 1)
        {
            playerUI.SetUI(playerUnit[0], 0);

            if (playerCharacters.Length >= 2)
            {
                playerUI.SetUI(playerUnit[1], 1);

                if (playerCharacters.Length == 3)
                {
                    playerUI.SetUI(playerUnit[2], 2);
                }
            }
        }

        if (enemyCharacters.Length >= 1)
        {
            enemyUI.SetUI(enemyUnit[0], 0);

            if (enemyCharacters.Length >= 2)
            {
                enemyUI.SetUI(enemyUnit[1], 1);

                if (enemyCharacters.Length == 3)
                {
                    enemyUI.SetUI(enemyUnit[2], 2);
                }
            }
        }

        yield return new WaitForSeconds(textDelay);

        playerUI.UIIndicator();

        state = BattleState.UPNEXT;
        UpNext();
    }

    void UpNext()
    {

        hasStarPos = false;

        if(playerCharacters.Length == 1)
        {
            if (playerUnit[currentPlayerNum].IsDead())
            {
                state = BattleState.LOST;

                EndBattle();
            }
            else
            {
                if (turnOrder.First().unitType == UnitType.PLAYER)
                {
                    state = BattleState.PLAYERTURN;

                    PlayerTurn();
                }
                else if (turnOrder.First().unitType == UnitType.ENEMY)
                {
                    state = BattleState.ENEMYTURN;

                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else if (playerCharacters.Length == 2)
        {
            if (playerUnit[0].IsDead() && playerUnit[1].IsDead())
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else if (!playerUnit[currentPlayerNum].IsDead())
            {            
                if (turnOrder.First().unitType == UnitType.PLAYER)
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
                else if (turnOrder.First().unitType == UnitType.ENEMY)
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
            else
            {
                playerUI.UIIndicator();

                if (turnOrder.First().unitType == UnitType.PLAYER)
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
                else if (turnOrder.First().unitType == UnitType.ENEMY)
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else if (playerCharacters.Length == 3)
        {
            if (playerUnit[0].IsDead() && playerUnit[1].IsDead() && playerUnit[2].IsDead())
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else if (!playerUnit[currentPlayerNum].IsDead())
            {
                if (turnOrder.First().unitType == UnitType.PLAYER)
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
                else if (turnOrder.First().unitType == UnitType.ENEMY)
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }


        //Checking if Won


        if (enemyCharacters.Length == 1)
        {
            if (enemyUnit[0].IsDead())
            {
                state = BattleState.WON;
                EndBattle();
            }
        }
        else if (enemyCharacters.Length == 2)
        {
            if (enemyUnit[0].IsDead() && enemyUnit[1].IsDead())
            {
                state = BattleState.WON;
                EndBattle();
            }
        }
        else if (enemyCharacters.Length == 3)
        {
            if (enemyUnit[0].IsDead() && enemyUnit[1].IsDead() && enemyUnit[2].IsDead())
            {
                state = BattleState.WON;
                EndBattle();
            }
        }

    }

    void PlayerTurn()
    {
        actionButtonsUI.SetActive(true);

        dialogueText.text = "Choose an action: ";

        if (playerUnit.Length == 1)
        {
            if (playerUnit[0].unitSpeed == 0)
            {
                currentPlayerNum = 0;
            }
        }

        if (playerUnit.Length == 2)
        {
            if (playerUnit[0].unitSpeed == 0)
            {
                currentPlayerNum = 0;
            }
            else if (playerUnit[1].unitSpeed == 0)
            {
                currentPlayerNum = 1;
            }
        }

        if (playerUnit.Length == 3)
        {
            if (playerUnit[2].unitSpeed == 0)
            {
                currentPlayerNum = 2;
            }
        }

        actionsUI.SetActive(false);
        enemySelectPanel.SetActive(false);
    }

    IEnumerator PlayerAttack()
    {
        actionsUI.SetActive(false);
        enemySelectPanel.SetActive(false);

        actionButtonsUI.SetActive(false);


        startPos = playerUnit[currentPlayerNum].transform.position;

        //Debug.Log("Start Pos: ");




        //Debug.Log("Start Pos: " + startPos + hasStarPos);

        while (playerUnit[currentPlayerNum].transform.position != middleOfScene.transform.position)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, middleOfScene.transform.position, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.UIIndicator();
            //Debug.Log("Start Pos: " + startPos + hasStarPos);


        }


        playerUnit[currentPlayerNum].NormalAttackSpeed();

        //Debug.Log("New Order");

        int count = 0;

        foreach (var x in turnOrder)
        {

            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitName.ToString() + " " + x.unitSpeed;
            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
        }



        if (currentActionNum == 0
        || currentActionNum == 3
            || currentActionNum == 6)
        {
            playerAnimator[currentPlayerNum].SetTrigger("QuickPlay");
        }

        if (currentActionNum == 1
                || currentActionNum == 4
                    || currentActionNum == 7)
        {
            playerAnimator[currentPlayerNum].SetTrigger("StandardPlay");
        }

        if (currentActionNum == 2
                || currentActionNum == 5
                    || currentActionNum == 8)
        {
            playerAnimator[currentPlayerNum].SetTrigger("HeavyPlay");
        }



        specialInputTimer = currentSpecialBonus;

        yield return new WaitForSeconds(1.5f);

        //specialInputActive = true;

        //yield return new WaitForSeconds(currentSpecialBonus + 0.01f);

        if (specialInputAchieved)
        {
            specialAccuracyBuffer = 0.9f;
            specialMoveMultiplier = 1.5f;
        }
        else
        {
            specialAccuracyBuffer = 1.1f;
            specialMoveMultiplier = 1f;
        }

        //Debug.Log("Special: " + specialInputAchieved);
        //Debug.Log("Special Allowed: " + specialAllowed);


        specialInputAchieved = false;
        PlayerPrefs.SetInt("SpecialBonusAchieved", 0);

        specialAllowed = true;

        int specialStreakBonus = 0;

        if (PlayerPrefs.GetInt("SpecialStreak") == 0)
        {
            specialStreakBonus = 0;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") == 1)
        {
            specialStreakBonus = 2;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") >= 3)
        {
            specialStreakBonus = 4;
        }

        damageCalculated = (currentAttackDamage + playerUnit[currentPlayerNum].attackPower + UnityEngine.Random.Range(-2, 2)) - enemyUnit[0].defensePower;
        damageCalculatedFloat = (damageCalculated * specialMoveMultiplier) - specialStreakBonus;
        damageCalculated = (int) damageCalculatedFloat;

        if (UnityEngine.Random.Range(0f, 10f/specialAccuracyBuffer) <= currentAccuracy)
        {
            enemyUnit[0].TakeDamage(damageCalculated);

            enemyUI.CreateDamagePopupUI(enemyUnit[0].gameObject.transform.position, damageCalculated);

            enemyUnit[0].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[0].gameObject.SetActive(true);

            if (specialAccuracyBuffer == 0.9f)
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Successful, damage increased by 1.5x \n";

                //enemyDamageAnimationText[0].text = damageCalculated.ToString();
            }
            else
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Missed, no damage increase. \n";

                //enemyDamageAnimationText[0].text = damageCalculated.ToString();
            }

        }
        else
        {
            enemyUI.CreateDamagePopupUI(enemyUnit[0].gameObject.transform.position, -1);

            dialogueText.text = "The attack was not successful!";

            //enemyDamageAnimationText[0].text = "MISSED";
        }



        while (playerUnit[currentPlayerNum].transform.position != startPos)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, startPos, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.currentTurnIndicator.SetActive(false);
        }



        enemyUI.SetHP(enemyUnit[0].unitCurrentHP, 0);

        yield return new WaitForSeconds(textDelay + 2);

        playerUI.UIIndicator();


        state = BattleState.UPNEXT;

        if (DeathChecker(0))
        {
            dialogueText.text = "Enemy knocked out";

            enemyUnit[0].unitSpeed = -1;

            turnOrder.Remove(enemyUnit[0]);

            state = BattleState.WON;


            yield return new WaitForSeconds(textDelay);
            EndBattle();
        }

        UpNext();
    }

    IEnumerator EnemyTurn()
    {

        if (enemyUnit[0].unitSpeed == 0)
        {
            currentEnemyNum = 0;
        }
        else if (enemyUnit[1].unitSpeed == 0)
        {
            currentEnemyNum = 1;
        }
        else if (enemyUnit.Length == 2 & enemyUnit[2].unitSpeed == 0)
        {
            currentEnemyNum = 2;
        }

        startPos = enemyUnit[currentEnemyNum].transform.position;

            //Debug.Log("Start Pos: ");

        


        //Debug.Log("Start Pos: " + startPos + hasStarPos);

        while (enemyUnit[currentEnemyNum].transform.position != middleOfScene.transform.position)
        {
            enemyUnit[currentEnemyNum].transform.position = Vector3.MoveTowards(enemyUnit[currentEnemyNum].transform.position, middleOfScene.transform.position, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.UIIndicator();
            //Debug.Log("Start Pos: " + startPos + hasStarPos);


        }


        int randomNum = UnityEngine.Random.Range(0, 2);

        if (randomNum == 1)
        {
            actions.EnemyQuickMove();

            currentAttackDamage = actions.actionDamage;
            currentAttackSpeed = actions.actionSpeed;
            currentAccuracy = actions.actionAccuracy;
        }
        else if (randomNum == 2)
        {
            actions.EnemyStandardMove();

            currentAttackDamage = actions.actionDamage;
            currentAttackSpeed = actions.actionSpeed;
            currentAccuracy = actions.actionAccuracy;
        }

        enemyUnit[currentEnemyNum].NormalAttackSpeed();

        //Debug.Log("New Order");

        //Debug.Log("Start Pos: " + startPos);

        int count = 0;

        foreach (var x in turnOrder)
        {
            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitName.ToString() + " " + x.unitSpeed;
            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
        }

        dialogueText.text = enemyUnit[currentEnemyNum].unitName + " attacks!";

        yield return new WaitForSeconds(textDelay);

        //playerUI.UIIndicator();

        int num = 0;

        if (playerUnit.Length >= 1)
        {
            num = 0;

            if (playerUnit.Length >= 2)
            {
                if (!playerUnit[0].IsDead())
                {
                    num = UnityEngine.Random.Range(0, playerUnit.Length);
                }
                else
                {
                    num = 1;
                }

                if (playerUnit[1].IsDead())
                {
                    num = 0;
                }

                if (playerUnit.Length == 3)
                {
                    if (!playerUnit[0].IsDead() && !playerUnit[1].IsDead() && !playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[3];
                        numbers[0] = 0;
                        numbers[1] = 1;
                        numbers[2] = 2;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (playerUnit[0].IsDead() && !playerUnit[1].IsDead() && !playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[2];
                        numbers[0] = 1;
                        numbers[1] = 2;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (!playerUnit[0].IsDead() && playerUnit[1].IsDead() && !playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[2];
                        numbers[0] = 0;
                        numbers[1] = 2;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (!playerUnit[0].IsDead() && !playerUnit[1].IsDead() && playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[2];
                        numbers[0] = 0;
                        numbers[1] = 1;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (playerUnit[0].IsDead() && playerUnit[1].IsDead() && !playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[1];
                        numbers[0] = 0;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (!playerUnit[0].IsDead() && playerUnit[1].IsDead() && !playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[1];
                        numbers[0] = 1;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                    else if (!playerUnit[0].IsDead() && !playerUnit[1].IsDead() && playerUnit[3].IsDead())
                    {
                        int[] numbers = new int[1];
                        numbers[0] = 2;
                        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);

                        num = numbers[randomIndex];
                    }
                }
            }
        }






        if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
        {
            specialAccuracyBuffer = 0.9f;
            specialMoveMultiplier = 1.5f;
        }
        else
        {
            specialAccuracyBuffer = 1f;
            specialMoveMultiplier = 1f;
        }

        specialInputAchieved = false;
        PlayerPrefs.SetInt("SpecialBonusAchieved", 0);

        specialAllowed = true;

        damageCalculated = (currentAttackDamage + enemyUnit[currentEnemyNum].attackPower + UnityEngine.Random.Range(-2, 2)) - playerUnit[num].defensePower;
        damageCalculatedFloat = damageCalculated * specialMoveMultiplier;
        damageCalculated = (int)damageCalculatedFloat;

        //Debug.Log("DAMAGE AMOUNT ENEMY: " + damageCalculated);

        if (num == 0)
        {
            enemyAnimator[currentEnemyNum].SetTrigger("EnemyPlay");
        }

        if (num == 1)
        {
            enemyAnimator[currentEnemyNum].SetTrigger("EnemyPlayDown");
        }



        yield return new WaitForSeconds(2f);

        if (UnityEngine.Random.Range(0f, 10f / specialAccuracyBuffer) <= currentAccuracy)
        {
            playerUnit[num].TakeDamage(damageCalculated);

            playerUI.CreateDamagePopupUI(playerUnit[num].gameObject.transform.position, damageCalculated);

            playerUnit[num].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUnit[num].gameObject.SetActive(true);




            dialogueText.text = "The attack is successful! \n";

            //playerDamageAnimationText[num].text = damageCalculated.ToString();

        }
        else
        {

            playerUI.CreateDamagePopupUI(playerUnit[num].gameObject.transform.position, -1);

            dialogueText.text = "The attack was not successful!";
            //playerDamageAnimationText[num].text = "MISSED";
        }

        while (enemyUnit[currentEnemyNum].transform.position != startPos)
        {
            enemyUnit[currentEnemyNum].transform.position = Vector3.MoveTowards(enemyUnit[currentEnemyNum].transform.position, startPos, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.currentTurnIndicator.SetActive(false);
        }



        playerUI.SetHP(playerUnit[num].unitCurrentHP, num);

        yield return new WaitForSeconds(textDelay);

        if (PlayerDeathChecker(num))
        {
            dialogueText.text = "Player knocked out";

            turnOrder.Remove(playerUnit[num]);

            yield return new WaitForSeconds(textDelay);
        }

        playerUI.UIIndicator();

        state = BattleState.UPNEXT;
        UpNext();
    }

    private void EndBattle()
    {
        if (state == BattleState.WON)
        {
            PlayerPrefs.SetInt("LosingStreak", 0);

            int winningStreak;

            winningStreak = PlayerPrefs.GetInt("WinningStreak") + 1;
            PlayerPrefs.SetInt("WinningStreak", winningStreak);

            int coinsWon = 0;

            if (PlayerPrefs.GetInt("WinningStreak") == 0)
            {
                coinsWon = (enemyUnit.Length * 15 + UnityEngine.Random.Range(0, 6));
            }
            else if (PlayerPrefs.GetInt("WinningStreak") == 1)
            {
                coinsWon = (enemyUnit.Length * 14 + UnityEngine.Random.Range(0, 5));
            }
            else if (PlayerPrefs.GetInt("WinningStreak") == 2)
            {
                coinsWon = (enemyUnit.Length * 12 + UnityEngine.Random.Range(0, 5));
            }
            else if (PlayerPrefs.GetInt("WinningStreak") >= 3)
            {
                coinsWon = (enemyUnit.Length * 10 + UnityEngine.Random.Range(0, 5));
            }

            dialogueText.text = "YOU HAVE WON! \n" + "You received " + coinsWon + " coins!";

            int totalCoins = PlayerPrefs.GetInt("TotalCoins") + coinsWon;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            AudioManager.winAudio.Play();
            AudioManager.backgroundAudio.Stop();

            if (SceneManager.GetActiveScene().name == "Boss Battle 1" || SceneManager.GetActiveScene().name == "Boss Battle 2")
            {
                SceneManager.LoadScene("Winner Scene");
            }

        }
        else if (state == BattleState.LOST)
        {

            PlayerPrefs.SetInt("WinningStreak", 0);

            int losingStreak;

            losingStreak = PlayerPrefs.GetInt("LosingStreak") + 1;
            PlayerPrefs.SetInt("LosingStreak", losingStreak);

            int coinsLost = 0;

            if (PlayerPrefs.GetInt("LosingStreak") == 0)
            {
                coinsLost = (playerUnit.Length * 5 + UnityEngine.Random.Range(-2, 2));
            }
            else if (PlayerPrefs.GetInt("LosingStreak") == 1)
            {
                coinsLost = (playerUnit.Length * 5 + UnityEngine.Random.Range(-2, 2));
            }
            else if (PlayerPrefs.GetInt("LosingStreak") == 2)
            {
                coinsLost = (playerUnit.Length * 4 + UnityEngine.Random.Range(-1, 2));
            }
            else if (PlayerPrefs.GetInt("LosingStreak") >= 3)
            {
                coinsLost = (playerUnit.Length * 2 + UnityEngine.Random.Range(0, 2));
            }

            dialogueText.text = "You have been defeated... \n" + "You lost " + coinsLost + " coins...\n" + "You should visit the dojo for training";

            int totalCoins = PlayerPrefs.GetInt("TotalCoins") - coinsLost;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            AudioManager.loseAudio.Play();
        }

        Invoke("GoBack", 5f);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Tutorial World");
        PlayerPrefs.SetInt("Just Battled", 1);
    }

    IEnumerator PlayerHeal()
    {

        playerUnit[currentPlayerNum].Heal(12);

        playerUI.UIIndicator();

        playerUI.SetHP(playerUnit[currentPlayerNum].unitCurrentHP, currentPlayerNum);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(textDelay);

        state = BattleState.UPNEXT;
        UpNext();
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {

            if (playerUnit[currentPlayerNum].playerType == "Small")
            {
                actions.SmallPlayerQuickMove();
                actions.SmallPlayerStandardMove();
                actions.SmallPlayerHeavyMove();
            }

            if (playerUnit[currentPlayerNum].playerType == "Medium")
            {
                actions.MediumPlayerQuickMove();
                actions.MediumPlayerStandardMove();
                actions.MediumPlayerHeavyMove();
            }

            if (playerUnit[currentPlayerNum].playerType == "Heavy")
            {
                actions.HeavyPlayerQuickMove();
                actions.HeavyPlayerStandardMove();
                actions.HeavyPlayerHeavyMove();
            }

            actionsUI.SetActive(true);
            //StartCoroutine(PlayerAttack());
        }
    }

    public void onActionSelection()
    {



        if (playerUnit[currentPlayerNum].playerType == "Small")
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Move 1")
            {
                actions.SmallPlayerQuickMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 2")
            {
                actions.SmallPlayerStandardMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 3")
            {
                actions.SmallPlayerHeavyMove();
            }
        }

        if (playerUnit[currentPlayerNum].playerType == "Medium")
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Move 1")
            {
                actions.MediumPlayerQuickMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 2")
            {
                actions.MediumPlayerStandardMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 3")
            {
                actions.MediumPlayerHeavyMove();
            }
        }

        if (playerUnit[currentPlayerNum].playerType == "Heavy")
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Move 1")
            {
                actions.HeavyPlayerQuickMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 2")
            {
                actions.HeavyPlayerStandardMove();
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "Move 3")
            {
                actions.HeavyPlayerHeavyMove();
            }
        }

        currentAttackDamage = actions.actionDamage;
        currentAttackSpeed = actions.actionSpeed;
        currentSpecialBonus = actions.specialBonus;
        currentAccuracy = actions.actionAccuracy;

        currentActionNum = actions.actionNum;

        //1234

        if (enemyCharacters.Length == 1)
        {
            currentAttackDamage = actions.actionDamage;
            currentAttackSpeed = actions.actionSpeed;
            currentSpecialBonus = actions.specialBonus;
            currentAccuracy = actions.actionAccuracy;

            currentActionNum = actions.actionNum;

            StartCoroutine(PlayerAttack());
        }
        else if (enemyCharacters.Length == 2 && (!enemyUnit[1].IsDead() && !enemyUnit[0].IsDead()))
        {
            currentAttackDamage = actions.actionDamage;
            currentAttackSpeed = actions.actionSpeed;
            currentSpecialBonus = actions.specialBonus;
            currentAccuracy = actions.actionAccuracy;

            currentActionNum = actions.actionNum;

            enemySelectPanel.SetActive(true);
        }
        else if (enemyCharacters.Length == 2 && (!enemyUnit[1].IsDead() || !enemyUnit[0].IsDead()))
        {
            if (enemyUnit[1].IsDead())
            {
                currentAttackDamage = actions.actionDamage;
                currentAttackSpeed = actions.actionSpeed;
                currentSpecialBonus = actions.specialBonus;
                currentAccuracy = actions.actionAccuracy;

                currentActionNum = actions.actionNum;

                StartCoroutine(PlayerAttackEnemyVariations(0));
            }
            else if (enemyUnit[0].IsDead())
            {
                currentAttackDamage = actions.actionDamage;
                currentAttackSpeed = actions.actionSpeed;
                currentSpecialBonus = actions.specialBonus;
                currentAccuracy = actions.actionAccuracy;

                currentActionNum = actions.actionNum;

                StartCoroutine(PlayerAttackEnemyVariations(1));
            }

        }
    }



    IEnumerator PlayerAttackEnemyVariations(int enemyAlive)
    {
        actionsUI.SetActive(false);
        enemySelectPanel.SetActive(false);
        actionButtonsUI.SetActive(false);


        startPos = playerUnit[currentPlayerNum].transform.position;

        //Debug.Log("Start Pos: ");




        //Debug.Log("Start Pos: " + startPos + hasStarPos);

        while (playerUnit[currentPlayerNum].transform.position != middleOfScene.transform.position)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, middleOfScene.transform.position, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.UIIndicator();
            //Debug.Log("Start Pos: " + startPos + hasStarPos);


        }

        playerUnit[currentPlayerNum].NormalAttackSpeed();

        //Debug.Log("New Order");

        int count = 0;

        foreach (var x in turnOrder)
        {

            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitName.ToString() + " " + x.unitSpeed;
            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
        }


        if (currentActionNum == 0
        || currentActionNum == 3
            || currentActionNum == 6)
        {
            playerAnimator[currentPlayerNum].SetTrigger("QuickPlay");
        }

        if (currentActionNum == 1
                || currentActionNum == 4
                    || currentActionNum == 7)
        {
            playerAnimator[currentPlayerNum].SetTrigger("StandardPlay");
        }

        if (currentActionNum == 2
                || currentActionNum == 5
                    || currentActionNum == 8)
        {
            playerAnimator[currentPlayerNum].SetTrigger("HeavyPlay");
        }

        specialInputTimer = currentSpecialBonus;

        yield return new WaitForSeconds(1.5f);

        //specialInputActive = true;

        //yield return new WaitForSeconds(currentSpecialBonus + 0.01f);

        if (specialInputAchieved)
        {
            specialAccuracyBuffer = 0.9f;
            specialMoveMultiplier = 1.5f;
        }
        else
        {
            specialAccuracyBuffer = 1.1f;
            specialMoveMultiplier = 1f;
        }

        //Debug.Log("Special: " + specialInputAchieved);

        specialInputAchieved = false;
        PlayerPrefs.SetInt("SpecialBonusAchieved", 0);

        specialAllowed = true;

        int specialStreakBonus = 0;

        if (PlayerPrefs.GetInt("SpecialStreak") == 0)
        {
            specialStreakBonus = 0;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") == 1)
        {
            specialStreakBonus = 2;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") >= 3)
        {
            specialStreakBonus = 4;
        }

        damageCalculated = (currentAttackDamage + playerUnit[currentPlayerNum].attackPower + UnityEngine.Random.Range(-2, 2)) - enemyUnit[enemyAlive].defensePower;
        damageCalculatedFloat = (damageCalculated * specialMoveMultiplier) - specialStreakBonus;
        damageCalculated = (int)damageCalculatedFloat;












        yield return new WaitForSeconds(1f);



        if (UnityEngine.Random.Range(0f, 10f / specialAccuracyBuffer) <= currentAccuracy)
        {
            enemyUnit[enemyAlive].TakeDamage(damageCalculated);

            enemyUI.CreateDamagePopupUI(enemyUnit[enemyAlive].gameObject.transform.position, damageCalculated);

            enemyUnit[enemyAlive].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAlive].gameObject.SetActive(true);

            if (specialAccuracyBuffer == 0.9f)
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Successful, damage increased by 1.5x \n";

                //enemyDamageAnimationText[enemyAlive].text = damageCalculated.ToString();
            }
            else
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Missed, no damage increase. \n";

                //enemyDamageAnimationText[enemyAlive].text = damageCalculated.ToString();
            }

            //dialogueText.text = "The attack is successful! \n" + damageCalculated + " damage done to " + enemyUnit[enemyAlive].unitName;
        }
        else
        {
            enemyUI.CreateDamagePopupUI(enemyUnit[enemyAlive].gameObject.transform.position, -1);

            dialogueText.text = "The attack was not successful!";

            //enemyDamageAnimationText[enemyAlive].text = "MISSED";
        }



        while (playerUnit[currentPlayerNum].transform.position != startPos)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, startPos, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.currentTurnIndicator.SetActive(false);
        }



        //enemyAnimator[enemyAlive].SetTrigger("EnemyPlay");

        enemyUI.SetHP(enemyUnit[enemyAlive].unitCurrentHP, enemyAlive);

        yield return new WaitForSeconds(textDelay + 2);

        playerUI.UIIndicator();

        if (DeathChecker(enemyAlive))
        {
            dialogueText.text = "Enemy knocked out";

            enemyUnit[enemyAlive].unitSpeed = -1;

            turnOrder.Remove(enemyUnit[enemyAlive]);

            yield return new WaitForSeconds(textDelay);
        }

        state = BattleState.UPNEXT;
        UpNext();
    }



    public void OnEnemySelect(int selection)
    {
        enemyAttackSelection = selection;

        StartCoroutine(PlayerAttackEnemySelect());

        actionButtonsUI.SetActive(false);
    }

    private bool DeathChecker(int killed)
    {

        if (enemyUnit[killed].IsDead())
        {

            if (enemyUnit.Length == 2)
            {
                enemySpawnPoints[killed + 1].gameObject.SetActive(false);
            }
            else if (enemyUnit.Length == 1)
            {
                enemySpawnPoints[killed].gameObject.SetActive(false);
            }
            else if (enemyUnit.Length == 3)
            {
                enemySpawnPoints[killed + 2].gameObject.SetActive(false);
            }

            if (killed == 0)
            {
                enemyOneSprite.SetActive(false);
                enemyOneUISprite.SetActive(false);

                AudioManager.deathAudio.Play();
            }

            if (killed == 1)
            {
                enemyOneSprite.SetActive(false);
                enemyTwoUISprite.SetActive(false);

                AudioManager.deathAudio.Play();
            }

            return true;
        }
        else
        {
            return false;
        }

    }

    private bool PlayerDeathChecker(int killed)
    {

        if (playerUnit[killed].IsDead())
        {

            if (playerUnit.Length == 2)
            {
                unitSpawnPoints[killed + 1].gameObject.SetActive(false);
            }
            else if (playerUnit.Length == 1)
            {
                unitSpawnPoints[killed].gameObject.SetActive(false);
            }
            else if (playerUnit.Length == 3)
            {
                unitSpawnPoints[killed + 2].gameObject.SetActive(false);
            }

            if (killed == 0)
            {
                unitOneUISprite.SetActive(false);
                AudioManager.deathAudio.Play();
            }

            if (killed == 1)
            {
                unitTwoUISprite.SetActive(false);
                AudioManager.deathAudio.Play();
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    IEnumerator PlayerAttackEnemySelect()
    {

        actionsUI.SetActive(false);
        enemySelectPanel.SetActive(false);


        startPos = playerUnit[currentPlayerNum].transform.position;

        //Debug.Log("Start Pos: ");




        //Debug.Log("Start Pos: " + startPos + hasStarPos);

        while (playerUnit[currentPlayerNum].transform.position != middleOfScene.transform.position)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, middleOfScene.transform.position, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.UIIndicator();
            //Debug.Log("Start Pos: " + startPos + hasStarPos);


        }


        playerUnit[currentPlayerNum].NormalAttackSpeed();

        //Debug.Log("New Order");

        int count = 0;

        foreach (var x in turnOrder)
        {

            if (turnOrder[count] != null && count < 4)
            {
                turnsUI[count].text = x.unitName.ToString() + " " + x.unitSpeed;
            }

            //Debug.Log(x.ToString() + " " + x.unitSpeed);
        }



        if (currentActionNum == 0
        || currentActionNum == 3
            || currentActionNum == 6)
        {
            playerAnimator[currentPlayerNum].SetTrigger("QuickPlay");
        }

        if (currentActionNum == 1
                || currentActionNum == 4
                    || currentActionNum == 7)
        {
            playerAnimator[currentPlayerNum].SetTrigger("StandardPlay");
        }

        if (currentActionNum == 2
                || currentActionNum == 5
                    || currentActionNum == 8)
        {
            playerAnimator[currentPlayerNum].SetTrigger("HeavyPlay");
        }




        specialInputTimer = currentSpecialBonus;

        yield return new WaitForSeconds(1.5f);

        //specialInputActive = true;

        //yield return new WaitForSeconds(currentSpecialBonus + 0.01f);


        if (specialInputAchieved)
        {
            specialAccuracyBuffer = 0.9f;
            specialMoveMultiplier = 1.5f;
        }
        else
        {
            specialAccuracyBuffer = 1.1f;
            specialMoveMultiplier = 1f;
        }

        //Debug.Log("Special: " + specialInputAchieved);

        specialInputAchieved = false;
        PlayerPrefs.SetInt("SpecialBonusAchieved", 0);

        specialAllowed = true;

        int specialStreakBonus = 0;

        if (PlayerPrefs.GetInt("SpecialStreak") == 0)
        {
            specialStreakBonus = 0;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") == 1)
        {
            specialStreakBonus = 2;
        }
        else if (PlayerPrefs.GetInt("SpecialStreak") >= 3)
        {
            specialStreakBonus = 4;
        }

        damageCalculated = (currentAttackDamage + playerUnit[currentPlayerNum].attackPower + UnityEngine.Random.Range(-2, 2)) - enemyUnit[enemyAttackSelection].defensePower;
        damageCalculatedFloat = (damageCalculated * specialMoveMultiplier) - specialStreakBonus;
        damageCalculated = (int)damageCalculatedFloat;



        if (UnityEngine.Random.Range(0f, 10f / specialAccuracyBuffer) <= currentAccuracy)
        {
            enemyUnit[enemyAttackSelection].TakeDamage(damageCalculated);

            enemyUI.CreateDamagePopupUI(enemyUnit[enemyAttackSelection].gameObject.transform.position, damageCalculated);

            enemyUnit[enemyAttackSelection].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            enemyUnit[enemyAttackSelection].gameObject.SetActive(true);

            if (specialAccuracyBuffer == 0.9f)
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Successful, damage increased by 1.5x \n";

                //enemyDamageAnimationText[enemyAttackSelection].text = damageCalculated.ToString();
            }
            else
            {
                dialogueText.text = "The attack is successful! \n" + "Special Bonus Missed, no damage increase. \n";

                //enemyDamageAnimationText[enemyAttackSelection].text = damageCalculated.ToString();
            }

            //dialogueText.text = "The attack is successful! \n" + damageCalculated + " damage done to " + enemyUnit[enemyAttackSelection].unitName;
        }
        else
        {
            enemyUI.CreateDamagePopupUI(enemyUnit[enemyAttackSelection].gameObject.transform.position, -1);

            dialogueText.text = "The attack was not successful!";

            //enemyDamageAnimationText[enemyAttackSelection].text = "MISSED";
        }


        while (playerUnit[currentPlayerNum].transform.position != startPos)
        {
            playerUnit[currentPlayerNum].transform.position = Vector3.MoveTowards(playerUnit[currentPlayerNum].transform.position, startPos, Time.deltaTime * 13f);
            yield return new WaitForSeconds(0.001f);
            playerUI.currentTurnIndicator.SetActive(false);
        }


        //enemyAnimator[enemyAttackSelection].SetTrigger("PLAY");

        enemyUI.SetHP(enemyUnit[enemyAttackSelection].unitCurrentHP, enemyAttackSelection);

        yield return new WaitForSeconds(textDelay + 2);

        playerUI.UIIndicator();

        if (DeathChecker(enemyAttackSelection))
        {
            dialogueText.text = "Enemy knocked out";

            enemyUnit[enemyAttackSelection].unitSpeed = -1;

            turnOrder.Remove(enemyUnit[enemyAttackSelection]);

            yield return new WaitForSeconds(textDelay);
        }

        state = BattleState.UPNEXT;
        UpNext();
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            actionButtonsUI.SetActive(false);
            StartCoroutine(PlayerHeal());
        }
    }

    public void OnForfeitButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerForfeit());
        }
    }

    public void OnBackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            actionsUI.SetActive(false);
        }
    }

    IEnumerator PlayerForfeit()
    {

        dialogueText.text = "You have given up all hope and forfeit...";

        yield return new WaitForSeconds(textDelay);

        state = BattleState.LOST;
        EndBattle();
    }
}
