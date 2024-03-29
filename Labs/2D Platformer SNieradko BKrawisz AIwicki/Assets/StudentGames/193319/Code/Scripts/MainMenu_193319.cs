using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_193319 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevel1ButtonPressed()
    {
        StartCoroutine(LoadLevel("StudentGames/193319/Level/Scenes/193319"));
    }

    public void OnExitButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    private static IEnumerator LoadLevel(string levelName)
    {
        var operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
