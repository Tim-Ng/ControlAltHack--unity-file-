using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrawCards;
using TMPro;
using UserAreas;

namespace TradeScripts
{
    /// <summary> 
    /// This script is to control all the elements for each trading box [ each player has one ]
    /// </summary>
    public class TradeArea : MonoBehaviour
    {
        /// <summary>
        /// This is a game object required for each trading box [ each player has one ]
        /// </summary>
        [SerializeField,Header("Game objects needed")] private GameObject NickName = null;
        /// <summary>
        /// This is a game object required each trading box [ each player has one ]
        /// </summary>
        [SerializeField] private GameObject AttendingOrNotPanel = null,panelText = null, AskToTradeButton = null, CancelTrade = null, AcceptTrade = null, rejectTrade = null, askingText = null, beingAskText = null, missionCard = null, inputBribe = null;
        /// <summary>
        /// This hold the script for the UserAreaControlers
        /// </summary>
        [SerializeField] private UserAreaControlers userControler = null ;
        /// <summary>
        /// The string for the nickname of the player
        /// </summary>
        private string nickname;
        /// <summary>
        /// To get/set the nickname of the player when set the UI for the nickname will be updated to the new value
        /// </summary>
        public string nickName
        {
            get { return nickname; }
            set
            {
                nickname = value;
                NickName.GetComponent<Text>().text = nickname;
            }
        }
        /// <summary>
        /// The value if he/she is attending the meeting
        /// </summary>
        private bool Attending;
        /// <summary>
        /// To get/set the attending value when set the UI if person is attending not attending will change accordingly 
        /// </summary>
        public bool attending
        {
            get { return Attending; }
            set
            {
                Attending = value;
                AttendingOrNotPanel.SetActive(!Attending);
            }
        }
        /// <summary>
        /// This is to set the panelText text 
        /// </summary>
        public string attending_text
        {
            set
            {
                panelText.GetComponent<Text>().text = value;
            }
        }
        /// <summary>
        /// To get/set if the interactability of the button AskToTradeButton
        /// </summary>
        public bool askButton
        {
            get { return AskToTradeButton.GetComponent<Button>().interactable; }
            set
            {
                AskToTradeButton.GetComponent<Button>().interactable = value;
            }
        }
        /// <summary>
        /// To get/set if the interactability of the button CancelTrade
        /// </summary>
        public bool cancelButton
        {
            get { return CancelTrade.GetComponent<Button>().interactable; }
            set
            {
                CancelTrade.GetComponent<Button>().interactable = value;
            }
        }
        /// <summary>
        /// To get/set if the interactability of the button AcceptTrade
        /// </summary>
        public bool acceptButton
        {
            get { return AcceptTrade.GetComponent<Button>().interactable; }
            set
            {
                AcceptTrade.GetComponent<Button>().interactable = value;
            }
        }
        /// <summary>
        /// To get/set if the interactability of the button rejectButton
        /// </summary>
        public bool rejectButton
        {
            get { return GetComponent<Button>().interactable; }
            set
            {
                rejectTrade.GetComponent<Button>().interactable = value;
            }
        }
        /// <summary>
        /// To get/set the text for the game object askingText
        /// </summary>
        public string setAskingText
        {
            get { return askingText.GetComponent<Text>().text; }
            set 
            { 
                askingText.GetComponent<Text>().text = value;
            }
        }
        /// <summary>
        /// To get/set the text for the game object beingAskText
        /// </summary>
        public string setgettingAskText
        {
            get { return beingAskText.GetComponent<Text>().text; }
            set
            {
                beingAskText.GetComponent<Text>().text = value;
            }
        }
        /// <summary>
        /// The ID of their mission card ID 
        /// </summary>
        private int missionID;
        /// <summary>
        /// To get/set the missionID of the player as well as setting the Image of missionCard when set
        /// </summary>
        public int setgetmissionID
        {
            get { return missionID; }
            set
            {
                missionID = value;
                missionCard.GetComponent<Image>().sprite = missionCardDeck.cardDeck[missionID - 1].artwork_front_info;
            }
        }
        /// <summary>
        /// To get/set if the interactability of the button inputBribe
        /// </summary>
        public bool BribeInput
        {
            get { return inputBribe.GetComponent<TMP_InputField>().interactable; }
            set
            {
                inputBribe.GetComponent<TMP_InputField>().interactable = value;
            }
        }
        /// <summary>
        /// to set the TMP_InputField text 
        /// </summary>
        public string setBribeInputValue
        {
            set
            {
                inputBribe.GetComponent<TMP_InputField>().text = value;
            }
        }
        /// <summary>
        /// To get/set the value of amountAskingBribing
        /// </summary>
        public int amountAskingBribing { get; set; }
        /// <summary>
        /// To get/set the value of amountBeingBribed
        /// </summary>
        public int amountBeingBribed { get; set; }
        /// <summary>
        /// To get/set the value of AttendedThisRound
        /// </summary>
        public bool AttendedThisRound { get; set; }
        /// <summary>
        /// This is to detect if the input of the inputBribe componet has changed
        /// </summary>
        public void OnDetectInputchange()
        {
            int convertedToInt;
            bool isNumeric = int.TryParse(inputBribe.GetComponent<TMP_InputField>().text, out convertedToInt);
            if (isNumeric)
            {
                if (convertedToInt > userControler.users[0].amountOfMoney)
                {
                    setAskingText = "Error input is more than your amount of money";
                    askButton = false;
                }
                else if (convertedToInt < 0)
                {
                    setAskingText = "Error input is lesser than 0";
                    askButton = false;
                }
                else
                {
                    setAskingText = "Money Bribe " + convertedToInt ;
                    askButton = true;
                }
            }
            else
            {
                setAskingText = "Error input is string";
                askButton = false;
            }
        }
        /// <summary>
        /// When the ask trade button is clicked 
        /// </summary>
        public void setAskTrade()
        {
            amountAskingBribing = int.Parse(inputBribe.GetComponent<TMP_InputField>().text);
            cancelButton = true;
        }
    }
}
