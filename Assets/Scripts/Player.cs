using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Wrapper script for basic Player logic, accepts damage input from projectiles when hit, and will destroy if health is <= 0
    //May need special logic in future to disable controller associated with it unlike just the TargetScript.
    public float m_health = 100;
    public int m_joy_num = 0; // Sets the controller for this player
    public List<Fire> m_gun_list; // gun list controlled by the player
    private void Start()
    {
        foreach (Fire gun in m_gun_list)
        {
            gun.SetJoystick(m_joy_num);
        }
        Movement mover = this.gameObject.GetComponent<Movement>();
        mover.SetController(m_joy_num);
    }
    public void HitPlayer(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
