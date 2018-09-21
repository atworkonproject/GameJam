using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{

    //================================ ENUMS ==================================
    public enum GAME_STATE
    {
        LOGO,
        MENU_MAIN,
        GAME,

        _COUNT
    }

    public enum INGAME_STATE
    {
        READY,//all is loaded and things are stopped (excl. weather etc.)
        GAME,
        GAME_OVER,

        _COUNT
    }

    //================================ VARIABLES ==================================
    private static gameController _instance = null;
    public static gameController i { get { return _instance; } }//Instance

    public GAME_STATE gameState { get; private set; }
    public INGAME_STATE ingameState { get; private set; }

    public float TITLE_DISPLAY_TIME;
    float logoTimeCountdown;

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

    }
    void Start()
    {
        //manage game state
        gameState = GAME_STATE._COUNT;
        ingameState = INGAME_STATE._COUNT;
        ChangeGameState(GAME_STATE.LOGO);
        ChangeIngameState(INGAME_STATE.READY);

        //logo time
        logoTimeCountdown = 0.0f;
    }

    void Update()
    {
        MainLoop();
    }

    public void ChangeGameState(GAME_STATE newState)
    {
        if (newState == gameState)
            Debug.Log("Changing the same game state");

        GAME_STATE oldState = gameState;

        switch (gameState)
        {
            case GAME_STATE._COUNT:
                //whean it leaves count - first game launch
                oldState = newState;
                break;
            case GAME_STATE.LOGO:
                break;
            case GAME_STATE.MENU_MAIN:
                break;
            case GAME_STATE.GAME:
                break;
            default:
                break;
        }

        //Debug.Log("Game state changed from " + gameState.ToString() + " to " + newState.ToString());
        gameState = newState;

        switch (gameState)
        {
            case GAME_STATE.LOGO:
                logoTimeCountdown = 0.0f;
                break;
            case GAME_STATE.MENU_MAIN:
                break;
            case GAME_STATE.GAME:
                CleanStage();
                ChangeIngameState(INGAME_STATE.READY);
                break;
            default:
            case GAME_STATE._COUNT:
                break;
        }

    }
    public void ChangeIngameState(INGAME_STATE newState)
    {
        if (newState == ingameState)
            Debug.Log("Changing the same in-game state");

        INGAME_STATE oldState = ingameState;

        switch (ingameState)
        {
            case INGAME_STATE._COUNT:
                //whean it leaves count - first game launch
                oldState = newState;
                break;

            case INGAME_STATE.READY:
                break;
            case INGAME_STATE.GAME:
                break;
            case INGAME_STATE.GAME_OVER:
                break;
            default:
                break;
        }

        //Debug.Log("IN Game state changed from " + ingameState.ToString() + " to " + newState.ToString());
        ingameState = newState;

        switch (ingameState)
        {
            case INGAME_STATE.READY:
                break;
            case INGAME_STATE.GAME:
                break;
            case INGAME_STATE.GAME_OVER:
                break;
            default:
            case INGAME_STATE._COUNT:
                break;
        }
    }

    private void MainLoop()
    {
        switch (gameState)
        {
            case GAME_STATE.LOGO:
                if (logoTimeCountdown > TITLE_DISPLAY_TIME)
                    ChangeGameState(GAME_STATE.MENU_MAIN);
                logoTimeCountdown += Time.deltaTime;
                break;
            case GAME_STATE.MENU_MAIN:
                break;
            case GAME_STATE.GAME:
                GameLoop();
                break;
            default:
            case GAME_STATE._COUNT:
                break;
        }
    }
    private void GameLoop()
    {
        switch (ingameState)
        {
            case INGAME_STATE.READY:
                break;
            case INGAME_STATE.GAME:
                break;
            case INGAME_STATE.GAME_OVER:
                break;
            default:
            case INGAME_STATE._COUNT:
                break;
        }
    }

    private void CleanStage()
    {
    }

    public void GameOver()
    {
        if (gameState != gameController.GAME_STATE.GAME ||
            ingameState != gameController.INGAME_STATE.GAME)
            return;

        ChangeIngameState(gameController.INGAME_STATE.GAME_OVER);
    }

}
