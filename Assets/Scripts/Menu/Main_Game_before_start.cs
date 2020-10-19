using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class Main_Game_before_start : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject startButtonOBJ;
    public Text player_username,opponent1_username, opponent2_username, opponent3_username, opponent4_username, opponent5_username,Roomtext;
    List<string> playerNames = new List<string> ();
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
        if (PhotonNetwork.IsMasterClient)
            startButtonOBJ.SetActive(true);
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;
        player_username.text = PhotonNetwork.NickName;
        PV.RPC("UpdateName", RpcTarget.All);
    }
    [PunRPC]
    public void UpdateName()
    {
        int i=0;
        foreach (Photon.Realtime.Player otherplayer in PhotonNetwork.PlayerListOthers)
        {
            textOtherPlayers[i].text = otherplayer.NickName;
            i++;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player Leave room");
        UpdateName();
    }
}
