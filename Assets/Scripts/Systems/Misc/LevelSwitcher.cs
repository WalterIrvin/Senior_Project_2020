using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{
    public JukeBox m_player;
    public void SetLevel(string name)
    {
        if (m_player != null)
        {
            m_player.Stop();
        }
        SceneManager.LoadScene(name);
    }
}
