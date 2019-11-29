using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player m_player_ref;
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

    public void InitAll(Vector3 direction, float speed, float max_range, float damage, Player player_ref)
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
        else if(other.gameObject.tag == "Target")
        {
            Target ts = other.GetComponent<Target>();
            ts.HitTarget(m_damage);
            m_kill = true;
        }
        m_kill = true;
        
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
