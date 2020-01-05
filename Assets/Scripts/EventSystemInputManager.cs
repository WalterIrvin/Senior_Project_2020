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
    void Start()
    {
        UpdateInputManager();
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
}
