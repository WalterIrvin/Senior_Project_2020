using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int m_joystick = 0;
    public GameObject m_coreRef;
    public Image m_healthBarRef;
    public float m_maxHealth = 100;
    public Targeting m_targetReticule;
    public TextMeshProUGUI m_curRound;
    public TextMeshProUGUI m_roundTime;
    public TextMeshProUGUI m_killCount;
    public TextMeshProUGUI m_winCount;
    public TextMeshProUGUI m_lossCount;
    public AudioSource m_deathSound;
    public ParticleSystem m_deathAnimation; // Death explosion
    private float m_curHealth;
    private int m_kills = 0;
    private int m_wins = 0;
    private int m_losses = 0;
    private float m_respawnTime = MatchChecker.MatchSpawnDelay;
    private float m_curRespawnTime = 0;
    private float m_animationTimer = 1.5f; // time that explosion gets to animate
    private bool m_alreadyDead = false;
    private void Start()
    {
        m_coreRef.GetComponent<Movement>().SetController(m_joystick);
        m_curHealth = m_maxHealth;
    }
    public void Update()
    {
        if (m_curHealth <= 0)
        {
            if (m_animationTimer > 0)
            {
                m_animationTimer -= Time.deltaTime;
            }
            else
            {
                m_animationTimer = 4f;
                m_deathAnimation.Stop();
                m_coreRef.SetActive(false);
            }
        }
    }
    public void Respawn()
    {
        m_curHealth = m_maxHealth;
        m_coreRef.SetActive(true);
        m_curRespawnTime = 0;
    }
    public void RespawnCheck()
    {
        //Checks if the core needs to be respawned
        if(m_coreRef.activeSelf == false)
        {
            m_curRespawnTime += Time.deltaTime;
            if (m_curRespawnTime >= m_respawnTime)
            {
                Respawn();
                m_alreadyDead = false;
            }
        }
    }
    public bool DamagePlayer(float damage)
    {
        ///Returns true if damage killed player
        m_curHealth -= damage;
        if (m_curHealth <= 0 && !m_alreadyDead)
        {
            m_curHealth = 0;
            m_deathSound.Play();
            m_deathAnimation.Play();
            m_alreadyDead = true;
            return true;
        }
        return false;
    }
    public void AddKill()
    {
        m_kills++;
    }
    public void ClearKills()
    {
        m_kills = 0;
    }
    public int GetKills()
    {
        return m_kills;
    }
    public void AddWin()
    {
        m_wins++;
    }
    public int GetWins()
    {
        return m_wins;
    }
    public void AddLoss()
    {
        m_losses++;
    }
    public int GetLosses()
    {
        return m_losses;
    }
    
    public void ControllerUpdate(float roundTime, int curRound)
    {
        m_roundTime.SetText("Time Left: " + Mathf.Round(roundTime));
        m_curRound.SetText("Round: " + curRound);
        m_killCount.SetText("Kills: " + m_kills);
        m_winCount.SetText("Wins: " + m_wins);
        m_lossCount.SetText("Losses: " + m_losses);
        m_healthBarRef.fillAmount = m_curHealth / m_maxHealth;
    }
}
