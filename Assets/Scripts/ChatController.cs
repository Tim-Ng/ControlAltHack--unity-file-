using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UserAreas;
using main;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject ScriptOBJ = null, playerDropDownList = null, scrollText = null, newMessageSprite = null, chatPopUp = null, chatButtonText = null;
    [SerializeField] private TMP_InputField messageInput = null;
    private UserAreaControlers userInfos = null;
    private EventHandeler eventHandeler = null;
    private List<Player> NickNames = new List<Player>();
    private Player whichPlayer = null;

    public int amountOfNewText=0;
    private int setamountOfNewText
    {
        set { 
            amountOfNewText = value;
            setNumberOfNewText();
        }
        get { return amountOfNewText; }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.UpArrow))
        {
            whenClickOnSendMessage();
        }
    }
    private void Awake()
    {
        setamountOfNewText = 0;
        userInfos = ScriptOBJ.GetComponent<UserAreaControlers>();
        eventHandeler = ScriptOBJ.GetComponent<EventHandeler>();
    }
    public void setUpDropdownList()
    {
        NickNames.Clear();
        var dropdownPlayer = playerDropDownList.GetComponent<Dropdown>();
        dropdownPlayer.options.Clear();
        dropdownPlayer.options.Add(new Dropdown.OptionData() { text = "All" });
        for (int i = 1; i < 6; i++)
        {
            if (userInfos.users[i].filled == true)
            {
                NickNames.Add(userInfos.users[i].playerPhoton);
            }
        }
        for (int i =0; i < NickNames.Count; i++)
        {
            dropdownPlayer.options.Add(new Dropdown.OptionData() { text = NickNames[i].NickName });
        }
        dropdownPlayer.onValueChanged.AddListener(delegate { dropDownChange(dropdownPlayer.value); });
    }
    public void dropDownChange(int drowDownValue)
    {
        if (drowDownValue == 0)
        {
            whichPlayer = null;
            Debug.Log("All Player");
        }
        else
        {
            whichPlayer = NickNames[drowDownValue - 1];
            Debug.Log("The player Selected is "+ whichPlayer.NickName);
        }
    }
    public void whenClickOnSendMessage()
    {
        if (!string.IsNullOrEmpty(messageInput.text)) 
        {
            if (whichPlayer == null)
            {
                if (messageInput.text[0] == '/')
                {
                    // for easier testing 
                    if(messageInput.text == "/iquit")
                    {
                        userInfos.subMyCred(userInfos.users[0].amountOfCred);
                    }
                }
                else
                {
                    onReceiveMessage(messageInput.text, PhotonNetwork.LocalPlayer, false);
                    object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, false };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, eventHandeler.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                }
            }
            else
            {
                onReceiveMessage(messageInput.text, PhotonNetwork.LocalPlayer, true);
                object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, true };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, new RaiseEventOptions { TargetActors = new int[] { whichPlayer.ActorNumber } }, SendOptions.SendReliable);
            }
        }
        messageInput.text = null;
    }
    public void onReceiveMessage(string Message,Player sender,bool isPrivate)
    {
        if (!chatPopUp.activeSelf)
        {
            setamountOfNewText += 1;
        }
        if (sender == null)
        {
            scrollText.GetComponent<Text>().text +="<color=#BF0000><i> <b>"+"Server" + ":" + Message + "</b></i></color>" +"\n";
        }
        else
        {
            if (isPrivate)
            {
                scrollText.GetComponent<Text>().text += "<color=#E56717><i><b>(Private</b></i></color>";
                if (sender.IsLocal)
                {
                    scrollText.GetComponent<Text>().text += "<color=#E56717><i><b> to " + whichPlayer.NickName+")</b></i></color> ";
                }
                else
                {
                    scrollText.GetComponent<Text>().text += "<color=#E56717><i><b>)</b></i></color> ";
                }
            }
            if (sender.IsLocal)
            {
                scrollText.GetComponent<Text>().text += "<color=#008000><i><b>" + sender.NickName + "(YOU)</b>: " + Message + "</i></color>\n";
            }
            else
            {
                scrollText.GetComponent<Text>().text += "<color=#0A1172><i><b>" + sender.NickName + "</b>: "+ Message + "</i></color>\n";
            }
        }
    }
    public void setNumberOfNewText()
    {
        if (amountOfNewText != 0)
        {
            newMessageSprite.SetActive(true);
            chatButtonText.GetComponent<Text>().text = amountOfNewText.ToString();
        }
        else
        {
            newMessageSprite.SetActive(false);
            chatButtonText.GetComponent<Text>().text = "";
        }
    }
    public void clickOnMessageButton()
    {
        setamountOfNewText = 0;
        chatPopUp.SetActive(!chatPopUp.activeSelf);
    }
}
