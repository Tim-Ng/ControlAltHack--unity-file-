﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Avertars;
using Music;
using System.Collections.Generic;

namespace MainMenu
{
    /// <summary>
    /// This class is to setup photon add your nickname and join or host room
    /// </summary>
    struct RegionName
    {
        public string Name, Key;
        public RegionName(string Name,string Key)
        {
            this.Name = Name;
            this.Key = Key;
        }
    }
    public class NickNameRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField join_input = null, host_input = null, nameInputFeild = null;
        [SerializeField] private GameObject HostJoin = null, Nickname = null, Status = null, message = null, reconnectToInternet = null, avertarSelectedOBJ = null;
        [SerializeField] private GameObject dropDownForRegion = null;
        [SerializeField] private AvertarList avertarList = null;
        [SerializeField] private SliderPopUp sliderPopUp = null;
        private bool connected = false;
        private string currentAvertar = null;
        private readonly List<RegionName> regionList = new List<RegionName>() { 
            new RegionName("Melbourne","au"), 
            //new RegionName("Montreal","cae"), 
            //new RegionName("Shanghai","cn"), 
            //new RegionName("Amsterdam","eu"), 
            new RegionName("Chennai","in"), 
            new RegionName("Tokyo","jp"), 
            //new RegionName("Moscow","ru"), 
            //new RegionName("Khabarovsk","rue"), 
            // new RegionName("Johannesburg","za"), 
            //new RegionName("Sao Paulo","sa"),
            new RegionName("Singapore","asia"),
            new RegionName("Seoul","kr"),
            new RegionName("Washington D.C.","us"),
            new RegionName("San José","usw")
        };
        public string setStatus
        {
            set { Status.GetComponent<Text>().text = "Status: " + value; }
        }
        [SerializeField] private Button join_button = null, host_button = null, continueButton = null;
        public static byte MinimumPeople = 2, MaximumPeople = 6;
        private string regionSelected = "";
        private const string PlayerPrefsNameKey = "PlayerName";
        private const string regionKey = "regionKey";
        private const string avertarCode = "AvertarCode";
        private const string GameVersion = "0.1";
        /// <summary>
        /// Start set nickname.
        /// </summary>
        void Start()
        {
            PhotonNetwork.Disconnect();
            GlobalMusicContorler.duringStart();
            sliderPopUp.startSlider();
            Nickname.SetActive(true);
            HostJoin.SetActive(false);
            SetUpRegionSelector();
            checkInternet();
            SetUpInputFeild();
            if (!PlayerPrefs.HasKey(avertarCode))
            {
                currentAvertar = "0";
                PlayerPrefs.SetString(avertarCode, currentAvertar);
            }
            else
            {
                currentAvertar = PlayerPrefs.GetString(avertarCode);
            }
            if (!(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(avertarCode)))
                PhotonNetwork.LocalPlayer.CustomProperties.Add(avertarCode, currentAvertar);
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
                    if (PlayerPrefs.HasKey(regionKey))
                    {
                        regionSelected = PlayerPrefs.GetString(regionKey);
                        PhotonNetwork.PhotonServerSettings.DevRegion = regionSelected;
                    }
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
        public void ConnectToRegion()
        {
            if (PhotonNetwork.CloudRegion != regionSelected)
            {
                continueButton.interactable = false;
                PhotonNetwork.Disconnect();
                PhotonNetwork.ConnectToRegion(regionSelected);
            }
        }
        internal void SetUpRegionSelector()
        {
            Debug.Log("Set up Region nnnnn ");
            Dropdown dropComp = dropDownForRegion.GetComponent<Dropdown>();
            dropComp.interactable = false;
            dropComp.options.Clear();
            for (int i = 0; i < regionList.Count; i++)
            {
                dropComp.options.Add(new Dropdown.OptionData() { text = regionList[i].Name });
            }
            dropComp.RefreshShownValue();
            dropComp.onValueChanged.AddListener(delegate { regionSelected = regionList[dropComp.value].Key; ConnectToRegion(); });
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
            PlayerPrefs.SetString(regionKey, regionSelected);
            setStatus = " Connected to Server";
            dropDownForRegion.GetComponent<Dropdown>().interactable = true;
            continueButton.interactable = true;
            checkInputJoin();
            checkInputHost();
            checkInputNickname();
            for (int i = 0; i < regionList.Count; i++)
            {
                if (regionList[i].Key == PhotonNetwork.CloudRegion) dropDownForRegion.GetComponent<Dropdown>().value = i;
            }
        }
        public void checkInputNickname() => continueButton.interactable = !string.IsNullOrEmpty(nameInputFeild.text) && connected;
        public void checkInputHost() => host_button.interactable = !string.IsNullOrEmpty(host_input.text) && connected;
        public void checkInputJoin() => join_button.interactable = !string.IsNullOrEmpty(join_input.text) && connected;

        //Start Hosting room functions 
        public void clickOnHostButton()
        {
            HostJoin.SetActive(false);
            CreateRoom(TrimWords(host_input.text));
        }
        public void CreateRoom(string roomName)  // create room 
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaximumPeople;
            setStatus = "Creating Room Named \"" + host_input.text + "\" ";
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
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
            setStatus = "The room name has already exsist or the server has issue please try agian...";
            host_input.text = null;
            HostJoin.SetActive(true);
        }
        //End of Hosting room functions 

        //Start Joining room functions 
        public void clickOnJoinButton()
        {
            HostJoin.SetActive(false);
            JoinRoom(TrimWords(join_input.text));
        }
        public void JoinRoom(string roomName)
        {
            Debug.Log("Joining Room...");
            setStatus = "Joining Room Named \"" + join_input.text + "\" ";
            PhotonNetwork.JoinRoom(roomName);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            setStatus = "Joined Room named \"" + PhotonNetwork.CurrentRoom.Name + "\" ";
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
            Debug.Log("Change Avertar");
            currentAvertar = which;
            PlayerPrefs.SetString(avertarCode, currentAvertar);
            PhotonNetwork.LocalPlayer.CustomProperties[avertarCode] = which;
            avertarList.onClickClosePopup();
            setImageCharOBJ();
        }
        private string TrimWords(String stringToTrim)
        {
            bool HeadCount = false;
            int Start = 0, End = 0;
            for (int i = 0; i < stringToTrim.Length; i++)
            {
                if (stringToTrim[i]!=' ')
                {
                    if (!HeadCount)
                    {
                        Start = i;
                        HeadCount = true;
                    }
                    End = i;
                }
            }
            return stringToTrim.Substring(Start, (End==Start ?1:End - Start));
        }
    }
    
}