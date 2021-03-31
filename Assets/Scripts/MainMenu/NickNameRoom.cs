using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Avatars;
using Music;
using System.Collections.Generic;

namespace MainMenu
{
    /// <summary>
    /// This is to hold the value of region values
    /// </summary>
    struct RegionName
    {
        /// <summary>
        /// This is the name of the country
        /// </summary>
        public string Name;
        /// <summary>
        /// This is the key to the server
        /// </summary>
        public string Key;
        /// <summary>
        /// This is a constructor to set up the data for this struct
        /// </summary>
        /// <param name="Name"> The name of the country</param>
        /// <param name="Key"> The key to find the server</param>
        public RegionName(string Name, string Key)
        {
            this.Name = Name;
            this.Key = Key;
        }
    }
    /// <summary>
    /// This is the class to control the NickName&JoinHost scene 
    /// </summary>
    public class NickNameRoom : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// This is the input feild for the join input 
        /// </summary>
        [SerializeField, Header("Input Fields")] private TMP_InputField join_input = null;
        /// <summary>
        /// This is the input feild for the host input 
        /// </summary>
        [SerializeField] private TMP_InputField host_input = null;
        /// <summary>
        /// This is the input feild for the Nickname input 
        /// </summary>
        [SerializeField] private TMP_InputField nameInputFeild = null;

        /// <summary>
        /// The game object to holds all the elements on for the host join
        /// </summary>
        [SerializeField, Header("HostJoin Or NickName")] private GameObject HostJoin = null;
        /// <summary>
        /// The game object to holds all the elements on for the NickNames
        /// </summary>
        [SerializeField] private GameObject Nickname = null;
        /// <summary>
        /// This is game object of the UI for the current status
        /// </summary>
        [SerializeField] private GameObject Status = null;
        /// <summary>
        /// This is message stating the persons nick name during host join
        /// </summary>
        [SerializeField] private GameObject message = null;
        /// <summary>
        /// This is the avertar object to hold the current avertar image and also to open the pop up to choose the avertar
        /// </summary>
        [SerializeField] private GameObject avertarSelectedOBJ = null;
        /// <summary>
        /// The game object to the dropdown to select region
        /// </summary>
        [SerializeField] private GameObject dropDownForRegion = null;
        /// <summary>
        /// The text for the ping
        /// </summary>
        [SerializeField] private GameObject pingOBJ = null;
        /// <summary>
        /// The value to check if connected to the wifi or not
        /// </summary>
        /// <remarks>
        /// True = connected <br/>
        /// False = not connected 
        /// </remarks>
        private bool connected = false;
        /// <summary>
        /// The current avertar that the player has chosed 
        /// </summary>
        private string currentAvatar = null;
        /// <summary>
        /// This is the list of the regions that can be connected to 
        /// </summary>
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
        /// <summary>
        /// set the status UI text
        /// </summary>
        public string setStatus
        {
            set { Status.GetComponent<Text>().text = "Status: " + value; }
        }
        /// <summary>
        /// To set the UI of the ping to the server 
        /// </summary>
        public int setPing
        {
            set
            {
                string colourPing = "";
                if (value < 100)
                {
                    colourPing = "<color=#0B6623>" + value.ToString() + "</color>";
                }
                else if (value < 300)
                {
                    colourPing = "<color=#FFF200>" + value.ToString() + "</color>";
                }
                else
                {
                    colourPing = "<color=#D30000>" + value.ToString() + "</color>";
                }
                pingOBJ.GetComponent<Text>().text = "Current Ping:~" + colourPing + " ms";
            }
        }
        /// <summary>
        /// The button for joining
        /// </summary>
        [SerializeField,Header("Buttons")] private Button join_button = null;
        /// <summary>
        /// The button for hosting 
        /// </summary>
        [SerializeField] private Button host_button = null;
        /// <summary>
        /// The button for continuing
        /// </summary>
        [SerializeField] private Button continueButton = null;
        /// <summary>
        /// This is the button to reconnecting to the server
        /// </summary>
        [SerializeField] private GameObject reconnectToInternet = null;
        /// <summary>
        /// The script for the avartarList
        /// </summary>
        [SerializeField,Header("Scripts")] private AvatarList avartarList = null;
        /// <summary>
        /// The script for the SliderPopUp 
        /// </summary>
        [SerializeField] private SliderPopUp sliderPopUp = null;
        /// <summary>
        /// The minimum nunmber of people
        /// </summary>
        public static byte MinimumPeople = 2;
        /// <summary>
        /// The maximium nunmber of people
        /// </summary>
        public static byte MaximumPeople = 6;
        /// <summary>
        /// The current region selected
        /// </summary>
        private string regionSelected = "";
        /// <summary>
        /// The key for the Player Name preference
        /// </summary>
        private const string PlayerPrefsNameKey = "PlayerName";
        /// <summary>
        /// The key for the region key preference
        /// </summary>
        private const string regionKey = "regionKey";
        /// <summary>
        /// The key for the avatarCode preference
        /// </summary>
        private const string avatarCode = "AvatarCode";
        /// <summary>
        /// The GameVersion control
        /// </summary>
        private const string GameVersion = "0.1";
        /// <summary>
        /// This is for the pingTimer
        /// </summary>
        private float pingTimer = 0.0f;
        /// <summary>
        /// This function will run when the script is rendered
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
            if (!PlayerPrefs.HasKey(avatarCode))
            {
                currentAvatar = "0";
                PlayerPrefs.SetString(avatarCode, currentAvatar);
            }
            else
            {
                currentAvatar = PlayerPrefs.GetString(avatarCode);
            }
            if (!(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(avatarCode)))
                PhotonNetwork.LocalPlayer.CustomProperties.Add(avatarCode, currentAvatar);
            setImageCharOBJ();
        }
        /// <summary>
        /// This function will be called every frame and is for the ping
        /// </summary>
        void Update()
        {
            if (pingTimer > 3 && PhotonNetwork.IsConnectedAndReady)
            {
                setPing = PhotonNetwork.GetPing();
                pingTimer = 0;
            }
            pingTimer += Time.deltaTime;
        }
        /// <summary>
        /// This is to check the internet connection
        /// </summary>
        /// <remarks>
        /// If haven't connected to the server then it will check the wifi connection <br/>
        /// If there is wifi then will connect to server <br/>
        /// If no wifi then will open the reconnectToInternet button
        /// </remarks>
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
        /// <summary>
        /// This is to connect to another region when the a dropdown is selected
        /// </summary>
        public void ConnectToRegion()
        {
            if (PhotonNetwork.CloudRegion != regionSelected)
            {
                continueButton.interactable = false;
                PhotonNetwork.Disconnect();
                PhotonNetwork.ConnectToRegion(regionSelected);
            }
        }
        /// <summary>
        /// This is to set up the dropdown to select the regions
        /// </summary>
        internal void SetUpRegionSelector()
        {
            Debug.Log("Set up Region nnnnn ");
            Dropdown dropComp = dropDownForRegion.GetComponent<Dropdown>();
            dropComp.interactable = false;
            dropComp.options.Clear();
            for (int i = 0; i < regionList.Count; i++)
            {
                dropComp.options.Add(new Dropdown.OptionData() { text = regionList[i].Name + " [" + regionList[i].Key + "]" });
            }
            dropComp.RefreshShownValue();
            dropComp.onValueChanged.AddListener(delegate { regionSelected = regionList[dropComp.value].Key; ConnectToRegion(); });
        }
        /// <summary>
        /// This is to set up the input of the nickname and set it to the prefeb nickname
        /// </summary>
        private void SetUpInputFeild()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
            string defultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            nameInputFeild.text = defultName;
            checkInputNickname();
        }
        /// <summary>
        /// This is when the continue button is pressed on. This will set the prefeb and show the HostJoin object and close the Nickname object
        /// </summary>
        public void SavePlayerName()
        {
            string playerName = nameInputFeild.text.Trim();
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            PhotonNetwork.GameVersion = GameVersion;
            HostJoin.SetActive(true);
            Nickname.SetActive(false);
            if (IfAllSpaceOrEmpty(PhotonNetwork.NickName))
            {
                Debug.Log("Name is null... loading main screen");
                PhotonNetwork.LoadLevel(0);
            }
            message.GetComponent<Text>().text = "Hi \"" + PhotonNetwork.NickName+"\"";
        }
        /// <summary>
        /// This function is called when we connected to the master server
        /// </summary>
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
            setPing = PhotonNetwork.GetPing();
            pingTimer = 0;
        }
        /// <summary>
        /// When there is a change with the input of nameInputFeild
        /// </summary>
        /// <remarks>
        /// If nameInputFeild != null then continueButton.interactable = true && connected <br/>
        /// If nameInputFeild == null then continueButton.interactable = false && connected <br/>
        /// </remarks>
        public void checkInputNickname() => continueButton.interactable = !IfAllSpaceOrEmpty(nameInputFeild.text) && connected;
        /// <summary>
        /// When there is a change with the input of host_input
        /// </summary>
        /// <remarks>
        /// If host_input != null then host_button.interactable = true && connected <br/>
        /// If host_input == null then host_button.interactable = false && connected <br/>
        /// </remarks>
        public void checkInputHost() => host_button.interactable = !IfAllSpaceOrEmpty(host_input.text) && connected;
        /// <summary>
        /// When there is a change with the input of join_input
        /// </summary>
        /// <remarks>
        /// If join_input != null then join_button.interactable = true && connected <br/>
        /// If join_input == null then join_button.interactable = false && connected <br/>
        /// </remarks>
        public void checkInputJoin() => join_button.interactable = !IfAllSpaceOrEmpty(join_input.text) && connected;

        /// <summary>
        /// This is when the host button is clicked on
        /// </summary>
        public void clickOnHostButton()
        {
            HostJoin.SetActive(false);
            CreateRoom(host_input.text.Trim());
        }
        //start of Hosting room functions 
        /// <summary>
        /// This function is used to create a new room
        /// </summary>
        /// <param name="roomName">The name of the room</param>
        public void CreateRoom(string roomName)  // create room 
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaximumPeople;
            setStatus = "Creating Room Named \"" + host_input.text + "\" ";
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        }
        /// <summary>
        /// Function is called when the room is created
        /// </summary>
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            setStatus = "Room Named \"" + PhotonNetwork.CurrentRoom.Name + "\" is created...";
        }
        /// <summary>
        /// Function is called when the room is not created
        /// </summary>
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
        /// <summary>
        /// This function is called when the join button is click on 
        /// </summary>
        public void clickOnJoinButton()
        {
            HostJoin.SetActive(false);
            JoinRoom(join_input.text.Trim());
        }
        /// <summary>
        /// This function is used to join the room
        /// </summary>
        /// <param name="roomName"></param>
        public void JoinRoom(string roomName)
        {
            Debug.Log("Joining Room...");
            setStatus = "Joining Room Named \"" + join_input.text + "\" ";
            PhotonNetwork.JoinRoom(roomName);
        }
        /// <summary>
        /// This function is called when the player joined a room
        /// </summary>
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
        /// <summary>
        /// This function is called when joining a room has failed
        /// </summary>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Room is not found");
            setStatus = "There are no room with the name \"" + join_input.text + "\" currently being hosted";
            join_input.text = null;
            HostJoin.SetActive(true);
        }
        //End Joining room functions 
        /// <summary>
        /// This function is called when a new player has entered the room
        /// </summary>
        /// <param name="newPlayer"> The info of the new player</param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaximumPeople)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Room FULL");
            }
        }
        /// <summary>
        /// This function is called when the player has disconnected to the photonNetwork
        /// </summary>
        /// <param name="cause"> This is the disconnetion cause </param>
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to : {cause}");
        }
        /// <summary>
        /// This is called when the button to change NickName is pressed
        /// </summary>
        public void onClickChageName()
        {
            Nickname.SetActive(true);
            HostJoin.SetActive(false);
        }
        /// <summary>
        /// This function is used to change the avertar sprite on the object avertarSelectedOBJ
        /// </summary>
        public void setImageCharOBJ()
        {
            avertarSelectedOBJ.GetComponent<Image>().sprite = AvatarList.AvatarLists[int.Parse(currentAvatar)];
        }
        /// <summary>
        /// This is used to change to a new avertar 
        /// </summary>
        /// <param name="which"> The name of the new avertar </param>
        public void setCharacter(string which)
        {
            Debug.Log("Change Avertar");
            currentAvatar = which;
            PlayerPrefs.SetString(avatarCode, currentAvatar);
            PhotonNetwork.LocalPlayer.CustomProperties[avatarCode] = which;
            avartarList.onClickClosePopup();
            setImageCharOBJ();
        }
        /// <summary>
        /// This function is used to check if the input is empty or only contains space
        /// </summary>
        /// <param name="stringToCheck">The sting to check</param>
        /// <returns>Returns true if empty or only contains space </returns>
        internal bool IfAllSpaceOrEmpty(string stringToCheck)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                return true;
            }
            else
            {
                for(int i = 0; i < stringToCheck.Length; i++)
                {
                    if (stringToCheck[i] != ' ')
                        return false;
                }
                return true;
            }
        }

    }

}