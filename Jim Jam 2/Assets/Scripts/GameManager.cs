using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public bool isLevelActive;
    public bool doingSetup;
    private bool doingGameOverPause;
    private bool doingWinPause;

    public int maxNumberofLevels;
    private Text endingText;

    public float levelStartDelay = 2f;
    public float levelEndDelay = 0f;
    public static GameManager instance = null;

    private Text levelText;
    private Text levelTimerText;
    private Text readyText;
    private Text gameOverText;
    private Text winText;

    public static int level = 1;

    private OuroborosSystem ouroborosSystem;

    public GameObject gatheringStationPrefab;

    public float timeToCompleteGame;
    public float gameTimer;

    public float startingHeadValue;

    // Use this for initialization
    void Awake () {

        doingWinPause = false;
        doingGameOverPause = false;
        doingSetup = true;
        isLevelActive = false;

        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        //Set game timer
        gameTimer = timeToCompleteGame;

        StartLevel();
		
	}

    // Update is called once per frame
    void Update () {

        GameObject[] gatheringStations = GameObject.FindGameObjectsWithTag("Gathering Station");


        if (!doingGameOverPause) //|| !doingWinPause)
        {
            if (Mathf.Floor(gameTimer / 60) == 0 && Mathf.Floor(gameTimer % 60) == 0)
                GameOver();

            if (isLevelActive && gameTimer > 0.0f)
            {
                gameTimer -= Time.deltaTime;
                SetLevelTimerText();

                if (ouroborosSystem.fillAmount == 100.0f && !ouroborosSystem.isResetting)
                {
                    ResetLevel();
                    //WinLevel();
                }
                    

            }
        }

	}


    private void StartLevel()
    {
        doingSetup = true;

        //Find Ouroboros System
        ouroborosSystem = GameObject.Find("Ouroboros System").GetComponent<OuroborosSystem>();

        //Find level timer text
        levelTimerText = GameObject.Find("LevelTimerText").GetComponent<Text>();

        //Find Level text
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        levelText.text = "Level: " + level.ToString();

        //Find ready text
        readyText = GameObject.Find("ReadyText").GetComponent<Text>();

        //Find Win Text
        winText = GameObject.Find("WinText").GetComponent<Text>();

        //Find GameOver text
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();

        //Find Ending Text
        endingText = GameObject.Find("EndingText").GetComponent<Text>();

        //Set the timer text
        SetLevelTimerText();

        //Set the starting head value
        SetStartingHeadValue();

        //Randomly place the gathering stations
        SetGatheringStations();

        //Randomize gathering stations and ouroboros requirements
        RandomizeLevel();

        //Remove Ready Text
        Invoke("HideReadyText", levelStartDelay);

    }

    void SetStartingHeadValue()
    {
        if (level == 1)
            startingHeadValue = 70.0f;
        else
            startingHeadValue = 70.0f - (level * 10.0f);

        if (startingHeadValue < 0.0f)
            startingHeadValue = 0.0f;

        ouroborosSystem.SetStartingHeadValue(startingHeadValue);
    }

    void SetGatheringStations()
    {
        int numberOfStations = 2;

        if (level == 1)
            numberOfStations = 2;
        if (level == 2)
            numberOfStations = 3;
        if (level >= 3)
            numberOfStations = 4;

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Gathering Spawn");


        List<Transform> availableSpawns = new List<Transform>();

        foreach (GameObject spawn in spawns)
        {
            availableSpawns.Add(spawn.GetComponent<Transform>());
        }


        for (int i = 0; i < numberOfStations; i++)
        {

            int index = Random.Range(0, availableSpawns.Count);

            Transform chosenSpawn = availableSpawns[index];

            Instantiate(gatheringStationPrefab, chosenSpawn.position, Quaternion.identity);

            availableSpawns.RemoveAt(index);
        }

    }

    void DestroyGatheringStations()
    {
        GameObject[] gatheringStationsToDestroy = GameObject.FindGameObjectsWithTag("Gathering Station");

        foreach (GameObject gatheringStation in gatheringStationsToDestroy)
            DestroyImmediate(gatheringStation);


        GameObject[] gatheringStations = GameObject.FindGameObjectsWithTag("Gathering Station");

    }
    

    void RandomizeLevel()
    {

        //Find Gathering Stations
        GameObject[] gatheringStations = GameObject.FindGameObjectsWithTag("Gathering Station");

        List<int> availableElementValues = new List<int>{ 0, 1, 2, 3 };

        List<int> chosenGatheringElements = new List<int>();


        //Choose random Elements for the Gathering Stations
        foreach (GameObject gatheringStation in gatheringStations)
        {

            int index = Random.Range(0, availableElementValues.Count);

            chosenGatheringElements.Add(availableElementValues[index]);

            gatheringStation.GetComponent<GatheringStation>().element = availableElementValues[index];

            availableElementValues.RemoveAt(index);

        }

        //Choose a random correct Ouroboros combo
        List<int> availableOuroborosValues = chosenGatheringElements;

        List<int> chosenOuroborosElements = new List<int>();

        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, availableOuroborosValues.Count);

            chosenOuroborosElements.Add(availableOuroborosValues[index]);

            availableOuroborosValues.RemoveAt(index);
        }


        ouroborosSystem.correctElement1 = chosenOuroborosElements[0];
        ouroborosSystem.correctElement2 = chosenOuroborosElements[1];

    }


    private void ResetLevel()
    {
        gameTimer += 10.0f + (2.0f * level);
        level++;
        DestroyGatheringStations();
        ouroborosSystem.StartResetting();
        StartLevel();
        
    }



    void HideReadyText()
    {
        readyText.text = "";

        doingSetup = false;

        isLevelActive = true;

    }

    void SetLevelTimerText()
    {
        string minutes = Mathf.Floor(gameTimer / 60).ToString("00");
        string seconds = Mathf.Floor(gameTimer % 60).ToString("00");

        levelTimerText.text = "Time Left: " + minutes + ":" + seconds;

    }

    public void GameOver()
    {

        isLevelActive = false;

        gameOverText.text = "Game Over";

        doingGameOverPause = true;

        //Remove Game Over Text after delay
        Invoke("HideGameOverText", levelEndDelay);
    }

    void HideGameOverText()
    {
        gameOverText.text = "";

        doingGameOverPause = false;

        level = 1;

        //Restart level if you lose
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinLevel()
    {
        isLevelActive = false;

        //winText.text = "You Win!";

        doingWinPause = true;

        Invoke("HideWinText", levelEndDelay);

    }

    void HideWinText()
    {
        winText.text = "";

        GoToNextLevel();
    }

    void GoToNextLevel()
    {
        if (level != maxNumberofLevels)
        {

            level++;

            doingWinPause = false;

            //Restart level
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //-1 because build index starts at 0 while level number starts at 1
            //SceneManager.LoadScene(level - 1);
        }
        else if (level == maxNumberofLevels)
        {
            endingText.text = "Thanks for Playing!";
        }

    }
}
