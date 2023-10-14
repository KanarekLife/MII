using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end_level : MonoBehaviour
{
    public GameObject text_to_show;
    // Start is called before the first frame update
    void Start()
    {
        //hide text
        text_to_show.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // detect collisoin with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end of the level");
            text_to_show.gameObject.SetActive(true);
        }
    }   
}
