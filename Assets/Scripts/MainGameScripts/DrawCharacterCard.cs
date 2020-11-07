using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawCharacterCard : MonoBehaviourPunCallbacks

{
    [SerializeField] private MoneyAndPoints moneyAndPointScripts;
    public GameObject PlayerArea;
    public GameObject cardTemplate;
    [SerializeField] private CharacterCardDeck cardDeck = null;
    public GameObject StartGameButtonOBJ;
    private int x;
    public GameObject LeaveRoomButton;

    public MissionCardDeck missionCard;

    public popupcardwindowChar Popup;
    public GameObject clone_to_delete;
    public GameObject select_button;
    public DrawEntropyCard entropyCard;

    public Image avertarUser, avertarPlayer1, avertarPlayer2, avertarPlayer3, avertarPlayer4, avertarPlayer5;
    public Sprite defultImage;
    private CharCardScript chosed_character_user = null, chosed_character_player1 = null, chosed_character_player2 = null, chosed_character_player3 = null, chosed_character_player4 = null, chosed_character_player5 = null;
    public Button userAvertarButton, Player1AvertarButton, Player2AvertarButton, Player3AvertarButton, Player4AvertarButton, Player5AvertarButton;
    private List<CharCardScript> cardsInfoDraw = new List<CharCardScript>();
    private List<CharCardScript> otherPlayerCharacterInfo = new List<CharCardScript>();
    private List<Image> otherPlayerAvertar = new List<Image>();
    private List<Button> otherAvertarPlayerButton = new List<Button>();
    private int number_of_players, number_of_character_cards;
    [SerializeField] private TMP_InputField numberOfRounds_input = null, numberOfCredAhead_input = null;
    [SerializeField] private rollTime rollTimeScript;

    private List<Player> whichPlayerLeading = new List<Player>();
    private List<byte> AllPlayerPoint = new List<byte>();
    public static int[] GameProperties = { 0, 0 };

    [SerializeField] private DrawMissionCard drawMissionCard;

    [SerializeField] private Main_Game_before_start main_Game_Before_Start;

    private int nextMyPlayerId;
    private bool returnToPlayerStartAround;
    public bool getSetreturnToPlayerStartAround
    {
        get { return returnToPlayerStartAround; }
        set { returnToPlayerStartAround = value; }
    }
    private bool gameHasStart = false;
    public bool getSetGameHasStart
    {
        get{return gameHasStart;}
        set { gameHasStart = value; }
    }

    [SerializeField] private GameObject gameWinArea;
    [SerializeField] private GameObject firstPlaceOBJ, sendPlaceOBJ, thirdPlaceOBJ;
    [SerializeField] private Image AverPlace1, AverPlace2, AverPlace3;
    [SerializeField] private Text Nickwin1, Nickwin2, Nickwin3;

    private int ActorNumberOfStartPlayer; // player that start the turn
    private int NumDoneSelectChar = 0;

    private MissionCardScript currentUserMission = null;

    [SerializeField] private PanelToTrade panelToTrade;
 
    private RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.Others,
    };
    private RaiseEventOptions AllPeople = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.All
    };
    private enum PhotonEventCode
    {
        LeaveButton = 0,
        UpdateTurnPlayer = 1,
        SelectChar = 2,
        RemoveCharCard = 3,
        sortToMaster=4,
        TurnChanged =5,
        CheckAllDoneChar=6,
        winnnerDone = 18,
    }

    public int TurnNumber = 0;
    public int PlayerIdToMakeThisTurn;
    public bool IsMyTurn
    {
        get
        {
            return this.PlayerIdToMakeThisTurn == PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }
    public GameObject tableStartContent;
    void Start()
    {
        putCharCardsInList();
    }
    public void closeStartContentGame()
    {
        tableStartContent.SetActive(false);
    }

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
        if (obj.Code == (byte)PhotonEventCode.LeaveButton)
        {
            main_Game_Before_Start.setHoldPlayerListAfterStartGame();
            gameHasStart = true;
            noleave();
            closeStartContentGame();
        }
        else if (obj.Code == (byte)PhotonEventCode.UpdateTurnPlayer)
        {
            object[] carddata = (object[])obj.CustomData;
            PlayerIdToMakeThisTurn = (int)carddata[0];
            checkWhichFunctionToRun();
        }
        else if (obj.Code == (byte)PhotonEventCode.RemoveCharCard)
        {
            object[] carddata = (object[])obj.CustomData;
            int datacode = (int)carddata[0];
            RemoveThisCard(datacode);
        }
        else if (obj.Code == (byte)PhotonEventCode.SelectChar)
        {
            object[] characterInfo = (object[])obj.CustomData;
            SetCharacterInfo((int)characterInfo[0], (Player)characterInfo[1]);
        }
        else if (obj.Code == (byte)PhotonEventCode.sortToMaster)
        {
            nextPlayerOnLeadingList(PlayerIdToMakeThisTurn);
            MasterSortTurn();
        }
        else if (obj.Code == (byte)PhotonEventCode.TurnChanged)
        {
            object[] carddata = (object[])obj.CustomData;
            TurnNumber = (int)carddata[0];
            if (TurnNumber > 1 && TurnNumber % 2 != 0)
            {
                rollTimeScript.restRollTime();
            }
        }
        else if (obj.Code == (byte)PhotonEventCode.CheckAllDoneChar)
        {
            Debug.Log("A player is done selecting character");
            NumDoneSelectChar += 1;
            Debug.Log(NumDoneSelectChar + "and round is " +TurnNumber + " number of people on room " + PhotonNetwork.CurrentRoom.PlayerCount);
            checkWhichFunctionToRun();
        }
        else if (obj.Code == (byte)PhotonEventCode.winnnerDone)
        {
            object[] whowin = (object[])obj.CustomData;
            gameEndDisplayWin((int)whowin[0], (int)whowin[1], (int)whowin[2]);
        }
    }
    private void putCharCardsInList()
    {
        Debug.Log("Input Character card into list");
        cardsInfoDraw = cardDeck.getCharDeck();
        otherPlayerCharacterInfo.Add(chosed_character_player1);
        otherPlayerCharacterInfo.Add(chosed_character_player2);
        otherPlayerCharacterInfo.Add(chosed_character_player3);
        otherPlayerCharacterInfo.Add(chosed_character_player4);
        otherPlayerCharacterInfo.Add(chosed_character_player5);
        avertarUser.sprite = defultImage;
        avertarPlayer1.sprite = defultImage;
        avertarPlayer2.sprite = defultImage;
        avertarPlayer3.sprite = defultImage;
        avertarPlayer4.sprite = defultImage;
        avertarPlayer5.sprite = defultImage;
        otherPlayerAvertar.Add(avertarPlayer1);
        otherPlayerAvertar.Add(avertarPlayer2);
        otherPlayerAvertar.Add(avertarPlayer3);
        otherPlayerAvertar.Add(avertarPlayer4);
        otherPlayerAvertar.Add(avertarPlayer5);
        otherAvertarPlayerButton.Add(Player1AvertarButton);
        otherAvertarPlayerButton.Add(Player2AvertarButton);
        otherAvertarPlayerButton.Add(Player3AvertarButton);
        otherAvertarPlayerButton.Add(Player4AvertarButton);
        otherAvertarPlayerButton.Add(Player5AvertarButton);
        foreach (Button which in otherAvertarPlayerButton)
        {
            which.interactable = false;
        }
        userAvertarButton.interactable = false;
    }
    // when host click on the start button;
    public void OnClickTodrawCard()
    {
        StartGameButtonOBJ.SetActive(false);
        if (!(string.IsNullOrEmpty(numberOfRounds_input.text)))
        {
            if ((numberOfRounds_input.text).All(char.IsDigit))
                GameProperties[0] = int.Parse(numberOfRounds_input.text);
            else
                GameProperties[0] = 0;
        }
        if (!(string.IsNullOrEmpty(numberOfCredAhead_input.text)))
        {
            if ((numberOfCredAhead_input.text).All(char.IsDigit))
                GameProperties[1] = int.Parse(numberOfCredAhead_input.text);
            else
                GameProperties[1] = 0;
        }
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log("Send to all draw character");
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.LeaveButton, null, AllPeople, SendOptions.SendReliable); 
        checkTurn();
    }
    private void noleave() => LeaveRoomButton.SetActive(false);

    //Distribute card accordingly 
    public void Drawcard(int y)
    {
        for (var i = 0; i < y; i++)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            x = rand.Next(0, cardsInfoDraw.Count);
            Debug.Log("Random Number this loop is:" + x);
            GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
            characterPlayerCard1.name = cardsInfoDraw[x].character_card_name;
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().CharCard = cardsInfoDraw[x];
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().FrontSide.SetActive(true);
            characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            object[] data = new object[] { cardsInfoDraw[x].character_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.RemoveCharCard, data, AllPeople, SendOptions.SendReliable);
            Thread.Sleep(175);
        }
        Debug.Log("Got pass to here");
    }

    //when the local player picked on a character
    public void clickOnSelectCard()
    {
        select_button.SetActive(false);
        foreach (Transform child in clone_to_delete.transform)
        {
            Destroy(child.gameObject);
        }
        chosed_character_user = Popup.GetCharCardScript();
        avertarUser.sprite = chosed_character_user.image_Avertar;
        object[] dataSelectCard = new object[] { chosed_character_user.character_code, PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.SelectChar, dataSelectCard, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        userAvertarButton.interactable = true;
        Popup.closePopup();
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.CheckAllDoneChar, null, AllPeople, SendOptions.SendReliable);
        if (chosed_character_user.character_code == 15)
        {
            moneyAndPointScripts.addMyMoney(3000);
        }
        else if (chosed_character_user.character_code == 1)
        {
            moneyAndPointScripts.addMyMoney(1000);
        }
        else
        {
            moneyAndPointScripts.addMyMoney(2000);
        }
        moneyAndPointScripts.addPoints(6);
    }

    //to remove this card from the deck for everyone so that we don't get the same character
    public void RemoveThisCard(int cardID)
    {
        foreach (CharCardScript checkCard in cardsInfoDraw)
        {
            if (cardID == checkCard.character_code)
            {
                cardsInfoDraw.Remove(checkCard);
                Debug.Log("Character Card ID :" + cardID + " is removed");
                break;
            }
        }
        Debug.Log("Number of Character cards left in deck " + cardsInfoDraw.Count);
    }

    //find which player selected the card in your point of view 
    public void SetCharacterInfo(int character_code, Player sendingPlayer)
    {
        Debug.Log("setting opponents avertar");
        int i = 0;
        foreach (Player whichplayer in PhotonNetwork.PlayerListOthers)
        {
            if (sendingPlayer == whichplayer)
            {
                break;
            }
            i++;
        }
        foreach (CharCardScript charScript in cardDeck.getCharDeck())
        {
            Debug.Log("card id :");
            if (charScript.character_code == character_code)
            {
                Debug.Log("Avetar of player " + sendingPlayer.NickName + " is set ");
                otherPlayerCharacterInfo[i] = charScript;
                otherPlayerAvertar[i].sprite = otherPlayerCharacterInfo[i].image_Avertar;
                otherAvertarPlayerButton[i].interactable = true;
                break;
            }
        }

    }
    // when the avertar is click 
    public void PopUpCharacterInfo(int i)
    {
        if ((i != 5))
        {
            Popup.openCharacterCard(otherPlayerCharacterInfo[i], false);
        }
        else
        {
            Popup.openCharacterCard(chosed_character_user, false);
        }
    }
    //all of the different avertars user and the opponents 
    public void clickAvertarPlayer1() => PopUpCharacterInfo(0);
    public void clickAvertarPlayer2() => PopUpCharacterInfo(1);
    public void clickAvertarPlayer3() => PopUpCharacterInfo(2);
    public void clickAvertarPlayer4() => PopUpCharacterInfo(3);
    public void clickAvertarPlayer5() => PopUpCharacterInfo(4);
    public void clickAvertarPlayerUser() => PopUpCharacterInfo(5);

    // find the next player after you
    public Player NextOpponent
    {
        get
        {
            Player opp = PhotonNetwork.LocalPlayer.GetNext();
            //Debug.Log("you: " + this.LocalPlayer.ToString() + " other: " + opp.ToString());
            return opp;
        }
    }

    //hading over the turn to the next player
    public void handOverToNextPlayerIfPlayerIsStartAndLeft(Player playerInQuestion)
    {
        int i = 0;
        foreach (Player PlayerCheck in main_Game_Before_Start.getPlayerList())
        {
            if (PlayerCheck == playerInQuestion)
            {
                break;
            }
            i++;
        }
        if (i == (main_Game_Before_Start.getotherPlayerListHoldAfterGame.Count - 1))
        {
            ActorNumberOfStartPlayer = PhotonNetwork.MasterClient.ActorNumber;
        }
        else
        {
            ActorNumberOfStartPlayer = main_Game_Before_Start.getotherPlayerListHoldAfterGame[i + 1];
        }
    }
    // Check the number of players and distribute the number of character cards accordingly 
    public void countNumCharCards()
    {
        number_of_players = PhotonNetwork.CurrentRoom.PlayerCount;
        if (number_of_players == 6)
        {
            Debug.Log("Number of players is 6 , distributing 2 cards");
            number_of_character_cards = 2;
        }
        else
        {
            Debug.Log("Number of players is " + number_of_players + ", distributing 3 cards");
            number_of_character_cards = 3;
        }
    }
    public void checkWhichFunctionToRun()
    {
        if ((TurnNumber == 0) && IsMyTurn)
        {
            countNumCharCards();
            Drawcard(number_of_character_cards);
            EndTurn();
        }
        else if ((TurnNumber % 2 != 0) && IsMyTurn)
        {
            if (NumDoneSelectChar == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("Entropy Draw");
                if (TurnNumber == 1)
                {
                    entropyCard.distribute_entropycard(5);
                }
                drawMissionCard.whoDrawMissionCard(chosed_character_user.character_code);
                EndTurn();
            }  
        }
        else if (TurnNumber % 2 == 0 && panelToTrade.getEveroneDone() && IsMyTurn)
        {
            rollTimeScript.startRollTurn();
        }
    }
    public CharCardScript getPlayerCharterSet(Player playerInQuestion)
    {
        if (playerInQuestion == PhotonNetwork.LocalPlayer)
        {
            return chosed_character_user;
        }
        else
        {
            int i = 1;
            foreach (Player checkPlayer in PhotonNetwork.PlayerListOthers)
            {
                if (checkPlayer == playerInQuestion)
                {
                    return otherPlayerCharacterInfo[i];
                }
                i += 1;
            }
        }
        return null;
    }
    public void sendAllSomeoneWin()
    {
        List<int> actorNumberTemp = new List<int>();
        actorNumberTemp.Add(999);
        actorNumberTemp.Add(999);
        actorNumberTemp.Add(999);
        int i=0;
        foreach (Player checkPlayer in PhotonNetwork.PlayerListOthers)
        {
            actorNumberTemp[i] = checkPlayer.ActorNumber;
            if (i == 3)
            {
                break;
            }
            i++;
        }
        object[] top3winner = new object[] { actorNumberTemp[0], actorNumberTemp[1] , actorNumberTemp[2] };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.winnnerDone, top3winner, AllPeople, SendOptions.SendReliable);
    }
    public void setCurrentMissionScript(MissionCardScript missionScriptSet)
    {
        currentUserMission = missionScriptSet;
    }
    public MissionCardScript getCurrentMissionScript()
    {
        return currentUserMission;
    }
    public MissionCardScript getWhichMissionCardScript(string whichCardId)
    {
        foreach (MissionCardScript checkMissionScript in missionCard.getMissionCardDeck())
        {
            if (checkMissionScript.Mission_code == whichCardId)
            {
                return checkMissionScript;
            }
        }
        Debug.LogError("No card Found");
        return null;
    }
    public CharCardScript getMyCharScript()
    {
        return chosed_character_user;
    }
    public CharCardScript getWhichOtherCharScript(int which)
    {
        return otherPlayerCharacterInfo[which];
    }
    public void updatePointsToChecker(byte pointsSender, Player SenderPlayer)
    {
        int i = 0;
        foreach (Player CheckPlayer in whichPlayerLeading)
        {
            if (CheckPlayer == SenderPlayer)
            {
                AllPlayerPoint[i] = pointsSender;
            }
            i++;
        }
    }
    public void gameEndDisplayWin(int firstPlace , int secondPlace, int thirdPlace )
    {
        gameWinArea.SetActive(true);
        if (secondPlace==999 && thirdPlace == 999)
        {
            Debug.Log("Win only 1 player");
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(firstPlace), firstPlaceOBJ, Nickwin1, AverPlace1);
        }
        else if (secondPlace == 999)
        {
            Debug.Log("Win only 2 player");
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(firstPlace), firstPlaceOBJ, Nickwin1, AverPlace1);
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(secondPlace), sendPlaceOBJ, Nickwin2, AverPlace2);
        }
        else
        {
            Debug.Log("Win only 3 player");
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(firstPlace), firstPlaceOBJ, Nickwin1, AverPlace1);
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(secondPlace), sendPlaceOBJ, Nickwin2, AverPlace2);
            placeInfoPlace(main_Game_Before_Start.FindPlayersWhoHadPlayed(thirdPlace), thirdPlaceOBJ, Nickwin3, AverPlace3);
        }
    }
    public void placeInfoPlace(Player whichPlayer,GameObject placeObj,Text nickNameObj,Image AvertarOBJ)
    {
        placeObj.SetActive(true);
        if (whichPlayer == PhotonNetwork.LocalPlayer)
        {
            AvertarOBJ.sprite = chosed_character_user.image_Avertar;
            nickNameObj.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            nickNameObj.text = main_Game_Before_Start.getNickNameOfHadPlayed(main_Game_Before_Start.positionOfHadPlayed(whichPlayer));
            AvertarOBJ.sprite = getAvertarOfPlayerHadPlayed(main_Game_Before_Start.positionOfHadPlayed(whichPlayer));
        }
    }
    public Sprite getAvertarOfPlayerHadPlayed(int Position)
    {
        return otherPlayerCharacterInfo[Position].image_Avertar;
    }
    public void ResetDrawCharCards()
    {
        gameHasStart = true;
        firstPlaceOBJ.SetActive(false);
        sendPlaceOBJ.SetActive(false);
        thirdPlaceOBJ.SetActive(false);
    }
    public void setWinnerList()
    {
        whichPlayerLeading.Clear();
        AllPlayerPoint.Clear();
        if (!main_Game_Before_Start.ifYouAreDead)
        {
            whichPlayerLeading.Add(PhotonNetwork.LocalPlayer);
            AllPlayerPoint.Add(moneyAndPointScripts.getMyPoints());
        }
        foreach (Player playerInList in main_Game_Before_Start.getPlayerList())
        {
            whichPlayerLeading.Add(playerInList);
            AllPlayerPoint.Add(moneyAndPointScripts.getOpponentPointsWithPlayer(main_Game_Before_Start.findPlayerPosition(playerInList)));
        }
    }
    public void checkTurn()
    {
        setWinnerList();
        if (TurnNumber == 0 || TurnNumber == 1 || TurnNumber == 2)
        {
            Debug.Log("Checking turn");
            // when the game first start the player will be set to the master 
            PlayerIdToMakeThisTurn = whichPlayerLeading[0].ActorNumber;
            object[] dataPlayerStart = new object[] { PlayerIdToMakeThisTurn };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataPlayerStart, AllPeople, SendOptions.SendReliable);
        }
        else if (TurnNumber > 2)
        {
            //using bubble sort to sort from lowest to higest 
            for (int whichPlaceInList = 0; whichPlaceInList <= whichPlayerLeading.Count - 2; whichPlaceInList++)
            {
                for (int i = whichPlaceInList; i <= AllPlayerPoint.Count - 2; i++)
                {
                    Player holdPlayer;
                    byte holdPoints;
                    if (AllPlayerPoint[i] > AllPlayerPoint[i + 1])
                    {
                        holdPlayer = whichPlayerLeading[i];
                        whichPlayerLeading[i] = whichPlayerLeading[i + 1];
                        whichPlayerLeading[i + 1] = holdPlayer;
                        holdPoints = AllPlayerPoint[i];
                        AllPlayerPoint[i] = AllPlayerPoint[i + 1];
                        AllPlayerPoint[i + 1] = holdPoints;
                    }
                }
            }
            whichPlayerLeading.Reverse();
            bool ifGotwinner;
            if (AllPlayerPoint[0] - AllPlayerPoint[1] >= 5 || PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                ifGotwinner= false;
            }
            else
            {
                ifGotwinner= true;
            }
            if (!ifGotwinner)
            {
                sendAllSomeoneWin();
            }
            else
            {
                PlayerIdToMakeThisTurn = whichPlayerLeading[0].ActorNumber;
                object[] dataPlayerStart = new object[] { PlayerIdToMakeThisTurn };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataPlayerStart, AllPeople, SendOptions.SendReliable);
            }
        }
    }
    public void EndTurn()
    {
        Debug.Log("sending to master");
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sortToMaster, null, new RaiseEventOptions { TargetActors = new int[] { PhotonNetwork.MasterClient.ActorNumber } }, SendOptions.SendReliable);
    }
    public void MasterSortTurn()
    {
        Debug.Log("Ending turn");
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Left room while waiting for end of turn. Skip end of turn.");
            return;
        }
        else
        {
            Debug.Log("Ending turn for player: " + PlayerIdToMakeThisTurn);
            HandoverTurnToNextPlayer();
            if (PlayerIdToMakeThisTurn > 0)
            {
                Player opponent = PhotonNetwork.CurrentRoom.Players[PlayerIdToMakeThisTurn];
                if (opponent.IsInactive)
                {
                    // TODO: show some hint that the other is not active and wait time will be longer!
                }
            }
        }
    }
    public void HandoverTurnToNextPlayer()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            if (getSetreturnToPlayerStartAround)
            {
                TurnNumber += 1;
                object[] turndata = new object[] { TurnNumber };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.TurnChanged, turndata, AllPeople, SendOptions.SendReliable);
                checkTurn();
            }
            else
            {
                this.PlayerIdToMakeThisTurn = nextMyPlayerId;
                object[] dataSelectCard = new object[] { PlayerIdToMakeThisTurn };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataSelectCard, AllPeople, SendOptions.SendReliable);
            }
        }
    }
    private void nextPlayerOnLeadingList(int playerID)
    {
        Debug.Log("checking who next");
        int i = 0;
        foreach (Player whichPlayer in whichPlayerLeading)
        {
            if (playerID == whichPlayer.ActorNumber)
            {
                Debug.Log("Found Player " + whichPlayer.ActorNumber);
                break;
            }
            i++;
        }
       
        Debug.Log("i =" + i +" and last is" + (whichPlayerLeading.Count-1));
        if (i == (whichPlayerLeading.Count - 1))
        {
            Debug.Log("No next Player");
            getSetreturnToPlayerStartAround = true;
        }
        else
        {
            Debug.Log("Got next Player");
            getSetreturnToPlayerStartAround = false;
            i++;
            foreach (Player which in PhotonNetwork.PlayerList)
            {
                if (which == whichPlayerLeading[i])
                {
                    nextMyPlayerId = which.ActorNumber;
                    break;
                }
            }
        }
    }
}