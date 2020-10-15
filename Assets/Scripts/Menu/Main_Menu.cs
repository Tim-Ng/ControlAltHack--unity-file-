using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainMenu
{
    public class Main_Menu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField join_input = null, host_input = null;
        [SerializeField] private Text message;
        public Button join_button, host_button;
        public static MainMenu instance;
        private const string GameVersion = "0.1"; //not the same will not connect together 
        private const int MaxPlayersPerRoom = 6;
        void Start()
        {
            checkInputJoin();
            checkInputHost();
            PhotonNetwork.GameVersion = GameVersion;
            message.text = "Hi " + PhotonNetwork.NickName;
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Master connected to master");
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to : {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"No clients are waiting for an opponent,try another code or host room");
            join_input.text = null;
        }
        public void CreateRoom(string roomName)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaxPlayersPerRoom;
            PhotonNetwork.CreateRoom(host_input.text, roomOptions, null);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCount != MaxPlayersPerRoom)
            {
                //waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                //waitingStatusText.text = "Opponent Found";
                Debug.Log("Matching is ready to begin");
                ChangesScene("MainGameStart");
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                //waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }
        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        public void ChangesScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }


        public void clickOnJoinButton()
        {
            JoinRoom(join_input.text);
        }
        public void checkInputJoin()
        {
            join_button.interactable = !string.IsNullOrEmpty(join_input.text);
        }

        public void clickOnHostButton()
        {
            CreateRoom(host_input.text);
        }
        public void checkInputHost()
        {
            host_button.interactable = !string.IsNullOrEmpty(host_input.text);
        }
    }
}