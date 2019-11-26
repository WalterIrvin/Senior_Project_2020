﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    //Wrapper script for basic Target logic, accepts damage input from projectiles when hit, and will destroy if health is <= 0
    public float m_health = 100;
    public void HitTarget(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            Destroy(this.gameObject, 0.5f);
        }
    }
}
