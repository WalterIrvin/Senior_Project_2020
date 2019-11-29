using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Player m_player_ref;
    public GameObject m_target_ref;
    public bool m_kill = false;
    public float m_angle_per_sec = 10f;
    private float m_speed;
    private float m_range;
    private float m_damage;
    private float m_cur_range = 0;
    private Vector3 m_oldPos;
    private Vector3 m_direction;
    private void Start()
    {
        m_oldPos = transform.position;
    }

    public void InitAll(Quaternion direction, float speed, float max_range, float damage, Player player_ref, GameObject target_ref=null)
    {
        transform.rotation = direction;
        m_speed = speed;
        m_range = max_range;
        m_damage = damage;
        m_player_ref = player_ref;
        m_target_ref = target_ref;
        Debug.Log(transform.position);
    }
    public void InitAllDumb(Vector3 direction, float speed, float max_range, float damage, Player player_ref)
    {
        m_direction = direction;
        m_speed = speed;
        m_range = max_range;
        m_damage = damage;
        m_player_ref = player_ref;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement core = other.GetComponent<Movement>();
            bool killed_player = core.DamagePlayer(m_damage);
            if (killed_player)
            {
                m_player_ref.AddKill();
            }
            m_kill = true;
        }
        else if (other.gameObject.tag == "Target")
        {
            Target ts = other.GetComponent<Target>();
            ts.HitTarget(m_damage);
            m_kill = true;
        }
        m_kill = true;
    }
    void Update()
    {
        if (m_target_ref != null)
        {
            Vector3 forward =  transform.forward * m_speed * Time.deltaTime * 100; //(m_target_ref.transform.position - transform.position)
            m_cur_range += forward.magnitude;
            if (m_cur_range >= m_range || m_target_ref.activeSelf == false)
            {
                // If range is greater then max range, or target is not active, then kill rocket
                m_kill = true;
            }
            transform.position += forward;
            Vector3 delta_vec = m_target_ref.transform.position - transform.position;
            Quaternion ideal_direction = Quaternion.LookRotation(delta_vec, Vector3.up);
            Quaternion smoothed_rotation = Quaternion.Slerp(transform.rotation, ideal_direction, m_angle_per_sec * Time.deltaTime);
            transform.rotation = smoothed_rotation;
        }
        else
        {
            Vector3 delta_vec = m_direction + (transform.position - m_oldPos) * m_speed * Time.deltaTime;
            m_cur_range += delta_vec.magnitude;
            transform.position += delta_vec;
            if (m_cur_range >= m_range)
            {
                m_kill = true;
            }
        }
        
    }
}
