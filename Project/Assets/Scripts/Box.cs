using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    //This script is for boxes to register collisions with the subject
    public bool tracking = true;
    public bool hasCollided;
    public float whenCollided;
    public float TimeForCollisionReset = 5f;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //If box is red and enough time has taken turn back to normal
        if (hasCollided)
        {
            if (Time.time > whenCollided + TimeForCollisionReset)
            {
                
                material.color = Color.white;
                hasCollided = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if subject collides with the box track down the collision
        if (collision.gameObject.tag == "Player")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                whenCollided = Time.time;
                material.color = Color.red;
                if(tracking)
                    Tracking.Instance.addTrackingEvent(TrackingEventType.Crash, "Box");
            }
        }
    }
}
