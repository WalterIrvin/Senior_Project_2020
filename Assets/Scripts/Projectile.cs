using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_lifeSpan = 10f;
    public float m_damage = 15f;
    public float m_speed = 100f;
    public float m_angle_per_sec = 85f;
    private WeaponType m_weaponType;
    private GameObject m_coreRef;
    private GameObject m_parentRef;
    private GameObject m_targetRef;
    private Player m_playerRef;
    private Vector3 m_convergePoint;
    private Vector3 m_convergeForward;
    private bool m_willDieAfterSound = false; //Used to prevent annoying sound cutting on bullets
    public void Init(Fire fireScript, GameObject target=null)
    {
        ///Determines the type of the weapon based on what type the gun was, and also sets the gun to "parent" without being bound to its position/rotation
        m_weaponType = fireScript.m_weaponType;
        m_parentRef = fireScript.gameObject;
        m_coreRef = m_parentRef.transform.parent.gameObject; // Gets the core object, the reference is needed to know to avoid collision detection with self
        m_targetRef = fireScript.GetPlayerScript().m_targetReticule.GetTargetRef();
        m_convergePoint = fireScript.GetPlayerScript().m_targetReticule.GetConvergePoint();
        m_convergeForward = (m_convergePoint - transform.position).normalized;
        m_playerRef = m_coreRef.transform.parent.gameObject.GetComponent<Player>(); //Gets the parent obj of the core, and gets a reference to its Player class
    }
    private void SimpleMovement()
    {
        ///Used for projectiles which fire at a designated point and keep going in a straight line, examples include plasma and rail weapon types
        ///Makes projectile move in straight line towards target location forming a X shape if lifespan is long enough and they converge on the point
        transform.position += m_convergeForward * m_speed * Time.deltaTime;
    }

    private void TrackingMovement()
    {
        ///Used for projectiles which need to home in on moving targets, examples include missiles
        ///Projectile moves according to changing forward vector which smoothly pans to the target object at a constant turning rate
        ///warning, can end up orbiting object if collison box is not high enough or turning radius is too large.
        if (m_targetRef != null)
        {
            if(m_targetRef.activeSelf)
            {
                Vector3 forward = transform.forward * m_speed * Time.deltaTime;
                transform.position += forward;
                Vector3 delta_vec = m_targetRef.transform.position - transform.position;
                Quaternion ideal_direction = Quaternion.LookRotation(delta_vec, Vector3.up);
                Quaternion smoothed_rotation = Quaternion.Slerp(transform.rotation, ideal_direction, m_angle_per_sec * Time.deltaTime);
                transform.rotation = smoothed_rotation;
            }
            else
            {
                //If target is inactive, destroy missile
                DelegateDeath();
            }
        }
        else
        {
            //If no valid target found, make missiles dumb fire
            SimpleMovement();
        }
    }
    private void DelegateDeath()
    {
        //Handles all death scenarios, makes sure sound finishes playing before destroying object
        AudioSource soundPlayer = this.gameObject.GetComponent<AudioSource>();
        if(soundPlayer.isPlaying)
        {
            m_willDieAfterSound = true;
            MeshRenderer mesh = this.gameObject.GetComponent<MeshRenderer>();
            mesh.enabled = false;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != m_coreRef)
        {
            //If not colliding with the core ship
            if (other.gameObject.tag == "Player")
            {
                Player otherPlayer = other.transform.parent.gameObject.GetComponent<Player>();
                bool killResult = otherPlayer.DamagePlayer(m_damage);
                if (killResult)
                {
                    m_playerRef.AddKill();
                }
                DelegateDeath();
            }
        }
        if (other.gameObject.tag == "Neutral")
        {
            DelegateDeath();
        }
    }
    private void Update()
    {
        m_lifeSpan -= Time.deltaTime;
        if (m_lifeSpan <= 0)
        {
            this.gameObject.SetActive(false);
        }
        if (!m_willDieAfterSound)
        {
            switch (m_weaponType)
            {
                case WeaponType.PLASMA:
                    SimpleMovement();
                    break;
                case WeaponType.RAIL:
                    SimpleMovement();
                    break;
                case WeaponType.ROCKET:
                    TrackingMovement();
                    break;
                default:
                    Debug.LogWarning("Not a valid weaponType in Projectile.cs, weaponType: " + m_weaponType);
                    break;
            }
        }
    }
}
