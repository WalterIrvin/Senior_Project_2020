using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class Movement : MonoBehaviour
    {
        //Turning variables
        private Vector2 delta_turn;

        //Movement variables
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
        }
        private void UpdateLook()
        {
            rb.AddTorque(this.transform.right * (-delta_turn.y * 0.1f), ForceMode.Acceleration);
            rb.AddTorque(this.transform.forward * (-delta_turn.x * 0.1f), ForceMode.Acceleration);
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