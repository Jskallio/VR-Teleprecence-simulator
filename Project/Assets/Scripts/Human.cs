using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    //Script for the human movement


    public bool tracking;
    public List<Vector3> waypoints = new List<Vector3>();
    public int wayID;
    //public Vector3 target;
    public bool fail;
    public float checkDelay = 1f;

    public float startRandomizer = 1;

    public float speedMult = 1f;
    public float angularMult = 0.25f;

    public float pointDistance = 1f;

    public Transform target;
    public bool wait;

    public Animator animator;

    public bool hasCollided;
    public float whenCollided;
    public float TimeForCollisionReset = 5f;


    public Material[] materials;

    public bool loop = false;
    // Start is called before the first frame update
    void Start()
    {

        //Get animator component and materials from the object
        animator = GetComponentInChildren<Animator>();
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            materials[i] = renderers[i].material;
    }

    private void Update()
    {
        //If we have collided and enough time passed return to normal state
        if(hasCollided)
        {
            if(Time.time>whenCollided+TimeForCollisionReset)
            {
                foreach(Material m in materials)
                    m.color = Color.white;
                hasCollided = false;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("Idle", !wait);
        //Do we have waypoints
        if(waypoints!=null)
        {
            //If we are not in the end of the sequence and we are not told to wait
            if(wayID<waypoints.Count && !wait)
            {
                //Get the direction  vector for next waypoint
                Vector3 direction = waypoints[wayID] - transform.position;
                direction.y = 0;
                float l = direction.magnitude;
                //Is the human close to the next waypoint
                if(l<pointDistance)
                {
                    //Take next waypoint. If we are in last point and loop is enabled then continue from start
                    wayID++;
                    if (wayID==waypoints.Count)
                    {
                        if(!loop)
                        {
                            waypoints = null;
                            wait = true;
                        }
                        else
                        {
                            wayID = 0;
                        }
                        
                    }
                }
                else
                {
                    //Move towards the next waypoint

                    //Normalize direction vector
                    direction = direction / l;

                    // Get speed and direction vectors. Sign is to determine which direction we need to turn
                    float speed = Vector3.Dot(transform.forward, direction)*speedMult;
                    float dir = Vector3.Angle(transform.forward, direction)*angularMult;
                    float sign = Vector3.Cross(transform.forward,direction).y;

                    //Select turning direction based on sign value
                    if (sign < 0)
                    {
                        dir = -dir;
                    }

                    //Move forward and rotate according to speed and direction vectors
                    transform.position += transform.forward * speed * Time.fixedDeltaTime;
                    transform.Rotate(Vector3.up*dir*Time.fixedDeltaTime);
                    Debug.DrawLine(transform.position, waypoints[wayID]);

                }
                
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //If we collide with player then track down collision
        if(other.tag=="Player")
        {
            if(!hasCollided)
            {
                hasCollided = true;
                whenCollided = Time.time;
                foreach (Material m in materials)
                    m.color = Color.red;
                if(tracking)
                    Tracking.Instance.addTrackingEvent(TrackingEventType.Crash, "Human");
            }
        }
    }

    /*public IEnumerator askForPath(float startTime, float delay)
    {
        yield return new WaitForSeconds(startTime);
        while(true)
        {
            if(wait)
            {
                if(Random.Range(0f,1f)>0.1f)
                    wait = false;
                continue;
            }
            if(!controller.humen.Contains(this))
                controller.humen.Enqueue(this);
            yield return new WaitForSeconds(delay);
        }
    }*/
}
