using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;

namespace MainMenu
{
    public class Main_Menu : MonoBehaviourPunCallbacks
    {
        public static MainMenu mainmenu;
        [SerializeField] private TMP_InputField join_input = null, host_input = null, nameInputFeild = null;
        [SerializeField] private Text message=null;
        public Text loading_text;
        public GameObject TurnHost, TurnJoin, loading_OBJ, messageOBJ, ChangeNickNameButton;
        public Button join_button, host_button, continueButton;
        public static MainMenu instance;
        private const string GameVersion = "0.1"; //not the same will not connect together 
        private const int MaxPlayersPerRoom = 6;
        private const string PlayerPrefsNameKey = "PlayerName";
        private bool turnOnButtons=false;
        public GameObject OBJUserName, OBJHostJoin;
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            OnEnable();
        }
        void Start()
        {
            OBJHostJoin.SetActive(false);
            OBJUserName.SetActive(true);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GameVersion;
            }
            SetUpInputFeild();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("We are now connected to " + PhotonNetwork.CloudRegion + "sever!");
            turnOnButtons = true;
            checkInputJoin();
            checkInputHost();
        }
        public void callHostJoin()
        {
            OBJHostJoin.SetActive(true);
            OBJUserName.SetActive(false);
            ChangeNickNameButton.SetActive(false);
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
        //input name start
        private void SetUpInputFeild()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
            string defultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            nameInputFeild.text = defultName;
            SetPlayerName(defultName);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(nameInputFeild.text);
        }
        public void SavePlayerName()
        {
            string playerName = nameInputFeild.text;
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            callHostJoin();
        }

        //input name end


        /// Host functions starts
        public void clickOnHostButton()
        {
            TurnHost.SetActive(false);
            TurnJoin.SetActive(false);
            loading_OBJ.SetActive(true);
            ChangeNickNameButton.SetActive(false);
            CreateRoom(host_input.text);
        }
        public void checkInputHost() =>host_button.interactable = !string.IsNullOrEmpty(host_input.text) && turnOnButtons; // will send to create room

        public void CreateRoom(string roomName)  // create room 
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaxPlayersPerRoom;
            loading_text.text = "Creating Room Named \"" + host_input.text + "\" ";
            Thread.Sleep(500);
            PhotonNetwork.CreateRoom(host_input.text, roomOptions, null);
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            loading_text.text = "Room Named \"" + PhotonNetwork.CurrentRoom.Name + "\" is created...";
            Thread.Sleep(500);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Created room failed " );
            loading_text.text = "Room failed to create";
            Thread.Sleep(500);
            loading_text.text = "The room name has already exsist or the server has issue please try agian...";
            Thread.Sleep(500);
            host_input.text = null;
            TurnHost.SetActive(true);
            TurnJoin.SetActive(true);
            loading_OBJ.SetActive(false);
            ChangeNickNameButton.SetActive(true);
        }
        /// Host functions end 

        /// Join functions starts
        public void clickOnJoinButton()
        {
                ChangeNickNameButton.SetActive(false);
                TurnHost.SetActive(false);
                TurnJoin.SetActive(false);
                loading_OBJ.SetActive(true);
                JoinRoom(join_input.text);
                
        }
        public void checkInputJoin() => join_button.interactable = !string.IsNullOrEmpty(join_input.text) && turnOnButtons;
        public void JoinRoom(string roomName)
        {
            Debug.Log("Joining Room...");
            loading_text.text = "Joining Room Named \"" + join_input.text + "\" ";
            Thread.Sleep(500);
            PhotonNetwork.JoinRoom(roomName);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            loading_text.text = "Joined Room named \"" + PhotonNetwork.CurrentRoom.Name + "\" ";
            Thread.Sleep(500);
            PhotonNetwork.LoadLevel(1);
            Debug.Log("Client is waiting for an opponent");
            Debug.Log("Matching is ready to begin");
            join_input.text = null;
            TurnHost.SetActive(true);
            TurnJoin.SetActive(true);
            loading_OBJ.SetActive(false);

        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        { 
            Debug.Log("Room is not found");
            loading_text.text = "There are no room with the name \"" + join_input.text + "\" currently being hosted";
            Thread.Sleep(500);
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
        public void onClickChageName()
        {
            OBJHostJoin.SetActive(false);
            OBJUserName.SetActive(true);
            ChangeNickNameButton.SetActive(true);
        }
    }
}