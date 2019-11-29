using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundUpdater : MonoBehaviour
{
    public TextMeshProUGUI m_win_text;
    public List<Player> m_player_list;
    public float m_round_time = 30; // time in seconds
    public int m_best_out_of = 3;
    private float m_time_left;
    void Start()
    {
        m_time_left = m_round_time;
    }
    void CheckWinState()
    {
        m_time_left = m_round_time;
        int max_kills = 0;
        int cur_idx = -1; // is set to -1 to signify nobody currently winning
                          // Check for round winner
        for (int i = 0; i < m_player_list.Count; i++)
        {
            int cur_kills = m_player_list[i].GetKillCount();
            if (cur_kills > max_kills)
            {
                max_kills = cur_kills;
                cur_idx = i;
            }
            else if(cur_kills == max_kills)
            {
                //in this case there is tie, nobody wins
                cur_idx = -1;
            }
        }
        // Reset for new round 
        for (int i = 0; i < m_player_list.Count; i++)
        {
            Player player_ref = m_player_list[i];
            if (i == cur_idx)
            {
                player_ref.WinRound();
            }
            else if (cur_idx != -1) //If not tie
            {
                player_ref.LoseRound();
            }
            player_ref.ResetRound();
        }

        int min_to_win = Mathf.FloorToInt(m_best_out_of / 2) + 1;
        int max_wins = 0;
        //Find whoever is in the lead
        for (int i = 0; i < m_player_list.Count; i++)
        {
            int cur_wins = m_player_list[i].GetWins();
            if (cur_wins > max_wins)
            {
                max_wins = cur_wins;
            }
        }
        List<Player> tied_players = new List<Player>();
        int second_max = 0;
        //Now check for the second highest player, or for any players tied to determine if match is won or not.
        for (int i = 0; i < m_player_list.Count; i++)
        {
            int cur_wins = m_player_list[i].GetWins();
            if (cur_wins > second_max && cur_wins < max_wins) // Case 1, find the second highest player
            {
                second_max = cur_wins;
            }
            else if (cur_wins == max_wins) // Case 2, there is tied with winner, should just be one if nobody is tied with winner
            {
                tied_players.Add(m_player_list[i]);
            }
        }
        //If there is someone in the lead, check they have at least min_to_win lead on second highest
        if (tied_players.Count == 1 && cur_idx != -1)
        {
            int lead = max_wins - second_max;
            if (lead >= min_to_win)
            {
                m_win_text.SetText(m_player_list[cur_idx].gameObject.name + " Wins!");
                m_win_text.gameObject.SetActive(true);
            }
        }
    }
    void Update()
    {
        m_time_left -= Time.deltaTime;
        foreach (Player player_ref in m_player_list)
        {
            player_ref.UpdateRoundTimer(m_time_left);
        }
        if (m_time_left <= 0)
        {
            CheckWinState();

        }
    }
}
