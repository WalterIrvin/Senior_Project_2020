using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum WeaponType
{
PLASMA,ROCKET,RAIL,RAM
}
public class Fire : MonoBehaviour
{
    public WeaponType m_weaponType;
    public GameObject m_projectilePrefab;
    public float m_reloadTime = 0.5f;
    private GameObject m_playerRef;
    private GameObject m_coreRef;
    private Player m_playerScript;
    private int m_maxProjectiles = 200;
    private float m_fireAxis;
    private List<GameObject> m_projectileList = new List<GameObject>();
    private float m_curTimer = 0;
    private bool m_fired = false;
    private void Start()
    {
        m_coreRef = this.transform.parent.gameObject;
        m_playerRef = this.transform.parent.parent.gameObject;
        m_playerScript = m_playerRef.GetComponent<Player>();
    }
    public Player GetPlayerScript()
    {
        return m_playerScript;
    }
    private void Update()
    {
        GetInput();
        CheckSpawnProjectile();
        CleanList();
    }
    private void CleanList()
    {
        List<GameObject> deadList = new List<GameObject>();
        foreach (GameObject obj in m_projectileList)
        {
            if (!obj.activeSelf)
            {
                deadList.Add(obj);
            }
        }
        foreach (GameObject obj in deadList)
        {
            m_projectileList.Remove(obj);
            Destroy(obj);
        }
    }
    private void CheckSpawnProjectile()
    {
        //Debug.Log("Fired: " + m_fired + ", fireAxis =  " + m_fireAxis + ", projectileCount = " + m_projectileList.Count);
        if (!m_fired && (m_fireAxis > 0 && m_projectileList.Count < m_maxProjectiles))
        {
            GameObject new_projectile = Instantiate(m_projectilePrefab);
            new_projectile.transform.position = this.transform.position;
            new_projectile.transform.rotation = this.transform.rotation;
            Projectile projectile_script = new_projectile.GetComponent<Projectile>();
            projectile_script.Init(this);

            m_projectileList.Add(new_projectile);
            m_fired = true;
        }
        else if (m_fired)
        {
            m_curTimer += Time.deltaTime;
            if(m_curTimer >= m_reloadTime)
            {
                m_fired = false;
                m_curTimer = 0;
            }
        }
    }
    private void GetInput()
    {
        int joynum = m_coreRef.GetComponent<Movement>().GetController();
        switch (m_weaponType)
        {
            case WeaponType.PLASMA:
                m_fireAxis = Input.GetAxis("P" + joynum + "_PrimaryFire");
                break;
            case WeaponType.ROCKET:
                m_fireAxis = Input.GetAxis("P" + joynum + "_SecondaryFire");
                break;
            case WeaponType.RAIL:
                m_fireAxis = Input.GetAxis("P" + joynum + "_SecondaryFire");
                break;
            default:
                break;
        }
    }
}
