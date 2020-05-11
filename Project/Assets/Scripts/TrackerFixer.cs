using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TrackerFixer : MonoBehaviour
{
//This script was made to fix the bug in track files

    //Container for tracking points in file
    public TrackingContainer trackingPoints = new TrackingContainer();

    //Where we get tracking files from?
    public string fromPath;
    //Where to save fixed files to
    public string toPath;
    public bool fixAll = false;
    //We need to start outputdata script after we are done
    public OutputData outdata;
    // Start is called before the first frame update
    void Start()
    {
        outdata = GetComponent<OutputData>();
        if(fixAll)
        {
            DirectoryInfo info = new DirectoryInfo(fromPath);
            FileInfo[] files = info.GetFiles();

            foreach (FileInfo f in files)
            {
                if (f.Extension == ".json")
                {
                    loadTracking(f.FullName);
                    fix();
                    saveTracking(toPath + f.Name);
                    Debug.Log("Fixed file " + f.Name);
                }
            }
        }

        outdata.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fix()
    {
        //Problem with files was the control method switches to come out as double. Because of that the used movement methods were messed up. 
        //We had to manually change the starting method from some of the files
        bool started = false;
        int current = 0;
        List<int> toRemove = new List<int>();
        for(int i=0; i<trackingPoints.points.Count; i++)
        {
            TrackingEvent e = trackingPoints.points[i];
            //When the starting event is found, take the next event, which is control method event, and take the method into variable
            if(e.type == TrackingEventType.Start)
            {
                started = true;
                current = trackingPoints.points[i + 1].data == "WII" ? 0 : 1;
                i++;
            }
            else if(!started)
            {
                //If we didn't meet the starting event yet we can delete these events as they are from practice range
                toRemove.Add(i);
            }
            else if(e.type == TrackingEventType.Change)
            {
                //When we get control change event we override the information about which control method is used based on the starting method
                e.data = current == 1 ? "WII" : "JOY";
                if (current == 1)
                    current = 0;
                else
                    current = 1;
                i++;
                toRemove.Add(i);
            }
        }
        //Remove all points that happened before Start-event
        toRemove.Reverse();
        foreach(int i in toRemove)
        {
            trackingPoints.points.RemoveAt(i);
        }
    }

    public void loadTracking(string filename)
    {
        //Simple script for loading the tracking data
        using (StreamReader sr = new StreamReader(filename))
        {
            trackingPoints = JsonUtility.FromJson<TrackingContainer>(sr.ReadToEnd());
        }
    }

    public void saveTracking(string filename)
    {
        string data = JsonUtility.ToJson(trackingPoints,true);
        File.WriteAllText(filename, data);
        
    }
}
