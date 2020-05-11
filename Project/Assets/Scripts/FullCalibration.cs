using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;

public class FullCalibration : MonoBehaviour
{
    //This script does the full calibration sequence

    public Text instructionText;
    public Text subjectText;

    public GameObject startText, centerCalib, frontCalib, backCalib, endCalib;
    public bool started;


    public SteamVR_Action_Boolean grabPinch;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    public bool fullCalibration = true;

    Main main;


    private void OnEnable()
    {
        //Set trigger listener
        if(grabPinch != null)
        {
            grabPinch.AddOnChangeListener(TriggerPressedOrReleased, inputSource);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        main = Main.Instance;
        subjectText.text = "SubjectID: " + main.subjectID;
    }

    // Update is called once per frame
    void Update()
    {

        //At start pressing space will start the calibration
        if (!started && Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            startText.SetActive(false);
            centerCalib.SetActive(true);
            instructionText.text = "Please stand still on the center of the board";
        }
    }

    public void pressedButton(string state)
    {
        //Handle button presses
        switch(state)
        {
            case "center": // Send data to WiiController script about center point. After center calibration go to middle calibration. Except when we are doing only half calibration when we go straight to the end
                //main.calibration.middle(...)
                WiiController.Instance.CalibrateCenterOfBalance();
                if (fullCalibration)
                {
                    centerCalib.SetActive(false);
                    frontCalib.SetActive(true);
                    instructionText.text = "Lean as far forward as you feel comfortable and signal when you are ready";
                }
                else
                {
                    centerCalib.SetActive(false);
                    endCalib.SetActive(true);
                    instructionText.text = "Stand still and wait for next round to start";
                }
                break;
            case "front": //Send data to WiiController script about front leaning.After front calibration comes the back calibration
                WiiController.Instance.GetLeanLimits(true);
                frontCalib.SetActive(false);
                backCalib.SetActive(true);
                instructionText.text = "Lean as far back as you feel comfortable and signal when you are ready";
                break;
            case "back": //Send data to WiiController script about back leaning.After front calibration comes the instruction time
                WiiController.Instance.GetLeanLimits(false);
                backCalib.SetActive(false);
                endCalib.SetActive(true);
                instructionText.text = "Stand still and take the headset off for instructions";

                break;
            case "start": // After giving instructions go to next scene
                main.changeState();
                break;
            case "reset": //Reset calibration sequence
                SceneManager.LoadScene("FullCalibration");
                break;
        }
    }

    public void endCalibration()
    {
        main.changeState();
    }

    private void TriggerPressedOrReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool Active)
    {
        
    }
}
