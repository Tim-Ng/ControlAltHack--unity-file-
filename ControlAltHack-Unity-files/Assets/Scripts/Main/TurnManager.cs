using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UserAreas;
using DrawCards;
using ExitGames.Client.Photon;
using rollmissions;
using UnityEngine.UI;
using System;
using TradeScripts;

namespace main
{
    /// <summary>
    /// This is used to package List<int> to convert it into json and send it to other other players
    /// </summary>
    public struct ListContainer
    {
        public List<int> datalist;
        public ListContainer(List<int> dataList)
        {
            datalist = dataList;
        }
    }
    /// <summary>
    /// This is the script that determines who's turn is next, the current round number, when the game ends and setting the winner popup.
    /// </summary>
    public class TurnManager : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// This is the game object where this script is attatched to.
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// Holds the script drawCharacterCard
        /// </summary>
        private drawCharacterCard drawCard = null;
        /// <summary>
        /// Holds the script drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntro= null;
        /// <summary>
        /// Holds the script EventHandeler
        /// </summary>
        private EventHandeler EventManager = null;
        /// <summary>
        /// Holds the script drawMissionCard
        /// </summary>
        private drawMissionCard drawMission = null;
        /// <summary>
        /// Holds the script rollingMissionControl
        /// </summary>
        private rollingMissionControl rollingMission = null;
        /// <summary>
        /// Holds the script UserAreaControlers
        /// </summary>
        private UserAreaControlers userControlAreas = null;
        /// <summary>
        /// Holds the script TradeControler
        /// </summary>
        private TradeControler tradeController= null;

        /// <summary>
        /// This is the game object that displays the round number
        /// </summary>
        [SerializeField] private GameObject roundIndicator = null;
        /// <summary>
        /// This the value of the position of who's turn it is in the arrangedActor list
        /// </summary>
        [HideInInspector]
        private int CurrentTurn;
        /// <summary>
        /// This list is used to hold the arranged player list on who is next 
        /// </summary>
        /// <remarks>
        /// This is determine by the host and share to everyother player in the room
        /// </remarks>
        private List<int> arrangedActors = new List<int>();
        /// <summary>
        /// The Actor number of the current player who has the turn
        /// </summary>
        [HideInInspector]
        public int PlayerIdToMakeThisTurn;
        /// <summary>
        /// This is the turn number which is around 2 times the round number which is used to check what to do 
        /// </summary>
        [HideInInspector]
        public int TurnNumber = 0;
        /// <summary>
        /// This is the number that hold current RoundNumber 
        /// </summary>
        [HideInInspector]
        public int RoundNumber=1;
        /// <summary>
        /// This is used to wait till everyone is done and then the game will continue to count the TurnNumber
        /// </summary>
        private bool waiting= false;
        /// <summary>
        /// This is used to check the number of people who are done
        /// </summary>
        private List<int> actorsDone = new List<int>();
        /// <summary>
        /// When the script is loaded this function will fill in the data for the scripts that we this class needs
        /// </summary>
        private void Start()
        {
            ScriptsODJ = gameObject;
            drawCard = ScriptsODJ.GetComponent<drawCharacterCard>();
            drawEntro = ScriptsODJ.GetComponent<drawEntropyCard>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            rollingMission = ScriptsODJ.GetComponent<rollingMissionControl>();
            userControlAreas = ScriptsODJ.GetComponent<UserAreaControlers>();
            tradeController = ScriptsODJ.GetComponent<TradeControler>();
        }
        /// <summary>
        /// This is used to check if it is your turn
        /// </summary>
        /// <remarks>
        /// Will check your ActorNumber with PlayerIdToMakeThisTurn <br/>
        /// If ActorNumber == PlayerIdToMakeThisTurn the true <br/>
        /// Else false
        /// </remarks>
        public bool IsMyTurn
        {
            get
            {
                return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
            }
        }
        /// <summary>
        /// This function is only run by the host at the start of every Round.
        /// </summary>
        /// <remarks>
        /// This function will arrage according to the player's cred from the highest to the lowest <br/>
        /// If the cred of two player is the same that the player with the lowest Actor Number
        /// will be place before the person with the higher Actor Number. <br/>
        /// Then this function will send the list to other players in the room.
        /// </remarks>
        public void setArrangementForTurn()
        {
            List<PlayerInfo> userTemp = new List<PlayerInfo>();
            for (int i = 0; i< userControlAreas.users.Count; i++)
            {
                if (userControlAreas.users[i].filled && !userControlAreas.users[i].fired)
                {
                    userTemp.Add(userControlAreas.users[i]);
                }
            }
            if (userTemp.Count != 1)
            {
                PlayerInfo holdInfo;
                for (int j = 0; j <= userTemp.Count - 2; j++)
                {
                    for (int i = 0; i <= userTemp.Count - 2; i++)
                    {
                        if (userTemp[i].amountOfCred > userTemp[i + 1].amountOfCred)
                        {
                            holdInfo = userTemp[i];
                            userTemp[i] = userTemp[i + 1];
                            userTemp[i + 1] = holdInfo;
                        }
                        else if (userTemp[i].amountOfCred == userTemp[i + 1].amountOfCred)
                        {
                            if (userTemp[i].ActorID < userTemp[i + 1].ActorID)
                            {
                                holdInfo = userTemp[i];
                                userTemp[i] = userTemp[i + 1];
                                userTemp[i + 1] = holdInfo;
                            }
                        }
                    }
                }
                userTemp.Reverse();
                List<int> tempActorID = new List<int>();
                for (int i = 0; i< userTemp.Count; i++)
                {
                    tempActorID.Add(userTemp[i].ActorID);
                }
                ListContainer container = new ListContainer(tempActorID);
                object[] arrangement = new object[] { JsonUtility.ToJson(container) };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.inputArrangement, arrangement, EventManager.AllPeople, SendOptions.SendReliable);
            }
            else
            {
                setWinnerList();
            }
        }
        /// <summary>
        /// This function is to set the arragement of all the players turn.
        /// </summary>
        /// <remarks>
        /// It will then set the player with the current turn to be the first on the list
        /// </remarks>
        /// <param name="jsonList"> A json string to convert the data into ListContainer</param>
        public void inputArrangement(string jsonList)
        {
            arrangedActors = JsonUtility.FromJson<ListContainer>(jsonList).datalist;
            CurrentTurn = 0;
            PlayerIdToMakeThisTurn = arrangedActors[0];
            Debug.Log("Set turn to " + CurrentTurn + " actor ID "+PlayerIdToMakeThisTurn);
            Debug.Log("My ID is" + PhotonNetwork.LocalPlayer.ActorNumber);
            if (PhotonNetwork.IsMasterClient && (RoundNumber > userControlAreas.AmountOfRounds) && (userControlAreas.users[userControlAreas.findPlayerPosition(arrangedActors[0])].amountOfCred != userControlAreas.users[userControlAreas.findPlayerPosition(arrangedActors[1])].amountOfCred))
            {
                setWinnerList();
            }
            if (IsMyTurn)
            {
                checkTurn();
            }
        }
        /// <summary>
        /// This is to receive then the turn is changed to a next player / the player with the current turn has end his/her turn
        /// </summary>
        public void playerChanged()
        {
            CurrentTurn += 1;
            if (CurrentTurn >= arrangedActors.Count)
            {
                if (TurnNumber == 0 || TurnNumber % 2 == 1)
                {
                    waiting = true;
                    actorsDone.Clear();
                }
                else
                {
                    waiting = false;
                    setNextRound();
                }
            }
            else
            {
                PlayerIdToMakeThisTurn = arrangedActors[CurrentTurn];
                Debug.Log("Set turn to " + CurrentTurn + " actor ID " + PlayerIdToMakeThisTurn);
                if (IsMyTurn)
                {
                    checkTurn();
                }
            }
        }
        /// <summary>
        /// This function is called when a player has left/done in the game.
        /// </summary>
        /// <remarks>
        /// This functino will also check if all the players are done. <br/>
        /// If they are done then the function setNextRound will be called.
        /// </remarks>
        /// <param name="Actorid"></param>
        /// <param name="left">
        /// If true the player has left and will be remove form the actorsDone list if he/she was done with their turn<br/>
        /// Else the player will be added in the actorsDone list if they havent yet.
        /// </param>
        public void actorsDoneEdit(int Actorid, bool left)
        {
            if (left)
            {
                if (actorsDone.Contains(Actorid))
                {
                    actorsDone.Remove(Actorid);
                }
            }
            else
            {
                if (!actorsDone.Contains(Actorid))
                {
                    actorsDone.Add(Actorid);
                }
            }
            //check 
            if (actorsDone.Count == arrangedActors.Count)
            {
                setNextRound();
            }
        }
        /// <summary>
        /// This function will set next round.
        /// </summary>
        /// <remarks>
        /// TurnNumber plus 1  <br/>
        /// RoundNumber will be calculated using TurnNumber <br/>
        /// The TurnNumber will be check and the function will act accordingly
        /// </remarks>
        public void setNextRound()
        {
            TurnNumber += 1;
            RoundNumber =(int)Math.Ceiling(TurnNumber / 2.0);
            if (TurnNumber % 2 == 1)
            {
                rollingMission.switchStage(4);
                roundIndicator.SetActive(true);
                if (RoundNumber > userControlAreas.AmountOfRounds)
                    roundIndicator.GetComponent<Text>().text = "Tie Breaker Round";
                else
                    roundIndicator.GetComponent<Text>().text = "Round " + RoundNumber;
            }
            else if (TurnNumber % 2 == 0)
                roundIndicator.SetActive(false);
            waiting = false;
            if (PhotonNetwork.IsMasterClient)
            {
                setArrangementForTurn();
            }
        }
        /// <summary>
        /// This function is called when a player is either fired or left the game
        /// </summary>
        /// <param name="ActorID"></param>
        public void playerLeft(int ActorID)
        {
            if (PlayerIdToMakeThisTurn == ActorID)
            {
                playerChanged();
            }
            if (arrangedActors.Contains(ActorID))
            {
                arrangedActors.Remove(ActorID);
            }
            if (arrangedActors.Count <= 1)
            {
                setWinnerList();
            }
            if (waiting)
            {
                actorsDoneEdit(ActorID, true);
            }
        }
        /// <summary>
        /// This function is called to check what to do by looking at the TurnNumber and act accordingly
        /// </summary>
        public void checkTurn()
        {
            if (TurnNumber == 0)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 6)
                {
                    drawCard.drawCharCards(2);
                }
                else
                {
                    drawCard.drawCharCards(3);
                }
                EndTurn();
            }
            else if (TurnNumber % 2 == 1)
            {
                tradeController.resetHasAttended();
                tradeController.HowManyPeople = 0;
                if (TurnNumber == 1)
                {
                    userControlAreas.addMyCred(6);
                    drawEntro.drawEntropyCards(5);
                }
                else
                {
                    drawEntro.drawEntropyCards(1);
                    if (arrangedActors[0] == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        if (userControlAreas.users[0].amountOfCred > userControlAreas.users[userControlAreas.findPlayerPosition(arrangedActors[1])].amountOfCred)
                        {
                            userControlAreas.addMyMoney(1000);
                        }
                    }
                }
                drawMission.removeAllCard();
                if (userControlAreas.users[0].characterScript.character_code == 7)
                {
                    drawMission.drawMissionCards(2);
                }
                else
                {
                    drawMission.drawMissionCards(1);
                }
                if (userControlAreas.users[0].characterScript.character_code == 3)
                    userControlAreas.addMyMoney(1000);
                else if (userControlAreas.users[0].characterScript.character_code == 16)
                    userControlAreas.addMyMoney(3000);
                else
                    userControlAreas.addMyMoney(2000);

                EndTurn();
            }
            else if (TurnNumber % 2 == 0)
            {
                rollingMission.setRollTimeIsMyTurn();
            }
        }
        /// <summary>
        /// This function is used to end a player's turn
        /// </summary>
        public void EndTurn()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerChanged, null, EventManager.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is used to set the winner and display the winner list
        /// </summary>
        public void setWinnerList()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                userControlAreas.onReceiveWinner1(PhotonNetwork.LocalPlayer.ActorNumber);
            }
            else
            {
                if (arrangedActors.Count == 1)
                {
                    object[] winnerData = new object[] { 1, arrangedActors[0] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else if (arrangedActors.Count == 2)
                {
                    object[] winnerData = new object[] { 2, arrangedActors[0], arrangedActors[1] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else if (arrangedActors.Count >= 3)
                {
                    object[] winnerData = new object[] { 3, arrangedActors[0], arrangedActors[1], arrangedActors[2] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
            }
        }
    }
}
