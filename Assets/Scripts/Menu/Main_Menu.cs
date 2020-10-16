using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class Main_Menu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField join_input = null, host_input = null;
        [SerializeField] private Text message;
        public Text loading_text;
        public GameObject TurnHost, TurnJoin, loading_OBJ, messageOBJ;
        public Button join_button, host_button;
        public static MainMenu instance;
        private const string GameVersion = "0.1"; //not the same will not connect together 
        private const int MaxPlayersPerRoom = 6;
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            checkInputJoin();
            checkInputHost();
            PhotonNetwork.GameVersion = GameVersion;
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                Debug.Log("Name is null... loading main screen");
                PhotonNetwork.LoadLevel(0);
            }
            message.text = "Hi " + PhotonNetwork.NickName;
        }
        /// Host functions starts
        public void clickOnHostButton()
        {
            TurnHost.SetActive(false);
            TurnJoin.SetActive(false);
            loading_OBJ.SetActive(true);
            CreateRoom(host_input.text);
        }
        public void checkInputHost() => host_button.interactable = !string.IsNullOrEmpty(host_input.text); // will send to create room

        public void CreateRoom(string roomName)  // create room 
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaxPlayersPerRoom;
            loading_text.text = "Creating Room Named \"" + host_input.text + "\" ";
            PhotonNetwork.CreateRoom(host_input.text, roomOptions, null);
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            loading_text.text = "Room Named \"" + PhotonNetwork.CurrentRoom.Name + "\" is created...";
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            /*Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            loading_text.text = "Room Named \"" + PhotonNetwork.CurrentRoom.Name + "\" failed to create";
            Task.Delay(5000);
            loading_text.text = "The room name has already exsist or the server has issue please try agian...";
            Task.Delay(5000);
            host_input = null;
            TurnHost.SetActive(true);
            TurnJoin.SetActive(true);
            loading_OBJ.SetActive(false);*/
        }
        /// Host functions end 

        /// Join functions starts
        public void JoinRoom(string roomName)
        {
            Debug.Log("Joining Room...");
            loading_text.text = "Joining Room Named \"" + join_input.text + "\" ";
            PhotonNetwork.JoinRoom(roomName);
        }
        public void clickOnJoinButton() 
        {
            TurnHost.SetActive(false);
            TurnJoin.SetActive(false);
            loading_OBJ.SetActive(true);
            JoinRoom(join_input.text);
        }
        public void checkInputJoin() => join_button.interactable = !string.IsNullOrEmpty(join_input.text);
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCount != MaxPlayersPerRoom)
            {
                loading_text.text = "Joined Room named \"" + PhotonNetwork.CurrentRoom.Name + "\" ";
                SceneManager.LoadScene(2);
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                //waitingStatusText.text = "Opponent Found";
                SceneManager.LoadScene("OtherSceneName", LoadSceneMode.Additive);
                Debug.Log("Matching is ready to begin");
                join_input.text = null;
                TurnHost.SetActive(true);
                TurnJoin.SetActive(true);
                loading_OBJ.SetActive(false);
            }
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"Room is not found");
            loading_text.text = "There are no room with the name \""+ join_input.text + "\" currently being hosted";
            join_input.text = null;
            TurnHost.SetActive(true);
            TurnJoin.SetActive(true);
            loading_OBJ.SetActive(false);
        }
        /// Join functions ends 
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                //waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to : {cause}");
        }

        public void ChangesScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }

        
    }
}