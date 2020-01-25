using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Keybinder : MonoBehaviour
{
    private TextMeshProUGUI m_textRef;
    private IEnumerator m_coroutine;
    private IEnumerator KeyWait()
    {
        float pid = BuildSubjectLogic.m_currentPlayerId;
        bool done = false;
        while (Input.GetKeyDown("joystick " + pid + " button 0"))
        {
            //Wait for player to release selection button before proceeding
            yield return null;
        }
        while (!done)
        {
            // Key checker 
            if (Input.GetKeyDown("joystick " + pid + " button 0"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 1"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 2"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 3"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 4"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 5"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 6"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 7"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 8"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 9"))
            {
                Debug.Log("Got button press");
                done = true;
            }
            // Axis Checker
            if (Mathf.Abs(Input.GetAxis("X" + pid + "_axis")) > 0.3)
            {
                Debug.Log("X axis");
                done = true;
            }
            if (Mathf.Abs(Input.GetAxis("Y" + pid + "_axis")) > 0.3)
            {
                Debug.Log("Y axis");
                done = true;
            }
            if (Mathf.Abs(Input.GetAxis("RX" + pid + "_axis")) > 0.3)
            {
                Debug.Log("RX axis");
                done = true;
            }
            if (Mathf.Abs(Input.GetAxis("RY" + pid + "_axis")) > 0.3)
            {
                Debug.Log("RY axis");
                done = true;
            }
            if (Mathf.Abs(Input.GetAxis("T" + pid + "_axis")) > 0.3)
            {
                Debug.Log("T axis");
                done = true;
            }
            yield return null;
        }
        EventSystemInputManager.ToggleInput();
    }
    public void SetTextRef(TextMeshProUGUI textRef)
    {
        m_textRef = textRef;
    }
    public void SetInputBinding(string newInputAxis)
    {
        //Sets the binding sent it to P(pid)_axis
        Debug.Log("Test");
        m_coroutine = KeyWait();
        StartCoroutine(m_coroutine);
        //Toggles the paused state for input controller while waiting for keybind input
        EventSystemInputManager.ToggleInput();
    }
    
}

