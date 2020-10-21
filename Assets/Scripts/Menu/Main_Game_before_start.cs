using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class Main_Game_before_start : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject startButtonOBJ=null;
    public Text player_username,opponent1_username, opponent2_username, opponent3_username, opponent4_username, opponent5_username,Roomtext;
    public Text amount_of_people;
    private int minmum_amount_of_people = 2;  // minimum amount of players
    public Button Start_Button,Leave_Button;
    List<Text> textOtherPlayers = new List<Text> ();
    private RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.All
    };
    public enum PhotonEventCode
    {
        UpdatePlayer_event = 0,
    }
    

    private void Start()
    {
        textOtherPlayers.Add(opponent1_username);
        textOtherPlayers.Add(opponent2_username);
        textOtherPlayers.Add(opponent3_username);
        textOtherPlayers.Add(opponent4_username);
        textOtherPlayers.Add(opponent5_username);
        startButtonOBJ.SetActive(false);
        allow_Master_client();
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;
        player_username.text = PhotonNetwork.NickName;
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdatePlayer_event, null, AllOtherThanMePeopleOptions, SendOptions.SendUnreliable);
    }
    private void OnEnable()
    {
        Debug.Log("Listen to event");
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        Debug.Log("Event heard");
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        Debug.Log("Event Ended");
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)PhotonEventCode.UpdatePlayer_event)
        {
            UpdateName();
        }
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
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Player Left room");
        PhotonNetwork.LoadLevel(0);
    }
}
