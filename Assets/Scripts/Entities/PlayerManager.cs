using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Assets.Scripts.Input;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public PhotonView view;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    private bool leaving = false;

    private void Start()
    {
        CreateController(0);
    }

    private void Update()
    {
        
        int max_score = -999;
        int min_score = 0;
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject winner_ref = null;
        foreach (GameObject player in players)
        {
            Player player_script = player.GetComponent<Player>();
            if (player_script.getScore() > max_score)
            {
                max_score = player_script.getScore();
                winner_ref = player;
            }
            if (player_script.getScore() < min_score)
            {
                min_score = player_script.getScore();
            }
        }
        if (Mathf.Abs(max_score - min_score) > 3)
        {
            if (winner_ref != null && !leaving)
            {
                winner_ref.GetComponent<Player>().Win();
                EndMatch();
                leaving = true;
            }
               
        }
    }
    void EndMatch()
    {
        view.RPC("RPC_EndMatch", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_EndMatch()
    {
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("setup_screen");
    }
    private void CreateController(int customScore)
    {
        Debug.Log("custom score: " + customScore);
        Vector3 randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        GameObject tmp = PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity, 0, new object[] { view.ViewID });
        tmp.GetComponent<Player>().setScore(customScore);
    }

    public void Die(GameObject obj)
    {
        int cur_score = obj.GetComponent<Player>().getScore();
        cur_score -= 1;
        Debug.Log("cure score: " + cur_score);
        PhotonNetwork.Destroy(obj);
        CreateController(cur_score);
    }
}
