using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapSelection : MonoBehaviour
{
    public TextMeshProUGUI m_textRef;
    public string m_name; //name of map scene
    public void SetMapName()
    {
        m_textRef.text = m_name;
    }
}
