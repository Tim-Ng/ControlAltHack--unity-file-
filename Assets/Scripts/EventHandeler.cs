using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UserAreas;
using DrawCards;
using TradeScripts;
namespace main
{
    enum PhotonEventCode
    {
        upDateOtherOnGameRounds = 0,
        startGame = 1,
        inputArrangement = 2,
        playerChanged = 3,
        drawCharacterRemove= 4,
        setMyChar = 5,
        setWaiting= 6,
        playerMoney = 7,
        playerCred  = 8,
        drawEntropyRemove = 9,
        drawEntropyUsed = 10,
        sendAmountOfEntropy = 11,
        drawMissionRemove = 12,
        drawMissionUsed = 13,
        tradeNotAttending = 14,
        tradeAttending = 15,
        receiveSomeoneAskToTrade =16,
        receiveSoneoneCancelAsk = 17,
        declineSomeoneAsk = 18,
        AcceptSomeoneAsk = 19,
        sendMissionCardChanged = 20,

    }
    public class EventHandeler : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private drawCharacterCard drawChar = null;
        [SerializeField] private TurnManager turnManager= null;
        [SerializeField] private drawEntropyCard drawEntropy = null;
        [SerializeField] private drawMissionCard drawMission = null;
        [SerializeField] private TradeControler tradeControl = null;
        public RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others,
        };
        public RaiseEventOptions AllPeople = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        private void OnEnable()
        {
            Debug.Log("Listen to event");
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
            Debug.Log("Event heard");
        }
        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
            Debug.Log("Event Ended");
        }


        private void NetworkingClient_EventReceived(EventData obj)
        {
            Debug.Log("Code =" + obj.Code);
            if (obj.Code == (byte)PhotonEventCode.upDateOtherOnGameRounds)
            {
                Debug.Log("Passed to round");
                object[] rounds = (object[])obj.CustomData;
                userControler.upDateOtherOnGameRounds((int)rounds[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.startGame)
            {
                Debug.Log("Passed to startGame");
                userControler.startingGame();
            }
            else if (obj.Code == (byte)PhotonEventCode.inputArrangement)
            {
                object[] arragement = (object[])obj.CustomData;
                Debug.Log("Input Arrangement : " + (int)arragement[0]);
                turnManager.inputArrangement((int)arragement[0], (bool)arragement[1], (bool)arragement[2]);
            }
            else if (obj.Code == (byte)PhotonEventCode.playerChanged)
            {
                turnManager.playerChanged();
            }
            else if (obj.Code == (byte)PhotonEventCode.drawCharacterRemove)
            {
                object[] cardID = (object[])obj.CustomData;
                drawChar.removeFormDeck((int)cardID[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setMyChar)
            {
                object[] charStuff = (object[])obj.CustomData;
                userControler.setOtherCharacter((Player)charStuff[0],(int)charStuff[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setWaiting)
            {
                object[] player = (object[])obj.CustomData;
                turnManager.actorsDoneEdit((int)player[0],false);
            }
            else if (obj.Code == (byte)PhotonEventCode.playerMoney)
            {
                object[] amount = (object[])obj.CustomData;
                userControler.receiveOtherMoney((Player)amount[0], (int)amount[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.playerCred)
            {
                object[] amount = (object[])obj.CustomData;
                userControler.receiveOtherCred((Player)amount[0], (int)amount[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.drawEntropyRemove)
            {
                object[] whichCard = (object[])obj.CustomData;
                drawEntropy.removeFormDeck((int)whichCard[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.drawEntropyUsed)
            {
                object[] whichCard = (object[])obj.CustomData;
                drawEntropy.addToPlayedDeck((int)whichCard[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.sendAmountOfEntropy)
            {
                object[] amount = (object[])obj.CustomData;
                userControler.receiveOtherAmountOfCards((Player)amount[0], (int)amount[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.drawMissionRemove)
            {
                object[] whichCard = (object[])obj.CustomData;
                drawMission.removeFormDeck((int)whichCard[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.drawMissionUsed)
            {
                object[] whichCard = (object[])obj.CustomData;
                drawMission.addToPlayedDeck((int)whichCard[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.tradeAttending)
            {
                object[] info = (object[])obj.CustomData;
                userControler.receiveOtherAttending((Player)info[0], (int)info[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.tradeNotAttending)
            {
                object[] info = (object[])obj.CustomData;
                userControler.receiveOtherNotAttending((Player)info[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.receiveSomeoneAskToTrade)
            {
                Debug.Log("Receive Ask Trade");
                object[] info = (object[])obj.CustomData;
                tradeControl.receiveAskFromOtherPlayer((int)userControler.findPlayerPosition((Player)info[0]),(int)info[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.receiveSoneoneCancelAsk)
            {
                Debug.Log("Receive Cancel Trade");
                object[] info = (object[])obj.CustomData;
                tradeControl.receiveCancelAskFromOtherPlayer((int)userControler.findPlayerPosition((Player)info[0]));
            }
            else if (obj.Code == (byte)PhotonEventCode.declineSomeoneAsk)
            {
                Debug.Log("Receive Trade Was Decline");
                object[] info = (object[])obj.CustomData;
                tradeControl.ReceiveDeclineTrade((int)userControler.findPlayerPosition((Player)info[0]),"This player has decline your trade.");
            }
            else if (obj.Code == (byte)PhotonEventCode.AcceptSomeoneAsk)
            {
                Debug.Log("Receive Trade Was Accept");
                object[] info = (object[])obj.CustomData;
                tradeControl.ReceiveOnSomeoneAcceptYourTrade((int)userControler.findPlayerPosition((Player)info[0]));
            } 
            else if (obj.Code == (byte)PhotonEventCode.sendMissionCardChanged)
            {
                Debug.Log("Someone change cards");
                object[] info = (object[])obj.CustomData;
                tradeControl.onReceiveChangeMissionCard((int)userControler.findPlayerPosition((Player)info[0]),(int)info[1]);
            }
        }
    }
}
