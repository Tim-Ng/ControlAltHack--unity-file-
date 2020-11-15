using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class MoneyAndPoints : MonoBehaviour
{
    private byte MyPoints = 0;
    private byte opponent1Points = 0;
    private byte opponent2Points = 0;
    private byte opponent3Points = 0;
    private byte opponent4Points = 0;
    private byte opponent5Points = 0;
    [SerializeField] public Text MyPointsOBJ, opponent1PointsOBJ, opponent2PointsOBJ, opponent3PointsOBJ, opponent4PointsOBJ, opponent5PointsOBJ;
    private List<byte> opponentpointslist = new List<byte>();
    private List<Text> opponentpointslistOBJ = new List<Text>();

    private int MyMoney = 0;
    private int opponent1Money = 0; 
    private int opponent2Money =0; 
    private int opponent3Money=0;
    private int opponent4Money =0;
    private int opponent5Money =0;
    [SerializeField]public Text MyMoneyOBJ, opponent1MoneyOBJ, opponent2MoneyOBJ, opponent3MoneyOBJ, opponent4MoneyOBJ, opponent5MoneyOBJ;
    private List<int> opponentpointsMoneylist = new List<int>();
    private List<Text> opponentpointsMoneylistOBJ = new List<Text>();

    private int MyEntropyCards = 0;
    private int opponent1EntropyCards = 0;
    private int opponent2EntropyCards = 0;
    private int opponent3EntropyCards = 0;
    private int opponent4EntropyCards = 0;
    private int opponent5EntropyCards = 0;
    [SerializeField] private GameObject userEntropyCardsArea,opponent1EntropyCardsArea, opponent2EntropyCardsArea, opponent3EntropyCardsArea, opponent4EntropyCardsArea, opponent5EntropyCardsArea, entropyICON;
    private List<int> amountOfEntropyCards = new List<int>();
    private List<GameObject> opponentEntorpyCardsAreaList = new List<GameObject>();

    [SerializeField] private DrawCharacterCard drawCharacterCard;

    [SerializeField] private Main_Game_before_start main_Game_Before_Start;
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
        receiverPoints = 9,
        receiverMoney  = 10,
        sendPeopleCards = 23,
    }
    void Start()
    {
        opponentpointslist.Add(opponent1Points);
        opponentpointslist.Add(opponent2Points);
        opponentpointslist.Add(opponent3Points);
        opponentpointslist.Add(opponent4Points);
        opponentpointslist.Add(opponent5Points);
        opponentpointsMoneylist.Add(opponent1Money);
        opponentpointsMoneylist.Add(opponent2Money);
        opponentpointsMoneylist.Add(opponent3Money);
        opponentpointsMoneylist.Add(opponent4Money);
        opponentpointsMoneylist.Add(opponent5Money);
        opponentpointslistOBJ.Add(opponent1PointsOBJ);
        opponentpointslistOBJ.Add(opponent2PointsOBJ);
        opponentpointslistOBJ.Add(opponent3PointsOBJ);
        opponentpointslistOBJ.Add(opponent4PointsOBJ);
        opponentpointslistOBJ.Add(opponent5PointsOBJ);
        opponentpointsMoneylistOBJ.Add(opponent1MoneyOBJ);
        opponentpointsMoneylistOBJ.Add(opponent2MoneyOBJ);
        opponentpointsMoneylistOBJ.Add(opponent3MoneyOBJ);
        opponentpointsMoneylistOBJ.Add(opponent4MoneyOBJ);
        opponentpointsMoneylistOBJ.Add(opponent5MoneyOBJ);
        opponentEntorpyCardsAreaList.Add(opponent1EntropyCardsArea);
        opponentEntorpyCardsAreaList.Add(opponent2EntropyCardsArea);
        opponentEntorpyCardsAreaList.Add(opponent3EntropyCardsArea);
        opponentEntorpyCardsAreaList.Add(opponent4EntropyCardsArea);
        opponentEntorpyCardsAreaList.Add(opponent5EntropyCardsArea);
        amountOfEntropyCards.Add(opponent1EntropyCards);
        amountOfEntropyCards.Add(opponent2EntropyCards);
        amountOfEntropyCards.Add(opponent3EntropyCards);
        amountOfEntropyCards.Add(opponent4EntropyCards);
        amountOfEntropyCards.Add(opponent5EntropyCards);
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
        if (obj.Code == (byte)PhotonEventCode.receiverPoints)
        {
            object[] pointdata = (object[])obj.CustomData;
            byte senderPoints = (byte)pointdata[0];
            Player senderPlayer = (Player)pointdata[1];
            if (senderPoints == 0)
            {
                main_Game_Before_Start.thisplayerIsFired(senderPlayer.ActorNumber);
            }
            else
            {
                opponentpointslist[main_Game_Before_Start.findPlayerPosition(senderPlayer)] = (byte)senderPoints;
                opponentpointslistOBJ[main_Game_Before_Start.findPlayerPosition(senderPlayer)].text = senderPoints.ToString();
            }
        }
        else if (obj.Code == (byte)PhotonEventCode.receiverMoney)
        {
            object[] moneydata = (object[])obj.CustomData;
            int senderMoney = (int)moneydata[0];
            Player senderPlayer = (Player)moneydata[1];
            opponentpointsMoneylist[main_Game_Before_Start.findPlayerPosition(senderPlayer)] = senderMoney;
            opponentpointsMoneylistOBJ[main_Game_Before_Start.findPlayerPosition(senderPlayer)].text = "$" + senderMoney.ToString();
        }
        else if (obj.Code == (byte)PhotonEventCode.sendPeopleCards)
        {
            object[] EntorpyData = (object[])obj.CustomData;
            int numberOfEntorpy = (int)EntorpyData[0];
            Player senderPlayer = (Player)EntorpyData[1];
            amountOfEntropyCards[main_Game_Before_Start.findPlayerPosition(senderPlayer)] = numberOfEntorpy;
            foreach (Transform child in opponentEntorpyCardsAreaList[main_Game_Before_Start.findPlayerPosition(senderPlayer)].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            for (int i = 0; i < numberOfEntorpy; i++)
            {
                GameObject missionCard = Instantiate(entropyICON, transform.position, Quaternion.identity);
                missionCard.transform.SetParent(opponentEntorpyCardsAreaList[main_Game_Before_Start.findPlayerPosition(senderPlayer)].transform, false);
            }
        }
    }
    public void addMyMoney(int amount)
    {
        MyMoney += amount;
        sendAllMyCurrentMON();
    }
    public void subMyMoney(int amount)
    {
        MyMoney -= amount;
        sendAllMyCurrentMON();
    }
    private void sendAllMyCurrentMON()
    {
        MyMoneyOBJ.text = "$" + MyMoney.ToString();
        object[] dataMyMoney = new object[] { MyMoney , PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiverMoney, dataMyMoney, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
    }
    public void addPoints(byte amount)
    {
        MyPoints += amount;
        sendAllMyCurrentPoint();
    }
    public void subPoints(byte amount)
    {
        if (MyPoints <= amount)
        {
            MyPoints = 0;
        }
        else
        {
            MyPoints -= amount;
        }
        if (MyPoints == 0)
        {
            main_Game_Before_Start.ifYouAreDead = true;
            main_Game_Before_Start.thisplayerIsFired(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        sendAllMyCurrentPoint();
    }
    private void sendAllMyCurrentPoint()
    {
        MyPointsOBJ.text = MyPoints.ToString();
        object[] dataMyPoint = new object[] { MyPoints, PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.receiverPoints, dataMyPoint, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
    }
    public int getMyMoneyAmount()
    {
        return MyMoney;
    }
    public byte getMyPoints()
    {
        return MyPoints;
    }
    public List<byte> getOpponentPoints()
    {
        return opponentpointslist;
    }
    public byte getOpponentPointsWithPlayer(int whichPlayer)
    {
        return opponentpointslist[whichPlayer];
    }
    public void resetMoneyAndPoints()
    {
        MyPoints = 0;
        for (int i = 0; i < opponentpointslist.Count; i++)
        {
            opponentpointslist[i] = 0;
            opponentpointslistOBJ[i].text= opponentpointslist[i].ToString();
        }
        MyMoney = 0;
        for (int i = 0; i < opponentpointsMoneylist.Count; i++)
        {
            opponentpointsMoneylist[i] = 0;
            opponentpointsMoneylistOBJ[i].text = opponentpointsMoneylist[i].ToString();
        }
    }
    public int getMyAmountOfEntropyCards()
    {
        return MyEntropyCards;
    }
    public void countMyNumOfEntropyCards()
    {
        MyEntropyCards =userEntropyCardsArea.transform.childCount;
        object[] dataCards = new object[] { MyEntropyCards, PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendPeopleCards, dataCards, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
    }
}
