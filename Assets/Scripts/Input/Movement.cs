using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class Movement : MonoBehaviour
    {
        public GameObject playermesh_ref;
        private Vector3 velocity;

        private Vector2 delta_turn;
        private Vector2 turn_dampener; // Use in mesh turning to prevent jittery behaviour.
        private Vector2 turn_vel;

        private Vector2 delta_move;
        private float sqr_max_vel;
        private float throttle_acc = 75;
        private float strafe_acc = 50;
        private Rigidbody rb;

        // private float rot_speed = 90f;

        public void Start()
        {
            rb = this.gameObject.GetComponent<Rigidbody>();
            delta_move = new Vector2();
            delta_turn = new Vector2();
            sqr_max_vel = 15000;
            velocity = Vector3.zero;
            turn_vel = Vector2.zero;
        }

        public void Update()
        {
            LockCursor();
            UpdateMovement();
            UpdateLook();
            
        }
        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void UpdateMovement()
        {
            Vector3 forward = this.transform.forward;
            Vector3 right = this.transform.right;
            float delta_forward = delta_move.y * throttle_acc;
            float delta_right = delta_move.x * strafe_acc;
            Vector3 acc_vec = (forward * delta_forward) + (right * delta_right);
            if (rb.velocity.sqrMagnitude < sqr_max_vel)
            {
                rb.AddForce(acc_vec, ForceMode.Acceleration);
            }
            MeshMove(acc_vec);
        }
        private void UpdateLook()
        {
            float pitch_amt = -delta_turn.y * 0.01f;
            float  roll_amt = -delta_turn.x * 0.01f;
            Vector3 pitch = this.transform.right * pitch_amt;
            Vector3 roll = this.transform.forward * roll_amt;
            rb.AddTorque(pitch, ForceMode.Acceleration);
            rb.AddTorque(roll, ForceMode.Acceleration);

            if (turn_dampener == null)
            {
                turn_dampener = new Vector2(delta_turn.x, delta_turn.y);
            }
            else
            {
                turn_dampener = Vector2.SmoothDamp(turn_dampener, delta_turn, ref turn_vel, 1.5f);
            }
            float mesh_pitch = -turn_dampener.y;
            float mesh_roll = -turn_dampener.x;
            MeshTurn(mesh_pitch * 2.5f, mesh_roll * 2.5f);
        }
        private void MeshMove(Vector3 acc_vec)
        {
            // pushes player mesh towards direction of acceleration for smooth effect.
            Vector3 target = this.transform.position + (acc_vec * 0.1f);
            playermesh_ref.transform.position = Vector3.SmoothDamp(playermesh_ref.transform.position, target, ref velocity, 1.5f);
        }

        private void MeshTurn(float pitch, float roll)
        {
            // Turns player mesh towards torque direction for smooth movement
            Quaternion new_rot = Quaternion.Euler(pitch, 0, roll);
            playermesh_ref.transform.rotation = this.transform.rotation * new_rot;
        }

        
        public void Move(InputAction.CallbackContext context)
        {
            //input context ref: [strafe, throttle]
            Vector2 ctx = context.ReadValue<Vector2>();
            delta_move = ctx;
        }

        public void Look(InputAction.CallbackContext context)
        {
            Vector2 ctx = context.ReadValue<Vector2>();
            delta_turn = ctx;
        }

        public void Fire(InputAction.CallbackContext context)
        {
            //Debug.Log(context.ReadValue<float>());
        }


    }
}