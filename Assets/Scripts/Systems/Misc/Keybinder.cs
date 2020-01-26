using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Keybinder : MonoBehaviour
{
    private TextMeshProUGUI m_textRef;
    private IEnumerator m_coroutine;
    public static int m_YawInv = -1;
    public static int m_RollInv = -1;
    public static int m_PitchInv = 1;
    public static int m_ThrustInv = 1;
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

    //Generic button mapping to input axis to unity, this allows it to be picked up by Input
    private static Dictionary<string, string> m_buttonToAxis = new Dictionary<string, string>()
    {
        {"joystick 1 button 0", "P1_A"},
        {"joystick 2 button 0", "P2_A"},

        {"joystick 1 button 1", "P1_B"},
        {"joystick 2 button 1", "P2_B"},

        {"joystick 1 button 2", "P1_X"},
        {"joystick 2 button 2", "P2_X"},

        {"joystick 1 button 3", "P1_Y"},
        {"joystick 2 button 3", "P2_Y"},

        {"joystick 1 button 4", "P1_LB"},
        {"joystick 2 button 4", "P2_LB"},

        {"joystick 1 button 5", "P1_RB"},
        {"joystick 2 button 5", "P2_RB"},

        {"joystick 1 button 6", "P1_Back"},
        {"joystick 2 button 6", "P2_Back"},

        {"joystick 1 button 7", "P1_Start"},
        {"joystick 2 button 7", "P2_Start"},

        {"joystick 1 button 8", "P1_LStick"},
        {"joystick 2 button 8", "P2_LStick"},

        {"joystick 1 button 9", "P1_RStick"},
        {"joystick 2 button 9", "P2_RStick"}
    };
    //Translate raw keybindings into more human-like text
    private static Dictionary<string, string> m_localizer = new Dictionary<string, string>()
    {
        {"P1_A", "A button"},
        {"P2_A", "A button"},

        {"P1_B", "B button"},
        {"P2_B", "B button"},

        {"P1_X", "X button"},
        {"P2_X", "X button"},

        {"P1_Y", "Y button"},
        {"P2_Y", "Y button"},

        {"P1_LB", "LB button"},
        {"P2_LB", "LB button"},

        {"P1_RB", "RB button"},
        {"P2_RB", "RB button"},

        {"P1_Back", "Back button"},
        {"P2_Back", "Back button"},

        {"P1_Start", "Start button"},
        {"P2_Start", "Start button"},

        {"P1_LStick", "L-Stick button"},
        {"P2_LStick", "L-Stick button"},

        {"P1_RStick", "R-Stick button"},
        {"P2_RStick", "R-Stick button"},

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
        {"T2_axis", "T axis"}
    };
    public List<TextMeshProUGUI> m_textList = new List<TextMeshProUGUI>();
    public TextMeshProUGUI m_playerTextRef;
    private void Update()
    {
        if(m_playerTextRef != null)
            m_playerTextRef.SetText("Keybinding for P: " + BuildSubjectLogic.m_currentPlayerId);
    }
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 0"];
                    FinishBinding(inputAxis, fullBinding);
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 1"];
                    FinishBinding(inputAxis, fullBinding);
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 2"];
                    FinishBinding(inputAxis, fullBinding);
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 3"];
                    FinishBinding(inputAxis, fullBinding);
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 4"];
                    FinishBinding(inputAxis, fullBinding);
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
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 5"];
                    FinishBinding(inputAxis, fullBinding);
                }
                done = true;
            }
            //No valid axis binding zone
            if (m_typeDictionary[axis] != "Axis")
            {
                if (Input.GetKeyDown("joystick " + pid + " button 6"))
                {
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 6"];
                    FinishBinding(inputAxis, fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 7"))
                {
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 7"];
                    FinishBinding(inputAxis, fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 8"))
                {
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 8"];
                    FinishBinding(inputAxis, fullBinding);
                    done = true;
                }
                if (Input.GetKeyDown("joystick " + pid + " button 9"))
                {
                    string inputAxis = m_buttonToAxis["joystick " + pid + " button 9"];
                    FinishBinding(inputAxis, fullBinding);
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
    public void ResetDefaultText()
    {
        //Resets all attached text meshes to default text
        foreach (TextMeshProUGUI textRef in m_textList)
        {
            textRef.SetText("<Default>");
        }
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

