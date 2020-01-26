using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSubjectRotator : MonoBehaviour
{
    public bool m_isSelected;
    
    private float m_axis_yaw = 0;
    private float m_axis_pitch = 0;
    private float m_axis_roll = 0;
    private float m_base_turn_rate = 100;
    private void GetInput()
    {
        int pitchInv = 1;
        int yawInv = 1;
        int rollInv = 1;
        int thrustInv = 1;
        if (BuildSubjectLogic.m_currentPlayerId == 1)
        {
            pitchInv = Keybinder.m_P1_PitchInv;
            yawInv = Keybinder.m_P1_YawInv;
            rollInv = Keybinder.m_P1_RollInv;
            thrustInv = Keybinder.m_P1_ThrustInv;
        }
        else if (BuildSubjectLogic.m_currentPlayerId == 2)
        {
            pitchInv = Keybinder.m_P2_PitchInv;
            yawInv = Keybinder.m_P2_YawInv;
            rollInv = Keybinder.m_P2_RollInv;
            thrustInv = Keybinder.m_P2_ThrustInv;
        }
        //Input grabber for the spaceship
        m_axis_yaw = Input.GetAxis(Keybinder.m_axisDictionary["P" + BuildSubjectLogic.m_currentPlayerId + "_Yaw"]) * yawInv;
        m_axis_pitch = Input.GetAxis(Keybinder.m_axisDictionary["P" + BuildSubjectLogic.m_currentPlayerId + "_Pitch"]) * pitchInv;
        m_axis_roll = Input.GetAxis(Keybinder.m_axisDictionary["P" + BuildSubjectLogic.m_currentPlayerId + "_Roll"]) * rollInv;
    }
    private void RotationUpdate()
    {
        Vector3 angles;
        angles.x = m_axis_pitch * m_base_turn_rate * Time.deltaTime;
        angles.y = m_axis_yaw * m_base_turn_rate * Time.deltaTime;
        angles.z = m_axis_roll * m_base_turn_rate * Time.deltaTime;
        transform.Rotate(angles);
    }
    void Update()
    {
        GetInput();
        RotationUpdate();
    }
}
