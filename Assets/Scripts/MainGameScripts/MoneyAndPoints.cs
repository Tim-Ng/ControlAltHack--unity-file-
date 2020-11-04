using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            int senderPoints = (byte)pointdata[0];
            Player senderPlayer = (Player)pointdata[1];
            int i = 0;
            foreach(Player checkPlayer in PhotonNetwork.PlayerListOthers)
            {
                if (checkPlayer == senderPlayer)
                {
                    opponentpointslist[i] = (byte) senderPoints;
                    opponentpointslistOBJ[i].text = senderPoints.ToString();
                    break;
                }
                i++;
            }
        }

        else if (obj.Code == (byte)PhotonEventCode.receiverMoney)
        {
            object[] moneydata = (object[])obj.CustomData;
            int senderMoney = (int)moneydata[0];
            Player senderPlayer = (Player)moneydata[1];
            int i = 0;
            foreach (Player checkPlayer in PhotonNetwork.PlayerListOthers)
            {
                if (checkPlayer == senderPlayer)
                {
                    opponentpointsMoneylist[i] = senderMoney;
                    opponentpointsMoneylistOBJ[i].text ="$" + senderMoney.ToString();
                    break;
                }
                i++;
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
        MyPoints -= amount;
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
}
