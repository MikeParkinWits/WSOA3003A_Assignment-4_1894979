using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DojoButtons : MonoBehaviour
{

    public Text coinText;
    public Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Tutorial World");

        coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
        dialogueText.text = "";
        PlayerPrefs.SetInt("Just In Dojo", 1);
    }

    public void OnQuickAttack()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 25)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 25);

            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "Quick attack now available in battle!";

            PlayerPrefs.SetInt("Quick Attack Learnt", 1);
        }

    }

    public void OnHeavyAttack()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 75)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 75);


            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "Heavy attack now available in battle!";

            PlayerPrefs.SetInt("Heavy Attack Learnt", 1);
        }
    }

    public void OnSpecialBonus()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 150)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 150);


            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "Special Bonus now learnt!\nPress space right as the player punches for a special bonus";

            PlayerPrefs.SetInt("Special Bonus Learnt", 1);
        }
    }

    public void OnRequestPartner()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 200)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 200);


            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "A new partner will now join you in battle!";

            PlayerPrefs.SetInt("New Partner Gained", 1);
        }
    }

    public void OnStatUpgradeOne()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 50)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 50);


            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "All stats have been upgraded";

            PlayerPrefs.SetInt("Stat 1 Upgraded", 1);
        }
    }

    public void OnStatUpgradeTwo()
    {
        if (PlayerPrefs.GetInt("TotalCoins") < 150)
        {
            dialogueText.text = "Could Not Purchase, not enough money!";
        }
        else
        {
            PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - 150);

            coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();
            dialogueText.text = "All stats have been maxed out";

            PlayerPrefs.SetInt("Stat 2 Upgraded", 1);
        }
    }
}
