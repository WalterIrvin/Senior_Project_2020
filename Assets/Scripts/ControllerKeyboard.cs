using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerKeyboard : MonoBehaviour
{
    /// <summary>
    /// Simplistic UI keyboard, takes each key and puts them into the associated TextMeshProUGUI object, with ENTER and DELETE being special cases which
    /// close the keyboard and delete one key respectively.
    /// </summary>
    public TextMeshProUGUI m_textField;
    public InputFieldKeyboard m_inputFieldScript;
    public void AddKey(string key)
    {
        if(key == "DELETE")
        {
            if(m_textField.text.Length - 1 >= 0)
                m_textField.text = m_textField.text.Substring(0, m_textField.text.Length - 1);
        }
        else if (key == "ENTER")
        {
            m_inputFieldScript.CloseKeyboard();
        }
        else if (key != "DELETE" && key != "ENTER")
        {
            if (m_textField.text.Length <= 16)
                m_textField.text += key;
        }
    }
}
