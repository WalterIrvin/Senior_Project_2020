using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float m_damage;
    private Vector3 m_direction;
    private float m_speed;
    private float m_range;
    private Vector3 m_oldPos;
    public bool m_kill = false;

    private void Start()
    {
        m_oldPos = transform.position;
    }

    public void InitAll(Vector3 direction, float speed, float max_range, float damage)
    {
        m_direction = direction;
        m_speed = speed;
        m_range = max_range;
        m_damage = damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerScript ps = other.GetComponent<PlayerScript>();
            ps.HitPlayer(m_damage);
            m_kill = true;
        }
        else if(other.gameObject.tag == "Target")
        {
            TargetScript ts = other.GetComponent<TargetScript>();
            ts.HitTarget(m_damage);
            m_kill = true;
        }
    }
    void Update()
    {
        transform.position += m_direction * m_speed * Time.deltaTime;
        Vector3 delta_vec = transform.position - m_oldPos;
        if(delta_vec.magnitude >= m_range)
        {
            m_kill = true;
        }
    }
}
