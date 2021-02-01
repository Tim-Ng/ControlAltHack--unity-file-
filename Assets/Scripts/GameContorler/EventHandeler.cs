using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UserAreas;
using DrawCards;
using TradeScripts;
using rollmissions;

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
        setTimerForRoll = 21,
        sendWhoRolling = 22,
        textForTextBeforeRoll = 23,
        setToDuringRoll = 24,
        setDuringRollText= 25,
        setProcessText = 26,
        setStatusText = 27,
        rolledNumberMission = 28,
        setCurrentMissionStatus =29,
        setEndScene = 30,
        setNumberOfChances =31,
        setStatusOutput = 32,
        setSceneForReRoll = 33,
        shareFate = 34,
        cancelMissionID28 = 35,
        skillChangeID26 = 36,
        goZeroMoneyID29 = 37,
        lightningRoll = 38,
        sendLightingStrikeRoll = 39,
        sendLightingStrikeRolled = 40,
        sendPlayerFired = 41,
        receiveWinner = 42,
        resetGame = 43,
        forChat = 44,
        foundSameName =45,
    }
    public class EventHandeler : MonoBehaviour
    {
        private GameObject ScriptsODJ = null;
        private UserAreaControlers userControler = null;
        private drawCharacterCard drawChar = null;
        private TurnManager turnManager= null;
        private drawEntropyCard drawEntropy = null;
        private drawMissionCard drawMission = null;
        private TradeControler tradeControl = null;
        private rollingMissionControl rollingMission = null;
        private playEntropyCard playEntropy= null;
        private ChatController chatController = null;

        private void Start()
        {
            ScriptsODJ = gameObject;
            drawChar = ScriptsODJ.GetComponent<drawCharacterCard>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
            userControler = ScriptsODJ.GetComponent<UserAreaControlers>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            rollingMission = ScriptsODJ.GetComponent<rollingMissionControl>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
            playEntropy = ScriptsODJ.GetComponent<playEntropyCard>();
            tradeControl = ScriptsODJ.GetComponent<TradeControler>();
            chatController = ScriptsODJ.GetComponent<ChatController>();
        }
        [SerializeField] private GameObject cardArea= null,roundNumberOBJ = null;
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
                userControler.receiveOtherNotAttending((Player)info[0],(bool)info[1]);
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
            else if (obj.Code == (byte)PhotonEventCode.setTimerForRoll)
            {
                Debug.Log("Timerset");
                object[] timer = (object[])obj.CustomData;
                rollingMission.setTimer((string)timer[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.sendWhoRolling)
            {
                Debug.Log("Receive someone rolling");
                object[] playerInfo = (object[])obj.CustomData;
                rollingMission.onReceiveSetOtherPlayerRoll((Player)playerInfo[0],(int)playerInfo[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.textForTextBeforeRoll)
            {
                Debug.Log("Receive text before rolling");
                object[] taskInfo = (object[])obj.CustomData;
                rollingMission.onReceiveTextBeforeRollTasks((string)taskInfo[0], (bool)taskInfo[1], (string)taskInfo[2], (bool)taskInfo[3], (string)taskInfo[4]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setToDuringRoll)
            {
                Debug.Log("Receive text during rolling");
                object[] playerDuringdata = (object[])obj.CustomData;
                rollingMission.onReceiveChangeToDuringTask();
            }
            else if (obj.Code == (byte)PhotonEventCode.setDuringRollText)
            {
                Debug.Log("Receive set text during rolling");
                object[] playerDuringtextdata = (object[])obj.CustomData;
                rollingMission.onReceiveControllText((string)playerDuringtextdata[0], (string)playerDuringtextdata[1], (string)playerDuringtextdata[2]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setProcessText)
            {
                Debug.Log("Receive set text progress during rolling");
                object[] setProcessTextdata = (object[])obj.CustomData;
                rollingMission.onReceiveDuringTaskProgressText((int)setProcessTextdata[0], (string )setProcessTextdata[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setStatusText)
            {
                Debug.Log("Receive set text status during rolling");
                object[] setStatusTextdata = (object[])obj.CustomData;
                rollingMission.onReceiveTaskStatusText((int)setStatusTextdata[0], (string )setStatusTextdata[1]);
            }
            else if (obj.Code == (byte)PhotonEventCode.rolledNumberMission)
            {
                Debug.Log("Receive player RolledNumber rolling");
                object[] rolledData = (object[])obj.CustomData;
                rollingMission.onReceiveRolledValue((int)rolledData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setCurrentMissionStatus)
            {
                Debug.Log("Receive current mission status rolling");
                object[] CurrentMissionStatusData = (object[])obj.CustomData;
                rollingMission.onReceiveCurrentMissionStatus((string)CurrentMissionStatusData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setEndScene)
            {
                Debug.Log("Receive end rolling scene");
                rollingMission.onReceiveEndScene();
            }
            else if (obj.Code == (byte)PhotonEventCode.setNumberOfChances)
            {
                Debug.Log("Receive number of chances ");
                object[] numberOfChanceData = (object[])obj.CustomData;
                rollingMission.onReceiveNumberOfChances((string)numberOfChanceData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setStatusOutput)
            {
                Debug.Log("Receive output");
                object[] CurrentMissionStatusOutputData = (object[])obj.CustomData;
                rollingMission.onReceiveCurrentMissionOutputText((string)CurrentMissionStatusOutputData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setSceneForReRoll)
            {
                rollingMission.switchStage(2);
            }
            else if (obj.Code == (byte)PhotonEventCode.shareFate)
            {
                object[] Amount = (object[])obj.CustomData;
                userControler.addMyCred((int)Amount[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.cancelMissionID28)
            {
                turnManager.EndTurn();
            }
            else if (obj.Code == (byte)PhotonEventCode.skillChangeID26)
            {
                rollingMission.addSkillEffector(AllJobs.NetNinja, turnManager.RoundNumber, -2);
                rollingMission.addSkillEffector(AllJobs.SocialEng, turnManager.RoundNumber, -2);
            }
            else if (obj.Code == (byte)PhotonEventCode.goZeroMoneyID29)
            {
                userControler.subMyMoney(userControler.users[0].amountOfMoney);
            }
            else if (obj.Code == (byte)PhotonEventCode.lightningRoll)
            {
                object[] entropyOBJData = (object[])obj.CustomData;
                playEntropy.lightningRoll((int)entropyOBJData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.sendLightingStrikeRoll)
            {
                object[] entropyLightningRollJData = (object[])obj.CustomData;
                playEntropy.onReceiveSomeoneLightningRoll((int)entropyLightningRollJData[0], (string)entropyLightningRollJData[1], (int)entropyLightningRollJData[2]);
            }
            else if (obj.Code == (byte)PhotonEventCode.sendLightingStrikeRolled)
            {
                object[] entropyLightningRolledJData = (object[])obj.CustomData;
                playEntropy.onReceiveRolled((string)entropyLightningRolledJData[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.sendPlayerFired)
            {
                object[] playerFired = (object[])obj.CustomData;
                userControler.onReceiveFiredPlayer((Player)playerFired[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.receiveWinner)
            {
                object[] winnerData = (object[])obj.CustomData;
                if ((int)winnerData[0] == 1)
                {
                    userControler.onReceiveWinner1((int)winnerData[1]);
                }
                else if ((int)winnerData[0] == 2)
                {
                    userControler.onReceiveWinner2((int)winnerData[1], (int)winnerData[2]);
                }
                else if ((int)winnerData[0] == 3)
                {
                    userControler.onReceiveWinner3((int)winnerData[1], (int)winnerData[2],(int)winnerData[3]);
                }
            }
            else if (obj.Code == (byte)PhotonEventCode.resetGame)
            {
                turnManager.TurnNumber = 0;
                turnManager.RoundNumber = 1;
                roundNumberOBJ.SetActive(false);
                userControler.startOBJs();
                foreach (Transform child in cardArea.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                drawMission.removeAllCard();
                drawEntropy.startDraw();
                drawMission.startDraw();
                drawChar.startDraw();
                rollingMission.switchStage(0);
                playEntropy.onPlayRollReset();
                chatController.onReceiveMessage("The Game has been reset", null, false);
            }
            else if (obj.Code == (byte)PhotonEventCode.forChat)
            {
                object[] chatInfo = (object[])obj.CustomData;
                chatController.onReceiveMessage((string)chatInfo[0], (Player)chatInfo[1], (bool)chatInfo[2]);
            }
            else if (obj.Code == (byte)PhotonEventCode.foundSameName)
            {
                userControler.updateNumberOfPlayers();
            }
        }
    }
}
