using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class DrawCharacterCard : MonoBehaviour
{
    public GameObject PlayerArea;
    public GameObject cardTemplate;
    [SerializeField] private CharCardScript CharCard1;
    public GameObject StartGameButtonOBJ;
    private PhotonView pv;
    private int x;
    public Button LeaveRoomButton;

    public UserInfoScript user_input_info;
    public popupcardwindowChar Popup;
    public GameObject clone_to_delete;
    public GameObject select_button;
    public DrawEntropyCard entropyCard;

    public Image avertarUser,avertarPlayer1 , avertarPlayer2, avertarPlayer3, avertarPlayer4, avertarPlayer5;
    public popupcardwindowChar PopUp;
    public Button avertar_allow_click;
    public Sprite defultImage;
    private CharCardScript chosed_character_user, chosed_character_player1=null, chosed_character_player2 = null, chosed_character_player3 = null, chosed_character_player4 = null, chosed_character_player5 = null;
    private List<CharCardScript> cardsInfoDraw = new List<CharCardScript>();
    private List<CharCardScript> cardsInfo = new List<CharCardScript>();
    private List<CharCardScript> otherPlayerCharacterInfo = new List<CharCardScript>();
    private List<Image> otherPlayerAvertar = new List<Image>();
    void Start()
    {
        pv = GetComponent<PhotonView>();
        putCharCardsInList();
        avertar_allow_click.enabled = false;
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
    }

    public void OnClickTodrawCard()
    {
        if (pv.IsMine)
        {
            pv.RPC("noleave", RpcTarget.All);
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            pv.RPC("Drawcard",RpcTarget.AllBufferedViaServer);
        }
    }
    [PunRPC]
    private void noleave() => LeaveRoomButton.interactable = false; 
    [PunRPC]
    public void Drawcard()
    {
        for (var i = 0; i < 3; i++)
        {
            x = Random.Range(0, (cardsInfo.Count - 1));
            GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().CharCard = cardsInfo[x];
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().FrontSide.SetActive(true);
            characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            //pv.RPC("RemoveThisCard", RpcTarget.All,cardsInfo[x].character_code);
        }
        StartGameButtonOBJ.SetActive(false);
    }
    public void clickOnSelectCard()
    {
        pv.RPC("SetCharacterInfo", RpcTarget.Others, PopUp.GetCharCardScript().character_code, PhotonNetwork.LocalPlayer);
        select_button.SetActive(false);
        foreach (Transform child in clone_to_delete.transform)
        {
            Destroy(child.gameObject);
        }
        chosed_character_user = PopUp.GetCharCardScript();
        avertarUser.sprite = chosed_character_user.image_Avertar;
        avertar_allow_click.enabled = true;
        PopUp.closePopup();
        entropyCard.distribute_entropycard(5);
    }
    [PunRPC]
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
    [PunRPC]
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
            }
        }
        
    }
}
