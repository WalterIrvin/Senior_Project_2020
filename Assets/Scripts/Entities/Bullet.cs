using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Assets.Scripts.Input;

public class Bullet : MonoBehaviour
{
    public PhotonView view;
    public float damage = 10f;
    public float speed = 1000f;
    private float timeout = 5f;
    private Player source;
    void Start()
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            Debug.Log("testing");
        }
        /*
        if (view.IsMine)
        {
            if (other != null)
            {
                if (other.gameObject != null)
                {
                    if (other.gameObject.tag == "Player")
                    {
                        if (other.gameObject != source.gameObject)
                        {
                            Debug.Log("Testing");
                            Player target = other.gameObject.GetComponent<Player>();
                            target.onBulletHit(source, damage);
                        }
                    }
                }
            }
            PhotonNetwork.Destroy(view);
        }
        */
    }

    void Update()
    {
        if (view.IsMine)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
                PhotonNetwork.Destroy(view);
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
;