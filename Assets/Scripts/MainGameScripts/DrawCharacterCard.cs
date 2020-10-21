using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DrawCharacterCard : MonoBehaviourPunCallbacks

{
    public GameObject PlayerArea;
    public GameObject cardTemplate;
    [SerializeField] private CharCardScript CharCard1;
    public GameObject StartGameButtonOBJ;
    private int x;
    public Button LeaveRoomButton;

    public popupcardwindowChar Popup;
    public GameObject clone_to_delete;
    public GameObject select_button;
    public DrawEntropyCard entropyCard;

    public Image avertarUser, avertarPlayer1, avertarPlayer2, avertarPlayer3, avertarPlayer4, avertarPlayer5;
    public Sprite defultImage;
    private CharCardScript chosed_character_user, chosed_character_player1 = null, chosed_character_player2 = null, chosed_character_player3 = null, chosed_character_player4 = null, chosed_character_player5 = null;
    public Button userAvertarButton, Player1AvertarButton, Player2AvertarButton, Player3AvertarButton, Player4AvertarButton, Player5AvertarButton;
    private List<CharCardScript> cardsInfoDraw = new List<CharCardScript>();
    private List<CharCardScript> cardsInfo = new List<CharCardScript>();
    private List<CharCardScript> otherPlayerCharacterInfo = new List<CharCardScript>();
    private List<Image> otherPlayerAvertar = new List<Image>();
    private List<Button> otherAvertarPlayerButton = new List<Button>();
    private int number_of_players,number_of_character_cards;

    [SerializeField] private TMP_InputField numberOfRounds_input = null, numberOfCredAhead_input = null;
    public static int[] GameProperties = { 0, 0 };

    private RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.Others
    };
    public enum PhotonEventCode
    {
        LeaveButton = 0,
        DrawCharCards = 1,
        SelectChar = 2,
        RemoveCharCard = 3
    }
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
        else if (obj.Code == (byte)PhotonEventCode.DrawCharCards)
        {
            object[] num_cards = (object[])obj.CustomData;
            int int_num_cards = (int)num_cards[0];
            Drawcard(int_num_cards);
        }
        else if (obj.Code == (byte)PhotonEventCode.RemoveCharCard)
        {
            object[] carddata = (object[])obj.CustomData;
            string datacode = (string)carddata[0];
            RemoveThisCard(datacode);
        }
        else if (obj.Code == (byte)PhotonEventCode.SelectChar)
        {
            object[] characterInfo = (object[])obj.CustomData;
            SetCharacterInfo((string) characterInfo[0],(Player)characterInfo[1]);
        }
    }

    private void putCharCardsInList()
    {
        Debug.Log("Input Character card into list");
        cardsInfo.Add(CharCard1);
        /*cardsInfo.Add(CharCard2);
        cardsInfo.Add(CharCard3);
        cardsInfo.Add(CharCard4);
        cardsInfo.Add(CharCard5);
        cardsInfo.Add(CharCard6);
        cardsInfo.Add(CharCard7);
        cardsInfo.Add(CharCard8);
        cardsInfo.Add(CharCard9);
        cardsInfo.Add(CharCard10);
        cardsInfo.Add(CharCard11);
        cardsInfo.Add(CharCard12);*/
        cardsInfoDraw = cardsInfo;
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
    public void OnClickTodrawCard()
    {
        StartGameButtonOBJ.SetActive(false);
        if (!(numberOfRounds_input == null))
        {
            if ((numberOfRounds_input.text).All(char.IsDigit))
                GameProperties[0] = int.Parse(numberOfRounds_input.text);
            else
                GameProperties[0] = 0;
        }
        if (!(numberOfCredAhead_input == null))
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
        if (number_of_players == 6)
        {
            number_of_character_cards = 2;
        }
        else
        {
            number_of_character_cards = 3;
        }
        Drawcard(number_of_character_cards);
        object[] num_of_cards = new object[] { number_of_character_cards  };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.DrawCharCards, num_of_cards, AllOtherThanMePeopleOptions, SendOptions.SendUnreliable);
    }
    private void noleave() => LeaveRoomButton.interactable = false; 
    public void Drawcard(int y)
    {
        for (var i = 0; i < y; i++)
        {
            x = Random.Range(0, (cardsInfoDraw.Count - 1));
            GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().CharCard = cardsInfoDraw[x];
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().FrontSide.SetActive(true);
            characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            object[] data = new object[] {y };
            /*PhotonNetwork.RaiseEvent((byte)PhotonEventCode.RemoveCharCard, cardsInfoDraw[x].character_code, AllPeopleOptions, SendOptions.SendUnreliable);*/
        }
        
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
        object[] dataSelectCard = new object[] { chosed_character_user.character_code , PhotonNetwork.LocalPlayer};
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.SelectChar, dataSelectCard, AllOtherThanMePeopleOptions, SendOptions.SendUnreliable);
        entropyCard.distribute_entropycard(5);
    }
    public void RemoveThisCard(string cardID)
    { 
        foreach (CharCardScript checkCard in cardsInfo)
        {
            if (cardID == checkCard.character_code)
            {
                cardsInfoDraw.Remove(checkCard);
                Debug.Log ("Card ID :" + cardID + " is removed");
            }
        }
    }
    public void SetCharacterInfo(string character_code, Player sendingPlayer)
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
    public void setOtherPlayer(int i,string charcode)
    {
        foreach (CharCardScript charScript in cardsInfo)
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


}
