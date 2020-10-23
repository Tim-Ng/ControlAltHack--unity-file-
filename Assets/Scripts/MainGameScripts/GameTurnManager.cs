using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class GameTurnManager : MonoBehaviourPunCallbacks
{
    public int TurnNumber = 0;
    public int PlayerIdToMakeThisTurn;
    public bool IsMyTurn
    {
        get
        {
            return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }
    public byte MyPoints = 0;
    public byte opponent1Points=0;
    public byte opponent2Points=0;
    public byte opponent3Points=0;
    public byte opponent4Points=0;
    public byte opponent5Points=0;

    public Player NextOpponent
    {
        get
        {

            Player opp = PhotonNetwork.LocalPlayer.GetNext();
            //Debug.Log("you: " + this.LocalPlayer.ToString() + " other: " + opp.ToString());
            return opp;
        }
    }
    public void HandoverTurnToNextPlayer()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            Player nextPlayer = PhotonNetwork.LocalPlayer.GetNextFor(this.PlayerIdToMakeThisTurn);
            if (nextPlayer != null)
            {
                this.PlayerIdToMakeThisTurn = nextPlayer.ActorNumber;
                return;
            }
        }

        this.PlayerIdToMakeThisTurn = 0;
    }

    public void setPlayerStart(int ActorNumber)
    {
        this.PlayerIdToMakeThisTurn = ActorNumber;
    }

}
