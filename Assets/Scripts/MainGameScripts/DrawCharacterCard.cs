using ExitGames.Client.Photon;
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
        ChangePlayer = 1,
        SelectChar = 2,
        RemoveCharCard = 3
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
        else if (obj.Code == (byte)PhotonEventCode.ChangePlayer)
        {
            object[] carddata = (object[])obj.CustomData;
            PlayerIdToMakeThisTurn = (int)carddata[0];
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
    void Update()
    {
        if (IsMyTurn && TurnNumber == 0) //this is only the first turn
        {
            Debug.Log("My turn to draw card");
            countNumCharCards();
            Drawcard(number_of_character_cards);
        }
    }
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

        number_of_players = PhotonNetwork.CountOfPlayersInRooms;

        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log("Send to all draw character");
        setPlayerStart(PhotonNetwork.LocalPlayer.ActorNumber); // as only master can click on this button 

    }
    private void noleave() => LeaveRoomButton.interactable = false;
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
            object[] data = new object[] { cardsInfoDraw[x].character_code};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.RemoveCharCard, data, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            RemoveThisCard(cardsInfoDraw[x].character_code);
        }
        Debug.Log("Got pass to here");
        EndTurn();
    }
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
    public void clickAvertarPlayer1() => PopUpCharacterInfo(0);
    public void clickAvertarPlayer2() => PopUpCharacterInfo(1);
    public void clickAvertarPlayer3() => PopUpCharacterInfo(2);
    public void clickAvertarPlayer4() => PopUpCharacterInfo(3);
    public void clickAvertarPlayer5() => PopUpCharacterInfo(4);
    public void clickAvertarPlayerUser() => PopUpCharacterInfo(5);

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
            if (nextPlayer.ActorNumber == 1 ){
                TurnNumber = 1;
            }
            else if (nextPlayer != null)
            {
                this.PlayerIdToMakeThisTurn = nextPlayer.ActorNumber;
                object[] data = new object[] { PlayerIdToMakeThisTurn };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.ChangePlayer, data, AllPeople, SendOptions.SendReliable);
                return;
            }
            else
                return;
        }

        this.PlayerIdToMakeThisTurn = 0;
    }

    public void setPlayerStart(int ActorNumber)
    {
        this.PlayerIdToMakeThisTurn = ActorNumber;
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
}