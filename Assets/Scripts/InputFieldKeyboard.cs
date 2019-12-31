﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InputFieldKeyboard : MonoBehaviour
{
    /// <summary>
    /// Class dedicated to passing name of ship onto the build subject when build is complete.
    /// </summary>
    public GameObject m_doneArrow;
    public GameObject m_keyboard;
    public GameObject m_firstKey;
    public void OpenKeyboard()
    {
        if (EventSystem.current.alreadySelecting == false)
        {
            EventSystem.current.SetSelectedGameObject(null);
            m_keyboard.SetActive(true);
            EventSystem.current.SetSelectedGameObject(m_firstKey);
        }
    }
    public void CloseKeyboard()
    {
        if (EventSystem.current.alreadySelecting == false)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_doneArrow);
            m_keyboard.SetActive(false);
        }
        
    }
}
