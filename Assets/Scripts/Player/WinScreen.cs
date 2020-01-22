using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    private string m_name;
    void Start()
    {
        m_name = RoundUpdater.winning_player;
        m_text.SetText(m_name + " Wins!");
    }
}
