using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class make_light_193319 : MonoBehaviour
{
    public Light2D playerLight;
    public Light2D globalLight;

    public float changeSpeed = 0.5f;
    public float playerTarget = 0.0f;
    public float globalTarget = 1.0f;

    private bool inLight;

    // Start is called before the first frame update
    void Start()
    {
        inLight = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        inLight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inLight) return;

        bool changing = false;

        if(globalLight.intensity < globalTarget) {
            globalLight.intensity += changeSpeed * Time.deltaTime;
            changing = true;
        } else {
            globalLight.intensity = globalTarget;
        }

        if(playerLight.intensity > playerTarget) {
            playerLight.intensity -= changeSpeed * Time.deltaTime;
            changing = true;
        } else {
            playerLight.intensity = playerTarget;
        }

        if(!changing) {
            inLight = false;
        }
    }
}
