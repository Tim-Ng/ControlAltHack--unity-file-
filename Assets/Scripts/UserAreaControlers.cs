using MainMenu;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace UserAreas
{
    public class UserAreaControlers : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInfo thisUserArea,userArea1, userArea2, userArea3, userArea4, userArea5;
        [SerializeField] private TMP_InputField numberOfRounds_input;
        [SerializeField] private Text numberOfPeople;
        [SerializeField] private GameObject startGameItems, setRoundsButton,startGameButton,roomCode;
        [SerializeField] private DrawCards.drawCharacterCard drawCard;
        public static int AmountOfRounds;
        public static PhotonView pv;
        public static bool GameHasStarted { get; set; }
        private List<PlayerInfo> users = new List<PlayerInfo>();
        private void Start()
        {
            pv = GetComponent<PhotonView>();
            AmountOfRounds = 6;
            startGameItems.SetActive(true);
            GameHasStarted = false;
            roomCode.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
            users.Clear();
            users.Add(thisUserArea);
            users.Add(userArea1);
            users.Add(userArea2);
            users.Add(userArea3);
            users.Add(userArea4);
            users.Add(userArea5);
            updateNumberOfPlayers();
        }
        public void updateNumberOfPlayers()
        {
            numberOfPeople.text = PhotonNetwork.CurrentRoom.PlayerCount + "/6";
            if (PhotonNetwork.IsMasterClient)
            {
                startGameButton.SetActive(true);
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                    startGameButton.GetComponent<Button>().interactable = true;
                else
                    startGameButton.GetComponent<Button>().interactable = false;
                setRoundsButton.SetActive(true);
                setRoundsButton.GetComponent<Button>().interactable = false;
                numberOfRounds_input.interactable = true;
                pv.RPC("upDateOtherOnGameRounds", RpcTarget.All, AmountOfRounds);
            }
            else
            {
                startGameButton.SetActive(false);
                setRoundsButton.SetActive(false);
                numberOfRounds_input.interactable = false;
            }
            users[0].playerPhoton = PhotonNetwork.LocalPlayer;
            users[0].Nickname = PhotonNetwork.LocalPlayer.NickName;
            users[0].ActorID = PhotonNetwork.LocalPlayer.ActorNumber;
            users[0].amountOfCred = 0;
            users[0].amountOfMoney = 0;
            users[0].filled = true;
            Player[] listHoldCurrentPlayer = PhotonNetwork.PlayerListOthers;
            for (int i = 0; i < 5; i++)
            {
                if ((i+1) <= listHoldCurrentPlayer.Length)
                {
                    users[i + 1].playerPhoton = listHoldCurrentPlayer[i];
                    users[i + 1].Nickname = listHoldCurrentPlayer[i].NickName;
                    users[i + 1].ActorID = listHoldCurrentPlayer[i].ActorNumber;
                    users[i + 1].amountOfCred = 0;
                    users[i + 1].amountOfMoney = 0;
                    users[i + 1].filled = true;
                }
                else
                {
                    users[i + 1].playerPhoton = null;
                    users[i + 1].Nickname = "Waiting...";
                    users[i + 1].ActorID = 7;
                    users[i + 1].amountOfCred = 0;
                    users[i + 1].amountOfMoney = 0;
                    users[i + 1].filled = false;
                }
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == NickNameRoom.MaximumPeople)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Room FULL");
            }
            updateNumberOfPlayers();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (otherPlayer.IsMasterClient)
            {
                Debug.Log("Host named " + otherPlayer.NickName + " has left the room");
                Debug.Log("Host is changed to player named " + PhotonNetwork.PlayerList[0].NickName);
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
            }
            if (!GameHasStarted)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount < NickNameRoom.MaximumPeople)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    Debug.Log("Room not FULL again");
                }
                updateNumberOfPlayers();
            }
            else
            {
                int userposition = findPlayerPosition(otherPlayer);
                users[userposition].Nickname = "Player Left";
                users[userposition].filled = false;
            }
        }
        public int findPlayerPosition(Player whichPlayer)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].playerPhoton == whichPlayer)
                {
                    return i;
                }
            }
            Debug.LogError("Can't Find Player");
            return 7;
        }
        public bool ifRoundInputCanUse()
        {
            if (!(string.IsNullOrEmpty(numberOfRounds_input.text)))
            {
                if ((numberOfRounds_input.text).All(char.IsDigit))
                {
                    if (int.Parse(numberOfRounds_input.text) < 6)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
        public void whenRoundsInputChange() => setRoundsButton.GetComponent<Button>().interactable = true;
        public void onSetRoundInputButton()
        {
            if (ifRoundInputCanUse())
            {
                AmountOfRounds = int.Parse(numberOfRounds_input.text);
                numberOfRounds_input.text = AmountOfRounds.ToString();
                pv.RPC("upDateOtherOnGameRounds", RpcTarget.Others, AmountOfRounds);
            }
            else
            {
                numberOfRounds_input.text = AmountOfRounds.ToString();
            }
        }
        [PunRPC]
        public void upDateOtherOnGameRounds(int num_rounds)
        {
            AmountOfRounds = num_rounds;
            numberOfRounds_input.text = AmountOfRounds.ToString();
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            numberOfRounds_input.text = propertiesThatChanged["NumberRounds"].ToString();
        }
        public void startGame()
        {
            GameHasStarted = true;
            pv.RPC("startingGame", RpcTarget.All);
        }
        [PunRPC]
        public void startingGame()
        {
            startGameItems.SetActive(false);
            drawCard.drawCharCards(2);
        }
    }
}
