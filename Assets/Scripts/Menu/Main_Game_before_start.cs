using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;


public class Main_Game_before_start : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject startButtonOBJ=null;
    public Text player_username,opponent1_username, opponent2_username, opponent3_username, opponent4_username, opponent5_username,Roomtext;
    public Text amount_of_people;
    private int minmum_amount_of_people = 2;  // minimum amount of players
    public Button Start_Button,Leave_Button;
    List<Text> textOtherPlayers = new List<Text> ();
    private PhotonView PV;


    private void Start()
    {
        textOtherPlayers.Add(opponent1_username);
        textOtherPlayers.Add(opponent2_username);
        textOtherPlayers.Add(opponent3_username);
        textOtherPlayers.Add(opponent4_username);
        textOtherPlayers.Add(opponent5_username);
        PV = GetComponent<PhotonView>();
        startButtonOBJ.SetActive(false);
        allow_Master_client();
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;
        player_username.text = PhotonNetwork.NickName;
        PV.RPC("UpdateName", RpcTarget.All);
    }
    private void allow_Master_client()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButtonOBJ.SetActive(true);
            Start_Button.interactable = false;
        }
    }
    [PunRPC]
    public void UpdateName()
    {
        allow_Master_client(); // check if master client changed
        int i = 0;
        for (int j = 0; j < textOtherPlayers.Count; j++)
        {
            textOtherPlayers[j].text = "Waiting for Player ";
        }
        Debug.Log("A player entered in put name and recount of people");
        foreach (Player otherplayer in PhotonNetwork.PlayerListOthers)
        {
            textOtherPlayers[i].text = otherplayer.NickName;
            i++;
        }
        Start_Button.interactable = !(PhotonNetwork.CurrentRoom.PlayerCount<minmum_amount_of_people);
        amount_of_people.text = (PhotonNetwork.CurrentRoom.PlayerCount).ToString() + "/6";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.IsMasterClient)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
            Debug.Log("Host named " + otherPlayer.NickName + " has left the room");
            Debug.Log("Host is changed to player named " + PhotonNetwork.PlayerList[0].NickName);
        }
        Debug.Log(otherPlayer.NickName + " has left room");
        UpdateName();
    }
    public void clickOnLeaveGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
        }
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Player Left room");
        PhotonNetwork.LoadLevel(0);
    }
}
