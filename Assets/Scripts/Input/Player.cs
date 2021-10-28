using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Input
{
    public class Player : MonoBehaviourPunCallbacks, Assets.Scripts.Utility.IDamageable
    {
        public LineRenderer line_render;
        private float max_health;
        public float health = 100f;
        public float score = 0f;
        public Camera cam_ref;
        public GameObject ui_ref;
        PhotonView view;
        public GameObject fire_type;
        public GameObject ammo;
        public GameObject bullet;
        public GameObject spawnpoint;
        public GameObject health_ref;
        public int mag_size = 21;
        float delay = 0.1f;
        public float damage = 10f;
        public float range = 1000f;
        private bool firing = false;
        private int fired = 0;
        private float reload_time = 4f;
        private bool reloading = false;
        private int fire_rate = 0;  // 0 - full auto, 1 - burst fire, 2 - single
        private int burst_amt = 3;
        private int burst_fired = 0;
        private bool switched = false;

        public GameObject thruster_manager;
        public GameObject speedbar_ref;
        public GameObject playermesh_ref;
        private Vector3 velocity;

        private Vector2 delta_turn;
        private Vector2 turn_dampener; // Use in mesh turning to prevent jittery behaviour.
        private Vector2 turn_vel;

        private Vector2 delta_move = Vector2.zero;
        public float sqr_max_vel = 6500;
        private float throttle_acc = 75;
        private float strafe_acc = 50;
        private Rigidbody rb;

        public float mesh_pitchrate = 2.5f;
        public float mesh_rollrate = 3.5f;
        public float player_pitchrate = 2.5f;
        public float player_rollrate = 3.5f;
        private Vector2 centerpoint;

        private float delta_spd = 0;
        private bool accelerating = false;

        // private float rot_speed = 90f;

        public void Start()
        {
            max_health = health;
            view = GetComponent<PhotonView>();
            if (!view.IsMine)
            {
                cam_ref.enabled = false;
                ui_ref.SetActive(false);
            }
            rb = this.gameObject.GetComponent<Rigidbody>();
            delta_move = new Vector2();
            delta_turn = new Vector2();
            centerpoint = new Vector2(Screen.width / 2.0f, Screen.height / 2f);
            velocity = Vector3.zero;
            turn_vel = Vector2.zero;
        }

        public void Update()
        {
            if (view.IsMine)
            {
                LockCursor();
                UpdateMovement();
                UpdateFire();
                centerpoint = new Vector2(Screen.width / 2.0f, Screen.height / 2f);
                UpdateLook();
            }
            
        }
        // WEAPON SECTION
        private void UpdateFire()
        {
            if (view.IsMine)
            {
                if (reloading)
                {
                    ammo.GetComponent<TextMeshProUGUI>().text = "Reloading... (" + Mathf.Round(delay) + ")";
                }
                if (fired >= mag_size)
                {
                    line_render.enabled = false;
                    //SyncCloseLine();
                    Reload();
                }
                delay -= Time.deltaTime;
                if (delay <= 0)
                {
                    line_render.enabled = false;
                    //SyncCloseLine();
                    delay = 0;
                    reloading = false;
                    ammo.GetComponent<TextMeshProUGUI>().text = "Ammo: (" + (mag_size - fired) + ")";
                }
                if (firing && delay <= 0)
                {
                    line_render.enabled = true;
                    switch (fire_rate)
                    {
                        case 0:
                            Full_Auto();
                            break;
                        case 1:
                            Burst_Fire();
                            break;
                        case 2:
                            Semi_Auto();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void Spawn_Bullet()
        {
            Vector3 pos = spawnpoint.transform.position;
            Vector3 forward = this.transform.forward;

            GameObject new_bullet = PhotonNetwork.Instantiate(bullet.name, pos, Quaternion.identity);
            new_bullet.GetComponent<Bullet>().Fire(this, pos, forward);
            delay = 0.1f;
            fired += 1;
            ammo.GetComponent<TextMeshProUGUI>().text = "Ammo: (" + (mag_size - fired) + ")";
        }

        private void Fire_Ray()
        {
            Vector3 pos = spawnpoint.transform.position;
            Vector3 forward = playermesh_ref.transform.forward;
            line_render.SetPosition(0, pos);
            line_render.SetPosition(1, pos + forward * range);
            RaycastHit hit;
            if(Physics.Raycast(cam_ref.transform.position, cam_ref.transform.forward, out hit, range))
            {
                line_render.SetPosition(1, hit.transform.position);
                Player target = hit.transform.GetComponent<Player>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    //SyncDrawLine(pos, hit.transform.position);
                }
            }
            delay = 0.1f;
            fired += 1;
            ammo.GetComponent<TextMeshProUGUI>().text = "Ammo: (" + (mag_size - fired) + ")";
        }
        public void OnSwitchMode(InputAction.CallbackContext context)
        {
            if (view.IsMine)
            {
                if (context.ReadValue<float>() == 1 && !switched)
                {
                    switched = true;
                    fire_rate += 1;
                    if (fire_rate > 2)
                    {
                        fire_rate = 0;
                    }
                    switch (fire_rate)
                    {
                        case 0:
                            fire_type.GetComponent<TextMeshProUGUI>().text = "Fire: Full-Auto";
                            break;
                        case 1:
                            fire_type.GetComponent<TextMeshProUGUI>().text = "Fire: Burst-Fire";
                            break;
                        case 2:
                            fire_type.GetComponent<TextMeshProUGUI>().text = "Fire: Semi-Auto";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switched = false;
                }
            }

        }
        public void Burst_Fire()
        {
            if (burst_fired < burst_amt)
            {
                Fire_Ray();
                burst_fired += 1;
            }
            else
            {
                burst_fired = 0;
                firing = false;
            }
        }
        public void Semi_Auto()
        {
            Fire_Ray();
            firing = false;
        }
        public void Full_Auto()
        {
            Fire_Ray();
        }
        public void OnReload(InputAction.CallbackContext context)
        {
            if (view.IsMine)
            {
                Reload();
            }
        }
        public void Reload()
        {
            burst_fired = 0;
            firing = false;
            fired = 0;
            delay = reload_time;
            reloading = true;
        }
        public void FireWeapon(InputAction.CallbackContext context)
        {
            if (view.IsMine)
            {
                if (context.ReadValue<float>() == 1)
                {
                    if (delay <= 0)
                    {
                        firing = true;
                    }
                }
                else if (context.ReadValue<float>() == 0 && fire_rate != 1) // does not trigger to stop firing for burst fire only
                {
                    firing = false;
                }
            }
        }

        // MOVEMENT SECTION
        private void LockCursor()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update_Sound()
        {
            this.gameObject.GetComponent<AudioSource>().volume = delta_move.y;
        }
        private void Update_Plume()
        {
            thruster_manager.GetComponent<Thrusters>().Update_Thrusters(delta_move.y);
        }
        private void UpdateMovement()
        {
            if (accelerating)
            {
                float diff = delta_spd * Time.deltaTime;
                if (delta_move.y + diff > 0 && delta_move.y + diff <= 1)
                {
                    delta_move.y += diff;
                }
                else if (delta_move.y + diff > 1)
                {
                    delta_move.y = 1;
                }
                else
                {
                    delta_move.y = 0;
                }
                speedbar_ref.GetComponent<Image>().fillAmount = delta_move.y;

            }
            Update_Sound();
            Update_Plume();
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
            if (view.IsMine)
            {
                //input context ref: [strafe, throttle]
                Vector2 ctx = context.ReadValue<Vector2>();
                accelerating = false;
                if (Mathf.Abs(ctx.y) > 0)
                    accelerating = true;
                delta_spd = ctx.y;
            }
        }

        public void Look(InputAction.CallbackContext context)
        {
            if (view.IsMine) 
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
        }

        void Die()
        {
            PhotonNetwork.Destroy(view);
        }

        public void TakeDamage(float damage)
        {
            view.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }

        public void SyncDrawLine(Vector3 source, Vector3 dest)
        {
            view.RPC("RPC_SyncDrawLine", RpcTarget.All, source, dest);
        }

        public void SyncCloseLine()
        {
            view.RPC("RPC_SyncCloseLine", RpcTarget.All);
        }
        [PunRPC]
        void RPC_SyncCloseLine()
        {
            if (!view.IsMine)
            {
                return;
            }
            line_render.enabled = false;
        }
        [PunRPC]
        void RPC_SyncDrawLine(Vector3 source, Vector3 dest)
        {
            line_render.enabled = true;
            if (!view.IsMine)
            {
                return;
            }
            line_render.SetPosition(0, source);
            line_render.SetPosition(1, dest);
        }
        [PunRPC]
        void RPC_TakeDamage(float damage)
        {
            if (!view.IsMine)
            {
                return;
            }
            else
            {
                health -= damage;
                TextMeshProUGUI health_text = health_ref.GetComponent<TextMeshProUGUI>();
                if (health_text != null)
                {
                    health_text.text = "Health: " + health + "/" + max_health;
                }
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }
}