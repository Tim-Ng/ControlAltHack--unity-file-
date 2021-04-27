using DrawCards;
using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;
namespace TradeScripts
{
    /// <summary>
    /// This is the script that controls the trade panels.
    /// </summary>
    public class TradeControler : MonoBehaviour
    {
        /// <summary>
        /// This is the game object where this script is attatched to.
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// Holds the script EventHandeler
        /// </summary>
        private EventHandeler EventManger = null;
        /// <summary>
        /// Holds the script drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntroy = null;
        /// <summary>
        /// Holds the script missionPopup
        /// </summary>
        private missionPopup missionPOPUP = null;
        /// <summary>
        /// Holds the script UserAreaControlers
        /// </summary>
        private UserAreaControlers userControler = null;
        /// <summary>
        /// Holds the script for the other player's trade areas
        /// </summary>
        [SerializeField, Header("Other player's Trade Areas")] private TradeArea opponent1 = null;
        /// <summary>
        /// Holds the script for the other player's trade areas
        /// </summary>
        [SerializeField] private TradeArea opponent2 = null, opponent3 = null, opponent4 = null, opponent5 = null;
        /// <summary>
        /// This records how many people attended the meeting
        /// </summary>
        [HideInInspector]
        public int HowManyPeople = 0;
        /// <summary>
        /// This holds the list for the tradeAreas
        /// </summary>
        private List<TradeArea> tradeAreaContollers = new List<TradeArea>();
        /// <summary>
        /// The game object that holds the mission card
        /// </summary>
        [SerializeField, Header("The players Info Area")] private GameObject missionCardArea = null;
        /// <summary>
        /// This is the game object for the display the current player's nickname
        /// </summary>
        [SerializeField] private GameObject MyNickName = null;
        /// <summary>
        /// This is the game object for the display the current amount of money of the current player
        /// </summary>
        [SerializeField] private GameObject MyMoney = null;
        /// <summary>
        /// This is the game object for the mission card UI of the current player's card
        /// </summary>
        [SerializeField] private GameObject myMissionCard = null;
        /// <summary>
        /// This is the game object for the the trade panel to hold all the elements 
        /// </summary>
        [SerializeField,Header("Trade Panel")] private GameObject tradePanel = null;
        /// <summary>
        /// When the script is loaded this function will fill in the data for the scripts that we this class needs as well as adding to the trade areas on the tradeAreaContollers and reset
        /// </summary>
        private void Start()
        {
            ScriptsODJ = gameObject;
            EventManger = ScriptsODJ.GetComponent<EventHandeler>();
            missionPOPUP = ScriptsODJ.GetComponent<missionPopup>();
            drawEntroy = ScriptsODJ.GetComponent<drawEntropyCard>();
            userControler = ScriptsODJ.GetComponent<UserAreaControlers>();
            MyNickName.GetComponent<Text>().text = userControler.users[0].Nickname;
            tradeAreaContollers.Add(opponent1);
            tradeAreaContollers.Add(opponent2);
            tradeAreaContollers.Add(opponent3);
            tradeAreaContollers.Add(opponent4);
            tradeAreaContollers.Add(opponent5);
            resetHasAttended();
        }
        /// <summary>
        /// This function is used to set up the name and if all current player has attended and the player's own trade area
        /// </summary>
        public void setAllAreas()
        {
            myMissionCard.GetComponent<Image>().sprite = userControler.users[0].missionScript.artwork_front_info;
            MyMoney.GetComponent<Text>().text = userControler.users[0].amountOfMoney.ToString()+"\n"+ userControler.users[0].amountOfCred + " Creds";
            for (int i = 1; i <6; i++)
            {
                if (userControler.users[i].playerPhoton != null)
                {
                    tradeAreaContollers[i - 1].setBribeInputValue = "0";
                    tradeAreaContollers[i - 1].nickName = userControler.users[i].Nickname+"\n"+ userControler.users[i].amountOfCred + " Creds";
                    tradeAreaContollers[i - 1].setAskingText = "You have asked anyone";
                    tradeAreaContollers[i - 1].setgettingAskText = "No players have asked you yet";
                    if (tradeAreaContollers[i - 1].attending == true)
                    {
                        HowManyPeople = +1;
                    }
                    if (!tradeAreaContollers[i - 1].AttendedThisRound)
                    {
                        tradeAreaContollers[i - 1].attending_text = "Waiting...";
                    }
                    tradeAreaContollers[i - 1].acceptButton = false;
                    tradeAreaContollers[i - 1].rejectButton = false;
                    tradeAreaContollers[i - 1].askButton = true;
                    tradeAreaContollers[i - 1].cancelButton = false;
                }
                else
                {
                    tradeAreaContollers[i - 1].attending_text = "No player";
                }
                tradeAreaContollers[i - 1].attending = userControler.users[i].attendingOrNot;
            }
            tradePanel.SetActive(true);
        }
        /// <summary>
        /// This function is called when another player has made their decision of attending the meeting or not and will set the attending text accordingly 
        /// </summary>
        /// <param name="which">Which player has sent the info </param>
        /// <param name="clickedOnDoneTrading"> True for attending False for skipping </param>
        public void PlayerAttentingChange(int which,bool clickedOnDoneTrading) {
            tradeAreaContollers[which - 1].attending = userControler.users[which].attendingOrNot;
            if (clickedOnDoneTrading)
            {
                tradeAreaContollers[which - 1].attending_text = "Done Trading";
            }
            else
            {
                tradeAreaContollers[which - 1].attending_text = "Not Attending";
            }
            tradeAreaContollers[which - 1].AttendedThisRound = true;
        }
        /// <summary>
        /// This function is called to update the UI of the players that have traded their cards 
        /// </summary>
        /// <param name="which">Which player</param>
        /// <param name="cardID">new Card ID </param>
        public void PlayerAttentingChange(int which, int cardID)
        {
            tradeAreaContollers[which - 1].attending = userControler.users[which].attendingOrNot;
            HowManyPeople = +1;
            tradeAreaContollers[which - 1].setgetmissionID = cardID;
        }
        /// <summary>
        /// This function is called when the done button is click and it will close the popup as well as telling other player that he/she is done
        /// </summary>
        public void ClickOnDone()
        {
            tradePanel.SetActive(false);
            userControler.setandsendIfNotAttending(true);
            drawEntroy.drawEntropyCards(1);
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
            object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " is done trading.", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to reset all the attend this round to false
        /// </summary>
        public void resetHasAttended()
        {
            for(int i =0; i < 5; i++)
            {
                tradeAreaContollers[i].AttendedThisRound = false;
            }
        }
        /// <summary>
        /// This is to click on the all mission card display to open up pop up mission card to check their task
        /// </summary>
        /// <param name="which"> Which player </param>
        public void clickOncard(int which) => missionPOPUP.clickOnCard(missionCardDeck.cardDeck[tradeAreaContollers[which].setgetmissionID - 1], 0, false);
        /// <summary>
        /// This is to open the current player's mission card to open the mission popup
        /// </summary>
        public void clickOnMycard() => missionPOPUP.clickOnCard(userControler.users[0].missionScript, 0, false);
        /// <summary>
        /// This is to ask someone to trade and will send the info to the person being asked
        /// </summary>
        /// <param name="which">For which player</param>
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
        /// <summary>
        /// This function is called when someone is asking to trade 
        /// </summary>
        /// <param name="which"> vWhich player asking </param>
        /// <param name="amount"> The amount being bribed</param>
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
        /// <summary>
        /// To cancel asking someone to trade and send to that player
        /// </summary>
        /// <param name="which"></param>
        public void clickOnCancelAskTrade(int which)
        {
            ReceiveDeclineTrade(which+1,"You've cancelled to trade.");
            object[] player = new object[] { PhotonNetwork.LocalPlayer};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveSomeoneCancelAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is called,when a player cancel the asking trade with you
        /// </summary>
        /// <param name="which">  From which player</param>
        public void receiveCancelAskFromOtherPlayer(int which)
        {
            tradeAreaContollers[which - 1].setgettingAskText = "Player " + userControler.users[which].playerPhoton.NickName + " has canceled the trade.";
            tradeAreaContollers[which - 1].acceptButton = false;
            tradeAreaContollers[which - 1].rejectButton = false;
            tradeAreaContollers[which - 1].askButton= true;
            tradeAreaContollers[which - 1].BribeInput= true;
            tradeAreaContollers[which - 1].amountBeingBribed = 0;
        }
        /// <summary>
        /// This is to accept the trade. 
        /// </summary>
        /// <remarks>
        /// This will tell everyone that you have changed card as well as sending the player the player who ask that you've accepted the trade <br/>
        /// Further more will change your money according the bribed price
        /// </remarks>
        /// <param name="which">Accept which player's trade</param>
        public void clickOnAccenptTrade(int which)
        {
            object[] player = new object[] { PhotonNetwork.LocalPlayer };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.AcceptSomeoneAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
            object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName +" has traded with "+userControler.users[which+1].Nickname, null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
            userControler.users[0].missionScript = missionCardDeck.cardDeck[tradeAreaContollers[which].setgetmissionID - 1];
            changeMyMissionCardOnPlayArea();
            tradeAreaContollers[which].setgettingAskText = "You've accepted this Player's trade";
            userControler.addMyMoney(tradeAreaContollers[which].amountBeingBribed);
            MyMoney.GetComponent<Text>().text = userControler.users[0].amountOfMoney.ToString() + "\n" + userControler.users[0].amountOfCred + " Creds";
            tradeAreaContollers[which].amountBeingBribed = 0;
            tradeAreaContollers[which].acceptButton = false;
            tradeAreaContollers[which].rejectButton = false;
            object[] playerChangedMission = new object[] { PhotonNetwork.LocalPlayer,userControler.users[0].missionScript.Mission_code,userControler.users[which+1].playerPhoton };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendMissionCardChanged, playerChangedMission, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is caleed when it is receive that a player has accepted your trade
        /// </summary>
        /// <remarks>
        /// The will tell other players we changed cards <br/>
        /// Further more will change your money according the bribed price
        /// </remarks>
        /// <param name="which"> Which player accepted the trade</param>
        public void ReceiveOnSomeoneAcceptYourTrade(int which)
        {
            userControler.users[0].missionScript = missionCardDeck.cardDeck[tradeAreaContollers[which-1].setgetmissionID - 1];
            changeMyMissionCardOnPlayArea();
            userControler.subMyMoney(tradeAreaContollers[which-1].amountAskingBribing);
            MyMoney.GetComponent<Text>().text = userControler.users[0].amountOfMoney.ToString() + "\n" + userControler.users[0].amountOfCred + " Creds";
            tradeAreaContollers[which-1].amountAskingBribing = 0;
            tradeAreaContollers[which-1].setgettingAskText = "Your trade is accepted";
            tradeAreaContollers[which - 1].askButton = true;
            tradeAreaContollers[which - 1].cancelButton = false;
            object[] playerChangedMission = new object[] { PhotonNetwork.LocalPlayer, userControler.users[0].missionScript.Mission_code, userControler.users[which + 1].playerPhoton };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendMissionCardChanged, playerChangedMission, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to change the card on the mission card area to update the UI
        /// </summary>
        public void changeMyMissionCardOnPlayArea()
        {
            myMissionCard.GetComponent<Image>().sprite = userControler.users[0].missionScript.artwork_front_info;
            GameObject cardOBJ = missionCardArea.transform.GetChild(0).gameObject;
            cardOBJ.GetComponent<missionDisplay>().setID(userControler.users[0].missionScript.Mission_code);
        }
        /// <summary>
        /// This is decline other people's ask for a trade
        /// </summary>
        /// <param name="which">Which player</param>
        public void clickOnDeclineTrade(int which)
        {
            tradeAreaContollers[which].amountBeingBribed = 0;
            tradeAreaContollers[which].acceptButton = false;
            tradeAreaContollers[which].rejectButton = false;
            tradeAreaContollers[which].setgettingAskText= "Rejected Player";
            object[] player = new object[] { PhotonNetwork.LocalPlayer };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.declineSomeoneAsk, player, new RaiseEventOptions { TargetActors = new int[] { userControler.users[which + 1].playerPhoton.ActorNumber } }, SendOptions.SendReliable);
        }
        /// <summary>
        /// On receiving a decline on asking trading
        /// </summary>
        /// <param name="which"> Which player </param>
        /// <param name="whatMessage"> What is the message </param>
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
        /// <summary>
        /// This function is called when another player changed their card durning trading and update the UI
        /// </summary>
        /// <param name="which"> Which player</param>
        /// <param name="whichCard"> Which card ID traded</param>
        /// <param name="WhoTradedWith"> Who was it traded with </param>
        public void onReceiveChangeMissionCard(int which,int whichCard,Player WhoTradedWith)
        {
            tradeAreaContollers[which - 1].setgetmissionID = whichCard;
            if (tradeAreaContollers[which - 1].cancelButton && WhoTradedWith!=PhotonNetwork.LocalPlayer)
            {
                clickOnCancelAskTrade(which - 1);
            }
        }
    }
}
