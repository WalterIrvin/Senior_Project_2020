using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Rigidbody m_body;
    public Player m_player_ref;
    public Image m_fillBar;
    public AudioSource m_thrustSound; // plays when thrusting
    public List<ParticleSystem> m_thrusterPlumes;
    public ParticleSystem m_hyperspace;
    public ParticleSystem m_hyperspace_b;
    private ParticleSystem.MainModule m_hype_b;
    private ParticleSystem.MainModule m_hype;
    private ParticleSystem.MainModule m_tlplume;  //This is beyond annoying, but the way start speed works means these variables must be class level
    private ParticleSystem.MainModule m_trplume;  //They don't take effect from the looks of it when unless they are pre-declared
    private ParticleSystem.MainModule m_blplume;
    private ParticleSystem.MainModule m_brplume;
    private ParticleSystem.MainModule m_midplume;
    private float m_max_plume_len = 40; // Set this to however long the max size of plume should be
    //Turning accel vars
    private float m_cur_pitch_rate = 0;
    private float m_cur_roll_rate = 0;
    private float m_cur_yaw_rate = 0;
    private float m_max_turning_rate = 305; // max turning rate accel
    private float m_turning_accel = 297.69f; // amt to add per deltaTime
    private float m_angle_friction = 19.7f; // turning friction
    //Movement accel vars
    private float m_acceleration_rate = 18000; // rate at which to increase acceleration to max 
    private float m_max_vel = 30000;
    private float m_cur_vel = 0;

    private float m_axis_yaw = 0;
    private float m_axis_pitch = 0;
    private float m_axis_roll = 0;
    private float m_axis_thrust = 0;
    private int m_joy_num = 1; //Change
    private bool m_colliding = false;
    private bool m_tooslow = true; // ship going too slow for hyperspace
    private bool m_tooslow_b = true; // ship not reversing fast enough for back-space

    private void Start()
    {
        m_tlplume = m_thrusterPlumes[0].main;
        m_trplume = m_thrusterPlumes[1].main;
        m_blplume = m_thrusterPlumes[2].main;
        m_brplume = m_thrusterPlumes[3].main;
        m_midplume = m_thrusterPlumes[4].main;
        m_hype = m_hyperspace.main;
        m_hype_b = m_hyperspace_b.main;
    }
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
        int pitchInv = 1;
        int yawInv = 1;
        int rollInv = 1;
        int thrustInv = 1;
        if (m_joy_num == 1)
        {
            pitchInv = Keybinder.m_P1_PitchInv;
            yawInv = Keybinder.m_P1_YawInv;
            rollInv = Keybinder.m_P1_RollInv;
            thrustInv = Keybinder.m_P1_ThrustInv;
        }
        else if (m_joy_num == 2)
        {
            pitchInv = Keybinder.m_P2_PitchInv;
            yawInv = Keybinder.m_P2_YawInv;
            rollInv = Keybinder.m_P2_RollInv;
            thrustInv = Keybinder.m_P2_ThrustInv;
        }
        //Input grabber for the spaceship
        m_axis_yaw = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Yaw"]) * yawInv;
        m_axis_pitch = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Pitch"]) * pitchInv;
        m_axis_roll = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Roll"]) * rollInv;
        m_axis_thrust = Input.GetAxis(Keybinder.m_axisDictionary["P" + m_joy_num + "_Thrust"]) * thrustInv;
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
            if (Mathf.Abs(cur_angle_rate) <= 70)
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
    private void UpdatePlumes()
    {
        float percentage = m_max_plume_len / m_max_vel; // scales the vel down so that it fits in the 0-8 range
        float plume_len = m_cur_vel * percentage;
        if (plume_len <= 0)
        {
            plume_len = 0;
            m_thrustSound.Stop();
        }
            
        m_tlplume.startSpeed = plume_len;
        m_trplume.startSpeed = plume_len;
        m_blplume.startSpeed = plume_len;
        m_brplume.startSpeed = plume_len;
        m_midplume.startSpeed = plume_len;
        if (plume_len > 0)
        {
            if (!m_thrustSound.isPlaying)
            {
                m_thrustSound.Play();
            }
        }
        m_thrustSound.volume = (plume_len / m_max_plume_len) / 4;
    }
    private void UpdateHyperspace()
    {
        // Gets the magnitude of the distance travelled from old position to cur position
        // The 180 is because the max tested speed is around 5, and 180 * 5 = 900, which is about what we want the particle system to go at
        float percent = 9000 / m_max_vel; // scales the vel down so that it fits in the 0-900 range
        float cur_velocity = m_cur_vel * percent;
        float min_speed = 5000; // min speed to show particles
        if (cur_velocity < 0)
        {
            //If ship is going backwards, play the reverse particle system
            cur_velocity = Mathf.Abs(cur_velocity);
            m_hype.startSpeed = cur_velocity;
            Debug.Log(cur_velocity);
            if (m_hype.startSpeed.constant < min_speed)
            {
                //Clears the particle system if speed is too low
                m_hyperspace_b.Clear();
                m_hyperspace_b.Stop();
                m_tooslow_b = true;
            }
            else if (m_hype.startSpeed.constant > min_speed)
            {
                if (m_tooslow_b)
                {
                    // Start back up animation when ship speeds up
                    m_hyperspace_b.Play();
                    m_tooslow_b = false;
                }
            }
        }
        else
        {
            m_hype.startSpeed = cur_velocity;
            if (m_hype.startSpeed.constant < min_speed)
            {
                //Clears the particle system if speed is too low
                m_hyperspace.Clear();
                m_hyperspace.Stop();
                m_tooslow = true;
            }
            else if (m_hype.startSpeed.constant > min_speed)
            {
                if (m_tooslow)
                {
                    // Start back up animation when ship speeds up
                    m_hyperspace.Play();
                    m_tooslow = false;
                }
            }
        }
       
    }
    public void FixedUpdate()
    {
        GetInput();
        MovementUpdate();
        UpdatePlumes();
        UpdateHyperspace();
        if (m_cur_vel < 0)
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
