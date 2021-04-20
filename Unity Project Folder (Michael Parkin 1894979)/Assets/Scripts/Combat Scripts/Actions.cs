using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{

    [Header("Action Information")]
    public string actionName;

    public int actionDamage;

    public int actionSpeed;

    public float specialBonus;

    public float actionAccuracy;

    public float actionNum;

    public Text buttonOneName;
    public Text buttonOneCost;
    public int buttonOneDamage;

    public Text buttonTwoName;
    public Text buttonTwoCost;
    public int buttonTwoDamage;

    public Text buttonThreeName;
    public Text buttonThreeCost;
    public int buttonThreeDamage;

    public int smallAdjuster;
    public int mediumAdjuster;
    public int heavyAdjuster;
    public int speedAdjuster;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Stat 1 Upgraded") == 0)
        {
            smallAdjuster = 0;
            mediumAdjuster = 0;
            heavyAdjuster = 0;
            speedAdjuster = 0;
        }

        if (PlayerPrefs.GetInt("Stat 1 Upgraded") == 1)
        {
            smallAdjuster = 1;
            mediumAdjuster = 3;
            heavyAdjuster = 5;
            speedAdjuster = -5;

            if (PlayerPrefs.GetInt("Stat 2 Upgraded") == 1)
            {
                smallAdjuster = 3;
                mediumAdjuster = 5;
                heavyAdjuster = 7;
                speedAdjuster = -7;
            }
        }
    }

    //Small Player Moves

    public void SmallPlayerQuickMove()
    {
        actionSpeed = 10 + speedAdjuster;
        actionDamage = 5 + smallAdjuster;
        buttonOneName.text = "Quick Move";
        buttonOneCost.text = "Cool Down: " + actionSpeed;
        buttonOneDamage = actionDamage;
        specialBonus = 1.2f;
        actionAccuracy = 8;

        actionNum = 0;
    }

    public void SmallPlayerStandardMove()
    {
        actionSpeed = 25 + speedAdjuster;
        actionDamage = 8 + smallAdjuster;
        buttonTwoName.text = "Standard Move";
        buttonTwoCost.text = "Cool Down: " + actionSpeed;
        buttonTwoDamage = actionDamage;
        specialBonus = 1.5f;
        actionAccuracy = 10;

        actionNum = 1;
    }

    public void SmallPlayerHeavyMove()
    {
        actionSpeed = 35 + speedAdjuster;
        actionDamage = 11 + smallAdjuster;
        buttonThreeName.text = "Heavy Move";
        buttonThreeCost.text = "Cool Down: " + actionSpeed;
        buttonThreeDamage = actionDamage;
        specialBonus = 1.7f;
        actionAccuracy = 6.5f;

        actionNum = 2;
    }


    //Medium Player Moves

    public void MediumPlayerQuickMove()
    {
        actionSpeed = 15 + speedAdjuster;
        actionDamage = 6 + mediumAdjuster;
        buttonOneName.text = "Quick Move";
        buttonOneCost.text = "Cool Down: " + actionSpeed;
        buttonOneDamage = actionDamage;
        specialBonus = 1.4f;
        actionAccuracy = 7.5f;

        actionNum = 3;
    }

    public void MediumPlayerStandardMove()
    {
        actionSpeed = 26 + speedAdjuster;
        actionDamage = 8 + mediumAdjuster;
        buttonTwoName.text = "Standard Move";
        buttonTwoCost.text = "Cool Down: " + actionSpeed;
        buttonTwoDamage = actionDamage;
        specialBonus = 1.5f;
        actionAccuracy = 10;

        actionNum = 4;
    }

    public void MediumPlayerHeavyMove()
    {
        actionSpeed = 40 + speedAdjuster;
        actionDamage = 17 + mediumAdjuster;
        buttonThreeName.text = "Heavy Move";
        buttonThreeCost.text = "Cool Down: " + actionSpeed;
        buttonThreeDamage = actionDamage;
        specialBonus = 1.8f;
        actionAccuracy = 7f;

        actionNum = 5;
    }

    //Heavy Player Moves

    public void HeavyPlayerQuickMove()
    {
        actionSpeed = 20 + speedAdjuster;
        actionDamage = 7 + heavyAdjuster;
        buttonOneName.text = "Quick Move";
        buttonOneCost.text = "Cool Down: " + actionSpeed;
        buttonOneDamage = actionDamage;
        specialBonus = 1.4f;
        actionAccuracy = 7;

        actionNum = 6;
    }

    public void HeavyPlayerStandardMove()
    {
        actionSpeed = 25 + speedAdjuster;
        actionDamage = 8 + heavyAdjuster;
        buttonTwoName.text = "Standard Move";
        buttonTwoCost.text = "Cool Down: " + actionSpeed;
        buttonTwoDamage = actionDamage;
        specialBonus = 1.5f;
        actionAccuracy = 10;

        actionNum = 7;
    }

    public void HeavyPlayerHeavyMove()
    {
        actionSpeed = 55 + speedAdjuster;
        actionDamage = 17 + heavyAdjuster;
        buttonThreeName.text = "Heavy Move";
        buttonThreeCost.text = "Cool Down: " + actionSpeed;
        buttonThreeDamage = actionDamage;
        specialBonus = 2.0f;
        actionAccuracy = 6.5f;

        actionNum = 8;
    }

    //Possible Enemy Actions

    public void EnemyQuickMove()
    {
        actionSpeed = 20;
        actionDamage = 6;
        actionAccuracy = 9;
    }

    public void EnemyStandardMove()
    {
        actionSpeed = 30;
        actionDamage = 9;
        actionAccuracy = 8.5f;
    }
}
