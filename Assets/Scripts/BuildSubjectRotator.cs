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
        m_axis_yaw = Input.GetAxis("P" + BuildSubjectLogic.m_currentPlayerId + "_Yaw");
        m_axis_pitch = Input.GetAxis("P" + BuildSubjectLogic.m_currentPlayerId + "_Pitch");
        m_axis_roll = Input.GetAxis("P" + BuildSubjectLogic.m_currentPlayerId + "_Roll");
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
