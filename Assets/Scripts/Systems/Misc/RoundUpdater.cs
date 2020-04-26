using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundUpdater : MonoBehaviour
{
    public static string winning_player = "DebugTesting";
    private float m_roundTime = MatchChecker.MatchTimer;
    private int m_curRound = 0;
    private int m_bestOutOf = MatchChecker.MatchBestOf;
    public List<Player> m_playerList = new List<Player>();
    private float m_curTime = 0f;
    public JukeBox m_JukeBox;
    private void Update()
    {
        m_curTime += Time.deltaTime;
        if (m_curTime >= m_roundTime)
        {
            if(RoundWin())
            { 
                m_curRound++;
                //TODO - put some kind of round win screen, like 5 second delay, and then a hard respawn for everyone.
            }
            m_curTime = 0;

        }
        foreach (Player player in m_playerList)
        {
            player.ControllerUpdate(m_roundTime - m_curTime, m_curRound);
            player.RespawnCheck();
        }
    }

    private bool RoundWin()
    {
        ///Determines the outcome of the round based on number of kills each player gets. If the round is a tie, nobody wins/loses the round.
        int max_kill = 0;
        bool tie = false;
        Player max_player = null;
        foreach (Player player in m_playerList)
        {
            int cur_kills = player.GetKills();
            if (cur_kills == max_kill)
            {
                tie = true;
            }
            else if (cur_kills > max_kill)
            {
                tie = false;
                max_kill = cur_kills;
                max_player = player;
            }
        }
        if(!tie)
        {
            //If the round is not a tie, add win to the winning player
            //determine if match is complete based on the difference between highest and second highest win count.
            if (max_player != null)
            {
                max_player.AddWin();
                int min_to_win = Mathf.FloorToInt(m_bestOutOf / 2) + 1;
                int most_wins = 0;
                int second_most_wins = 0;
                foreach (Player player in m_playerList)
                {
                    if(player != max_player)
                    {
                        player.AddLoss();
                    }
                    player.ClearKills();
                    player.Respawn();
                    if(player.GetWins() > most_wins)
                    {
                        //If the player has more wins the current highest,
                        //set the second highest to the old value, and new max value to the one given by player.
                        second_most_wins = most_wins;
                        most_wins = player.GetWins();
                    }
                }
                if(most_wins - second_most_wins >= min_to_win)
                {
                    //If the difference between highest and second highest is equivalent to the minimum needed to win, the match ends.
                    winning_player = max_player.name;
                    BuildSubjectLogic.AllPlayerInfo = new List<PlayerInfo>();
                    if(m_JukeBox != null)
                    {
                        m_JukeBox.Stop();
                    }
                    SceneManager.LoadScene("EndScreen");
                }
            }
        }
        //Returns true if there isn't a tie, ie someone won the round, and if so advances the round counter
        return !tie;
    }
}
