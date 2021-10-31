using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    public PhotonView view;
    public Camera cam_ref;
    public GameObject mesh;
    Vector3 velocity = Vector3.zero;
    Vector2 move_dir = Vector2.zero;
    Vector2 look_dir = Vector2.zero;

    Vector3 turn_dampener = Vector3.zero;
    Vector3 turn_vel = Vector3.zero;
    float look_roll = 0;

    float base_turn_spd = 400f;
    float base_move_spd = 1500f;
    void Start()
    {
        if (!view.IsMine)
        {
            cam_ref.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        UpdateWeapon();
    }
    private void UpdateMove()
    {
        float delta = Time.deltaTime;
        Rigidbody rb = this.transform.GetComponent<Rigidbody>();
        //Turning
        float pitch_amt = -look_dir.y;
        float yaw_amt = look_dir.x;
        float roll_amt = -look_roll;
        Vector3 pitch = pitch_amt * base_turn_spd * transform.right * delta;
        Vector3 yaw = yaw_amt * base_turn_spd * transform.up * delta;
        Vector3 roll = roll_amt * base_turn_spd * transform.forward * delta;
        rb.AddTorque(pitch, ForceMode.Acceleration);
        rb.AddTorque(yaw, ForceMode.Acceleration);
        rb.AddTorque(roll, ForceMode.Acceleration);

        //Linear
        float forward_amt = move_dir.y * base_move_spd;
        float right_amt = move_dir.x * base_move_spd;
        Vector3 acc_vec = ((transform.forward * forward_amt) + (transform.right * right_amt)) * delta;
        rb.AddForce(acc_vec, ForceMode.Acceleration);
        MeshMove(acc_vec * 15);
        Vector3 turn = new Vector3(pitch_amt, yaw_amt, roll_amt);
        if (turn_dampener == null)
        {
            turn_dampener = turn;
        }
        else
        {
            turn_dampener = Vector3.SmoothDamp(turn_dampener, turn, ref turn_vel, 0.8f);
        }

        MeshTurn(turn_dampener);
    }
    private void MeshMove(Vector3 acc_vec)
    {
        // pushes player mesh towards direction of acceleration for smooth effect.
        Vector3 target = transform.position + (acc_vec * 0.1f);
        mesh.transform.position = Vector3.SmoothDamp(mesh.transform.position, target, ref velocity, 1.5f);
    }
    private void MeshTurn(Vector3 turn_vec)
    {
        // Turns player mesh towards torque direction for smooth movement
        Quaternion new_rot = Quaternion.Euler(turn_vec.x * 15f, turn_vec.y * 15f, turn_vec.z * 18f);
        mesh.transform.rotation = transform.rotation * new_rot;
    }
    private void UpdateWeapon()
    {
        float delta = Time.deltaTime;

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // context = Vector 2 (left-right, backward-forward) (-1 1)
        if (view.IsMine)
        {
            move_dir = context.ReadValue<Vector2>();
        }
    }

    public void OnLook(InputAction.CallbackContext context) 
    {
        // context = Vector2 (left-right, down-up) (-1 1)
        if (view.IsMine)
        {
            look_dir = context.ReadValue<Vector2>();
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (view.IsMine)
        {
            look_roll = context.ReadValue<float>();
        }
    }
}
