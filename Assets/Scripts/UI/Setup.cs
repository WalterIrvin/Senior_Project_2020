using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Setup : MonoBehaviourPunCallbacks
{
    string create_room = "default";
    string join_room = "default";
    
    public void updateCreateRoom(string input)
    {
        create_room = input;
    }

    public void updateJoinRoom(string input)
    {
        join_room = input;
    }

    public void createRoom()
    {
        PhotonNetwork.CreateRoom(create_room);
    }

    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(join_room);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("main");
    }
}
