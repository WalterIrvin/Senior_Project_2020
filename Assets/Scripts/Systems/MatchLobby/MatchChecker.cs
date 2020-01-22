using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MatchChecker : MonoBehaviour
{
    /// The match checker is attached to finish button on the match creation scene
    /// when all conditions are legal (ie all forms filled) the button will unlock allowing user to continue to build scene
    /// otherwise will lock button preventing advancing with bad data (no map, no round timer, etc.)
    public TextMeshProUGUI m_mapNameRef;
    public TextMeshProUGUI m_budgetRef;
    //Not currently implemented: public TextMeshProUGUI m_collectiblesRef; 
    public TextMeshProUGUI m_timerRef;
    public TextMeshProUGUI m_spawnDelayRef;
    public TextMeshProUGUI m_bestOutOfRef;
    public static string MatchMap = "Station";
    public static int MatchBudget = 20;
    public static int MatchTimer = 45;
    public static int MatchSpawnDelay = 3;
    public static int MatchBestOf = 4;
    private Button m_buttonRef;
    private bool CheckValid()
    {
        if (m_mapNameRef.text != "<No-map>")
        {
            try
            {
                int budget = int.Parse(m_budgetRef.text);
                int timer = int.Parse(m_timerRef.text);
                int spawnDelay = int.Parse(m_spawnDelayRef.text);
                int bestOf = int.Parse(m_bestOutOfRef.text);
                if (budget >= 20 && timer > 0 && spawnDelay >= 0 && bestOf > 0)
                {
                    //Budget needs to be enough to buy at least one weapon, timer needs to be at least > 0, and spawndelay can be instant or longer.
                    MatchMap = m_mapNameRef.text;
                    MatchBudget = budget;
                    MatchTimer = timer;
                    MatchSpawnDelay = spawnDelay;
                    MatchBestOf = bestOf;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        return false;
    }
    private void Start()
    {
        m_buttonRef = this.gameObject.GetComponent<Button>();
    }
    private void Update()
    {
        m_buttonRef.enabled = CheckValid();
    }
}
