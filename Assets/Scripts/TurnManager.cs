﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UserAreas;
using DrawCards;
using ExitGames.Client.Photon;
using System.Threading;

namespace main
{
    public class TurnManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private UserAreaControlers userContorlAreas = null;
        [SerializeField] private drawCharacterCard drawCard = null;
        [SerializeField] private EventHandeler EventManager = null;
        private int CurrentTurn;
        private List<int> arrangedActors = new List<int>();
        public int PlayerIdToMakeThisTurn;
        public int currentPositionInArray;
        private int TurnNumber = 0;
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
            if (CurrentTurn == arrangedActors.Count)
            {
                TurnNumber += 1;
                if (PhotonNetwork.IsMasterClient)
                {
                    //setArrangementForTurn();
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
        public void playerLeft(int ActorID)
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
            }
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerChanged,null, EventManager.AllPeople, SendOptions.SendReliable);
        }
        
    }
}
