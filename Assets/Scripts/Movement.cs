using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody m_body;
    public Player m_player_ref;
    public float m_turning_speed = 0; //base turning speed
    public float m_speed = 0;
    public float m_thrust_timer = 6.5f; // How long should it take for thrust force to equal 100%
    public float m_yaw_speed = 0.4f; // speed mult for yaw
    public float m_pitch_speed = 0.7f; // sped mult for pitch
    public float m_roll_speed = 0.9f; // speed mult for roll
    private float m_axis_yaw = 0;
    private float m_axis_pitch = 0;
    private float m_axis_roll = 0;
    private float m_axis_thrust = 0;
    private int m_joy_num = 0;

    public void SetController(int num)
    {
        m_joy_num = num;
    }
    private void GetInput()
    {
        //Input grabber for the spaceship
        m_axis_yaw = Input.GetAxis("P" + m_joy_num + "_Yaw");
        m_axis_pitch = Input.GetAxis("P" + m_joy_num + "_Pitch");
        m_axis_roll = Input.GetAxis("P" + m_joy_num + "_Roll");
        m_axis_thrust = Input.GetAxis("P" + m_joy_num + "_Thrust");
       // Debug.Log(m_axis_pitch + ", " + m_axis_roll + ", " + m_axis_yaw + ", " + m_axis_thrust);
    }

    private void MovementUpdate()
    {
        //Updates the rotation and force of the spaceship (TODO add gradual acceleration to top speed)
        Vector3 angles;
        angles.x = m_axis_pitch * m_turning_speed * Time.deltaTime * m_pitch_speed;
        angles.y = m_axis_yaw * m_turning_speed * Time.deltaTime * m_yaw_speed;
        angles.z = m_axis_roll * m_turning_speed * Time.deltaTime * m_roll_speed;
        transform.Rotate(angles);
        m_body.AddForce(transform.forward * m_axis_thrust * m_speed * Time.deltaTime);
    }

    public bool DamagePlayer(float damage)
    {
        return m_player_ref.HitPlayer(damage);
    }
    public void FixedUpdate()
    {
        GetInput();
        MovementUpdate();
    }
}
