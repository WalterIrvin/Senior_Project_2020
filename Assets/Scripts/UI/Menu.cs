using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviourPunCallbacks
{
    bool allow_start = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        allow_start = true;
        Debug.Log("Server join successful.");
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("setup_screen");
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if (allow_start) 
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.Log("Not connected yet...");
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
