﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

[System.Serializable]
public class WeaponryInfo
{
    /// <summary>
    /// Serializable class which contains the index slot of the weapon (the weapon slot on the ship)
    /// also contains the prefab, which is the gameobject to spawn in that location (Plasma, Rocket, etc.)
    /// </summary>
    public int m_index;
    public GameObject m_prefab;
    public WeaponryInfo(int index, GameObject obj)
    {
        m_index = index;
        m_prefab = obj;
    }
}

[System.Serializable]
public class PlayerInfo
{
    /// <summary>
    /// Serves as the total useful info gathered during build scene, used to construct custom player during actual battle.
    /// </summary>
    public int m_playerId; // same number as their joystick num
    public string m_name;
    public List<WeaponryInfo> m_placedWeapons = new List<WeaponryInfo>(); // is what weapons are placed and in which slots
    public PlayerInfo(int playerId, string name, List<WeaponryInfo> weapons)
    {
        m_playerId = playerId;
        m_name = name;
        m_placedWeapons = weapons;
    }
}
public class BuildSubjectLogic : MonoBehaviour
{
    /// <summary>
    /// Script attached to build ref, stores and maintains static list of all player info to be loaded in game once build is finished.
    /// keeps and maintains list of currently added weaponry and in which locations on the ship weapon array.
    /// </summary>
    private EventSystemInputManager m_inputManager;
    public Mesh m_defaultMesh;
    public static int m_currentPlayerId = 1; //The current ID to use for the player, is the same as joystick number.
    public TextMeshProUGUI m_textRef;
    public List<GameObject> m_weaponSlots = new List<GameObject>();
    public List<WeaponryInfo> m_placedWeapons = new List<WeaponryInfo>();
    public static List<PlayerInfo> AllPlayerInfo = new List<PlayerInfo>(); // the static list holding all players build info
    private void Start()
    {
        try
        {
            m_inputManager = EventSystem.current.gameObject.GetComponent<EventSystemInputManager>();
        }
        catch(Exception)
        {
            Debug.LogError("Error, EventSystemInputManager script not found on the current EventSystem, trying attaching one and trying again.");
        }
    }
    public List<GameObject> GetWeaponSlots()
    {
        //Returns a list of gameobjects which are the weapon slots of the ship
        return m_weaponSlots;
    }
    public void AddWeapon(int slot, GameObject prefab)
    {
        //Checks if weapon has already been added to list, if so update, otherwise add new entry to the list.
        bool isInList = false;
        foreach (WeaponryInfo weapon in m_placedWeapons)
        {
            if(weapon.m_index == slot)
            {
                weapon.m_prefab = prefab;
                isInList = true;
                break;
            }
        }
        if(!isInList)
        {
            WeaponryInfo newWeapon = new WeaponryInfo(slot, prefab);
            m_placedWeapons.Add(newWeapon);
        }
    }
    private void ResetSlots()
    {
        //Clears out placed weapons and resets the mesh shown.
        m_placedWeapons = new List<WeaponryInfo>();
        foreach(GameObject obj in m_weaponSlots)
        {
            obj.GetComponent<MeshFilter>().sharedMesh = m_defaultMesh;
        }
    }
    public void FinalizeBuild()
    {
        //Once build is finished, pack up all data and store it into static list to be used during gameplay.
        string finalName = m_textRef.text;
        PlayerInfo newPlayer = new PlayerInfo(m_currentPlayerId, finalName, m_placedWeapons);
        AllPlayerInfo.Add(newPlayer);
        if (m_currentPlayerId < Input.GetJoystickNames().Length)
        {
            m_currentPlayerId++;
            m_inputManager.UpdateInputManager();
            ResetSlots();
        }
        else
        {
            //If all players have built, switch scene to the main game.
            this.gameObject.GetComponent<LevelSwitcher>().SetLevel(MatchChecker.MatchMap);
        }
    }
}