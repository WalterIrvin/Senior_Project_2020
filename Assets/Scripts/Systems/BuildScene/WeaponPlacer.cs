using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPlacer : MonoBehaviour
{
    public string m_name;
    public SlotPicker m_SlotPicker;
    public BuildSubjectLogic m_buildRef;
    public void PlaceWeapon(GameObject prefab)
    {
        //Places weapon on the currently selected slot of the ship
        int x = MatchChecker.MatchBudget;
        int cost = m_buildRef.GetWeaponCost(m_name);
        if (!m_buildRef.IsWeaponHere())
        {
            if (BuildSubjectLogic.m_curBuildCost + cost <= x)
            {
                BuildSubjectLogic.m_curBuildCost += cost;
                m_SlotPicker.AddWeapon(prefab, m_name);
                m_buildRef.UpdateBuildText();
            }
        }
        else
        {
            m_buildRef.RemoveLastWeapon();
            if (BuildSubjectLogic.m_curBuildCost + cost <= x)
            { 
                BuildSubjectLogic.m_curBuildCost += cost;
                m_SlotPicker.AddWeapon(prefab, m_name);
                m_buildRef.UpdateBuildText();
            }
        }
        
    }
}
