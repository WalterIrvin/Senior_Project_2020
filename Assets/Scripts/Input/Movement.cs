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

        public float mesh_pitchrate = 2.5f;
        public float mesh_rollrate = 3.5f;
        public float player_pitchrate = 2.5f;
        public float player_rollrate = 3.5f;
        private Vector2 centerpoint;

        // private float rot_speed = 90f;

        public void Start()
        {
            rb = this.gameObject.GetComponent<Rigidbody>();
            delta_move = new Vector2();
            delta_turn = new Vector2();
            centerpoint = new Vector2(Screen.width / 2.0f, Screen.height / 2f);
            sqr_max_vel = 15000;
            velocity = Vector3.zero;
            turn_vel = Vector2.zero;
        }

        public void Update()
        {
            //LockCursor();
            UpdateMovement();
            centerpoint = new Vector2(Screen.width / 2.0f, Screen.height / 2f);
            UpdateLook();
            
        }
        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void UpdateMovement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
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
            Vector3 pitch = pitch_amt * player_pitchrate * transform.right;
            Vector3 roll = player_rollrate * roll_amt * transform.forward;
            rb.AddTorque(pitch, ForceMode.Acceleration);
            rb.AddTorque(roll, ForceMode.Acceleration);

            if (turn_dampener == null)
            {
                turn_dampener = new Vector2(delta_turn.x, delta_turn.y);
            }
            else
            {
                turn_dampener = Vector2.SmoothDamp(turn_dampener, delta_turn, ref turn_vel, 0.33f);
            }
            float mesh_pitch = -turn_dampener.y;
            float mesh_roll = -turn_dampener.x;
            MeshTurn(mesh_pitch * mesh_pitchrate, mesh_roll * mesh_rollrate);
        }
        private void MeshMove(Vector3 acc_vec)
        {
            // pushes player mesh towards direction of acceleration for smooth effect.
            Vector3 target = transform.position + (acc_vec * 0.1f);
            playermesh_ref.transform.position = Vector3.SmoothDamp(playermesh_ref.transform.position, target, ref velocity, 1.5f);
        }

        private void MeshTurn(float pitch, float roll)
        {
            // Turns player mesh towards torque direction for smooth movement
            Quaternion new_rot = Quaternion.Euler(pitch, 0, roll);
            playermesh_ref.transform.rotation = transform.rotation * new_rot;
        }

        
        public void Move(InputAction.CallbackContext context)
        {
            //input context ref: [strafe, throttle]
            Vector2 ctx = context.ReadValue<Vector2>();
            delta_move = ctx;
        }

        public void Look(InputAction.CallbackContext context)
        {
            Vector2 delta = -(centerpoint - Mouse.current.position.ReadValue());
            if (delta.sqrMagnitude < 4000)
            {
                delta = Vector2.zero;
            }
            else if (delta.sqrMagnitude > 20000)
            {
                float max_delta_mag = 600;
                delta = Vector2.ClampMagnitude(delta, max_delta_mag);
            }
            delta_turn = delta / 50f;
        }

        public void Fire(InputAction.CallbackContext context)
        {
            //Debug.Log(context.ReadValue<float>());
        }


    }
}