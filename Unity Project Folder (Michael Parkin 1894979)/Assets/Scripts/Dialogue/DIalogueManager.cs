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

    GameObject dojoMaster;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;

        if (SceneManager.GetActiveScene().name == "Tutorial World")
        {
            dojoMaster = GameObject.FindGameObjectWithTag("Dojo Master");

            if (PlayerPrefs.GetInt("BeginningTextDone") == 0)
            {
                dojoMaster.SetActive(true);
            }
            else if (PlayerPrefs.GetInt("BeginningTextDone") == 1)
            {
                dojoMaster.SetActive(false);
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
                    dojoMaster.SetActive(false);
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
