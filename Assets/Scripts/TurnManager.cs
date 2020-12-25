using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UserAreas;
using DrawCards;
using ExitGames.Client.Photon;
using System.Threading;
using rollmissions;
using UnityEngine.UI;
using System;

namespace main
{
    public class TurnManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private UserAreaControlers userContorlAreas = null;
        [SerializeField] private drawCharacterCard drawCard = null;
        [SerializeField] private drawEntropyCard drawEntro= null;
        [SerializeField] private EventHandeler EventManager = null;
        [SerializeField] private drawMissionCard drawMission = null;
        [SerializeField] private rollingMissionControl rollingMission = null;
        [SerializeField] private GameObject roundIndicator = null;
        private int CurrentTurn;
        private List<int> arrangedActors = new List<int>();
        public int PlayerIdToMakeThisTurn;
        public int currentPositionInArray;
        public int TurnNumber = 0;
        public int RoundNumber;
        private bool waiting= false;
        private List<int> actorsDone = new List<int>();
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
                if (userContorlAreas.users[i].filled)
                {
                    userTemp.Add(userContorlAreas.users[i]);
                }
            }
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
                Debug.Log("In Host " + userTemp[i].ActorID);
            }
            for (int i = 0; i<userTemp.Count; i++)
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
                for (int i = 0; i < actorsDone.Count; i++)
                {
                    if (actorsDone[i] == Actorid)
                    {
                        actorsDone.Remove(Actorid);
                        break;
                    }
                }
            }
            else
            {
                actorsDone.Add(Actorid);
            }
            //check 
            if (actorsDone.Count == PhotonNetwork.CurrentRoom.PlayerCount)
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
            else
            {
                for (int i = CurrentTurn; i < arrangedActors.Count; i++)
                {
                    if (arrangedActors[i] == ActorID)
                    {
                        arrangedActors.Remove(ActorID);
                        break;
                    }
                }
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
            else if (TurnNumber %2 == 0)
            {
                rollingMission.setRollTimeIsMyTurn();
            }
            
        }
        public void EndTurn()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerChanged, null, EventManager.AllPeople, SendOptions.SendReliable);
        }
    }
}
