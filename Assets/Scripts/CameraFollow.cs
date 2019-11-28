using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform m_target;
    public Transform m_offset;
    public float m_smooth_time = 10.125f; // higher value, less smoothing 0...1
    private Vector3 m_velocity = Vector3.zero;

    void Update()
    {
        Vector3 offset_delta = m_offset.position - m_target.position;
        Vector3 desired_position = m_target.position + offset_delta;
        Vector3 smoothed_position = Vector3.SmoothDamp(transform.position, desired_position, ref m_velocity, m_smooth_time);
        Quaternion smoothed_rotation = Quaternion.Slerp(transform.rotation, m_target.rotation, m_smooth_time);
        transform.position = smoothed_position;
        transform.rotation = smoothed_rotation;
    }
}
