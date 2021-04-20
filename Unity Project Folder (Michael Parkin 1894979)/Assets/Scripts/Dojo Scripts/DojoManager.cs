using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DojoManager : MonoBehaviour
{

    public Text coinText;

    public Text dialogueText;

    [Header("Gameobjects")]
    public GameObject quickAttackGameobject;
    public GameObject quickAttackPurchasedGameobject;

    public GameObject heavyAttackGameobject;
    public GameObject heavyAttackPurchasedGameobject;
    public GameObject heavyAttackLockedGameobject;

    public GameObject specialBonusGameobject;
    public GameObject specialBonusPurchasedGameobject;
    public GameObject specialBonusLockedGameobject;

    public GameObject newPartnerGainedGameobject;
    public GameObject newPartnerGainedPurchasedGameobject;

    public GameObject statOneGameobject;
    public GameObject statOnePurchasedGameobject;

    public GameObject statTwoGameobject;
    public GameObject statTwoPurchasedGameobject;
    public GameObject statTwoLockedGameobject;

    // Start is called before the first frame update
    void Start()
    {

        //PlayerPrefs.SetInt("Point 2 Locked", 0);

        coinText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins").ToString();

        dialogueText.text = "Welcome to the Dojo! What do you want to upgrade?";

        quickAttackGameobject.SetActive(false);
        quickAttackPurchasedGameobject.SetActive(false);

        heavyAttackGameobject.SetActive(false);
        heavyAttackPurchasedGameobject.SetActive(false);
        heavyAttackLockedGameobject.SetActive(false);

        specialBonusGameobject.SetActive(false);
        specialBonusLockedGameobject.SetActive(false);
        specialBonusPurchasedGameobject.SetActive(false);

        newPartnerGainedGameobject.SetActive(false);
        newPartnerGainedPurchasedGameobject.SetActive(false);

        statOneGameobject.SetActive(false);
        statOnePurchasedGameobject.SetActive(false);

        statTwoGameobject.SetActive(false);
        statTwoPurchasedGameobject.SetActive(false);
        statTwoLockedGameobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerPrefs.GetInt("TotalCoins") < 0)
        {
            PlayerPrefs.SetInt("TotalCoins", 0);
        }

        if (PlayerPrefs.GetInt("Quick Attack Learnt") == 1)
        {
            quickAttackGameobject.SetActive(false);
            quickAttackPurchasedGameobject.SetActive(true);

            heavyAttackLockedGameobject.SetActive(false);
            specialBonusLockedGameobject.SetActive(true);

            if (PlayerPrefs.GetInt("Heavy Attack Learnt") == 1)
            {
                heavyAttackGameobject.SetActive(false);
                heavyAttackPurchasedGameobject.SetActive(true);

                specialBonusLockedGameobject.SetActive(false);

                if (PlayerPrefs.GetInt("Special Bonus Learnt") == 1)
                {
                    specialBonusGameobject.SetActive(false);
                    specialBonusPurchasedGameobject.SetActive(true);
                }
                else if (PlayerPrefs.GetInt("Special Bonus Learnt") == 0)
                {
                    specialBonusGameobject.SetActive(true);
                    specialBonusPurchasedGameobject.SetActive(false);
                }

            }
            else if (PlayerPrefs.GetInt("Heavy Attack Learnt") == 0)
            {
                heavyAttackGameobject.SetActive(true);
                heavyAttackPurchasedGameobject.SetActive(false);

                specialBonusGameobject.SetActive(false);
                specialBonusLockedGameobject.SetActive(true);
            }


        }
        else if (PlayerPrefs.GetInt("Quick Attack Learnt") == 0)
        {
            quickAttackGameobject.SetActive(true);
            quickAttackPurchasedGameobject.SetActive(false);

            heavyAttackGameobject.SetActive(false);
            heavyAttackLockedGameobject.SetActive(true);
            specialBonusLockedGameobject.SetActive(true);
        }


        if (PlayerPrefs.GetInt("New Partner Gained") == 1)
        {
            newPartnerGainedGameobject.SetActive(false);
            newPartnerGainedPurchasedGameobject.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("New Partner Gained") == 0)
        {
            newPartnerGainedGameobject.SetActive(true);
            newPartnerGainedPurchasedGameobject.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Stat 1 Upgraded") == 1)
        {
            statOneGameobject.SetActive(false);
            statOnePurchasedGameobject.SetActive(true);

            statTwoLockedGameobject.SetActive(false);

            if (PlayerPrefs.GetInt("Stat 2 Upgraded") == 1)
            {
                statTwoGameobject.SetActive(false);
                statTwoPurchasedGameobject.SetActive(true);
            }
            else if (PlayerPrefs.GetInt("Stat 2 Upgraded") == 0)
            {
                statTwoGameobject.SetActive(true);
                statTwoPurchasedGameobject.SetActive(false);
            }

        }
        else if (PlayerPrefs.GetInt("Stat 1 Upgraded") == 0)
        {
            statOneGameobject.SetActive(true);
            statOnePurchasedGameobject.SetActive(false);

            statTwoLockedGameobject.SetActive(true);
        }
    }
}
