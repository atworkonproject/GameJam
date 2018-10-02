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
    public UserData playerData2;
    public UserData AIData2;
    private AI ai;

    public GameObject buildController;

    public static List<Soldier> soldiers;

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
        playerData = playerData2;
        AIData = AIData2;
        soldiers = new List<Soldier>();
        ai = new AI();
    }
    void Start()
    {
        timeElapsed = 0;

        bool AIIsFallen = (0 == UnityEngine.Random.Range(0, 2));
        playerData.NewGame(!AIIsFallen, true);
        AIData.NewGame(AIIsFallen, false);
        ai.Init(AIData);

        UpdateBgSprite();
    }

    private void UpdateBgSprite()//rotate map if player is evil or good
    {
         GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().flipY = playerData.fallen;
        GameObject.FindWithTag("BackgroundSprite2").GetComponent<SpriteRenderer>().flipY = playerData.fallen;
    }

    void Update()
    {
        GameLoop();
    }

    private void GameLoop()
    {
        CheckIfWon();

         playerData.manualUpdate();
         AIData.manualUpdate();
        if (Input.GetKeyDown("r"))//todo 
            Win();
        if (Input.GetKeyDown("g"))//temp
            GameOver();

        // HotKey
        BuildController bc = buildController.GetComponent<BuildController>();

        if (Input.GetKeyDown("1"))
            bc.BuildPlayer(playerData, BASE_ID.FARM);
        if (Input.GetKeyDown("2"))
            bc.BuildPlayer(playerData, BASE_ID.BARRACKS_01);
        //if (Input.GetKeyDown("3"))
        //    bc.BuildPlayer(playerData, BASE_ID.BARRACKS_02);
        if (Input.GetKeyDown("4"))
            bc.BuildPlayer(playerData, BASE_ID.BARRACKS_03);

        ai.Update();

        timeElapsed += Time.deltaTime;
    }

    private void CleanStage()
    {
        BuildController bc = GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>();
        bc.CleanAllBasesOnMap();
        playerData.DestroyAllBases();
        AIData.DestroyAllBases();

        bc.playerAliveBases = 0;
        bc.AIaliveBases = 0;
        
        foreach (var x in soldiers)
            Destroy(x.gameObject);
        soldiers.Clear();
        GameObject[] units = GameObject.FindGameObjectsWithTag("soldier");
        foreach (var u in units)
        {
            HPBar bar = u.GetComponentInChildren<HPBar>(true);
            if (bar != null)
                Destroy(bar.gameObject);
            Destroy(u.gameObject);
        }

        GameObject[] slowlyBuilded = GameObject.FindGameObjectsWithTag("SlowBuilding");
        foreach (var b in slowlyBuilded)
            Destroy(b.gameObject);

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
        AIData.rec.TrimInitialDelay(ConfigController.Config.MAX_TIME_FOR_AI_TO_START);

        UpdateBgSprite();
    }

    public void GameOver()
    {
        CleanStage();
        //change scene
        SceneManager.LoadScene(2);
    }

    public void CheckIfWon()
    {
        if (timeElapsed < 10.0)
            return;//to fast to win, propably scene not created yet and bases = 0 so went here

        if(buildController.GetComponent<BuildController>().playerAliveBases <= 0)//jestli oba 0 to i tak lost
        {
            //game over
            GameOver();
        }
        else if (buildController.GetComponent<BuildController>().AIaliveBases <= 0)
        {
            //you wiin!
            Win();
        }
    }

}
