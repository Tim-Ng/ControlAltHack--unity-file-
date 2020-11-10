using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEditor;
using System.Globalization;
using UnityEngine.Windows.Speech;

public class Main_Game_before_start : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject startButtonOBJ = null;
    public Text player_username, opponent1_username, opponent2_username, opponent3_username, opponent4_username, opponent5_username, Roomtext;
    private string opponet1Name, opponet2Name, opponet3Name, opponet4Name, opponet5Name;
    public List<string> oppententNameTextInfo = new List<string>();
    public Text amount_of_people;
    private int minmum_amount_of_people = 2;  // minimum amount of players
    public Button Start_Button, Leave_Button;
    List<Text> textOtherPlayers = new List<Text>();
    private PhotonView pv;
    [SerializeField] private PanelToTrade panelToTrade;
    private Player opponent1player, opponent2player, opponent3player, opponent4player, opponent5player;
    private List<int> otherPlayerListHoldAfterGame = new List<int>();
    private List<Player> otherPlayerList = new List<Player>();

    private bool isYouAreDeadBool;
    private int OnlyOneLeft; 
    public int getSetIfONlyOneLeft
    {
        get { return (OnlyOneLeft); }
        set { OnlyOneLeft = value; }
    }
    public List<int> getotherPlayerListHoldAfterGame
    {
        get { return otherPlayerListHoldAfterGame; }
    }
    public bool ifYouAreDead
    {
        get { return isYouAreDeadBool;  }
        set { isYouAreDeadBool = value; }
    }

    [SerializeField] private DrawCharacterCard drawCharacterCard;
    private void Start()
    {
        ifYouAreDead = false;
        otherPlayerList.Add(opponent1player);
        otherPlayerList.Add(opponent2player);
        otherPlayerList.Add(opponent3player);
        otherPlayerList.Add(opponent4player);
        otherPlayerList.Add(opponent5player);
        oppententNameTextInfo.Add(opponet1Name);
        oppententNameTextInfo.Add(opponet2Name);
        oppententNameTextInfo.Add(opponet3Name);
        oppententNameTextInfo.Add(opponet4Name);
        oppententNameTextInfo.Add(opponet5Name);
        textOtherPlayers.Add(opponent1_username);
        textOtherPlayers.Add(opponent2_username);
        textOtherPlayers.Add(opponent3_username);
        textOtherPlayers.Add(opponent4_username);
        textOtherPlayers.Add(opponent5_username);
        startButtonOBJ.SetActive(false);
        allow_Master_client();
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;
        player_username.text = PhotonNetwork.NickName;
        Leave_Button.enabled = true;
        pv = GetComponent<PhotonView>();
        pv.RPC("UpdateName", RpcTarget.All);
    }
    private void allow_Master_client()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!drawCharacterCard.getSetGameHasStart)
            {
                startButtonOBJ.SetActive(true);
                Start_Button.interactable = false;
            }
        }
    }
    [PunRPC]
    public void UpdateName()
    {
        allow_Master_client(); // check if master client changed
        if (!drawCharacterCard.getSetGameHasStart)
        {
            for (int j = 0; j < textOtherPlayers.Count; j++)
            {
                textOtherPlayers[j].text = "Waiting for Player ";
            }
            int i = 0;
            Debug.Log("A player entered in put name and recount of people");
            foreach (Player otherplayer in PhotonNetwork.PlayerListOthers)
            {
                otherPlayerList[i] = otherplayer;
                oppententNameTextInfo[i] = otherplayer.NickName;
                textOtherPlayers[i].text = oppententNameTextInfo[i];
                i++;
            }
            Start_Button.interactable = !(PhotonNetwork.CurrentRoom.PlayerCount < minmum_amount_of_people);
            amount_of_people.text = (PhotonNetwork.CurrentRoom.PlayerCount).ToString() + "/6";
        }
    }
    public override void OnPlayerLeftRoom(Player LeavingPlayer)
    {
        if (!isActorIdInList(LeavingPlayer.ActorNumber))
            drawCharacterCard.removeAvertar(findPlayerPosition(LeavingPlayer));
        if (LeavingPlayer.IsMasterClient)
        {
            Debug.Log("Host named " + LeavingPlayer.NickName + " has left the room");
            Debug.Log("Host is changed to player named " + PhotonNetwork.PlayerList[0].NickName);
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
        }
        OnlyOneLeft -= 1;
        for (int i = 0; i < otherPlayerList.Count; i++)
        {
            if (LeavingPlayer == otherPlayerList[i])
            {
                otherPlayerList[i] = null;
                break;
            }
        }
        if (drawCharacterCard.getSetGameHasStart)
        {
            panelToTrade.setHoldDoneList(false, LeavingPlayer);
            panelToTrade.setPannelDoneList(false, LeavingPlayer);
            if (OnlyOneLeft == 1)
            {
                drawCharacterCard.sendAllSomeoneWin();
            }
            if (PhotonNetwork.IsMasterClient)
            {
                if (drawCharacterCard.PlayerIdToMakeThisTurn == LeavingPlayer.ActorNumber)
                {
                    Debug.Log("Player left turn so ending turn");
                    drawCharacterCard.EndTurn();
                }
            }
            int i = 0;
            foreach (int otherplayer in otherPlayerListHoldAfterGame)
            {
                if (otherplayer == LeavingPlayer.ActorNumber)
                {
                    textOtherPlayers[i].text = "Player Has Left";
                }
                i++;
            }
            drawCharacterCard.checkWhichFunctionToRun();
            panelToTrade.isAllDoneAttendance();
        }
        else
        {
            UpdateName();
        }
        Debug.Log(LeavingPlayer.NickName + " has left room");
        
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Changing scene");
        PhotonNetwork.LoadLevel(0);
    }
    public void clickOnLeaveGame()
    { 
        PhotonNetwork.LeaveRoom();
        Debug.Log("PlayerLeftRoom... loading scene");
    }
    public string getOtherPlayerName(int which)
    {
        return oppententNameTextInfo[which];
    }
    public int findPlayerPosition(Player PlayerToCheck)
    {
        int i = 0;
        foreach (Player CheckPlayer in otherPlayerList)
        {
            if (CheckPlayer == PlayerToCheck)
            {
                break;
            }
            i++;
        }
        return i;
    }
    public Player FindPlayerUsingActorId(int ActorId)
    {
        foreach (Player CheckPlayer in PhotonNetwork.PlayerList)
        {
            if (CheckPlayer.ActorNumber == ActorId)
            {
                return CheckPlayer;
            }
        }
        Debug.Log("Can't Find Player!!!!!!!!!!!!!!");
        return null;
    }
    public int positionOfHadPlayed(Player whichPlayer)
    {
        int i = 0;
        foreach (Player Checkplayer in otherPlayerList)
        {
            if (Checkplayer == whichPlayer)
            {
                break;
            }
            i++;
        }
        return i;
    }
    public List<Player> getPlayerList()
    {
        List<Player> tempList = new List<Player>();
        foreach (Player chackPlayer in otherPlayerList)
        {
            if (chackPlayer != null)
            {
                tempList.Add(chackPlayer);
            }
        }
        return tempList;
    }
    public List<Player> allPlayerList()
    {
        List<Player> tempPlayerList = new List<Player>();
        tempPlayerList.Add(PhotonNetwork.LocalPlayer);
        foreach (Player inList in getPlayerList())
        {
            tempPlayerList.Add(inList);
        }
        return tempPlayerList;
    }
    public void setHoldPlayerListAfterStartGame()
    {
        otherPlayerListHoldAfterGame.Clear();
        foreach (Player ActorPlayer in PhotonNetwork.PlayerListOthers)
        {
            otherPlayerListHoldAfterGame.Add( ActorPlayer.ActorNumber);
        }
    }
    public bool isActorIdInList(int ActorId)
    {
        if ((PhotonNetwork.LocalPlayer.ActorNumber==ActorId))
        {
            return true;
        }
        foreach (Player CheckPlayer in getPlayerList())
        {
            if (ActorId == CheckPlayer.ActorNumber)
            {
                return true;
            }
        }
        return false;
    }
    public void ResetMainGameBeforeStart()
    {
        Start();
        int i = 0;
        foreach (Player otherplayer in PhotonNetwork.PlayerListOthers)
        {
            otherPlayerList[i] = otherplayer;
            i++;
        }
    }
}
