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
    public TMP_Dropdown m_budgetRef;
    //Not currently implemented: public TextMeshProUGUI m_collectiblesRef; 
    public TMP_Dropdown m_timerRef;
    public TMP_Dropdown m_spawnDelayRef;
    public TMP_Dropdown m_bestOutOfRef;
    public static string MatchMap = "Station";
    public static int MatchBudget = 5000;
    public static int MatchTimer = 15;
    public static int MatchSpawnDelay = 5;
    public static int MatchBestOf = 4;
    private Button m_buttonRef;
    private Dictionary<int, int> m_budget_dict = new Dictionary<int, int> {{0, 100}, {1, 150}, { 2, 250} };
    private Dictionary<int, int> m_timer_dict = new Dictionary<int, int> { { 0, 10 }, { 1, 15 }, { 2, 30 } };
    private Dictionary<int, int> m_spawn_dict = new Dictionary<int, int> { { 0, 5 }, { 1, 10 }, { 2, 15 } };
    private Dictionary<int, int> m_boo_dict = new Dictionary<int, int> { { 0, 3 }, { 1, 4 }, { 2, 5 } };
    private bool CheckValid()
    {
        if (m_mapNameRef.text != "<No-map>")
        {
            try
            {
                int budget = m_budget_dict[m_budgetRef.value];
                int timer = m_timer_dict[m_timerRef.value];
                int spawnDelay = m_spawn_dict[m_spawnDelayRef.value];
                int bestOf = m_boo_dict[m_bestOutOfRef.value];

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
