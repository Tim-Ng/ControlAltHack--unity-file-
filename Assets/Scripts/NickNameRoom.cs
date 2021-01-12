using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Avertars;

namespace MainMenu
{
    /// <summary>
    /// This class is to setup photon add your nickname and join or host room
    /// </summary>
    public class NickNameRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField join_input = null, host_input = null, nameInputFeild = null;
        [SerializeField] private GameObject HostJoin=null, Nickname=null, Status = null, message = null,reconnectToInternet = null,avertarSelectedOBJ = null,selectPopup = null;
        private bool connected = false;
        private string currentAvertar = "0";
        public string setStatus
        {
            set { Status.GetComponent<Text>().text = "Status: " + value; }
        }
        [SerializeField] private Button join_button = null, host_button = null, continueButton = null;
        public static byte MinimumPeople = 2, MaximumPeople = 6;
        private const string PlayerPrefsNameKey = "PlayerName";
        private const string GameVersion = "0.1";
        /// <summary>
        /// Start set nickname.
        /// </summary>
        private void Start()
        {
            Nickname.SetActive(true);
            HostJoin.SetActive(false);
            checkInternet();
            SetUpInputFeild();
            PhotonNetwork.LocalPlayer.CustomProperties.Add("AvertarCode", currentAvertar);
            setImageCharOBJ();
        }
        public void checkInternet()
        {
            if (connected == false)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    Debug.Log("Network Available");
                    reconnectToInternet.SetActive(false);
                    PhotonNetwork.ConnectUsingSettings();
                    PhotonNetwork.GameVersion = GameVersion;
                }
                else
                {
                    Debug.Log("Network Not Available");
                    reconnectToInternet.SetActive(true);
                    setStatus = "No wifi...Click Button To reconnect";
                }
            }
        }
        private void SetUpInputFeild()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
            string defultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            nameInputFeild.text = defultName;
            checkInputNickname();
        }
        public void SavePlayerName()
        {
            string playerName = nameInputFeild.text;
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            PhotonNetwork.GameVersion = GameVersion;
            HostJoin.SetActive(true);
            Nickname.SetActive(false);
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                Debug.Log("Name is null... loading main screen");
                PhotonNetwork.LoadLevel(0);
            }
            message.GetComponent<Text>().text = "Hi " + PhotonNetwork.NickName;
        }
        public override void OnConnectedToMaster()
        {
            connected = true;
            Debug.Log("We are now connected to " + PhotonNetwork.CloudRegion + "sever!");
            setStatus = " Connected to Server";
            checkInputJoin();
            checkInputHost();
            checkInputNickname();
        }
        public void checkInputNickname() => continueButton.interactable = !string.IsNullOrEmpty(nameInputFeild.text) && connected;
        public void checkInputHost() => host_button.interactable = !string.IsNullOrEmpty(host_input.text) && connected;
        public void checkInputJoin() => join_button.interactable = !string.IsNullOrEmpty(join_input.text) && connected;

        //Start Hosting room functions 
        public void clickOnHostButton()
        {
            HostJoin.SetActive(false);
            Thread.Sleep(500);
            CreateRoom(host_input.text);
        }
        public void CreateRoom(string roomName)  // create room 
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaximumPeople;
            setStatus = "Creating Room Named \"" + host_input.text + "\" ";
            PhotonNetwork.CreateRoom(host_input.text, roomOptions, null);
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            setStatus = "Room Named \"" + PhotonNetwork.CurrentRoom.Name + "\" is created...";
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Created room failed ");
            setStatus = "Room failed to create";
            Thread.Sleep(500);
            setStatus = "The room name has already exsist or the server has issue please try agian...";
            Thread.Sleep(500);
            host_input.text = null;
            HostJoin.SetActive(true);
        }
        //End of Hosting room functions 

        //Start Joining room functions 
        public void clickOnJoinButton()
        {
            HostJoin.SetActive(false);
            JoinRoom(join_input.text);
        }
        public void JoinRoom(string roomName)
        {
            Debug.Log("Joining Room...");
            setStatus = "Joining Room Named \"" + join_input.text + "\" ";
            Thread.Sleep(500);
            PhotonNetwork.JoinRoom(roomName);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            setStatus = "Joined Room named \"" + PhotonNetwork.CurrentRoom.Name + "\" ";
            Thread.Sleep(500);
            PhotonNetwork.LoadLevel(1);
            Debug.Log("Client is waiting for an opponent");
            Debug.Log("Matching is ready to begin");
            join_input.text = null;
            HostJoin.SetActive(true);
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Room is not found");
            setStatus = "There are no room with the name \"" + join_input.text + "\" currently being hosted";
            Thread.Sleep(500);
            join_input.text = null;
            HostJoin.SetActive(true);
        }
        //End Joining room functions 
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaximumPeople)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Room FULL");
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to : {cause}");
        }
        public void onClickChageName()
        {
            Nickname.SetActive(true);
            HostJoin.SetActive(false);
        }
        public void setImageCharOBJ()
        {
            avertarSelectedOBJ.GetComponent<Image>().sprite = AvertarList.AvertarLists[int.Parse(currentAvertar)];
        }
        public void setCharacter(string which)
        {
            currentAvertar = which;
            PhotonNetwork.LocalPlayer.CustomProperties["AvertarCode"] = which;
            onClickClosePopup();
            setImageCharOBJ();
        }
        public void onClickPopupSelect()
        {
            selectPopup.SetActive(true);
        }
        public void onClickClosePopup()
        {
            selectPopup.SetActive(false);
        }
    }
    
}