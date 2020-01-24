using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private IEnumerator KeyWait()
    {
        yield return GetKeyPress();
    }
    private IEnumerator GetKeyPress()
    {
        float pid = BuildSubjectLogic.m_currentPlayerId;
        bool done = false;
        while(!done)
        {
            // Key checker 
            if (Input.GetKeyDown("joystick " + pid + " button 0"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 1"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 2"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 3"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 4"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 5"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 6"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 7"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 8"))
            {
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 9"))
            {
                done = true;
            }
            // Axis Checker
            if (Input.GetAxis("X" + pid + "_axis") > 0.3)
            {
                done = true;
            }
            if (Input.GetAxis("Y" + pid + "_axis") > 0.3)
            {
                done = true;
            }
            if (Input.GetAxis("RX" + pid + "_axis") > 0.3)
            {
                done = true;
            }
            if (Input.GetAxis("RY" + pid + "_axis") > 0.3)
            {
                done = true;
            }
            if (Input.GetAxis("T" + pid + "_axis") > 0.3)
            {
                done = true;
            }
        }
        return null;
    }
    public void SetInputBinding(string newInputAxis)
    {
        KeyWait();
    }
}
