using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 1000f;
    private float timeout = 5f;
    void Start()
    {
        
    }

    void Update()
    {
        timeout -= Time.deltaTime;
        if (timeout <= 0)
            Destroy(this.gameObject);
        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * speed;
    }

    public void Fire(Vector3 position, Vector3 direction)
    {
        this.transform.position = position;
        this.transform.forward = direction;
    }
}
