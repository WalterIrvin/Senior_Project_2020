using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Wrapper script for basic Player logic, accepts damage input from projectiles when hit, and will destroy if health is <= 0
    //May need special logic in future to disable controller associated with it unlike just the TargetScript.
    public float m_health = 100;
    public int m_joy_num = 0; // Sets the controller for this player
    public List<FireScript> m_gun_list; // gun list controlled by the player
    private void Start()
    {
        foreach (FireScript gun in m_gun_list)
        {
            gun.SetJoystick(m_joy_num);
        }
        MovementScript mover = this.gameObject.GetComponent<MovementScript>();
        mover.SetController(m_joy_num);
    }
    public void HitPlayer(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            Destroy(this.gameObject, 0.5f);
        }
    }
}
