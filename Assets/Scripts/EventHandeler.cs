using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UserAreas;
using DrawCards;

namespace main
{
    enum PhotonEventCode
    {
        upDateOtherOnGameRounds = 0,
        startGame = 1,
        inputArrangement = 2,
        playerChanged = 3,
        drawCharacterRemove= 4,
        setMyChar = 5,
    }
    public class EventHandeler : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private drawCharacterCard drawChar;
        [SerializeField] private TurnManager turnManager= null;
        public RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others,
        };
        public RaiseEventOptions AllPeople = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
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
            Debug.Log("Code =" + obj.Code);
            if (obj.Code == (byte)PhotonEventCode.upDateOtherOnGameRounds)
            {
                Debug.Log("Passed to round");
                object[] rounds = (object[])obj.CustomData;
                userControler.upDateOtherOnGameRounds((int)rounds[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.startGame)
            {
                Debug.Log("Passed to startGame");
                userControler.startingGame();
            }
            else if (obj.Code == (byte)PhotonEventCode.inputArrangement)
            {
                object[] arragement = (object[])obj.CustomData;
                Debug.Log("Input Arrangement : " + (int)arragement[0]);
                turnManager.inputArrangement((int)arragement[0], (bool)arragement[1], (bool)arragement[2]);
            }
            else if (obj.Code == (byte)PhotonEventCode.playerChanged)
            {
                turnManager.playerChanged();
            }
            else if (obj.Code == (byte)PhotonEventCode.drawCharacterRemove)
            {
                object[] cardID = (object[])obj.CustomData;
                drawChar.removeFormDeck((int)cardID[0]);
            }
            else if (obj.Code == (byte)PhotonEventCode.setMyChar)
            {
                object[] charStuff = (object[])obj.CustomData;
                userControler.setOtherCharacter((Player)charStuff[0],(int)charStuff[1]);
            }
        }
    }
}
