using DrawCards;
using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;
namespace TradeScripts
{
    public class TradeControler : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private TradeArea opponent1=null, opponent2 = null, opponent3 = null, opponent4 = null, opponent5 = null;
        [SerializeField] private missionPopup missionPOPUP = null;
        private List<TradeArea> tradeAreaContollers = new List<TradeArea>();
        [SerializeField] private GameObject tradePanel = null, MyNickName = null, MyMoney = null, myMissionCard = null;
        private void Start()
        {
            MyNickName.GetComponent<Text>().text = userControler.users[0].Nickname;
            tradeAreaContollers.Add(opponent1);
            tradeAreaContollers.Add(opponent2);
            tradeAreaContollers.Add(opponent3);
            tradeAreaContollers.Add(opponent4);
            tradeAreaContollers.Add(opponent5);
        }
        public void setAllAreas()
        {
            myMissionCard.GetComponent<Image>().sprite = userControler.users[0].missionScript.artwork_front_info;
            MyMoney.GetComponent<Text>().text = userControler.users[0].amountOfMoney.ToString();
            for (int i = 1; i <6; i++)
            {
                tradeAreaContollers[i-1].nickName = userControler.users[i].Nickname;
                tradeAreaContollers[i-1].setAskingText = "You have asked anyone";
                tradeAreaContollers[i-1].setgettingAskText = "No players have asked you yet";
                tradeAreaContollers[i-1].attending = userControler.users[i].attendingOrNot;
                tradeAreaContollers[i-1].acceptButton = false;
                tradeAreaContollers[i-1].rejectButton = false;
                tradeAreaContollers[i-1].askButton = true;
                tradeAreaContollers[i-1].cancelButton = false;
            }
            tradePanel.SetActive(true);
        }
        public void PlayerAttentingChange(int which)
        {
            tradeAreaContollers[which - 1].attending = userControler.users[which].attendingOrNot;
        }
        public void PlayerAttentingChange(int which,int cardID)
        {
            tradeAreaContollers[which - 1].attending = userControler.users[which].attendingOrNot;
            tradeAreaContollers[which - 1].setgetmissionID = cardID;
        }
        public void controlAllAskButton(bool how)
        {
            for (int i = 0; i < 5; i++)
            {
                tradeAreaContollers[i].askButton = how;
            }
        }
        public void playerAskToTrade(Player which,int amountBribed)
        {
            int playerPosition = userControler.findPlayerPosition(which) - 1;
            tradeAreaContollers[playerPosition].amountBeingBribed = amountBribed;
            tradeAreaContollers[playerPosition].acceptButton = false;
            tradeAreaContollers[playerPosition].rejectButton = false;
        }
        public void clickOnaskButton(int which)
        {
            controlAllAskButton(false);
            tradeAreaContollers[which].cancelButton = true;
        }
        public void ClickOnDone()
        {
            tradePanel.SetActive(false);
            userControler.setandsendIfNotAttending();
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void clickOncard(int which)
        {
            missionPOPUP.clickOnCard(tradeAreaContollers[which].setgetmissionID, 0);
        }
    }
}
