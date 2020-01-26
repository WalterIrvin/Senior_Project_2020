using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Rigidbody m_body;
    public Player m_player_ref;
    public Image m_fillBar;

    //Turning accel vars
    private float m_cur_pitch_rate = 0;
    private float m_cur_roll_rate = 0;
    private float m_cur_yaw_rate = 0;
    private float m_max_turning_rate = 175; // max turning rate accel
    private float m_turning_accel = 57.69f; // amt to add per deltaTime
    private float m_angle_friction = 9.7f; // turning friction
    //Movement accel vars
    private float m_acceleration_rate = 7000; // rate at which to increase acceleration to max 
    private float m_max_vel = 15000;
    private float m_cur_vel = 0;

    private float m_axis_yaw = 0;
    private float m_axis_pitch = 0;
    private float m_axis_roll = 0;
    private float m_axis_thrust = 0;
    private int m_joy_num = 1; //Change
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
        m_axis_yaw = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Yaw"]);
        m_axis_pitch = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Pitch"]);
        m_axis_roll = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Roll"]);
        m_axis_thrust = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Thrust"]);
    }

    private void MovementUpdate()
    {
        SmoothedLinearMovement(ref m_cur_vel, m_axis_thrust);
        m_body.AddForce(transform.forward * m_cur_vel * Time.deltaTime);
        if (m_colliding)
        {
            //If object is currently colliding with something, set the velocity to 0 (to prevent discrepancies between stated and actual velocity)
            m_cur_vel = 0;
        }
        SmoothedAngleMovement(ref m_cur_pitch_rate, m_axis_pitch);
        SmoothedAngleMovement(ref m_cur_roll_rate, m_axis_roll);
        SmoothedAngleMovement(ref m_cur_yaw_rate, m_axis_yaw);

        Vector3 angles;
        angles.x = m_cur_pitch_rate * Time.deltaTime;
        angles.y = m_cur_yaw_rate * Time.deltaTime;
        angles.z = m_cur_roll_rate * Time.deltaTime;
        transform.Rotate(angles);
        
    }
    void SmoothedLinearMovement(ref float cur_vel, float viewed_axis)
    {
        if (viewed_axis > 0.3 && (cur_vel < m_max_vel || cur_vel < 0))
        {
            cur_vel += m_acceleration_rate * Time.deltaTime;
        }
        else if (viewed_axis < -0.3 && (Mathf.Abs(cur_vel) < m_max_vel || cur_vel > 0))
        {
            cur_vel -= m_acceleration_rate * Time.deltaTime;
        }
    }
    void SmoothedAngleMovement(ref float cur_angle_rate, float viewed_axis)
    {
        //takes reference to cur turn rate of whatever axis, applies smooth acceleration / friction if no input recieved.
        bool reverse_a = (viewed_axis > 0.3 && cur_angle_rate < 0);
        bool reverse_b = (viewed_axis < -0.3 && cur_angle_rate > 0);

        if (viewed_axis > 0.3 && (cur_angle_rate < m_max_turning_rate || cur_angle_rate < 0))
        {
            cur_angle_rate += m_turning_accel * Time.deltaTime;
        }
        else if (viewed_axis < -0.3 && (Mathf.Abs(cur_angle_rate) < m_max_turning_rate || cur_angle_rate > 0))
        {
            cur_angle_rate -= m_turning_accel * Time.deltaTime;
        }
        if ((Mathf.Abs(viewed_axis) <= 0.3 || reverse_a || reverse_b) && Mathf.Abs(cur_angle_rate) > 5)
        {
            float angleVal = Mathf.Abs(cur_angle_rate);
            angleVal -= m_angle_friction;
            if (cur_angle_rate > 0)
            {
                cur_angle_rate = angleVal;
            }
            else
            {
                cur_angle_rate = -angleVal;
            }
            if (Mathf.Abs(cur_angle_rate) <= 50)
            {
                cur_angle_rate = 0;
            }
        }
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
        if(m_cur_vel < 0)
        {
            m_fillBar.color = new Color(1, 0, 0);
        }
        else
        {
            m_fillBar.color = new Color(1, 0.69f, 0);
        }
        m_fillBar.fillAmount = Mathf.Abs(m_cur_vel / m_max_vel);
    }
}
