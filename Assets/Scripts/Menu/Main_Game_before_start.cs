using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class Main_Game_before_start : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject startButtonOBJ;
    public Text player_username,opponent1_username, opponent2_username, opponent3_username, opponent4_username, opponent5_username,Roomtext;
    List<string> playerNames = new List<string> ();
    private void Start()
    {
        startButtonOBJ.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
            startButtonOBJ.SetActive(true);
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;
        player_username.text = PhotonNetwork.NickName;

    }

    /*private void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log ( "Player " + newPlayer.NickName +"has entered");        
    }*/
    private void OnPlayerExitRoom(Photon.Realtime.Player leftplayer)
    {
        if (leftplayer.IsMasterClient)
        {
            
        }
        Debug.Log("Player " + leftplayer.NickName + "has leave the room");
    }



}
