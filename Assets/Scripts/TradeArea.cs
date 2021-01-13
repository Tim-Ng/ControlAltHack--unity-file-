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
    public class TradeArea : MonoBehaviour
    {
        [SerializeField] private GameObject NickName = null, AttendingOrNotPanel = null,panelText = null, AskToTradeButton = null, CancelTrade = null, AcceptTrade = null, rejectTrade = null, askingText = null, beingAskText = null, missionCard = null, inputBribe = null;
        [SerializeField] private UserAreaControlers userControler = null ;
        private string nickname;
        public string nickName
        {
            get { return nickname; }
            set
            {
                nickname = value;
                NickName.GetComponent<Text>().text = nickname;
            }
        }
        private bool Attending;
        public bool attending
        {
            get { return Attending; }
            set
            {
                Attending = value;
                AttendingOrNotPanel.SetActive(!Attending);
            }
        }
        public string attending_text
        {
            set
            {
                panelText.GetComponent<Text>().text = value;
            }
        }

        private bool askingButton;
        public bool askButton
        {
            get { return askingButton; }
            set
            {
                askingButton = value;
                AskToTradeButton.GetComponent<Button>().interactable = askingButton;
            }
        }
        private bool cancelAskButton;
        public bool cancelButton
        {
            get { return cancelAskButton; }
            set
            {
                cancelAskButton = value;
                CancelTrade.GetComponent<Button>().interactable = cancelAskButton;
            }
        }
        private bool acceptTradeButton;
        public bool acceptButton
        {
            get { return acceptTradeButton; }
            set
            {
                acceptTradeButton = value;
                AcceptTrade.GetComponent<Button>().interactable = acceptTradeButton;
            }
        }
        private bool rejectTradeButton;
        public bool rejectButton
        {
            get { return rejectTradeButton; }
            set
            {
                rejectTradeButton = value;
                rejectTrade.GetComponent<Button>().interactable = rejectTradeButton;
            }
        }
        private string asking;
        public string setAskingText
        {
            get { return asking; }
            set 
            { 
                asking = value;
                askingText.GetComponent<Text>().text = asking;
            }
        }
        private string gettingAsk;
        public string setgettingAskText
        {
            get { return gettingAsk; }
            set
            {
                gettingAsk = value;
                beingAskText.GetComponent<Text>().text = gettingAsk;
            }
        }
        private int missionID;
        public int setgetmissionID
        {
            get { return missionID; }
            set
            {
                missionID = value;
                missionCard.GetComponent<Image>().sprite = missionCardDeck.cardDeck[missionID - 1].artwork_front_info;
            }
        }
        private bool bribeInput;
        public bool BribeInput
        {
            get { return bribeInput; }
            set
            {
                bribeInput = value;
                inputBribe.GetComponent<TMP_InputField>().interactable = bribeInput;
            }
        }
        public string setBribeInputValue
        {
            set
            {
                inputBribe.GetComponent<TMP_InputField>().text = value;
            }
        }
        public int amountAskingBribing { get; set; }
        public int amountBeingBribed { get; set; }
        public bool AttendedThisRound { get; set; }
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
        public void setAskTrade()
        {
            amountAskingBribing = int.Parse(inputBribe.GetComponent<TMP_InputField>().text);
            cancelAskButton = true;
        }
    }
}
