using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DIalogueManager : MonoBehaviour
{

    public GameObject dialogueBox;
    public GameObject dialogueButtons;
    public Text dialogueText;

    public bool dialogueActive = false;

    public string[] dialogueLines;
    public int currentLine;

    public bool canMove;

    public bool showActionButtons;

    public GameObject dojoMasterOne;
    public GameObject dojoMasterTwo;

    CharacterMovementLevel characterMovementLevel;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;

        if (SceneManager.GetActiveScene().name == "Tutorial World")
        {
            //dojoMasterOne = GameObject.FindGameObjectWithTag("Dojo Master 1");
            //dojoMasterTwo = GameObject.FindGameObjectWithTag("Dojo Master 2");
            characterMovementLevel = GameObject.FindGameObjectWithTag("Main Character Level").GetComponent<CharacterMovementLevel>();

            if (PlayerPrefs.GetInt("BeginningTextDone") == 0)
            {
                dojoMasterOne.SetActive(true);
            }
            else if (PlayerPrefs.GetInt("BeginningTextDone") == 1)
            {
                dojoMasterOne.SetActive(false);
            }

            if (PlayerPrefs.GetInt("SecondTextDone") == 0 && characterMovementLevel.currentPoint.name == "Waypoint 1")
            {
                dojoMasterTwo.SetActive(true);
            }
            else if (PlayerPrefs.GetInt("SecondTextDone") == 1)
            {
                dojoMasterTwo.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Dialogue Active: " + dialogueActive);

        if (dialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
        }

        if (currentLine >= dialogueLines.Length)
        {
            dialogueActive = false;
            dialogueBox.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Tutorial World")
            {
                if (PlayerPrefs.GetInt("BeginningTextDone") == 0)
                {
                    PlayerPrefs.SetInt("BeginningTextDone", 1);
                }

                if (PlayerPrefs.GetInt("BeginningTextDone") == 1)
                {
                    dojoMasterOne.SetActive(false);
                }

                if (PlayerPrefs.GetInt("SecondTextDone") == 0 && characterMovementLevel.currentPoint.name == "Waypoint 1")
                {
                    PlayerPrefs.SetInt("SecondTextDone", 1);
                    dojoMasterTwo.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("SecondTextDone") == 1)
                {
                    dojoMasterTwo.SetActive(false);
                }
            }

            if (showActionButtons)
            {
                dialogueButtons.SetActive(false);
            }

            currentLine = 0;

            Time.timeScale = 1;
            canMove = true;
        }

        dialogueText.text = dialogueLines[currentLine];

    }

    //Used to active dialogue box from other scripts

    public void ShowDialogue(string[] dialogueLinesToShow, bool showButtons)
    {

        dialogueLines = dialogueLinesToShow;

        dialogueActive = true;
        dialogueBox.SetActive(true);

        if (showButtons)
        {
            dialogueButtons.SetActive(true);
            showActionButtons = showButtons;
        }

        Time.timeScale = 0;
        canMove = false;
    }
}
