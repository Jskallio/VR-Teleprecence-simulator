using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHumanWalking : MonoBehaviour
{
    //This is for trigger to start humans walking
    public Human[] humans;
    bool hasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //If colliding with player then start given humans
        if(other.tag == "Player" && !hasTriggered)
        {
            hasTriggered = true;
            foreach (Human h in humans)
                h.wait = false;
        }
    }
}

