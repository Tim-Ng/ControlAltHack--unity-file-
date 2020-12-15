using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrawCards;
using TMPro;

namespace TradeScripts
{
    public class TradeArea : MonoBehaviour
    {
        [SerializeField] private GameObject NickName = null, AttendingOrNotPanel = null, AskToTradeButton = null, CancelTrade = null, AcceptTrade = null, rejectTrade = null, askingText = null, beingAskText = null, missionCard = null, inputBribe = null;
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
        private bool askingButton;
        public bool askButton
        {
            get { return askingButton; }
            set
            {
                askingButton = value;
                AskToTradeButton.GetComponent<Button>().interactable = askingButton;
                inputBribe.GetComponent<TMP_InputField>().interactable = askingButton;
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
        public int amountAskingBribing { get; set; }
        public int amountBeingBribed { get; set; }
    }
}
