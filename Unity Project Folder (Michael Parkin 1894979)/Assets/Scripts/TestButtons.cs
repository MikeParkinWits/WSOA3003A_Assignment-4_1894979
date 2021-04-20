
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestButtons : MonoBehaviour
{

    public GameObject rulesScreen;
    public GameObject playerStatsScreen;
    public Text totalCoinsTxt;

    public Unit[] playerStats = new Unit[3];

    [Header("Small Player UI Stats")]
    public Text smallMaxHPTxt;
    public Text smallSpeedTxt;
    public Text smallAttackPowerTxt;
    public Text smallDefencePowerTxt;

    [Header("Medium Player UI Stats")]
    public Text medMaxHPTxt;
    public Text medSpeedTxt;
    public Text medAttackPowerTxt;
    public Text medDefencePowerTxt;

    [Header("Heavy Player UI Stats")]
    public Text heavyMaxHPTxt;
    public Text heavySpeedTxt;
    public Text heavyAttackPowerTxt;
    public Text heavyDefencePowerTxt;

    // Start is called before the first frame update
    void Start()
    {
        rulesScreen.SetActive(false);
        playerStatsScreen.SetActive(false);

        //totalCoinsTxt.text = "Total Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();

        smallMaxHPTxt.text = "Max HP: " + playerStats[0].unitMaxHP.ToString();
        smallSpeedTxt.text = "Starting Speed: " + playerStats[0].unitSpeed.ToString();
        smallAttackPowerTxt.text = "Attack Power: " + playerStats[0].attackPower.ToString();
        smallDefencePowerTxt.text = "Defence Power: " + playerStats[0].defensePower.ToString();

        medMaxHPTxt.text = "Max HP: " + playerStats[1].unitMaxHP.ToString();
        medSpeedTxt.text = "Starting Speed: " + playerStats[1].unitSpeed.ToString();
        medAttackPowerTxt.text = "Attack Power: " + playerStats[1].attackPower.ToString();
        medDefencePowerTxt.text = "Defence Power: " + playerStats[1].defensePower.ToString();

        heavyMaxHPTxt.text = "Max HP: " + playerStats[2].unitMaxHP.ToString();
        heavySpeedTxt.text = "Starting Speed: " + playerStats[2].unitSpeed.ToString();
        heavyAttackPowerTxt.text = "Attack Power: " + playerStats[2].attackPower.ToString();
        heavyDefencePowerTxt.text = "Defence Power: " + playerStats[2].defensePower.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene(string name)
    {
        SceneManager.LoadScene(name);
        PlayerPrefs.SetString("Spawn Point", "Point 1");

        PlayerPrefs.SetInt("TotalCoins", 75);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnShowRules()
    {
        rulesScreen.SetActive(true);
    }

    public void OnCloseRules()
    {
        rulesScreen.SetActive(false);
    }

    public void OnShowStats()
    {
        playerStatsScreen.SetActive(true);
    }

    public void OnCloseStats()
    {
        playerStatsScreen.SetActive(false);
    }
}
