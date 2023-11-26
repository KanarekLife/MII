using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [Range(0.01f, 20.0f)][SerializeField] private float radius = 7.0f;
    [Range(0.01f, 20.0f)][SerializeField] private float speed = 7.0f;
    private const int PLATFORMS_NUM = 6;
    private GameObject[] platforms;
    private Vector3[] positions;
    private float offset = 0;

    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        for(int i = 0; i < platforms.Length; i++)
        {
            positions[i].x = (float)Math.Cos(i*2*Math.PI/platforms.Length) * radius;
            positions[i].y = (float)Math.Sin(i*2*Math.PI/platforms.Length) * radius;
            positions[i].z = 0;

            platforms[i] = Instantiate(platformPrefab, this.transform.position + positions[i], Quaternion.identity);
            platforms[i].transform.SetParent(this.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed * 2 * (float)Math.PI / 20.0f;
        offset %= 2 * (float)Math.PI;

        for (int i = 0; i < platforms.Length; i++)
        {
            positions[i].x = (float)Math.Cos(i * 2 * Math.PI / platforms.Length + offset) * radius;
            positions[i].y = (float)Math.Sin(i * 2 * Math.PI / platforms.Length + offset) * radius;
            positions[i].z = 0;

            platforms[i].transform.position = this.transform.position + positions[i];
        }
    }
}
