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
using DrawCards;
namespace TradeScripts
{
    public class TradeControler : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private drawEntropyCard drawEntroy = null;
        [SerializeField] private TradeArea opponent1=null, opponent2 = null, opponent3 = null, opponent4 = null, opponent5 = null;
        [SerializeField] private GameObject missionCardArea = null;
        [SerializeField] private missionPopup missionPOPUP = null;
        private int HowManyPeople = 0;
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
            HowManyPeople = 0;
            myMissionCard.GetComponent<Image>().sprite = userControler.users[0].missionScript.artwork_front_info;
            MyMoney.GetComponent<Text>().text = userControler.users[0].amountOfMoney.ToString();
            for (int i = 1; i <6; i++)
            {
                tradeAreaContollers[i-1].MyMoney = userControler.users[0].amountOfMoney;
                tradeAreaContollers[i - 1].setBribeInputValue = "0";
                tradeAreaContollers[i-1].nickName = userControler.users[i].Nickname;
                tradeAreaContollers[i-1].setAskingText = "You have asked anyone";
                tradeAreaContollers[i-1].setgettingAskText = "No players have asked you yet";
                tradeAreaContollers[i-1].attending = userControler.users[i].attendingOrNot;
                if (tradeAreaContollers[i - 1].attending == true)
                {
                    HowManyPeople = +1;
                }
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
            if (tradeAreaContollers[which - 1].attending)
            {
                HowManyPeople = +1;
            }
            tradeAreaContollers[which - 1].setgetmissionID = cardID;
        }
        public void playerAskToTrade(Player which,int amountBribed)
        {
            int playerPosition = userControler.findPlayerPosition(which) - 1;
            tradeAreaContollers[playerPosition].amountBeingBribed = amountBribed;
            tradeAreaContollers[playerPosition].acceptButton = false;
            tradeAreaContollers[playerPosition].rejectButton = false;
        }
        public void ClickOnDone()
        {
            tradePanel.SetActive(false);
            userControler.setandsendIfNotAttending();
            if (HowManyPeople == 0)
            {
                drawEntroy.drawEntropyCards(1);
            }
            drawEntroy.drawEntropyCards(1);
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void clickOncard(int which)
        {
            missionPOPUP.clickOnCard(missionCardDeck.cardDeck[tradeAreaContollers[which].setgetmissionID - 1], 0,false);
        }
        public void clickOnMycard()
        {
            missionPOPUP.clickOnCard(userControler.users[0].missionScript, 0,false);
        }
        public void clickOnAskTrade(int which)
        {
            tradeAreaContollers[which].setAskTrade();
            tradeAreaContollers[which].cancelButton = true;
            for (int i = 0; i < 5; i++)
            {
                tradeAreaContollers[i].BribeInput = false;
                tradeAreaContollers[i].askButton = false;
                tradeAreaContollers[i].BribeInput = false;
            }
            object[] player = new object[] { PhotonNetwork.LocalPlayer,tradeAreaContollers[which].amountAskingBribing};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveSomeoneAskToTrade, player, new RaiseEventOptions { TargetActors = new int[] {userControler.users[which+1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
        }
        public void receiveAskFromOtherPlayer(int which,int amount)
        {
            if (missionCardDeck.cardDeck[tradeAreaContollers[which-1].setgetmissionID - 1].Newb_job && userControler.users[0].amountOfCred < userControler.users[which].amountOfCred)
            {
                tradeAreaContollers[which - 1].setgettingAskText = "Player " + userControler.users[which].playerPhoton.NickName + " gave you a newb job with amount of $" + amount + ".";
                clickOnAccenptTrade(which - 1);
            }
            else
            {
                tradeAreaContollers[which - 1].setgettingAskText = "Player " + userControler.users[which].playerPhoton.NickName + " ask for a trade with a bribe of $" + amount + ".";
                tradeAreaContollers[which - 1].acceptButton = true;
                tradeAreaContollers[which - 1].rejectButton = true;
                tradeAreaContollers[which - 1].amountBeingBribed = amount;
            }
        }
        public void clickOnCancelAskTrade(int which)
        {
            ReceiveDeclineTrade(which+1,"You've cancelled to trade.");
            object[] player = new object[] { PhotonNetwork.LocalPlayer};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveSoneoneCancelAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
        }
        public void receiveCancelAskFromOtherPlayer(int which)
        {
            tradeAreaContollers[which - 1].setgettingAskText = "Player " + userControler.users[which].playerPhoton.NickName + " has canceled the trade.";
            tradeAreaContollers[which - 1].acceptButton = false;
            tradeAreaContollers[which - 1].rejectButton = false;
            tradeAreaContollers[which - 1].askButton= true;
            tradeAreaContollers[which - 1].BribeInput= true;
            tradeAreaContollers[which - 1].amountBeingBribed = 0;
        }
        public void clickOnAccenptTrade(int which)
        {
            object[] player = new object[] { PhotonNetwork.LocalPlayer };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.AcceptSomeoneAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
            userControler.users[0].missionScript = missionCardDeck.cardDeck[tradeAreaContollers[which].setgetmissionID-1];
            changeMyMissionCardOnPlayArea();
            userControler.addMyMoney(tradeAreaContollers[which].amountBeingBribed);
            MyNickName.GetComponent<Text>().text = userControler.users[0].Nickname;
            tradeAreaContollers[which].amountBeingBribed = 0;
            object[] playerChangedMission = new object[] { PhotonNetwork.LocalPlayer,userControler.users[0].missionScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendMissionCardChanged, playerChangedMission, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void ReceiveOnSomeoneAcceptYourTrade(int which)
        {
            userControler.users[0].missionScript = missionCardDeck.cardDeck[tradeAreaContollers[which-1].setgetmissionID - 1];
            changeMyMissionCardOnPlayArea();
            userControler.subMyMoney(tradeAreaContollers[which-1].amountAskingBribing);
            MyNickName.GetComponent<Text>().text = userControler.users[0].Nickname;
            tradeAreaContollers[which-1].amountAskingBribing = 0;
            tradeAreaContollers[which-1].setgettingAskText = "Your trade is accepted";
            object[] playerChangedMission = new object[] { PhotonNetwork.LocalPlayer, userControler.users[0].missionScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendMissionCardChanged, playerChangedMission, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void changeMyMissionCardOnPlayArea()
        {
            myMissionCard.GetComponent<Image>().sprite = userControler.users[0].missionScript.artwork_front_info;
            GameObject cardOBJ = missionCardArea.transform.GetChild(0).gameObject;
            cardOBJ.GetComponent<missionDisplay>().setID(userControler.users[0].missionScript.Mission_code);
        }
        public void clickOnDeclineTrade(int which)
        {
            tradeAreaContollers[which].amountBeingBribed = 0;
            tradeAreaContollers[which].rejectButton = false;
            tradeAreaContollers[which].acceptButton= false;
            tradeAreaContollers[which].setgettingAskText= "Rejected Player";
            object[] player = new object[] { PhotonNetwork.LocalPlayer };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.declineSomeoneAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
        }
        public void ReceiveDeclineTrade(int which,string whatMessage)
        {
            for (int i = 0; i < 5; i++)
            {
                tradeAreaContollers[i].BribeInput = true;
                tradeAreaContollers[i].askButton = true;
                tradeAreaContollers[i].setBribeInputValue = "0";
            }
            tradeAreaContollers[which-1].cancelButton = false;
            tradeAreaContollers[which - 1].setgettingAskText = whatMessage;
        }
        public void onReceiveChangeMissionCard(int which,int whichCard)
        {
            tradeAreaContollers[which - 1].setgetmissionID = whichCard;
            if (tradeAreaContollers[which - 1].cancelButton)
            {
                clickOnCancelAskTrade(which - 1);
            }
        }
    }
}
