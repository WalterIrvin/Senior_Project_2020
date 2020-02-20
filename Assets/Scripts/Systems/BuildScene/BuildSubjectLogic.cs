using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
[System.Serializable]
public class WeaponPlacerInfo
{
    public string m_name;
    public int m_cost;
}

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
    public SlotPicker m_pickerRef;
    public List<WeaponPlacerInfo> m_WeaponPlacerInfo = new List<WeaponPlacerInfo>();
    public static int m_currentPlayerId = 1; //The current ID to use for the player, is the same as joystick number.
    public static int m_curBuildCost = 0;
    public TextMeshProUGUI m_BuildCostTxt;
    public TextMeshProUGUI m_textRef;
    public List<GameObject> m_weaponSlots = new List<GameObject>();
    public List<WeaponryInfo> m_placedWeapons = new List<WeaponryInfo>();
    public static List<PlayerInfo> AllPlayerInfo = new List<PlayerInfo>(); // the static list holding all players build info
    private void Start()
    {
        UpdateBuildText();
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
        m_pickerRef.resetWeapon();
        m_curBuildCost = 0;
        UpdateBuildText();
        m_placedWeapons = new List<WeaponryInfo>();
        foreach(GameObject obj in m_weaponSlots)
        {
            obj.GetComponent<MeshFilter>().sharedMesh = m_defaultMesh;
        }
    }
    public bool IsWeaponHere()
    {
        string name = m_pickerRef.GetCurrentWeapon();
        Debug.Log(name);
        return name != "None";
    }
    public int GetWeaponCost(string name)
    {
        foreach (WeaponPlacerInfo info in m_WeaponPlacerInfo)
        {
            if (info.m_name == name)
            {
                return info.m_cost;
            }
        }
        return 0;
    }
    public void RemoveLastWeapon()
    {
        string name = m_pickerRef.GetCurrentWeapon();
        foreach (WeaponPlacerInfo info in m_WeaponPlacerInfo)
        {
            if (info.m_name == name)
            {
                m_curBuildCost -= info.m_cost;
                m_pickerRef.RemoveWeapon();
                return;
            }
        }
    }
    public void UpdateBuildText()
    {
        string data = m_curBuildCost + " / " + MatchChecker.MatchBudget;
        m_BuildCostTxt.SetText(data);
    }
    public void FinalizeBuild()
    {
        //Once build is finished, pack up all data and store it into static list to be used during gameplay.
        string finalName = m_textRef.text;
        PlayerInfo newPlayer = new PlayerInfo(m_currentPlayerId, finalName, m_placedWeapons);
        AllPlayerInfo.Add(newPlayer);
        UpdateBuildText();
        if (m_currentPlayerId < Input.GetJoystickNames().Length && m_currentPlayerId < 2)
        {
            //Allows to build up to 2 ships, game is hard capped at 2 joysticks though.
            m_currentPlayerId++;
            m_inputManager.UpdateInputManager();
            ResetSlots();
        }
        else
        {
            //If all players have built, switch scene to the main game.
            m_curBuildCost = 0;
            m_currentPlayerId = 1;
            this.gameObject.GetComponent<LevelSwitcher>().SetLevel(MatchChecker.MatchMap);
        }
    }
}
