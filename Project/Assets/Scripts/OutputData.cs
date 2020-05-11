using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OutputData : MonoBehaviour
{
    //This script extrats data from the tracking files
    public TrackingContainer trackingPoints = new TrackingContainer();
    public string path;
    public string output;
    // Start is called before the first frame update
    void Start()
    {
        //Open target folder
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] files = info.GetFiles();
        string toWrite = "Filename, Control method, Time, Collisions\n";

        foreach (FileInfo f in files)
        {
            if (f.Extension == ".json")
            {
                loadTracking(f.FullName);
                //Calculate time spent on track
                float time = trackingPoints.points[trackingPoints.points.Count - 1].time - trackingPoints.points[0].time;

                int collisions = 0;
                string wii = trackingPoints.points[1].data;
                //Count collisions
                foreach(TrackingEvent e in trackingPoints.points)
                {
                    if (e.type == TrackingEventType.Crash)
                        collisions++;

                }
                //write down all information about this file
                toWrite += f.Name + ", " + wii + ", " + time + ", " + collisions + "\n";
            }
        }
        //Write to output file
        File.WriteAllText(output, toWrite);
        Debug.Log("Done!!");
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadTracking(string filename)
    {
        //Simple function to load tracking file
        using (StreamReader sr = new StreamReader(filename))
        {
            trackingPoints = JsonUtility.FromJson<TrackingContainer>(sr.ReadToEnd());
        }
    }
}
