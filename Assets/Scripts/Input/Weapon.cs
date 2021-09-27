using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawnpoint;
    float delay = 0.01f;
    void Start()
    {
        
    }

    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
            delay = 0;
    }

    public void FireWeapon(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() == 1)
        {
            if (delay <= 0)
            {
                Vector3 pos = spawnpoint.transform.position;
                Vector3 forward = this.transform.forward;
                GameObject new_bullet = Instantiate(bullet);
                new_bullet.GetComponent<Bullet>().Fire(pos, forward);
                delay = 0.01f;
            }
        } 
    }
}
