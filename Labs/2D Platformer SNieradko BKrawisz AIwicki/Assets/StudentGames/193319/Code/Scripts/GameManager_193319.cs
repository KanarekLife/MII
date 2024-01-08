using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState_193319 { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS }

public class GameManager_193319 : MonoBehaviour
{
    public GameState_193319 currentGameState_193319 = GameState_193319.GS_PAUSEMENU;
    [Header("UI stuff")]
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public TMP_Text bigMessageText;
    public TMP_Text scoreText;
    public TMP_Text defeatedEnemiesText;
    public TMP_Text timeText;
    public TMP_Text qualityLabel;
    public Slider audioSlider;
    public Image[] keysTab;
    public Image[] livesTab;
    public const string keyHighScore = "HighScore_193319";
    public TMP_Text scoreLabel;
    public TMP_Text highScoreLabel;

    public static GameManager_193319 instance;

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
        Debug.Log(_score);
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
        
        AddPoints(_lives * 100);

        LevelCompleted();
    }

    private void SetGameState_193319(GameState_193319 newGameState_193319)
    {
        bigMessageText.text = newGameState_193319 switch
        {
            GameState_193319.GS_GAME_OVER => "Game Over",
            _ => bigMessageText.text
        };

        if (newGameState_193319 == GameState_193319.GS_GAME)
        {
            inGameCanvas.enabled = true;
            Time.timeScale = 1;
        }
        else
        {
            inGameCanvas.enabled = false;
            Time.timeScale = 0;
        }

        if (newGameState_193319 == GameState_193319.GS_LEVELCOMPLETED)
        {
            var currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "193319")
            {
                var highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < _score)
                {
                    highScore = _score;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }

                scoreLabel.text = "Your Score = " + _score;
                highScoreLabel.text = "Your best score = " + highScore;
            }
        }

        pauseMenuCanvas.enabled = newGameState_193319 == GameState_193319.GS_PAUSEMENU;
        optionsCanvas.enabled = newGameState_193319 == GameState_193319.GS_OPTIONS;
        levelCompletedCanvas.enabled = newGameState_193319 == GameState_193319.GS_LEVELCOMPLETED;
        currentGameState_193319 = newGameState_193319;
    }

    public void PauseMenu()
    {
        SetGameState_193319(GameState_193319.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState_193319(GameState_193319.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState_193319(GameState_193319.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState_193319(GameState_193319.GS_GAME_OVER);
    }

    public void Options()
    {
        SetGameState_193319(GameState_193319.GS_OPTIONS);
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

        if (!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }

        qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
        SetVolume(1.0f);
        audioSlider.SetValueWithoutNotify(1.0f);
        
        ResetGame();
        InGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool _keyStillPressed = false;

    // Update is called once per frame
    void Update()
    {
        if(currentGameState_193319 == GameState_193319.GS_GAME)
        {
            updateTime();
        }

        if (Input.GetKey(KeyCode.Escape) && !_keyStillPressed)
        {

            switch (currentGameState_193319)
            {
                case GameState_193319.GS_PAUSEMENU:
                    InGame();
                    break;
                case GameState_193319.GS_GAME:
                    PauseMenu();
                    break;
                case GameState_193319.GS_LEVELCOMPLETED:
                    break;
                case GameState_193319.GS_GAME_OVER:
                    ResetGame();
                    InGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _keyStillPressed = true;
        }

        if(!Input.GetKey(KeyCode.Escape))
        {
            _keyStillPressed = false;
        }
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Scenes/Main Menu");
        if (sceneIndex >= 0)
        {
            SceneManager.LoadSceneAsync(sceneIndex); //ładowanie sceny łączącej gry
        }
        else
        {
            StartCoroutine(LoadScene("StudentGames/193319/Level/Scenes/MainMenu"));
        }
    }

    public void OnOptionsButtonClicked()
    {
        Options();
    }

    private static IEnumerator LoadScene(string sceneName)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void IncreaseQualityLevel()
    {
        QualitySettings.IncreaseLevel();
        qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void DecreaseQualityLevel()
    {
        QualitySettings.DecreaseLevel();
        qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }
}
