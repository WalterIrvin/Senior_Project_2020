using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPicker : MonoBehaviour
{
    /// <summary>
    /// Main class dedicated to passing info from the gui to the build subject to be passed onto actual player when game starts.
    /// Depending on ship type can have multiple weapon slots >= 4, with the slotPicker able to select and highlight any in the array.
    /// When a weapon is selected, it changes the mesh of the weapon node, and then passes on the actual prefab to the build subject to be stored in static array.
    /// When the game loads, this static array is loaded and all relevant data pertaining to the ships core data is loaded. Ship names are handled in
    /// InputFieldKeyboard, and are similarly passed onto the buildsubject when the build is complete.
    /// </summary>
    public int m_currentWeaponSlot = 0;
    public Material m_selectedWeaponMat;
    public Material m_unselectedWeaponMat;
    public GameObject m_BuildSubjectRef;
    private List<GameObject> m_weaponSlots;
    private void Start()
    {
        m_weaponSlots = m_BuildSubjectRef.GetComponent<BuildSubjectLogic>().GetWeaponSlots();
        SetCurrentSlot(0);
    }
    public void AddWeapon(GameObject prefab)
    {
        Mesh prefabMesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        m_weaponSlots[m_currentWeaponSlot].GetComponent<MeshFilter>().sharedMesh = prefabMesh;
        m_BuildSubjectRef.GetComponent<BuildSubjectLogic>().AddWeapon(m_currentWeaponSlot, prefab);
    }
    public void SetCurrentSlot(int slot)
    {
        m_currentWeaponSlot = slot;
        for (int i = 0; i < m_weaponSlots.Count; i++)
        {
            if (i != slot)
            {
                m_weaponSlots[i].GetComponent<Renderer>().material = m_unselectedWeaponMat;
            }
            else if (i == slot)
            {
                m_weaponSlots[i].GetComponent<Renderer>().material = m_selectedWeaponMat;
            }
        }
    }
}
