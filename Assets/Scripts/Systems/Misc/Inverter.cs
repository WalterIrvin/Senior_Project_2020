using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    public GameObject m_inverterObj;
    public void toggleOn()
    {
        m_inverterObj.SetActive(!m_inverterObj.activeSelf);
    }
}
