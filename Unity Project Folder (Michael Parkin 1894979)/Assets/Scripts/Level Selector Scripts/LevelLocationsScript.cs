using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Directions { ABOVE, BELOW, LEFT, RIGHT }

public class LevelLocationsScript : MonoBehaviour
{

    [Header("Inspector Config")]
    public bool isAutomatic;
    public bool hideIcon;
    public string sceneToLoad;
    public string sceneToLoadTwoPlayers;
    public string locationName;
    public bool isLocked;
    public bool hasDialogue;
    public bool hasDialogueButtons;
    public bool onlyTreasure;

    [Header("Level Points")]
    public LevelLocationsScript pointAbove;
    public LevelLocationsScript pointBelow;
    public LevelLocationsScript pointLeft;
    public LevelLocationsScript pointRight;

    private Dictionary<Directions, LevelLocationsScript> levelLocationDirections = new Dictionary<Directions, LevelLocationsScript>();

    [Header("Dialogue Text")]
    public string dialogue;
    public DIalogueManager dialogueManager;

    public string[] dialogueLines;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueManager = FindObjectOfType<DIalogueManager>();

        LevelSelectionSetup();
    }

    private void LevelSelectionSetup()
    {
        dialogueManager = FindObjectOfType<DIalogueManager>();

        if (hideIcon)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        levelLocationDirections = new Dictionary<Directions, LevelLocationsScript>
        {
            { Directions.ABOVE, pointAbove },
            { Directions.BELOW, pointBelow },
            { Directions.LEFT, pointLeft },
            { Directions.RIGHT, pointRight }
        };

        IsLevelLocked();


    }

    public LevelLocationsScript GetNextPoint(LevelLocationsScript location)
    {
        return levelLocationDirections.FirstOrDefault(x => x.Value != null && x.Value != location).Value;
    }

    public LevelLocationsScript GetLevelDirections(Directions direction)
    {

        if (direction == Directions.ABOVE)
        {
            if (pointAbove != null)
            {
                if (!pointAbove.isLocked)
                {
                    return pointAbove;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        else if (direction == Directions.BELOW)
        {
            if (pointBelow != null)
            {
                if (!pointBelow.isLocked)
                {
                    return pointBelow;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        else if (direction == Directions.LEFT)
        {
            if (pointLeft != null)
            {
                if (!pointLeft.isLocked)
                {
                    return pointLeft;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        else if (direction == Directions.RIGHT)
        {
            if (pointRight != null)
            {
                if (!pointRight.isLocked)
                {
                    return pointRight;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }

    public void IsLevelLocked()
    {

            if (PlayerPrefs.GetInt(gameObject.name + " Locked") == 1)
            {
                isLocked = true;
            }
            else
            {
                isLocked = false;
            }

    }
}
