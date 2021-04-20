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

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
        }

        if (currentLine >= dialogueLines.Length)
        {
            dialogueActive = false;
            dialogueBox.SetActive(false);

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
