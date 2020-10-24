﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class DrawCharacterCard : MonoBehaviourPunCallbacks

{
    public GameObject PlayerArea;
    public GameObject cardTemplate;
    [SerializeField] private CharacterCardDeck cardDeck=null;
    public GameObject StartGameButtonOBJ;
    private int x;
    public Button LeaveRoomButton;

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
    public static int[] GameProperties = { 0, 0 };

    private int ActorNumberOfStartPlayer; // player that start the turn
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
    public enum PhotonEventCode
    {
        LeaveButton = 0,
        UpdateTurnPlayer = 1,
        SelectChar = 2,
        RemoveCharCard = 3,
        EventSetPlayerThatStart=4,
        TurnChanged =5,
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
    public byte MyPoints = 0;
    public byte opponent1Points = 0;
    public byte opponent2Points = 0;
    public byte opponent3Points = 0;
    public byte opponent4Points = 0;
    public byte opponent5Points = 0;
    void Start()
    {
        putCharCardsInList();
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
            noleave();
        }
        else if (obj.Code == (byte)PhotonEventCode.UpdateTurnPlayer)
        {
            object[] carddata = (object[])obj.CustomData;
            PlayerIdToMakeThisTurn = (int)carddata[0];
            if ((TurnNumber == 0) && IsMyTurn)
            {
                countNumCharCards();
                Drawcard(number_of_character_cards);
            }
            else
            {
                //draw entropycard 
            }
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
        else if (obj.Code == (byte)PhotonEventCode.EventSetPlayerThatStart){
            object[] carddata = (object[])obj.CustomData;
            ActorNumberOfStartPlayer = (int)carddata[0];
        }
        else if (obj.Code == (byte)PhotonEventCode.TurnChanged)
        {
            object[] carddata = (object[])obj.CustomData;
            TurnNumber = (int)carddata[0];
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
    private void noleave() => LeaveRoomButton.interactable = false;

    //Distribute card accordingly 
    public void Drawcard(int y)
    {
        for (var i = 0; i < y; i++)
        {
            x = Random.Range(0, cardsInfoDraw.Count);
            Debug.Log("Random Number this loop is:" + x);
            GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
            characterPlayerCard1.name = cardsInfoDraw[x].character_card_name;
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().CharCard = cardsInfoDraw[x];
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().FrontSide.SetActive(true);
            characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            object[] data = new object[] { cardsInfoDraw[x].character_code };
        }
        Debug.Log("Got pass to here");
        EndTurn();
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
        userAvertarButton.interactable = true;
        Popup.closePopup();
        object[] dataSelectCard = new object[] { chosed_character_user.character_code, PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.SelectChar, dataSelectCard, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        entropyCard.distribute_entropycard(5);
    }

    //to remove this card from the deck for everyone so that we don't get the same character
    public void RemoveThisCard(int cardID)
    {
        foreach (CharCardScript checkCard in cardDeck.getCharDeck())
        {
            if (cardID == checkCard.character_code)
            {
                cardsInfoDraw.Remove(checkCard);
                Debug.Log("Card ID :" + cardID + " is removed");
                break;
            }
        }
        Debug.Log("Number of cards left in deck " + cardsInfoDraw.Count);
    }

    //find which player selected the card in your point of view 
    public void SetCharacterInfo(int character_code, Player sendingPlayer)
    {
        int i = 0;
        foreach (Player whichplayer in PhotonNetwork.PlayerListOthers)
        {
            if (sendingPlayer == whichplayer)
            {
                setOtherPlayer(i, character_code);
            }
            i++;
        }

    }

    //then set information of the card and the avertar in the allocated space 
    public void setOtherPlayer(int i, int charcode)
    {
        foreach (CharCardScript charScript in cardDeck.getCharDeck())
        {
            if (charScript.character_code == charcode)
            {
                otherPlayerCharacterInfo[i] = charScript;
                otherPlayerAvertar[i].sprite = otherPlayerCharacterInfo[i].image_Avertar;
                otherAvertarPlayerButton[i].interactable = true;
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
    public void HandoverTurnToNextPlayer()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            Player nextPlayer = PhotonNetwork.LocalPlayer.GetNextFor(this.PlayerIdToMakeThisTurn);
            if (nextPlayer.ActorNumber == ActorNumberOfStartPlayer ){
                TurnNumber += 1;
                object[] turndata = new object[] { TurnNumber };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.TurnChanged, turndata, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                checkTurn();
            }
            else if (nextPlayer != null)
            {
                this.PlayerIdToMakeThisTurn = nextPlayer.ActorNumber;
                object[] dataSelectCard = new object[] { PlayerIdToMakeThisTurn  };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataSelectCard, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                return;
            }
            else
                return;
        }

        this.PlayerIdToMakeThisTurn = 0;
    }
    public void EndTurn()
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
    public void checkTurn()
    {
        if (TurnNumber == 0 || TurnNumber == 1)
        {
            Debug.Log("Checking turn");
            // when the game first start the player will be set to the master 
            PlayerIdToMakeThisTurn = PhotonNetwork.MasterClient.ActorNumber;
            ActorNumberOfStartPlayer = PlayerIdToMakeThisTurn;
            object[] dataPlayerStart = new object[] { PlayerIdToMakeThisTurn };
            object[] dataHoldPlayerStart = new object[] { ActorNumberOfStartPlayer };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.UpdateTurnPlayer, dataPlayerStart, AllPeople, SendOptions.SendReliable);
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.EventSetPlayerThatStart, dataHoldPlayerStart, AllPeople, SendOptions.SendReliable);
        }
        else if (TurnNumber > 1)
        {
            
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
}