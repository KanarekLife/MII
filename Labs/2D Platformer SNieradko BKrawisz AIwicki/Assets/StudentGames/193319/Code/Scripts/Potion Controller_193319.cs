using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController_193319 : MonoBehaviour
{
    public GameObject collectionObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SendMessage("IncreaseNumberOfLives");
            this.gameObject.SetActive(false);
            ParticleSystem ps = Instantiate(collectionObject, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new Color(255, 0, 0, 1);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
