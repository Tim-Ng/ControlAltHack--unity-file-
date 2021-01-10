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
    public class TurnManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject ScriptsODJ = null;

        private drawCharacterCard drawCard = null;
        private drawEntropyCard drawEntro= null;
        private EventHandeler EventManager = null;
        private drawMissionCard drawMission = null;
        private rollingMissionControl rollingMission = null;
        private UserAreaControlers userContorlAreas = null;
        private TradeControler tradeController= null;

        [SerializeField] private GameObject roundIndicator = null;
        private int CurrentTurn;
        private List<int> arrangedActors = new List<int>();
        public int PlayerIdToMakeThisTurn;
        public int currentPositionInArray;
        public int TurnNumber = 0;
        public int RoundNumber=1;
        private bool waiting= false;
        private List<int> actorsDone = new List<int>();
        private void Start()
        {
            drawCard = ScriptsODJ.GetComponent<drawCharacterCard>();
            drawEntro = ScriptsODJ.GetComponent<drawEntropyCard>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            rollingMission = ScriptsODJ.GetComponent<rollingMissionControl>();
            userContorlAreas = ScriptsODJ.GetComponent<UserAreaControlers>();
            tradeController = ScriptsODJ.GetComponent<TradeControler>();
        }
        public bool IsMyTurn
        {
            get
            {
                return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
            }
        }
        public void setArrangementForTurn()
        {
            List<PlayerInfo> userTemp = new List<PlayerInfo>();
            for (int i = 0; i< userContorlAreas.users.Count; i++)
            {
                if (userContorlAreas.users[i].filled && !userContorlAreas.users[i].fired)
                {
                    userTemp.Add(userContorlAreas.users[i]);
                }
            }
            Debug.LogWarning(userTemp.Count);
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
                for (int i = 0; i < userTemp.Count; i++)
                {
                    if (i == userTemp.Count - 1)
                    {
                        object[] arrangement = new object[] { userTemp[i].ActorID, false, true };
                        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.inputArrangement, arrangement, EventManager.AllPeople, SendOptions.SendReliable);
                    }
                    else if (i == 0)
                    {
                        object[] arrangement = new object[] { userTemp[i].ActorID, true, false };
                        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.inputArrangement, arrangement, EventManager.AllPeople, SendOptions.SendReliable);
                    }
                    else
                    {
                        object[] arrangement = new object[] { userTemp[i].ActorID, false, false };
                        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.inputArrangement, arrangement, EventManager.AllPeople, SendOptions.SendReliable);
                    }
                }
            }
            else
            {
                setWinnerList();
            }
        }
        public void inputArrangement(int actorID,bool first,bool last)
        {
            if (first)
            {
                arrangedActors.Clear();
                arrangedActors.Add(actorID);
            }
            else
            {
                arrangedActors.Add(actorID);
            }
            if (last)
            {
                CurrentTurn = 0;
                PlayerIdToMakeThisTurn = arrangedActors[0];
                Debug.Log("Set turn to " + CurrentTurn + " actor ID "+PlayerIdToMakeThisTurn);
                Debug.Log("My ID is" + PhotonNetwork.LocalPlayer.ActorNumber);
                if (PhotonNetwork.IsMasterClient && RoundNumber > userContorlAreas.AmountOfRounds && (userContorlAreas.users[userContorlAreas.findPlayerPosition(arrangedActors[0])] != userContorlAreas.users[userContorlAreas.findPlayerPosition(arrangedActors[1])]))
                {
                    setWinnerList();
                }
                if (IsMyTurn)
                {
                    checkTurn();
                }
            }
        }
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
        public void setNextRound()
        {
            TurnNumber += 1;
            RoundNumber =(int)Math.Ceiling(TurnNumber / 2.0);
            if (TurnNumber % 2 == 1)
            {
                rollingMission.switchStage(4);
                roundIndicator.SetActive(true);
                if (RoundNumber > userContorlAreas.AmountOfRounds)
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
                tradeController.HowManyPeople = 0;
                if (TurnNumber == 1)
                {
                    userContorlAreas.addMyCred(6);
                    drawEntro.drawEntropyCards(5);
                }
                else
                {
                    drawEntro.drawEntropyCards(1);
                    if (arrangedActors[0] == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        if (userContorlAreas.users[0].amountOfCred > userContorlAreas.users[userContorlAreas.findPlayerPosition(arrangedActors[1])].amountOfCred)
                        {
                            userContorlAreas.addMyMoney(1000);
                        }
                    }
                }
                drawMission.removeAllCard();
                if (userContorlAreas.users[0].characterScript.character_code == 7)
                {
                    drawMission.drawMissionCards(2);
                }
                else
                {
                    drawMission.drawMissionCards(1);
                }
                if (userContorlAreas.users[0].characterScript.character_code == 3)
                    userContorlAreas.addMyMoney(1000);
                else if (userContorlAreas.users[0].characterScript.character_code == 16)
                    userContorlAreas.addMyMoney(3000);
                else
                    userContorlAreas.addMyMoney(2000);

                EndTurn();
            }
            else if (TurnNumber % 2 == 0)
            {
                rollingMission.setRollTimeIsMyTurn();
            }
        }
        public void EndTurn()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerChanged, null, EventManager.AllPeople, SendOptions.SendReliable);
        }
        public void setWinnerList()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                userContorlAreas.onReceiveWinner1(PhotonNetwork.LocalPlayer.ActorNumber);
            }
            else
            {
                if (arrangedActors.Count == 1)
                {
                    object[] winnerData = new object[] { arrangedActors.Count, arrangedActors[0] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else if (arrangedActors.Count == 2)
                {
                    object[] winnerData = new object[] { arrangedActors.Count, arrangedActors[0], arrangedActors[1] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else if (arrangedActors.Count == 3)
                {
                    object[] winnerData = new object[] { arrangedActors.Count, arrangedActors[0], arrangedActors[1], arrangedActors[2] };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiveWinner, winnerData, EventManager.AllPeople, SendOptions.SendReliable);
                }
            }
        }
    }
}
