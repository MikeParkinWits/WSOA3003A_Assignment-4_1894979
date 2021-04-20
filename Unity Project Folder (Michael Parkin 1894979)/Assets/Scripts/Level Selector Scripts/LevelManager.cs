using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	[Header("Inspector Config")]
	public CharacterMovementLevel mainCharacter;
    public LevelLocationsScript pointToSpawn;
    public Text currentLevelText;

    // Start is called before the first frame update
    void Awake()
    {
		pointToSpawn = GameObject.Find(PlayerPrefs.GetString("Spawn Point")).GetComponent<LevelLocationsScript>();
		mainCharacter.Instantiate(this, pointToSpawn);

		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update()
    {
        if (!mainCharacter.isMoving)
        {
            GetInput();
        }

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Test Menu");
		}
	}

	private void GetInput()
	{

		if (mainCharacter.currentPoint.dialogueManager.canMove)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				mainCharacter.TrySetDirection(Directions.ABOVE);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				mainCharacter.TrySetDirection(Directions.BELOW);
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				mainCharacter.TrySetDirection(Directions.LEFT);
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				mainCharacter.TrySetDirection(Directions.RIGHT);
			}
		}
	}

	public void UpdateUI()
	{
		currentLevelText.text = string.Format(mainCharacter.currentPoint.locationName); //Displaying current location name
	}

	public void UpdateDialogueBox()
	{
		mainCharacter.currentPoint.dialogueManager = FindObjectOfType<DIalogueManager>();

			if (!mainCharacter.currentPoint.dialogueManager.dialogueActive && PlayerPrefs.GetInt("Just In Dojo") != 1)
			{
				mainCharacter.currentPoint.dialogueManager.currentLine = 0;

				mainCharacter.currentPoint.dialogueManager.ShowDialogue(mainCharacter.currentPoint.dialogueLines, mainCharacter.currentPoint.hasDialogueButtons);
			}
		
		
	}


	public void LoadSceneButton(string scene)
	{

		PlayerPrefs.SetString("Spawn Point", mainCharacter.currentPoint.name);
		SceneManager.LoadScene(scene);
		
	}

	public void CloseDialogueButton()
	{
		UpdateDialogueBox();
	}

}
