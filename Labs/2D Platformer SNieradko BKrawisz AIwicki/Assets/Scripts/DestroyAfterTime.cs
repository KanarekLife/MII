using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float waitTime = 1.0f;

    bool started = false;

    private IEnumerator KillAfterTime() {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(started) return;
        StartCoroutine (KillAfterTime());
        started = true;
    }
}
