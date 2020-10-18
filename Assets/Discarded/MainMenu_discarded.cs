using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace MainMenu
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1"; //not the same will not connect together 
        private const int MaxPlayersPerRoom = 6;

        private void Awake() =>PhotonNetwork.AutomaticallySyncScene = true;

        public void FindOpponent()
        {
            isConnecting = true;
            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);

            Debug.Log($"Disconnected due to : {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"No clients are waiting for an opponent,creating a new room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCount != MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Matching is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
                PhotonNetwork.LoadLevel("Scene_Login_Username");
            }
        }

        public static implicit operator MainMenu(Main_Menu v)
        {
            throw new NotImplementedException();
        }
    }
}

/* indian guy 
 [SerializeField] private GameObject connectScreen;
        [SerializeField] private TMP_InputField JoinRoomInput=null, CreatedRoomInput=null;
        [SerializeField] private Button join_button, host_button;
        private const string GameVersion = "0.1"; //not the same will not connect together 
        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Master Lobby Connected");
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("LobbyType Connected");
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("1");
        }

        public void onclick_JoinRoom()
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = 6;
            PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, room, TypedLobby.Default);

        }
        public void onclick_CreateRoom()
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = 6;
            PhotonNetwork.CreateRoom(CreatedRoomInput.text, room, null);
        }
        public void join_button_interatable()
        {
            join_button.interactable = !string.IsNullOrEmpty(JoinRoomInput.text);
        }
        public void host_button_interatable()
        {
            host_button.interactable = !string.IsNullOrEmpty(CreatedRoomInput.text);
        }




///temp put side 
///void Start()
        {
            checkInputJoin();
            checkInputHost();
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
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
        public void ChangesScene (string sceneName)
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
 
 */