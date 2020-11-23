using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        entropytoOthers = 25,
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
        else if (obj.Code == (byte)PhotonEventCode.entropytoOthers)
        {
            object[] whatEffect = (object[])obj.CustomData;
            if ((int)whatEffect[0] == 0)
            {
                if ((int)whatEffect[1] ==0)
                {
                    moneyAndPoints.subPoints(2);
                }
                else
                {
                    moneyAndPoints.addPoints(2);
                }
            }
            else if ((int)whatEffect[0] == 1)
            {
                //discard all your money only master 
                moneyAndPoints.subMyMoney(moneyAndPoints.getMyMoneyAmount());
            }
            else if ((int)whatEffect[0] == 2)
            {
                RollTime.convertFailedToPassed((string)whatEffect[1],(bool)whatEffect[2]);
            }
            else if ((int)whatEffect[0] == 3)
            {
                if ((int)whatEffect[1] == 1)
                {
                    RollTime.addSkillChanger("Network Ninja", (-2), (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 )).ToString());
                    RollTime.addSkillChanger("Social Engineering", (-2), (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2)).ToString());
                }
                else if ((int)whatEffect[1] == 2)
                {
                    RollTime.enturnForEntorpyLightning();
                }
            }
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
                RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease1, entropyCardScript.byHowMuchSkillIncrease1, (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 )).ToString());
                if (entropyCardScript.increaseSecondSkill)
                {
                    RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease2, entropyCardScript.byHowMuchSkillIncrease2, (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2)).ToString());
                }
            }
            else if (entropyCardScript.use_usage)
            {
                if (entropyCardScript.EntropyCardID == 1)
                {
                    //can do failed ninja network again, if still fail then the card is discrded
                    RollTime.RerollWhich("Network Ninja", true);
                    
                }
                else if (entropyCardScript.EntropyCardID == 2)
                {
                    RollTime.addSkillChanger("Hardware Hacking", 9999, (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 )).ToString());
                }
                else if (entropyCardScript.EntropyCardID == 4)
                {
                    //reroll failed entropycard 
                    popUpEntropy.addToRollTimeList(entropyCardScript.EntropyCardID);
                    RollTime.RerollWhich("Lockpicking", false);
                }
                else if (entropyCardScript.EntropyCardID == 7)
                {
                    //any failed Connections into pass
                    object[] whatEffect = new object[] { 2, "Connections", true };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, whatEffect, AllPeople, SendOptions.SendReliable);
                }
                else if (entropyCardScript.EntropyCardID == 8)
                {
                    //any failed Cryptanalysis into pass
                    object[] whatEffect = new object[] { 2, "Cryptanalysis" ,true};
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, whatEffect, AllPeople, SendOptions.SendReliable);
                }
            }
        }
        else if (entropyCardScript.IsLigthingStrikes)
        {
            if (entropyCardScript.UseSucFailLighting)
            {
                object[] dataRoll = new object[] { entropyCardScript.EntropyCardID };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendEntropyRollToOther, dataRoll, AllPeople, SendOptions.SendReliable);
            }
            else if (entropyCardScript.use_usage)
            {
                if (entropyCardScript.EntropyCardID == 14)
                {
                    object[] dataRoll = new object[] { 3, 1 };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, dataRoll, new RaiseEventOptions { TargetActors = new int[] { drawCharacterCard.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
                }
                else if (entropyCardScript.EntropyCardID == 16)
                {
                    //The Mission is canceled. You have no Mission for the turn, and therefore get no reward and suffer no penalty.
                    object[] dataRoll = new object[] { 3, 2 };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, dataRoll, new RaiseEventOptions { TargetActors = new int[] { drawCharacterCard.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
                }
                else if (entropyCardScript.EntropyCardID == 13)
                {
                    //Discard all your Money.
                    object[] dataRoll = new object[] { 1 };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, dataRoll, new RaiseEventOptions { TargetActors = new int[] { drawCharacterCard.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
                }
            }
        }
        else if (entropyCardScript.IsExtensiveExperience)
        {
            if (popUpEntropy.getBeforeRoll)
            {
                int howMuch = 0;
                if (drawCharacterCard.getMyCharScript().find_which(entropyCardScript.Title) > 12)
                {
                    howMuch = 1;
                }
                else
                {
                    howMuch = 12 - drawCharacterCard.getMyCharScript().find_which(entropyCardScript.Title);
                }
                RollTime.addSkillChanger(entropyCardScript.Title, howMuch, (Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 )).ToString());
            }
            else
            {
                object[] whatEffect = new object[] { 2, entropyCardScript.Title ,false};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, whatEffect, AllPeople, SendOptions.SendReliable);
            }
            popUpEntropy.addToRollTimeList(entropyCardScript.EntropyCardID);
        }
        else if (entropyCardScript.IsSharedFate)
        {
            if (entropyCardScript.EntropyCardID == 31)
            {
                object[] whatEffect = new object[] {0,0};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, whatEffect, AllPeople, SendOptions.SendReliable);
            }
            else if (entropyCardScript.EntropyCardID == 32)
            {
                object[] whatEffect = new object[] {0,1};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.entropytoOthers, whatEffect, AllPeople, SendOptions.SendReliable);
            }
        }
        if (entropyCardScript.removeAfterPlay)
        {
            removeCard(entropyCardScript);
        }
    }

    public void clickOnDiscardCard()
    {
        entropyCardScript = popUpEntropy.GetEntropyCardScript();
        if (drawCharacterCard.IsMyTurn && (drawCharacterCard.getMyCharScript().character_code == 12))
        {
            RollTime.addToRollChance(1);
        }
        removeCard(entropyCardScript);
        popUpEntropy.closePopup();
    }
    public void removeCard(EntropyCardScript whichEntropy)
    {
        foreach (Transform child in userEntorpyArea.transform)
        {
            if (child.GetComponent<EntropyCardDisplay>().entropyData == whichEntropy)
            {
                GameObject.Destroy(child.gameObject);
                break;
            }
        }
        moneyAndPoints.subMyCards(1);
    }
}
