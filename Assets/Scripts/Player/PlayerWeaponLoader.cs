using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponLoader : MonoBehaviour
{
    public List<GameObject> m_slotList = new List<GameObject>();
    private void Start()
    {
        Invoke("DisableSlots", 0.5f);
    }
    private void DisableSlots()
    {
        //If for whatever reason there is nothing built for the ship at all, this disables the slots from appearing in-game
        foreach (GameObject slot in m_slotList)
        {
            slot.SetActive(false);
        }
    }
    public void LoadAllWeapons(List<WeaponryInfo> weapons)
    {
        foreach (WeaponryInfo weapon in weapons)
        {
            //Loads each weapon into the slot, then sets its local position to be that of the empty node
            if (weapon.m_index < m_slotList.Count)
            {
                Transform idealLocation = m_slotList[weapon.m_index].transform;
                GameObject newWeapon = Instantiate(weapon.m_prefab, this.transform);
                newWeapon.transform.localPosition = idealLocation.localPosition;
            }
        }
        //Disable all gameobjects in the slotlist to prevent them being visible in game.
        DisableSlots();
    }
}
