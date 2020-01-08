using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody m_body;
    public Player m_player_ref;
    private float m_base_turn_rate = 85;
    private float m_acceleration_rate = 10000; // rate at which to increase acceleration to max 
    private float m_max_accel = 15000;  // max amount of acceleration object can recieve per second ( up to max velocity )
    private float m_max_velocity = 9600;  // max speed object can attain ( this will be number to cap out on when adding force )
    private float m_max_yaw_rate = 0.6f;  // max num of degrees that object can turn in a second
    private float m_max_roll_rate = 0.9f;
    private float m_max_pitch_rate = 1.5f;

    private float m_axis_yaw = 0;
    private float m_axis_pitch = 0;
    private float m_axis_roll = 0;
    private float m_axis_thrust = 0;
    private int m_joy_num = 1; //Change
    private float m_cur_accel = 0; //the current acceleration, gradually increments until equal to max acceleration
    private float m_cur_velocity = 0; // incremented gradually to max velocity, is the force that gets applied to object
    private float m_friction = 4500;
    private bool m_colliding = false;

    public void SetController(int num)
    {
        m_joy_num = num;
    }
    public int GetController()
    {
        return m_joy_num;
    }
    private void GetInput()
    {
        //Input grabber for the spaceship
        m_axis_yaw = Input.GetAxis("P" + m_joy_num + "_Yaw");
        m_axis_pitch = Input.GetAxis("P" + m_joy_num + "_Pitch");
        m_axis_roll = Input.GetAxis("P" + m_joy_num + "_Roll");
        m_axis_thrust = Input.GetAxis("P" + m_joy_num + "_Thrust");
    }

    private void MovementUpdate()
    {
        
        if (m_cur_accel < m_max_accel && m_axis_thrust > 0.001)
        {
            m_cur_accel += m_acceleration_rate * Time.deltaTime;
        }
        else if (m_cur_accel < m_max_accel && m_axis_thrust < -0.001)
        {
            m_cur_accel -= m_acceleration_rate * Time.deltaTime;
        }
        else
        {
            m_cur_accel = 0;
        }
        float new_vel = Mathf.Abs(m_cur_velocity + m_cur_accel * Time.deltaTime);  // used to check if adding the cur_acceleration would lower abs of velocity ie. braking force
        if (Mathf.Abs(m_cur_velocity) < m_max_velocity || new_vel < m_max_velocity)
        {
            m_cur_velocity += m_cur_accel * Time.deltaTime;
        }
        if (m_cur_accel == 0)
        {
            //Reduces the absolute value of the velocity down to zero if acceleration is 0
            int sign = m_cur_velocity > 0 ? 1 : -1;
            float new_val = Mathf.Abs(m_cur_velocity) - m_friction * Time.deltaTime;
            if (new_val < 0)
                new_val = 0;
            m_cur_velocity = new_val * sign;
        }
        m_body.AddForce(transform.forward * m_cur_velocity * Time.deltaTime);
        if (m_colliding)
        {
            //If object is currently colliding with something, set the velocity to 0 (to prevent discrepancies between stated and actual velocity)
            m_cur_velocity = 0;
        }
        Vector3 angles;
        angles.x = m_axis_pitch * m_max_pitch_rate * m_base_turn_rate * Time.deltaTime;
        angles.y = m_axis_yaw * m_max_yaw_rate * m_base_turn_rate * Time.deltaTime;
        angles.z = m_axis_roll * m_max_roll_rate * m_base_turn_rate * Time.deltaTime;
        transform.Rotate(angles);
        
    }
    void OnCollisionEnter(Collision other)
    {
        m_colliding = true;
    }
    void OnCollisionExit(Collision collision_info)
    {
        m_colliding = false;
    }
    public void FixedUpdate()
    {
        GetInput();
        MovementUpdate();
    }
}
