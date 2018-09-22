using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{

    
    //================================ VARIABLES ==================================
    private static gameController _instance = null;
    public static gameController i { get { return _instance; } }//Instance

    public static UserData playerData, AIData;

    public GameObject buildController;

    //================================ FUNCTIONS ==================================
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        Init();
    }
    void Init()
    {
        playerData = new UserData();
        AIData = new UserData();
    }
    void Start()
    {
        //TEMP
        bool AIIsFallen = (0 == UnityEngine.Random.Range(0, 1));
        playerData.NewGame(!AIIsFallen, true);
        AIData.NewGame(AIIsFallen, false);

        BuildController bc = buildController.GetComponent<BuildController>();
        bc.playerFallen = !AIIsFallen;
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

    public void Win()
    {
        //swap sides and play again
        CleanStage();
        playerData.NextLevel();
        AIData.NextLevel();
    }

    public void GameOver()
    {
        //change scene
        SceneManager.LoadScene(2);
    }

}
