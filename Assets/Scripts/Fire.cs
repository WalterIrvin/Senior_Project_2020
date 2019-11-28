using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_aimpoint;
    public float m_fire_delay;
    public float m_damage;
    public float m_speed;
    public float m_max_range;
    public int m_max_projectiles;
    public bool m_rocket = false; // set to true if the projectile should follow missle logic
    private float m_cur_fire_delay = 0;
    private List<GameObject> m_bulletList = new List<GameObject>();
    private int m_joy_num = 0;

    public void SetJoystick(int num)
    {
        m_joy_num = num;
    }

    private void GetInput()
    {
        float fire_axis = Input.GetAxis("P" + m_joy_num +"_Fire");
        if (fire_axis > 0 && m_cur_fire_delay == 0)
        {
            m_cur_fire_delay = m_fire_delay;
            FireBullet();
        }
        if (m_cur_fire_delay > 0.001)
        {
            m_cur_fire_delay -= Time.deltaTime;
        }
        else
        {
            m_cur_fire_delay = 0;
        }
    }
    private void FireBullet()
    {
        if (m_rocket)
        {
            // Future plans for missle logic
        }
        else
        {
            // Bullet spawning
            if (m_bulletList.Count <= m_max_projectiles)
            {
                GameObject obj = Instantiate(m_bullet);  // spawns a bullet and makes it child of the firescript gameobject
                obj.transform.rotation = transform.rotation;
                Bullet bullet = obj.GetComponent<Bullet>();
                //Vector3 forward = transform.forward.normalized;
                m_aimpoint.GetComponent<AimpointController>().UpdateAimpoint(); //Updates the z-axis of the aimpoint to be at the distance of an intersecting object
                Vector3 forward = (m_aimpoint.position - transform.position).normalized;
                obj.transform.position = transform.position + forward * 2;
                bullet.InitAll(forward, m_speed, m_max_range, m_damage);
                m_bulletList.Add(obj); 
            }
        }
    }
    private void CleanList()
    {
        List<GameObject> dead_list = new List<GameObject>();
        foreach (GameObject obj in m_bulletList)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet.m_kill)
            {
                dead_list.Add(obj);
            }
        }
        foreach (GameObject obj in dead_list)
        {
            m_bulletList.Remove(obj);
            Destroy(obj);
        }
    }

    private void Update()
    {
        GetInput();
        CleanList();
    }
}
