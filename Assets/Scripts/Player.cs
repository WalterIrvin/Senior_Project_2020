using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Wrapper script for basic Player logic, accepts damage input from projectiles when hit, and will destroy if health is <= 0
    //May need special logic in future to disable controller associated with it unlike just the TargetScript.
    public TextMeshProUGUI m_round_time;
    public TextMeshProUGUI m_wins;
    public TextMeshProUGUI m_losses;
    public TextMeshProUGUI m_kills;
    public float m_health = 100;
    public int m_joy_num = 0; // Sets the controller for this player
    public float m_respawn_time = 5f; // How many seconds to wait before respawning
    public Movement m_mover;
    public List<Fire> m_gun_list; // gun list controlled by the player
    private int m_kill_count = 0; 
    private int m_win_count = 0;
    private int m_loss_count = 0;
    private bool m_alive = true; // is the player alive? reset to true after respawning if not
    private float m_cur_respawn_time;
    void Start()
    {
        UpdateText();
        m_cur_respawn_time = 5f;
        foreach (Fire gun in m_gun_list)
        {
            gun.SetJoystick(m_joy_num);
        }
        m_mover.SetController(m_joy_num);
    }
    public void NewMatch()
    {
        ResetRound();
        m_win_count = 0;
        m_loss_count = 0;
        UpdateText();
    }
    public void WinRound()
    {
        m_win_count++;
        UpdateText();
    }
    public void LoseRound()
    {
        m_loss_count++;
        UpdateText();
    }
    public void ResetRound()
    {
        m_kill_count = 0;
        RespawnShip();
        UpdateText();
    }
    public void UpdateRoundTimer(float new_time)
    {
        m_round_time.SetText("Round Time: " + Mathf.Round(new_time));
    }
    public int GetWins()
    {
        return m_win_count;
    }
    public int GetKillCount()
    {
        return m_kill_count;
    }
    public void AddKill()
    {
        m_kill_count++;
        UpdateText();
    }
    public bool HitPlayer(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            m_mover.gameObject.SetActive(false);
            m_alive = false;
            m_health = 100;
            return true;
        }
        return false;
    }
    private void UpdateText()
    {
        m_wins.SetText("Wins: " + m_win_count);
        m_losses.SetText("Losses: " + m_loss_count);
        m_kills.SetText("Kills: " + m_kill_count);
    }
    private void RespawnShip()
    {
        m_alive = true;
        m_health = 100;
        m_mover.gameObject.SetActive(true);
        foreach(Transform child in m_mover.gameObject.transform)
        {
            //Sets all the children gameObjects of the core to be active
            //resets health if target type
            try
            {
                Target target_ref = child.gameObject.GetComponent<Target>();
                target_ref.Respawn();
            }
            catch(Exception)
            {;}
            child.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        if (m_alive == false)
        {
            m_cur_respawn_time -= Time.deltaTime;
            if(m_cur_respawn_time <= 0)
            {
                RespawnShip();
                m_cur_respawn_time = m_respawn_time;
                m_alive = true;
            }
        }
    }
}
