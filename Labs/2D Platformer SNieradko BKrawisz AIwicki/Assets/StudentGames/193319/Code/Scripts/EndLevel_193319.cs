using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndLevel_193319 : MonoBehaviour
{
    [FormerlySerializedAs("text_to_show")] public GameObject textToShow;
    
    // Start is called before the first frame update
    void Start()
    {
        //hide text
        textToShow.gameObject.SetActive(false);
    }

    // detect collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end of the level");
            textToShow.gameObject.SetActive(true);
        }
    }   
}
