using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UserAreas;

namespace main
{
    public class TurnManager : MonoBehaviour
    {
        private int CurrentTurn;
        private List<int> arrangedActors = new List<int>();
        public int PlayerIdToMakeThisTurn;
        public int currentPositionInArray;
        public bool IsMyTurn
        {
            get
            {
                return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
            }
        }
        public void setArrangementForTurn()
        {
            
        }
    }
}
