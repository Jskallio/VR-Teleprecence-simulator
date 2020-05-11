using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    //Redundant class for controlling the people
    /*public static CrowdController Instance;

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        Instance = this;
    }


    public Queue<Human> humen = new Queue<Human>();
    public List<Transform> targets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(calculatePaths());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator calculatePaths()
    {
        while (true)
        {
            if (humen.Count > 0)
            {
                Human h = humen.Dequeue();
                h.obstacle.enabled = false;
                NavMeshPath path = new NavMeshPath();
                bool result = h.meshAgent.CalculatePath(h.target, path);
                if (result)
                {
                    h.waypoints = path.corners;
                    h.fail = false;
                }
                else
                {
                    if (h.fail)
                    {
                        h.waypoints = null;
                    }
                    h.fail = true;
                }
                h.obstacle.enabled = true;
                h.wayID = 1;

            }
            yield return null;
        }
    }

    public Vector3 getTarget()
    {
        return targets[Random.Range(0, targets.Count)].position;
    }*/
}

