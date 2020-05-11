using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    //This script controls whole test process
    public static Main Instance;
    

    private void Awake()
    {
        if (Instance != null && Instance!=this)
            Destroy(gameObject);
        Instance = this;
    }

    //State for our test process
    public enum State { StartMenu, FullCalibration, FirstRun, SecondRun, Switch, MiddleCalibration};
    public State currentState;
    public State nextState;

    //Did we choose joystick first run
    public bool joystickFirst;

    //Our subject ID
    public string subjectID;

    public ControllerType startType = ControllerType.Joystick;
    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this.gameObject);
        currentState = State.StartMenu;
        nextState = State.FullCalibration;
    }

    //Functions for getting the information about if we want joystick first and the subject id
    public void jFirst(bool value)
    {
        joystickFirst = value;
    }

    public void changeSubjectID(string value)
    {
        subjectID = value;
    }

    public void changeState()
    {
        //Changge current state
        switch(currentState)
        {
            case State.StartMenu: // From start menu we go to full calibration
                currentState = nextState;
                nextState = State.FirstRun;
                SceneManager.LoadScene("FullCalibration");
                break;
            case State.FullCalibration: //From full calibration we go to first/second-scene
                currentState = nextState;
                
                if (joystickFirst)
                {
                    SceneManager.LoadScene("FirstAndSecondScene");

                    nextState = State.MiddleCalibration;
                }
                else
                {
                    SceneManager.LoadScene("FirstAndSecondScene");
                    nextState = State.MiddleCalibration;
                }
                break;
            case State.FirstRun: //After first run we go to middle calibration
                currentState = nextState;
                nextState = State.SecondRun;

                SceneManager.LoadScene("MiddleCalibration");
                break;
            case State.SecondRun: //After second run we go to middle calibration
                currentState = nextState;
                nextState = State.Switch;
                SceneManager.LoadScene("MiddleCalibration");
                break;
            case State.Switch:
                Application.Quit();
                break;
            case State.MiddleCalibration: //After middle calibration we go to either second run or to last run
                currentState = nextState;
                switch(currentState)
                {
                    case State.SecondRun:
                        nextState = State.MiddleCalibration;
                        if (joystickFirst)
                        {
                            SceneManager.LoadScene("FirstAndSecondScene");
                        }
                            
                        else
                        {
                            SceneManager.LoadScene("FirstAndSecondScene");
                        }
                            
                        break;
                    case State.Switch://After last run quit
                        nextState = State.StartMenu;
                        Application.Quit();
                        break;
                }
                break;
                    


        }
    }


    public string getFileName()//Funciont for giving file name for tracking file
    {
        string file = "/"+subjectID;

        switch(currentState)
        {
            case State.FirstRun:
                file += "_first.json";
                break;
            case State.SecondRun:
                file += "_second.json";
                break;
            case State.Switch:
                file += "_third.json";
                break;

        }

        return file;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    
}