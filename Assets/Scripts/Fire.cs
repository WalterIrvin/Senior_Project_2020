using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Player m_player_ref;
    public GameObject m_projectile;
    public Transform m_aimpoint;
    public float m_fire_delay;
    public float m_damage;
    public float m_speed;
    public float m_max_range;
    public int m_max_projectiles;
    public bool m_rocket = false; // set to true if the projectile should follow missle logic
    private float m_cur_fire_delay = 0;
    private List<GameObject> m_projectile_list = new List<GameObject>();
    private int m_joy_num = 0;
    public void SetJoystick(int num)
    {
        m_joy_num = num;
    }

    private void GetInput()
    {
        float fire_axis = Input.GetAxis("P" + m_joy_num +"_Fire");
        if (fire_axis > 0 && m_cur_fire_delay == 0 && !m_rocket)
        {
            m_cur_fire_delay = m_fire_delay;
            FireBullet();
        }
        else if (fire_axis < 0 && m_cur_fire_delay == 0 && m_rocket)
        {
            m_cur_fire_delay = m_fire_delay;
            FireRocket();
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
    private void FireRocket()
    {
        if(m_projectile_list.Count <= m_max_projectiles)
        {
            //Gets the target for the missle to follow
            GameObject target = m_aimpoint.GetComponent<AimpointController>().UpdateAimpoint();
            if (target == null)
            {
                //If the hitscan did not detect a valid target, then do dumb firing
                GameObject obj = Instantiate(m_projectile);  // spawns a bullet and makes it child of the firescript gameobject
                obj.transform.rotation = transform.rotation;
                Rocket rocket = obj.GetComponent<Rocket>();
                Vector3 delta_vec = (m_aimpoint.position - transform.position).normalized;
                obj.transform.position = transform.position + delta_vec * 2;
                rocket.InitAllDumb(delta_vec,
                                m_speed,
                                m_max_range,
                                m_damage,
                                m_player_ref);
                m_projectile_list.Add(obj);
            }
            else
            {
                GameObject obj = Instantiate(m_projectile);  // spawns a bullet and makes it child of the firescript gameobject
                obj.transform.rotation = transform.rotation;
                Rocket rocket = obj.GetComponent<Rocket>();
                Vector3 delta_vec = (target.transform.position - transform.position).normalized;
                obj.transform.position = transform.position + delta_vec * 2;
                rocket.InitAll(transform.rotation,
                                m_speed,
                                m_max_range,
                                m_damage,
                                m_player_ref,
                                target);
                m_projectile_list.Add(obj);
            }
            
        }
    }
    private void FireBullet()
    {
        // Bullet spawning
        if (m_projectile_list.Count <= m_max_projectiles)
        {
            GameObject obj = Instantiate(m_projectile);  // spawns a bullet and makes it child of the firescript gameobject
            obj.transform.rotation = transform.rotation;
            Bullet bullet = obj.GetComponent<Bullet>();
            //Vector3 forward = transform.forward.normalized;
            m_aimpoint.GetComponent<AimpointController>().UpdateAimpoint(); //Updates the z-axis of the aimpoint to be at the distance of an intersecting object
            Vector3 forward = (m_aimpoint.position - transform.position).normalized;
            obj.transform.position = transform.position + forward * 2;
            bullet.InitAll(forward,
                            m_speed,
                            m_max_range,
                            m_damage,
                            m_player_ref);
            m_projectile_list.Add(obj); 
        }
    }
    private void CleanBulletList()
    {
        List<GameObject> dead_list = new List<GameObject>();
        foreach (GameObject obj in m_projectile_list)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet.m_kill)
            {
                dead_list.Add(obj);
            }
        }
        foreach (GameObject obj in dead_list)
        {
            m_projectile_list.Remove(obj);
            Destroy(obj);
        }
    }
    private void CleanRocketList()
    {
        List<GameObject> dead_list = new List<GameObject>();
        foreach (GameObject obj in m_projectile_list)
        {
            Rocket rocket = obj.GetComponent<Rocket>();
            if (rocket.m_kill)
            {
                dead_list.Add(obj);
            }
        }
        foreach (GameObject obj in dead_list)
        {
            m_projectile_list.Remove(obj);
            Destroy(obj);
        }
    }
    private void Update()
    {
        GetInput();
        if (!m_rocket)
        {
            CleanBulletList();
        }
        else
        {
            CleanRocketList();
        }
        
    }
}
