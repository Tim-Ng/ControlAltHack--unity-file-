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
    [SerializeField] private GameObject ScriptOBJ = null, playerDropDownList = null, scrollText = null;
    [SerializeField] private TMP_InputField messageInput = null;
    private UserAreaControlers userInfos = null;
    private EventHandeler eventHandeler = null;
    private List<Player> NickNames = new List<Player>();
    private Player whichPlayer = null;
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.UpArrow))
        {
            whenClickOnSendMessage();
        }
    }
    private void Awake()
    {
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
                    scrollText.GetComponent<Text>().text += PhotonNetwork.LocalPlayer.NickName + ":" + messageInput.text + "\n";
                    object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, false };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, eventHandeler.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                }
            }
            else
            {
                scrollText.GetComponent<Text>().text += "(Private) " + PhotonNetwork.LocalPlayer.NickName + ":" + messageInput.text + "\n";
                object[] chatInfo = new object[] { messageInput.text, PhotonNetwork.LocalPlayer, true };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, new RaiseEventOptions { TargetActors = new int[] { whichPlayer.ActorNumber } }, SendOptions.SendReliable);
            }
        }
        messageInput.text = null;
    }
    public void onReceiveMessage(string Message,Player sender,bool isPrivate)
    {
        if (sender == null)
        {
            scrollText.GetComponent<Text>().text += "Server" + ":" + Message + "\n";
        }
        else
        {
            if (!isPrivate)
            {
                scrollText.GetComponent<Text>().text += sender.NickName + ":" + Message + "\n";
            }
            else
            {
                scrollText.GetComponent<Text>().text += "(Private) " + sender.NickName + ":" + Message + "\n";
            }
        }
    }

}
