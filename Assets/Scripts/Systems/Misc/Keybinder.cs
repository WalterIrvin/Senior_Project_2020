using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Keybinder : MonoBehaviour
{
    private TextMeshProUGUI m_textRef;
    private IEnumerator m_coroutine;
    //Current bindings for all controls
    //First is what input axis we are looking for, and the second parameter is the axis we are actually looking at
    //It allows Pitch to be determined by the "Yaw" controls for example, saving on having to physically edit keybindings
    public static Dictionary<string, string> m_axisDictionary = new Dictionary<string, string>() 
    {
        {"P1_Pitch",  "P1_Pitch"},
        {"P2_Pitch", "P2_Pitch"},
        {"P1_Yaw", "P1_Yaw"},
        {"P2_Yaw", "P2_Yaw"},
        {"P1_Roll", "P1_Roll"},
        {"P2_Roll", "P2_Roll"},
        {"P1_Thrust", "P1_Thrust"},
        {"P2_Thrust", "P2_Thrust"},
        {"P1_PrimaryFire", "P1_PrimaryFire"},
        {"P2_PrimaryFire", "P2_PrimaryFire"},
        {"P1_SecondaryFire", "P1_SecondaryFire"},
        {"P2_SecondaryFire", "P2_SecondaryFire"}
    };
    //Classifies the input scheme into axis only or key only types
    private static Dictionary<string, string> m_typeDictionary = new Dictionary<string, string>()
    {
        {"_Pitch", "Axis"},
        {"_Yaw", "Axis"},
        {"_Roll", "Axis"},
        {"_Thrust", "Axis"},
        {"_PrimaryFire", "Key"},
        {"_SecondaryFire", "Key"}
    };

    //Based on which button is pressed for an axis-only keybinding, will determine which input to reference as control scheme
    private static Dictionary<string, string> m_axisToButton = new Dictionary<string, string>()
    {
        {"joystick 1 button 0", "P1_(A/Y)"},
        {"joystick 2 button 0", "P2_(A/Y)"},

        {"joystick 1 button 3", "P1_(Y/A)"},
        {"joystick 2 button 3", "P2_(Y/A)"},

        {"joystick 1 button 1", "P1_(B/X)"},
        {"joystick 2 button 1", "P2_(B/X)"},

        {"joystick 1 button 2", "P1_(X/B)"},
        {"joystick 2 button 2", "P2_(X/B)"},

        {"joystick 1 button 4", "P1_(LB/RB)"},
        {"joystick 2 button 4", "P2_(LB/RB)"},

        {"joystick 1 button 5", "P1_(RB/LB)"},
        {"joystick 2 button 5", "P2_(RB/LB)"}
    };

    //Translate raw keybindings into more human-like text
    private static Dictionary<string, string> m_localizer = new Dictionary<string, string>()
    {
        {"joystick 1 button 0", "A button"},
        {"joystick 2 button 0", "A button"},

        {"joystick 1 button 1", "B button"},
        {"joystick 2 button 1", "B button"},

        {"joystick 1 button 2", "X button"},
        {"joystick 2 button 2", "X button"},

        {"joystick 1 button 3", "Y button"},
        {"joystick 2 button 3", "Y button"},

        {"joystick 1 button 4", "LB button"},
        {"joystick 2 button 4", "LB button"},

        {"joystick 1 button 5", "RB button"},
        {"joystick 2 button 5", "RB button"},

        {"joystick 1 button 6", "Back button"},
        {"joystick 2 button 6", "Back button"},

        {"joystick 1 button 7", "Start button"},
        {"joystick 2 button 7", "Start button"},

        {"joystick 1 button 8", "L-Stick button"},
        {"joystick 2 button 8", "L-Stick button"},

        {"joystick 1 button 9", "R-Stick button"},
        {"joystick 2 button 9", "R-Stick button"},

        {"P1_(A/Y)", "A/Y"},
        {"P2_(A/Y)", "A/Y"},
        {"P1_(Y/A)", "Y/A"},
        {"P2_(Y/A)", "Y/A"},

        {"P1_(B/X)", "B/X"},
        {"P2_(B/X)", "B/X"},
        {"P1_(X/B)", "X/B"},
        {"P2_(X/B)", "X/B"},

        {"P1_(LB/RB)", "LB/RB"},
        {"P2_(LB/RB)", "LB/RB"},
        {"P1_(RB/LB)", "RB/LB"},
        {"P2_(RB/LB)", "RB/LB"},

        //Axis only localiser
        {"X1_axis", "X axis"},
        {"X2_axis", "X axis"},

        {"Y1_axis", "Y axis"},
        {"Y2_axis", "Y axis"},

        {"RX1_axis", "RX axis"},
        {"RX2_axis", "RX axis"},

        {"RY1_axis", "RY axis"},
        {"RY2_axis", "RY axis"},

        {"T1_axis", "T axis"},
        {"T2_axis", "T axis"},
    };
    private void AxisButtonSetter(string inputFound, string fullBinding)
    {
        //If player attempts to set an axis bind to a key, will select the input they push as positive, with the reverse button as negative
        Debug.Log(inputFound);
        string axisInput = m_axisToButton[inputFound];
        FinishBinding(axisInput, fullBinding);
    }
    private void FinishBinding(string input, string fullBinding)
    {
        m_axisDictionary[fullBinding] = input;
        m_textRef.SetText(m_localizer[input]);
    }
    private IEnumerator KeyWait(string axis)
    {
        float pid = BuildSubjectLogic.m_currentPlayerId;
        bool done = false;
        string fullBinding = "P" + pid + axis;
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
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 0", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 0", fullBinding);
                }
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 1"))
            {
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 1", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 1", fullBinding);
                }
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 2"))
            {
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 2", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 2", fullBinding);
                }
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 3"))
            {
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 3", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 3", fullBinding);
                }
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 4"))
            {
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 4", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 4", fullBinding);
                }
                done = true;
            }
            if (Input.GetKeyDown("joystick " + pid + " button 5"))
            {
                if (m_typeDictionary[axis] == "Axis")
                {
                    //Setting axis up correctly if not button
                    AxisButtonSetter("joystick " + pid + " button 5", fullBinding);
                }
                else
                {
                    FinishBinding("joystick " + pid + " button 5", fullBinding);
                }
                done = true;
            }
            //No valid axis binding zone
            if (m_typeDictionary[axis] != "Axis")
            {
                if (Input.GetKeyDown("joystick " + pid + " button 6"))
                {
                    FinishBinding("joystick " + pid + " button 6", fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 7"))
                {
                    FinishBinding("joystick " + pid + " button 7", fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 8"))
                {
                    FinishBinding("joystick " + pid + " button 8", fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 9"))
                {
                    FinishBinding("joystick " + pid + " button 9", fullBinding);
                    done = true;
                }
            }
            // Axis Checker
            if (m_typeDictionary[axis] != "Key")
            {
                if (Mathf.Abs(Input.GetAxis("X" + pid + "_axis")) > 0.3)
                {
                    FinishBinding("X" + pid + "_axis", fullBinding);
                    done = true;
                }
                if (Mathf.Abs(Input.GetAxis("Y" + pid + "_axis")) > 0.3)
                {
                    FinishBinding("Y" + pid + "_axis", fullBinding);
                    done = true;
                }
                if (Mathf.Abs(Input.GetAxis("RX" + pid + "_axis")) > 0.3)
                {
                    FinishBinding("RX" + pid + "_axis", fullBinding);
                    done = true;
                }
                if (Mathf.Abs(Input.GetAxis("RY" + pid + "_axis")) > 0.3)
                {
                    FinishBinding("RY" + pid + "_axis", fullBinding);
                    done = true;
                }
                if (Mathf.Abs(Input.GetAxis("T" + pid + "_axis")) > 0.3)
                {
                    FinishBinding("T" + pid + "_axis", fullBinding);
                    done = true;
                }
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
        m_coroutine = KeyWait(newInputAxis);
        StartCoroutine(m_coroutine);
        //Toggles the paused state for input controller while waiting for keybind input
        EventSystemInputManager.ToggleInput();
    }
    
}

