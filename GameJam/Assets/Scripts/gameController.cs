using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    //uwaga warning
    //assuming that AI always on top of the screen and player on bottom of the screen - coordinate changes

    //================================ VARIABLES ==================================
    private static gameController _instance = null;
    public static gameController i { get { return _instance; } }//Instance

    public static UserData playerData, AIData;
    private AI ai;

    public GameObject buildController;

    public static float timeElapsed { get; private set; }//of current level

    //================================ FUNCTIONS ==================================
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(this.gameObject);

        Init();
    }
    void Init()
    {
        playerData = gameObject.AddComponent<UserData>();
        AIData = gameObject.AddComponent<UserData>();
        ai = new AI();
    }
    void Start()
    {
        timeElapsed = 0;
        //TEMP
        bool AIIsFallen = (0 == UnityEngine.Random.Range(0, 1));
        playerData.NewGame(!AIIsFallen, true);
        AIData.NewGame(AIIsFallen, false);
        ai.Init(AIData);

        setBuildingsFallen();
    }

    void Update()
    {
        GameLoop();
    }

    private void GameLoop()
    {
         playerData.manualUpdate();
         AIData.manualUpdate();
        if (Input.GetKeyDown("r"))//temp
            Win();
        if (Input.GetKeyDown("g"))//temp
            GameOver();

        ai.Update();

        timeElapsed += Time.deltaTime;
    }

    private void CleanStage()
    {
        GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>().CleanAllBasesOnMap();
        playerData.DestroyAllBases();
        AIData.DestroyAllBases();

        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("soldier");
            foreach (var u in units)
                Destroy(u);
        }
    }

    public void setBuildingsFallen()
    {
        BuildController bc = buildController.GetComponent<BuildController>();
        bc.playerFallen = playerData.fallen;
    }

    public void Win()
    {
        timeElapsed = 0;
        //swap sides and play again
        CleanStage();

        playerData.NextLevel();
        AIData.NextLevel();
        ai.NextLevel();

        //swap recordings...so the PC now has recorded player moves
        gameplayRecorder g = AIData.rec;
        AIData.rec = playerData.rec;
        playerData.rec = g;
        playerData.rec.ResetAll();
        AIData.rec.SwapSides();

        setBuildingsFallen();
    }

    public void GameOver()
    {
        CleanStage();
        //change scene
        SceneManager.LoadScene(2);
    }

}
