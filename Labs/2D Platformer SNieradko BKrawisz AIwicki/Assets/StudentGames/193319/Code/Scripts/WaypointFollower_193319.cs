using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower_193319 : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject[] waypoints;
    [Range(0.01f, 20.0f)][SerializeField] private float speed = 1.0f;

    private int _currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector2.Distance(platform.transform.position, waypoints[_currentWaypoint].transform.position);
        if (distanceToTarget < 0.1f)
        {
            _currentWaypoint++;
            _currentWaypoint %= waypoints.Length;
        }

        platform.transform.position = Vector2.MoveTowards(platform.transform.position, waypoints[_currentWaypoint].transform.position, speed * Time.deltaTime);
    }
}
