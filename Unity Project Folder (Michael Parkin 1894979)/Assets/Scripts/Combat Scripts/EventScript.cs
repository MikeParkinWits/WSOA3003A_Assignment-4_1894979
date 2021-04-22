using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{

    public bool specialStart;
    public static bool specialStartBool = false;
    public bool specialStartTest;

    private bool specialAchieved = false;

    // Start is called before the first frame update
    void Start()
    {
        specialStart = false;
    }

    public void SpecialStartNow()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") == 0)
        {
            specialStart = true;

            PlayerPrefs.SetInt("Special Start", 1);

        }
    }

    public void SpecialEndNow()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") == 0)
        {
            if (PlayerPrefs.GetInt("SpecialBonusAchieved") == 1)
            {
                specialAchieved = true;
            }

            specialStart = false;
            PlayerPrefs.SetInt("Special Start", 0);
        }

        if (specialAchieved)
        {
            int specialStreak;
            specialStreak = PlayerPrefs.GetInt("SpecialStreak") + 1;

            PlayerPrefs.SetInt("SpecialStreak", specialStreak);
            specialAchieved = false;
        }
        else
        {
            PlayerPrefs.SetInt("SpecialStreak", 0);
        }

    }

    public void SpecialStartNowHalved()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") == 1)
        {
            specialStart = true;

            PlayerPrefs.SetInt("Special Start", 1);

        }
    }

    public void SpecialEndNowHalved()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") == 1)
        {
            if (PlayerPrefs.GetInt("SpecialBonusAchieved") == 1)
            {
                specialAchieved = true;
            }

            specialStart = false;
            PlayerPrefs.SetInt("Special Start", 0);
        }
    }


    public void SpecialStartNowQuarter()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") >= 2)
        {
            specialStart = true;

            PlayerPrefs.SetInt("Special Start", 1);

        }
    }

    public void SpecialEndNowQuarter()
    {
        if (PlayerPrefs.GetInt("SpecialStreak") >= 2)
        {
            if (PlayerPrefs.GetInt("SpecialBonusAchieved") == 1)
            {
                specialAchieved = true;
            }

            specialStart = false;
            PlayerPrefs.SetInt("Special Start", 0);
        }
    }

    public void PlayHitSound()
    {
        AudioManager.hitAudio.Play();
    }
}
