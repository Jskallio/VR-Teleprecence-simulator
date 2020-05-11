using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Visualizer_FileController : MonoBehaviour
{
    //This is for UI of the visualizer file controller
    public static Visualizer_FileController Instance;

    //Path where we want to open the tracking files
    public string path;

    //Panels for choosing files
    List<Visualizer_FilePanel> panelList = new List<Visualizer_FilePanel>();

    //Prefab for file panel and parent transform where we put them
    public GameObject prefab;
    public Transform parent;

    public GameObject Menu;

    VisualizeJSON vis;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Get tracking files from wanted path
        vis = VisualizeJSON.Instance;
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] files = info.GetFiles();

        foreach(FileInfo f in files)
        {
            //If json file is found create file panel for it
            if (f.Extension == ".json")
            {
                GameObject temp = Instantiate(prefab, parent);
                Visualizer_FilePanel panel = temp.GetComponent<Visualizer_FilePanel>();
                panel.file = f.FullName;
                panel.toggle.isOn = false;
                panelList.Add(panel);
                
            }
        }
    }


    public void ShowLines()
    {
        //Call visualizer to clear old lines and show wanted lines
        vis.clearLines();
        Debug.Log("Clearing lines");
        foreach (Visualizer_FilePanel p in panelList)
        {
            if(p.toggle.isOn)
            {
                vis.loadTracking(p.file);
                vis.drawLines();
            }
            
        }
    }

    public void showHideMenu()
    {
        //Button for hiding/showing the menu
        if (Menu.activeSelf)
            Menu.SetActive(false);
        else
            Menu.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
