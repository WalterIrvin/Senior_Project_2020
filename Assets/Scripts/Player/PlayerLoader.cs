using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public List<GameObject> m_PlayerRefs = new List<GameObject>();
    void Start()
    {
        foreach(PlayerInfo playerData in BuildSubjectLogic.AllPlayerInfo)
        {
            foreach(GameObject player in m_PlayerRefs)
            {
                //Sets up each part of the data, player being the immovable root object gets the name and checks its joystick id against player id
                //The core then gets the weapon list, and loads them into the correct slots.
                if (playerData.m_playerId == player.GetComponent<Player>().m_joystick)
                {
                    player.GetComponent<Player>().name = playerData.m_name;
                    player.GetComponent<Player>().m_coreRef.GetComponent<PlayerWeaponLoader>().LoadAllWeapons(playerData.m_placedWeapons);
                    break;
                }
            }
        }
    }
}
