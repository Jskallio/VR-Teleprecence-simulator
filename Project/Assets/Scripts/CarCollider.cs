using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    //This script checks if robot has collided with the wall
    bool hasCollided;
    float timeWhenCollided;
    public float timeForCollision;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Working");
    }

    // Update is called once per frame
    void Update()
    {
        //Timer for collision state to go away
        if (Time.time > timeWhenCollided + timeForCollision && hasCollided)
            hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If we collide with world and collision height is more than 0.25 we can assume we collided with the wall
        
        if (collision.gameObject.tag == "Wall")
        {
            if (!hasCollided)
            {
                foreach (ContactPoint c in collision.contacts)
                {
                    if (Mathf.Abs(c.point.y) > 0.25f)
                    {
                        hasCollided = true;
                        timeWhenCollided = Time.time;
                        Tracking.Instance.addTrackingEvent(TrackingEventType.Crash, "Wall");
                    }
                        
                }
            }
        }
    }
}
