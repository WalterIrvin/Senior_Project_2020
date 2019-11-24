using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public GameObject m_bullet;
    public float m_fire_delay;
    public float m_damage;
    public float m_speed;
    public float m_max_range;
    public int m_max_projectiles;
    public bool m_rocket = false; // set to true if the projectile should follow missle logic
    private float m_cur_fire_delay;
    private List<GameObject> m_bulletList = new List<GameObject>();
    //future plans for missles
    private void Start()
    {
        m_cur_fire_delay = 0;
    }

    private void GetInput()
    {
        float fire_axis = Input.GetAxis("Fire");
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
                GameObject obj = Instantiate(m_bullet, transform);  // spawns a bullet and makes it child of the firescript gameobject
                obj.transform.position = transform.position;
                BulletScript bullet = obj.GetComponent<BulletScript>();
                Vector3 forward = transform.forward.normalized;
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
            BulletScript bullet = obj.GetComponent<BulletScript>();
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
