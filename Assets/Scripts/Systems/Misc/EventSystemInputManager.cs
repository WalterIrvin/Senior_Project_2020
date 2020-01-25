using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInputManager : MonoBehaviour
{
    /// <summary>
    /// Input manager class for Unity StandaloneInputModule
    /// depending on the current player selected (defined in BuildSubjectLogic),
    /// will update the input axis to allow only that gamepad to control the ui / build commands.
    /// </summary>
    public StandaloneInputModule m_inputRef;
    private bool m_paused = false;
    void Start()
    {
        UpdateInputManager();

    }
    public void TogglePause()
    {
        //Toggles the pause-state of the inputmanager
        m_paused = !m_paused;
        if (m_paused)
            PauseInputManager();
        else
            ResumeInputManager();
    }
    public void UpdateInputManager()
    {
        //Updates the input axis based on the current selected player to build.
        //Resets the currently selected object, if not already selecting something blocking
        m_inputRef.submitButton = "P" + BuildSubjectLogic.m_currentPlayerId + "_Submit";
        m_inputRef.cancelButton = "P" + BuildSubjectLogic.m_currentPlayerId + "_Cancel";
        m_inputRef.horizontalAxis = "P" + BuildSubjectLogic.m_currentPlayerId + "_Horizontal";
        m_inputRef.verticalAxis = "P" + BuildSubjectLogic.m_currentPlayerId + "_Vertical";
        if(EventSystem.current.alreadySelecting == false)
        {
            GameObject first = EventSystem.current.firstSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(first);
        }
    }
    public void ResumeInputManager()
    {
        //Returns InputManager to last state, doesn't reset the currently selected item
        m_inputRef.submitButton = "P" + BuildSubjectLogic.m_currentPlayerId + "_Submit";
        m_inputRef.cancelButton = "P" + BuildSubjectLogic.m_currentPlayerId + "_Cancel";
        m_inputRef.horizontalAxis = "P" + BuildSubjectLogic.m_currentPlayerId + "_Horizontal";
        m_inputRef.verticalAxis = "P" + BuildSubjectLogic.m_currentPlayerId + "_Vertical";
    }
    public void PauseInputManager()
    {
        //Effectively disables all input when setting keybindings, this prevents moving off button
        m_inputRef.submitButton = "Null";
        m_inputRef.cancelButton = "Null";
        m_inputRef.horizontalAxis = "Null";
        m_inputRef.verticalAxis = "Null";
    }
    public static void ToggleInput()
    {
        EventSystem.current.gameObject.GetComponent<EventSystemInputManager>().TogglePause();
    }
}
