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
using UnityEngine.SocialPlatforms;
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
    private List<CharCardScript> holdTocheckIngame= new List<CharCardScript>();
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

    private MissionCardScript currentUserMission = null;

    [SerializeField] private PanelToTrade panelToTrade;

    private bool iveDoneMyTurn = false;
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
    public int playersBeforeStartGame;
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
            iveDoneMyTurn = false;
            if (!PhotonNetwork.IsMasterClient)
            {
                setWinnerList();
            }
            main_Game_Before_Start.setHoldPlayerListAfterStartGame();
            playersBeforeStartGame = PhotonNetwork.CurrentRoom.PlayerCount;
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
            iveDoneMyTurn = (bool)carddata[1];
            Debug.Log("New Turn player have done turn = " + iveDoneMyTurn);
            if (!PhotonNetwork.IsMasterClient)
            {
                setWinnerList();
            }
            panelToTrade.setHoldDoneList(true, null);
            panelToTrade.setPannelDoneList(true, null);
            if (TurnNumber > 1 && TurnNumber % 2 != 0)
            {
                rollTimeScript.restRollTime();
            }
            if (PhotonNetwork.IsMasterClient)
            {
                checkTurn();
            }
        }
        else if (obj.Code == (byte)PhotonEventCode.CheckAllDoneChar)
        {
            Debug.Log("A player is done selecting character");
            checkWhichFunctionToRun();
        }
        else if (obj.Code == (byte)PhotonEventCode.winnnerDone)
        {
            gameWinArea.SetActive(true);
            object[] whowin = (object[])obj.CustomData;
            if ((int)whowin[6] == 1)
            {
                Debug.Log("Win only 1 player");
                placeInfoPlace(firstPlaceOBJ, Nickwin1, AverPlace1, (string)whowin[0], (int)whowin[1]);
            }
            else if ((int)whowin[6] == 2)
            {
                Debug.Log("Win only 2 player");
                placeInfoPlace(firstPlaceOBJ, Nickwin1, AverPlace1, (string)whowin[0], (int)whowin[1]);
                placeInfoPlace(sendPlaceOBJ, Nickwin2, AverPlace2, (string)whowin[2], (int)whowin[3]);
            }
            else if ((int)whowin[6] == 3)
            {
                Debug.Log("Win only 3 player");
                placeInfoPlace(firstPlaceOBJ, Nickwin1, AverPlace1, (string)whowin[0], (int)whowin[1]);
                placeInfoPlace(sendPlaceOBJ, Nickwin2, AverPlace2, (string)whowin[2], (int)whowin[3]);
                placeInfoPlace(thirdPlaceOBJ, Nickwin3, AverPlace3, (string)whowin[4], (int)whowin[5]);
            }
            else
            {
                Debug.LogError("EmptyList");
            }
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
        holdTocheckIngame = otherPlayerCharacterInfo;
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
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.LeaveButton, null, AllPeople, SendOptions.SendReliable);
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
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.CheckAllDoneChar, null, AllPeople, SendOptions.SendReliable);
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
        foreach (CharCardScript charScript in cardDeck.getCharDeck())
        {
            Debug.Log("card id :");
            if (charScript.character_code == character_code)
            {
                Debug.Log("Avetar of player " + sendingPlayer.NickName + " is set ");
                otherPlayerCharacterInfo[main_Game_Before_Start.findPlayerPosition(sendingPlayer)] = charScript;
                holdTocheckIngame[main_Game_Before_Start.findPlayerPosition(sendingPlayer)] = charScript;
                otherPlayerAvertar[main_Game_Before_Start.findPlayerPosition(sendingPlayer)].sprite = otherPlayerCharacterInfo[main_Game_Before_Start.findPlayerPosition(sendingPlayer)].image_Avertar;
                otherAvertarPlayerButton[main_Game_Before_Start.findPlayerPosition(sendingPlayer)].interactable = true;
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
    // Check the number of players and distribute the number of character cards accordingly 
    public void countNumCharCards()
    {
        number_of_players = playersBeforeStartGame;
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
    public void removeAvertar(int position)
    {
        holdTocheckIngame[position] = null;
    }
    public void checkWhichFunctionToRun()
    {
        if ((!main_Game_Before_Start.ifYouAreDead) )
        {  
            if ((TurnNumber == 0) && IsMyTurn )
            {
                if (!iveDoneMyTurn)
                {
                    countNumCharCards();
                    Drawcard(number_of_character_cards);
                    EndTurn();
                }
                else
                {
                    EndTurn();
                }
            }
            else if ((TurnNumber % 2 != 0) && IsMyTurn )
            {
                int NumberOfselectedPlayer = 0;
                if(chosed_character_user != null)
                {
                    NumberOfselectedPlayer += 1;
                }
                foreach (CharCardScript which in holdTocheckIngame)
                {
                    if (which != null)
                    {
                        NumberOfselectedPlayer += 1;
                    }
                }
                Debug.Log(NumberOfselectedPlayer + " done and round is " + TurnNumber + " number of people on room " + PhotonNetwork.CurrentRoom.PlayerCount);
                if (NumberOfselectedPlayer >= PhotonNetwork.CurrentRoom.PlayerCount)
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
            else if (TurnNumber % 2 == 0 && panelToTrade.getEveryoneDonePanel() && IsMyTurn)
            {
                rollTimeScript.startRollTurn();
            }
        }
        else
        {
            EndTurn();
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
        List<string> NickNameList = new List<string>();
        List<int> CharacterID = new List<int>();

        for (int i = 0; i<whichPlayerLeading.Count; i++)
        {
            main_Game_Before_Start.getSetIfONlyOneLeft = PhotonNetwork.CurrentRoom.PlayerCount;
            NickNameList.Add(whichPlayerLeading[i].NickName);
            if (whichPlayerLeading[i] == PhotonNetwork.LocalPlayer)
            {
                CharacterID.Add(chosed_character_user.character_code);
            }
            else
            {
                CharacterID.Add(AllCharacterPlayCharacter()[main_Game_Before_Start.findPlayerPosition(whichPlayerLeading[i])].character_code);
            }
            
            if (i == 2)
            {
                break;
            }        
        }
        if (NickNameList.Count == 1)
        {
            object[] top3winner = new object[] { NickNameList[0], CharacterID[0], null, null, null, null, 1 };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.winnnerDone, top3winner, AllPeople, SendOptions.SendReliable);
        }
        else if (NickNameList.Count == 2)
        {
            object[] top3winner = new object[] { NickNameList[0], CharacterID[0], NickNameList[1], CharacterID[1], null, null, 2 };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.winnnerDone, top3winner, AllPeople, SendOptions.SendReliable);
        }
        else if (NickNameList.Count == 3)
        {
            object[] top3winner = new object[] { NickNameList[0], CharacterID[0], NickNameList[1], CharacterID[2], NickNameList[1], CharacterID[2], 3 };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.winnnerDone, top3winner, AllPeople, SendOptions.SendReliable);
        }
        else
        {
            Debug.LogError("No player in list !?!?!?!");
        }
    }
    public List<CharCardScript> AllCharacterPlayCharacter()
    {
        List<CharCardScript> tempAllPlayerList = new List<CharCardScript>();
        tempAllPlayerList.Add(getMyCharScript());
        foreach(CharCardScript thisPlayer in otherPlayerCharacterInfo)
        {
            tempAllPlayerList.Add(thisPlayer);
        }
        return tempAllPlayerList;
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
    public void placeInfoPlace(GameObject placeObj, Text nickNameObj, Image AvertarOBJ,string NickName,int whichCharCode)
    {
        placeObj.SetActive(true);
        nickNameObj.text = NickName;
        foreach (CharCardScript whichScript in cardDeck.getCharDeck())
        {
            if (whichScript.character_code == whichCharCode)
            {
                AvertarOBJ.sprite = whichScript.image_Avertar;
            }
        }
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
        AllPlayerPoint.Reverse();
    }
    public void checkTurn()
    {
        setWinnerList();
        if (TurnNumber > 2)
        {
            bool gotPlayerWin;
            if (AllPlayerPoint.Count > 1)
            {
                if (AllPlayerPoint[0] - AllPlayerPoint[1] >= 5)
                {
                    gotPlayerWin = true;
                }
                else
                {
                    gotPlayerWin = false;
                }
            }
            else
            {
                gotPlayerWin = true;
            }
            if (gotPlayerWin)
            {
                sendAllSomeoneWin();
            }
        }
        PlayerIdToMakeThisTurn = whichPlayerLeading[0].ActorNumber;
        object[] dataPlayerStart = new object[] { PlayerIdToMakeThisTurn };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataPlayerStart, AllPeople, SendOptions.SendReliable);
    }
    public void EndTurn()
    {
        if (IsMyTurn)
        {
            iveDoneMyTurn = true;
        }
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
        }
    }
    public void HandoverTurnToNextPlayer()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            if (getSetreturnToPlayerStartAround)
            {
                TurnNumber += 1;
                object[] turndata = new object[] { TurnNumber , false};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.TurnChanged, turndata, AllPeople, SendOptions.SendReliable);
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
       
        Debug.Log("i =" + i + " and last is" + (whichPlayerLeading.Count - 1));
        if (i >= (whichPlayerLeading.Count - 1))
        {
            Debug.Log("No next Player");
            getSetreturnToPlayerStartAround = true;
        }
        else
        {
            i++;
            Debug.Log("Got next Player");
            getSetreturnToPlayerStartAround = false;
            if (main_Game_Before_Start.isActorIdInList(whichPlayerLeading[i].ActorNumber))
            {
                nextMyPlayerId = whichPlayerLeading[i].ActorNumber;
            }
            else
            {
                nextMyPlayerId = whichPlayerLeading[i].ActorNumber;
                nextPlayerOnLeadingList(nextMyPlayerId);
            }
        }
    }
}