using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UserAreas;
using main;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

namespace main
{
    /// <summary>
    /// This is the script to control the chat 
    /// </summary>
    public class ChatController : MonoBehaviour
    {
        /// <summary>
        /// These are all the gameobjects for the chat
        /// </summary>
        [Header("Object for chat")]
        [SerializeField] private GameObject playerDropDownList = null;
        /// <summary>
        /// These are all the gameobjects for the chat
        /// </summary>
        [SerializeField] private GameObject scrollText = null, newMessageSprite = null, chatPopUp = null, chatButtonText = null;
        /// <summary>
        /// The Input text for the chat
        /// </summary>
        [Header("Text input for the chat")]
        [SerializeField] private TMP_InputField messageInput = null;
        /// <summary>
        /// The audio source of message noticification
        /// </summary>
        [Header("Audio source")]
        [SerializeField] private AudioSource MessageNotification = null;
        /// <summary>
        /// The script object of where this script is attatched to.
        /// </summary>
        private GameObject ScriptOBJ = null;
        /// <summary>
        /// Holds the script for UserAreaControlers
        /// </summary>
        private UserAreaControlers userInfos = null;
        /// <summary>
        /// Holds the script for EventHandeler
        /// </summary>
        private EventHandeler eventHandeler = null;
        /// <summary>
        /// The List that holds all the player info
        /// </summary>
        private List<Player> NickNames = new List<Player>();
        /// <summary>
        /// Which player is currently selected for a private message
        /// </summary>
        private Player whichPlayer = null;

        /// <summary>
        /// The number of new text
        /// </summary>
        [HideInInspector]
        public int amountOfNewText = 0;
        /// <summary>
        /// To set the amout of new text.
        /// When a new amount is set it will then call the setNumberOfNewText() function to update the UI
        /// </summary>
        private int setamountOfNewText
        {
            set
            {
                amountOfNewText = value;
                setNumberOfNewText();
            }
            get { return amountOfNewText; }
        }
        /// <summary>
        /// This will update each frames to listen for the key of arrow up and keypadEnter to then call the whenClickOnSendMessage function
        /// </summary>
        void Update()
        {
            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.UpArrow))
            {
                whenClickOnSendMessage();
            }
        }
        /// <summary>
        /// When this script is called this function will run and set up the amount of new text to 0 and the scripts of userInfos and eventHandeler
        /// </summary>
        private void Awake()
        {
            setamountOfNewText = 0;
            ScriptOBJ = gameObject;
            userInfos = ScriptOBJ.GetComponent<UserAreaControlers>();
            eventHandeler = ScriptOBJ.GetComponent<EventHandeler>();
        }
        /// <summary>
        /// This is to set up the dropdown list of other people that are currently in the room
        /// </summary>
        public void setUpDropdownList()
        {
            NickNames.Clear();
            var dropdownPlayer = playerDropDownList.GetComponent<Dropdown>();
            dropdownPlayer.options.Clear();
            dropdownPlayer.options.Add(new Dropdown.OptionData() { text = "All" });
            for (int i = 1; i < 6; i++)
            {
                if (userInfos.users[i].filled == true)
                {
                    NickNames.Add(userInfos.users[i].playerPhoton);
                }
            }
            for (int i = 0; i < NickNames.Count; i++)
            {
                dropdownPlayer.options.Add(new Dropdown.OptionData() { text = NickNames[i].NickName });
            }
            dropdownPlayer.onValueChanged.AddListener(delegate { dropDownChange(dropdownPlayer.value); });
        }
        /// <summary>
        /// This is call when the dropdownvalue is changed.
        /// </summary>
        /// <remarks>
        /// Will set up the value of whichPlayer to send the info
        /// </remarks>
        /// <param name="drowDownValue"> the current value of the dropdown list</param>
        public void dropDownChange(int drowDownValue)
        {
            if (drowDownValue == 0)
            {
                whichPlayer = null;
                Debug.Log("All Player");
            }
            else
            {
                whichPlayer = NickNames[drowDownValue - 1];
                Debug.Log("The player Selected is " + whichPlayer.NickName);
            }
        }
        /// <summary>
        /// This function is called when the send message button is clicked.
        /// </summary>
        public void whenClickOnSendMessage()
        {
            if (!string.IsNullOrEmpty(messageInput.text))
            {
                if (whichPlayer == null)
                {
                    if (messageInput.text[0] == '/')
                    {
                        // for easier testing 
                        if (messageInput.text == "/iquit")
                        {
                            userInfos.subMyCred(userInfos.users[0].amountOfCred);
                        }
                    }
                    else
                    {
                        onReceiveMessage(messageInput.text, PhotonNetwork.LocalPlayer, false);
                        object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, false };
                        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, eventHandeler.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                    }
                }
                else
                {
                    onReceiveMessage(messageInput.text, PhotonNetwork.LocalPlayer, true);
                    object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, true };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, new RaiseEventOptions { TargetActors = new int[] { whichPlayer.ActorNumber } }, SendOptions.SendReliable);
                }
            }
            messageInput.text = null;
        }
        /// <summary>
        /// This is called when a message is received
        /// </summary>
        /// <param name="Message">The message</param>
        /// <param name="sender">The sender info</param>
        /// <param name="isPrivate">And if the messge is private or not</param>
        /// <remarks>
        /// If isPrivate is true = is Private
        /// else                 = not Private
        /// This will increase the amount of new text and play Message Notification
        /// </remarks>
        public void onReceiveMessage(string Message, Player sender, bool isPrivate)
        {
            MessageNotification.Play();
            if (!chatPopUp.activeSelf)
            {
                setamountOfNewText += 1;
            }
            if (sender == null)
            {
                scrollText.GetComponent<Text>().text += "<color=#BF0000><i> <b>" + "Server" + ":" + Message + "</b></i></color>" + "\n";
            }
            else
            {
                if (isPrivate)
                {
                    scrollText.GetComponent<Text>().text += "<color=#E56717><i><b>(Private</b></i></color>";
                    if (sender.IsLocal)
                    {
                        scrollText.GetComponent<Text>().text += "<color=#E56717><i><b> to " + whichPlayer.NickName + ")</b></i></color> ";
                    }
                    else
                    {
                        scrollText.GetComponent<Text>().text += "<color=#E56717><i><b>)</b></i></color> ";
                    }
                }
                if (sender.IsLocal)
                {
                    scrollText.GetComponent<Text>().text += "<color=#008000><i><b>" + sender.NickName + "(YOU)</b>: " + Message + "</i></color>\n";
                }
                else
                {
                    scrollText.GetComponent<Text>().text += "<color=#0A1172><i><b>" + sender.NickName + "</b>: " + Message + "</i></color>\n";
                }
            }
        }
        /// <summary>
        /// This function is to set up the UI of the number of new notifications
        /// </summary>
        public void setNumberOfNewText()
        {
            if (amountOfNewText != 0)
            {
                newMessageSprite.SetActive(true);
                chatButtonText.GetComponent<Text>().text = amountOfNewText.ToString();
            }
            else
            {
                newMessageSprite.SetActive(false);
                chatButtonText.GetComponent<Text>().text = "";
            }
        }
        /// <summary>
        /// This is a button to open/close the message popup button
        /// </summary>
        public void clickOnMessageButton()
        {
            setamountOfNewText = 0;
            chatPopUp.SetActive(!chatPopUp.activeSelf);
        }
    }
}
