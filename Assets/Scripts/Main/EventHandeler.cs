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
    /// <summary>
    /// The enum of all the Photon Events 
    /// </summary>
    enum PhotonEventCode
    {
        /// <summary>
        /// for updating the number of rounds for this current room
        /// </summary>
        upDateOtherOnGameRounds = 0,
        /// <summary>
        /// for when the game has started
        /// </summary>
        startGame = 1,
        /// <summary>
        /// for sending the arrangement of the player turns for this round
        /// </summary>
        inputArrangement = 2,
        /// <summary>
        /// for telling a player has ends thier turn
        /// </summary>
        playerChanged = 3,
        /// <summary>
        /// for when a character is drawn
        /// </summary>
        drawCharacterRemove = 4,
        /// <summary>
        /// for when a character choosen by another player 
        /// </summary>
        setMyChar = 5,
        /// <summary>
        /// for when we need eveyone to be done with something [choosing character, done trading and more]
        /// </summary>
        setWaiting= 6,
        /// <summary>
        /// for setting the amount of money of another players when we receive the data
        /// </summary>
        playerMoney = 7,
        /// <summary>
        /// for setting the amount of cred of another players when we receive the data
        /// </summary>
        playerCred = 8,
        /// <summary>
        /// for when an entorpy card is drawn by another player to be removed from the current deck
        /// </summary>
        drawEntropyRemove = 9,
        /// <summary>
        /// When an entorpy is used an added to the used entropy deck
        /// </summary>
        drawEntropyUsed = 10,
        /// <summary>
        /// for when receiving the amount of entropy card of another player to set the UI
        /// </summary>
        sendAmountOfEntropy = 11,
        /// <summary>
        /// for when an mission card is drawn by another player to be removed from the current deck
        /// </summary>
        drawMissionRemove = 12,
        /// <summary>
        /// When an mission is used an added to the used mission deck
        /// </summary>
        drawMissionUsed = 13,
        /// <summary>
        /// receive when a player is not attending to trade 
        /// </summary>
        tradeNotAttending = 14,
        /// <summary>
        /// receive when a player is attending to trade 
        /// </summary>
        tradeAttending = 15,
        /// <summary>
        /// for when someone ask to trade with you.If used to send then is to ask a trade request.
        /// </summary>
        receiveSomeoneAskToTrade = 16,
        /// <summary>
        /// for when someone cancel their request to trade with you .If used to send then is to cancel a trade request.
        /// </summary>
        receiveSomeoneCancelAsk = 17,
        /// <summary>
        /// for when someone decline your request to trade.If used to send then is to decline the trade request.
        /// </summary>
        declineSomeoneAsk = 18,
        /// <summary>
        /// for when someone accept your request to trade.If used to send then is to accept the trade request.
        /// </summary>
        AcceptSomeoneAsk = 19,
        /// <summary>
        /// for receiving changes in some other player's mission card is changed during trade / telling people you change 
        /// </summary>
        sendMissionCardChanged = 20,
        /// <summary>
        /// to start the timer for the before mission roll
        /// </summary>
        setTimerForRoll = 21,
        /// <summary>
        /// to get who is rolling / send you is rolling
        /// </summary>
        sendWhoRolling = 22,
        /// <summary>
        /// To set the text before roll
        /// </summary>
        textForTextBeforeRoll = 23,
        /// <summary>
        /// To change to during roll scene / send then start during roll
        /// </summary>
        setToDuringRoll = 24,
        /// <summary>
        /// To set during roll text / send during roll text
        /// </summary>
        setDuringRollText= 25,
        /// <summary>
        /// to set the text of the Process / send the text of the Process 
        /// </summary>
        setProcessText = 26,
        /// <summary>
        /// to set the text of the status / send the text of the status
        /// </summary>
        setStatusText = 27,
        /// <summary>
        /// receive value of the dice rolled / send the value of the dice rolled
        /// </summary>
        rolledNumberMission = 28,
        /// <summary>
        /// To set the current mission status / send the current mission status
        /// </summary>
        setCurrentMissionStatus =29,
        /// <summary>
        /// To set the end of the scene to end roll scene / send the its end roll scene
        /// </summary>
        setEndScene = 30,
        /// <summary>
        /// To set the number of roll chances / send the number of roll chances
        /// </summary>
        setNumberOfChances =31,
        /// <summary>
        /// set the current mission status / send the current mission status  
        /// </summary>
        setStatusOutput = 32,
        /// <summary>
        /// To set the scene for a reroll / send to set scene for a reroll
        /// </summary>
        setSceneForReRoll = 33,
        /// <summary>
        /// get entorpy card shareFate have been played / send entorpy card shareFate have been played 
        /// </summary>
        shareFate = 34,
        /// <summary>
        /// Ligthing strike ID 28 that cancels the current player mission / send to current rolling player to cancel mission from entropy card ID [35]
        /// </summary>
        cancelMissionID28 = 35,
        /// <summary>
        /// Ligthing strike ID 28 that debuff skill on current player mission / send to current rolling player to have from debuff with entropy card ID [36]
        /// </summary>
        skillChangeID26 = 36,
        /// <summary>
        /// Ligthing strike ID 28 that remove all money of the current player mission / send to current rolling player to remove all money with entropy card ID [29]
        /// </summary>
        goZeroMoneyID29 = 37,
        /// <summary>
        /// get that its currently a lighting roll / send current lighting roll
        /// </summary>
        lightningRoll = 38,
        /// <summary>
        /// receive that someone strike you with lighting strike / send lighting strike to current player rolling 
        /// </summary>
        sendLightingStrikeRoll = 39,
        /// <summary>
        /// receive rolled value during lighting strike roll / send rolled value during lighting strike roll 
        /// </summary>
        sendLightingStrikeRolled = 40,
        /// <summary>
        /// receive if a player if fired / send player that you are fired
        /// </summary>
        sendPlayerFired = 41,
        /// <summary>
        /// receive who are the top 3 winner / send who are the top 3 winner
        /// </summary>
        receiveWinner = 42,
        /// <summary>
        /// Send/receive reset the game 
        /// </summary>
        resetGame = 43,
        /// <summary>
        /// Receive/send infomation from/to the chat
        /// </summary>
        forChat = 44,
        /// <summary>
        /// receive name update due to that player having same name with another player / send your new name due to having t he same name as another player
        /// </summary>
        foundSameName =45,
        /// <summary>
        /// receive Done with lightning roll / send done with lightning roll
        /// </summary>
        LightningDone =46
    }

    /// <summary>
    /// This script is used to handle the events sent through photon event mangaer
    /// </summary>
    public class EventHandeler : MonoBehaviour
    {
        /// <summary>
        /// This holds the game object of the which the this script is attached to 
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// This holds the script for UserAreaControlers
        /// </summary>
        private UserAreaControlers userControler = null;
        /// <summary>
        /// This holds the script for drawCharacterCard
        /// </summary>
        private drawCharacterCard drawChar = null;
        /// <summary>
        /// This holds the script for TurnManager
        /// </summary>
        private TurnManager turnManager= null;
        /// <summary>
        /// This holds the script for drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntropy = null;
        /// <summary>
        /// This holds the script for drawMissionCard
        /// </summary>
        private drawMissionCard drawMission = null;
        /// <summary>
        /// This holds the script for TradeControler
        /// </summary>
        private TradeControler tradeControl = null;
        /// <summary>
        /// This holds the script for rollingMissionControl
        /// </summary>
        private rollingMissionControl rollingMission = null;
        /// <summary>
        /// This holds the script for playEntropyCard
        /// </summary>
        private playEntropyCard playEntropy= null;
        /// <summary>
        /// This holds the script for ChatController
        /// </summary>
        private ChatController chatController = null;

        /// <summary>
        /// This function is called when this script is rendered.
        /// </summary>
        /// <remarks>
        /// It will set all the scripts that are needed
        /// </remarks>
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
        /// <summary>
        /// This holds the game object of the cardArea
        /// </summary>
        [SerializeField] private GameObject cardArea = null;
        /// <summary>
        /// This holds the game object of the Round number indication
        /// </summary>
        [SerializeField] private GameObject roundNumberOBJ = null;
        /// <summary>
        /// This is the event option to send data to all other people currently in the room
        /// </summary>
        public RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others,
        };
        /// <summary>
        /// This is the event option to send data to all people currently in the room including the sender 
        /// </summary>
        public RaiseEventOptions AllPeople = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        /// <summary>
        /// The fuction is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            Debug.Log("Listen to event");
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
            Debug.Log("Event heard");
        }
        /// <summary>
        /// The fuction is called when the object becomes disabled and active.
        /// </summary>
        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
            Debug.Log("Event Ended");
        }
        /// <summary>
        /// This function will be called when a new data is receive 
        /// </summary>
        /// <param name="obj">
        /// The object package sent
        /// </param>
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
                Debug.Log("Input Arrangement : " + (string)arragement[0]);
                turnManager.inputArrangement((string)arragement[0]);
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
            else if (obj.Code == (byte)PhotonEventCode.receiveSomeoneCancelAsk)
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
                tradeControl.onReceiveChangeMissionCard((int)userControler.findPlayerPosition((Player)info[0]),(int)info[1],(Player)info[2]);
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
                playEntropy.onReceiveRolled((string)entropyLightningRolledJData[0],(bool)entropyLightningRolledJData[1]);
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
            else if (obj.Code == (byte)PhotonEventCode.LightningDone)
            {
                playEntropy.onReceiveDone();
            }
        }
    }
}
