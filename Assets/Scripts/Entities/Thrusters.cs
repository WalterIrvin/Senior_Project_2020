using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    public List<GameObject> thrusters;
    private float outer_thrust_lifetime = 0.6f;
    private float inner_thrust_lifetime = 0.4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Update_Thrusters(float pct)
    {
        for (int i = 0; i < thrusters.Count; i++)
        {
            ParticleSystem thruster = thrusters[i].GetComponent<ParticleSystem>();
            ParticleSystem.MainModule m = thruster.main;
            m.startLifetime = outer_thrust_lifetime * pct;

            ParticleSystem inner_thruster = thrusters[i].transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule n = inner_thruster.main;
            n.startLifetime = inner_thrust_lifetime * pct;
        }
    }
}
