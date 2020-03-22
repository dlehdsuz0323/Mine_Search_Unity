using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSuperVisor : MonoBehaviour
{
    public enum GAMESTATE
    {
        GAME_PLAY,
        GAME_OVER,
        GAME_CLEAR
    }

    public GameObject GameOverText;

    private static GameSuperVisor instance;
    public static GameSuperVisor GetInstance()
    {
        if (instance == null)
        {
            instance = new GameSuperVisor();
        }
        return instance;
    }

    public int GameState;

    private void Awake()
    {
        GameSuperVisor.instance = this;
    }

    void Start ()
    {
        GameState = (int)GAMESTATE.GAME_PLAY;

    }
	
	void Update ()
    {
		if(GameState != (int)GAMESTATE.GAME_PLAY)
        {
            GameOverText.SetActive(true);
        }
	}
}
