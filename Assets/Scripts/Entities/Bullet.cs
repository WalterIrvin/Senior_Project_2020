using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Assets.Scripts.Input;

public class Bullet : MonoBehaviour
{
    private PhotonView view;
    public float damage = 10f;
    public float speed = 1000f;
    private float timeout = 5f;
    private Player source;
    void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player target = other.gameObject.GetComponent<Player>();
            target.onBulletHit(source, damage);
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
                Destroy(this.gameObject);
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * speed;
        }
    }

    public void Fire(Player parent, Vector3 position, Vector3 direction)
    {
        this.source = parent;
        this.transform.position = position;
        this.transform.forward = direction;
    }
}
