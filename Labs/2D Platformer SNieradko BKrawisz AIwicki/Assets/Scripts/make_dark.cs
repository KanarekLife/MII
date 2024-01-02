using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class make_dark : MonoBehaviour
{
    public Light2D playerLight;
    public Light2D globalLight;

    public float changeSpeed = 0.5f;
    public float playerTarget = 1.0f;
    public float globalTarget = 0.2f;

    private bool inDark;

    // Start is called before the first frame update
    void Start()
    {
        inDark = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        inDark = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inDark) return;

        bool changing = false;

        if(globalLight.intensity > globalTarget) {
            globalLight.intensity -= changeSpeed * Time.deltaTime;
            changing = true;
        } else {
            globalLight.intensity = globalTarget;
        }

        if(playerLight.intensity < playerTarget) {
            playerLight.intensity += changeSpeed * Time.deltaTime;
            changing = true;
        } else {
            playerLight.intensity = playerTarget;
        }

        if(!changing) {
            inDark = false;
        }
    }
}
