using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{

    public bool specialStart;
    public static bool specialStartBool = false;
    public bool specialStartTest;

    // Start is called before the first frame update
    void Start()
    {
        specialStart = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpecialStartNow()
    {
        specialStart = true;

        PlayerPrefs.SetInt("Special Start", 1);

        Debug.Log("Special Started");
    }

    public void SpecialEndNow()
    {
        specialStart = false;
        PlayerPrefs.SetInt("Special Start", 0);
        Debug.Log("Special Ended");
    }

    public void PlayHitSound()
    {
        AudioManager.hitAudio.Play();
    }
}
