using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndEnd : MonoBehaviour
{
    //Script for detecting start line and end line crossings
    bool hasTriggered;

    public bool isStart = true;
    public bool allowSkip = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //If allowed to skip then S key skips stage. ONLY FOR DEBUG!!
        if (Input.GetKeyDown(KeyCode.S) && allowSkip)
        {
            Tracking.Instance.addTrackingEvent(TrackingEventType.End, "");
            Tracking.Instance.endTracking();
            //if(WiiController.Instance!=null)
            //  WiiController.Instance.allowSending = false;
            Main.Instance.changeState();
        }
        }

    private void OnTriggerEnter(Collider other)
    {
        //If collided with player
        if (other.tag == "Player" && !hasTriggered)
        {
            hasTriggered = true;
            if(isStart)//If it's start then start tracking
            {
                Tracking.Instance.startTracking();
            }
            else//If it's end then end tracking and go to next State
            {
                Tracking.Instance.addTrackingEvent(TrackingEventType.End, "");
                Tracking.Instance.endTracking();
                //if(WiiController.Instance!=null)
                //  WiiController.Instance.allowSending = false;
                Main.Instance.changeState();
            }
            
        }
    }
}
