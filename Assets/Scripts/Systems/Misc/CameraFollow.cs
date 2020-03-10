using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Canvas m_canvas;
    public GameObject m_focusPrefab;
    public GameObject m_pointerPrefab;
    public List<Transform> m_otherCores = new List<Transform>(); //used for the circle thing on enemies
    public Transform m_target;
    public Transform m_offset;
    public float m_smooth_time = 10.125f; // higher value, more smoothing 0...1
    private float m_angle_per_sec = 10f;
    private Vector3 m_velocity = Vector3.zero;
    private List<FocusIcon> m_focuserList = new List<FocusIcon>();
    private List<TargetIconLocator> m_pointerList = new List<TargetIconLocator>();

    void Start()
    {
        foreach (Transform core in m_otherCores)
        {
            //The focus obj is for the circle that is layered on the enemy, the pointer is the arrow that points towards the enemy when they are offscreen.
            GameObject obj = Instantiate(m_focusPrefab, m_canvas.transform);
            FocusIcon focuser = obj.GetComponent<FocusIcon>();
            focuser.Init(m_canvas, core.gameObject);
            m_focuserList.Add(focuser);

            GameObject obj2 = Instantiate(m_pointerPrefab, m_canvas.transform);
            TargetIconLocator pointer = obj2.GetComponent<TargetIconLocator>();
            pointer.Init(m_canvas, core.gameObject);
            m_pointerList.Add(pointer);
        }
    }
    void Update()
    {
        Vector3 old_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z); // gets a copy of old for velocity calc
        Vector3 offset_delta = m_offset.position - m_target.position;
        Vector3 desired_position = m_target.position + offset_delta;
        Vector3 smoothed_position = Vector3.SmoothDamp(transform.position, desired_position, ref m_velocity, m_smooth_time);
        Quaternion smoothed_rotation = Quaternion.Slerp(transform.rotation, m_target.rotation, m_angle_per_sec * Time.deltaTime);
        float angle = Quaternion.Angle(smoothed_rotation, transform.rotation);
        transform.position = smoothed_position;
        if (angle >= 0.00001f)
        {
            transform.rotation = smoothed_rotation;
        }
        
    }
}
