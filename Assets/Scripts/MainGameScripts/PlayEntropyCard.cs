using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEntropyCard : MonoBehaviour
{
    [SerializeField] private rollTime RollTime;
    [SerializeField] private MoneyAndPoints moneyAndPoints;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    [SerializeField] private popupcardwindowEntropy popUpEntropy;
    [SerializeField] private EntropyRollTime entropyRollTime;
    [SerializeField] private GameObject userEntorpyArea;
    private EntropyCardScript entropyCardScript;
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
        sendEntropyRollToOther = 22,
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)PhotonEventCode.sendEntropyRollToOther)
        {
            object[] data = (object[])obj.CustomData;
            entropyRollTime.startEntropyRollTurn((int)data[0]);
        }
    }
    public void clickOnPlayEntropyCard()
    {
        entropyCardScript = popUpEntropy.GetEntropyCardScript();
        if (moneyAndPoints.getMyMoneyAmount() >= entropyCardScript.Cost || entropyCardScript.Cost == 0)
        {
            moneyAndPoints.subMyMoney(entropyCardScript.Cost);
            popUpEntropy.closePopup();
            enoughMoney();
        }
        else
        {
            Debug.Log("Can't Play card");
            //cant play card 
        }
    }
    public void enoughMoney()
    {
        Debug.Log("Passed to check entropy card and what to do");
        if (entropyCardScript.IsBagOfTricks)
        {
            if (entropyCardScript.SkillEffecter)
            {
                RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease1, entropyCardScript.byHowMuchSkillIncrease1, Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1));
                if (entropyCardScript.increaseSecondSkill)
                {
                    RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease2, entropyCardScript.byHowMuchSkillIncrease2, Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1));
                }
                removeCard();
            }
            else
            {
                removeCard();
            }
        }
        else if (entropyCardScript.IsLigthingStrikes)
        {
            if (entropyCardScript.UseSucFailLighting)
            {
                object[] dataRoll = new object[] { entropyCardScript.EntropyCardID };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendEntropyRollToOther, dataRoll, AllPeople, SendOptions.SendReliable);
                removeCard();
            }
            else
            {
                removeCard();
            }
        }
    }
    private void removeCard()
    {
        foreach (Transform child in userEntorpyArea.transform)
        {
            if (child.GetComponent<EntropyCardDisplay>().entropyData == entropyCardScript)
            {
                GameObject.Destroy(child.gameObject);
                break;
            }
            else
            {
                removeCard();
            }
        }
        moneyAndPoints.countMyNumOfEntropyCards();
    }
}
