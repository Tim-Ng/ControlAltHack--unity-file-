using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace mainBrain
{
    public class TurnManager : MonoBehaviour
    {
        private int CurrentTurn;
        private List<int> arrangedActors = new List<int>();
        public int PlayerIdToMakeThisTurn;
        public bool doneTurn;
        public bool IsMyTurn
        {
            get
            {
                return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
            }
        }
    }
}
