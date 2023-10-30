using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    [Header("UI stuff")]
    public Canvas inGameCanvas;
    public TMP_Text bigMessageText;
    public TMP_Text scoreText;
    public TMP_Text defeatedEnemiesText;
    public TMP_Text timeText;
    public Image[] keysTab;
    public Image[] livesTab;

    public static GameManager instance;

    // prvate game state
    private int _lives = 3;
    private int _score = 0;
    private int _defeatedEnemies = 0;
    private int _keysFound = 0;
    private const int KeysNumber = 3;
    private float _timer = 0;

    public void AddLives(int lives)
    {
        _lives += lives;
        for (int i = 0; i < livesTab.Length; i++)
        {
            if (i >= _lives)
            {
                livesTab[i].enabled = false;
            } else
            {
                livesTab[i].enabled = true;
            }
        }

        if (_lives <= 0)
        {
            GameOver();
        }
    }

    public void AddPoints(int points)
    {
        _score += points;
        scoreText.text = _score.ToString();
    }

    public void AddDefeatedEnemy(int count)
    {
        _defeatedEnemies += count;
        defeatedEnemiesText.text = _defeatedEnemies.ToString();
    }

    public void AddKey()
    {
        _keysFound++;
        keysTab[_keysFound - 1].color = Color.white;
    }

    public void updateTime()
    {
        _timer += Time.deltaTime;
        timeText.text = string.Format("{0:00}:{1:00}", (int)(_timer/60), (int)_timer%60);
    }

    public void PlayerReachedEnd()
    {
        if (_keysFound < KeysNumber)
        {
            bigMessageText.text = "Not enough keys";
            return;
        }

        LevelCompleted();
    }

    void SetGameState(GameState newGameState)
    {
        switch(newGameState)
        {
            case GameState.GS_PAUSEMENU:
                bigMessageText.text = "Paused";
                break;
            case GameState.GS_GAME:
                bigMessageText.text = "";
                break;
            case GameState.GS_LEVELCOMPLETED:
                bigMessageText.text = "Level completed!";
                break;
            case GameState.GS_GAME_OVER:
                bigMessageText.text = "Game Over";
                break;
        }

        if (newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
            Time.timeScale = 1;
        }
        else
        {
            inGameCanvas.enabled = false;
            Time.timeScale = 0;
        }

        currentGameState = newGameState;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void ResetGame()
    {
        _score = 0;
        AddPoints(0);   // update UI
        _lives = 3;
        AddLives(0);    // update UI
        _timer = 0;
        _defeatedEnemies = 0;
        AddDefeatedEnemy(0);    // update UI

        // grey out key icons
        foreach (Image keyImage in keysTab)
        {
            keyImage.color = Color.grey;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        ResetGame();
        PauseMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool _keyStillPressed = false;

    // Update is called once per frame
    void Update()
    {
        if(currentGameState == GameState.GS_GAME)
        {
            updateTime();
        }

        if (Input.GetKey(KeyCode.Escape) && !_keyStillPressed)
        {

            switch (currentGameState)
            {
                case GameState.GS_PAUSEMENU:
                    InGame();
                    break;
                case GameState.GS_GAME:
                    PauseMenu();
                    break;
                case GameState.GS_LEVELCOMPLETED:
                    break;
                case GameState.GS_GAME_OVER:
                    ResetGame();
                    InGame();
                    break;
            }

            _keyStillPressed = true;
        }

        if(!Input.GetKey(KeyCode.Escape))
        {
            _keyStillPressed = false;
        }
    }
}
