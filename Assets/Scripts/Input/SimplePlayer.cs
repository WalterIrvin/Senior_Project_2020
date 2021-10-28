using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Camera cam_ref;
    private PhotonView view;
    private float tmp = 0; 
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            cam_ref.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        else
        {
            UpdateMove();
            UpdateWeapon();
        }
    }
    private void UpdateMove()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        Vector3 forward = transform.forward;
        float delta_forward = tmp * 10;
        Vector3 acc_vec = (forward * delta_forward);
        rb.AddForce(acc_vec, ForceMode.Acceleration);
    }
    private void UpdateWeapon()
    {

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (view.IsMine)
        {
            Vector2 ctx = context.ReadValue<Vector2>();
            tmp = ctx.y;
        }
    }
}
