using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlacer : MonoBehaviour
{
    public SlotPicker m_SlotPicker;
    public void PlaceWeapon(GameObject prefab)
    {
        //Places weapon on the currently selected slot of the ship
        m_SlotPicker.AddWeapon(prefab);
    }
}
