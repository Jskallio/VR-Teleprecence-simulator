using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObstacles : MonoBehaviour
{
    //This script is used to triggers for activating obstacles
    public GameObject[] show;
    public GameObject[] hide;
    bool hasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //When "player" collides with the trigger set chosen obstacles visible and others invisible
        if (other.tag == "Player" && !hasTriggered)
        {
            hasTriggered = true;
            foreach (GameObject h in show)
                h.SetActive(true);
            foreach (GameObject h in hide)
                h.SetActive(false);
        }
    }
}
