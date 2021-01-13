using MainMenu;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using main;
using DrawCards;
using TradeScripts;
using Avertars;
using System.Threading;

namespace UserAreas
{
    public class UserAreaControlers : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject ScriptsODJ = null;
        [SerializeField] private winCanvasController winCanvas = null;

        [SerializeField] private PlayerInfo thisUserArea = null, userArea1 = null, userArea2 = null, userArea3 = null, userArea4 = null, userArea5 = null;
        [SerializeField] private TMP_InputField numberOfRounds_input = null;
        [SerializeField] private Text numberOfPeople = null;
        [SerializeField] private GameObject startGameItems = null, setRoundsButton = null, startGameButton = null, roomCode = null, firedLeaveButton = null;

        private TurnManager turn = null;
        private EventHandeler EventManager = null;
        private TradeControler tradeControler= null;
        private drawEntropyCard drawEntropy = null;
        private drawMissionCard drawMission= null;
        private ChatController chatController = null;
        public int AmountOfRounds;
        public static bool GameHasStarted { get; set; }
        public List<PlayerInfo> users = new List<PlayerInfo>();
        private void Start()
        {
            turn = ScriptsODJ.GetComponent<TurnManager>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            tradeControler = ScriptsODJ.GetComponent<TradeControler>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            chatController = ScriptsODJ.GetComponent<ChatController>();
            chatController.onReceiveMessage("You have joined the room :"+PhotonNetwork.CurrentRoom.Name, null, false);
            string holdName;
            bool sameName;
            bool ChangedName = false;
            int howmanySameName = 0;
            do
            {
                sameName = false;
                holdName = PhotonNetwork.LocalPlayer.NickName;
                if (howmanySameName != 0)
                {
                    holdName += "(" + (char)(64 + howmanySameName) + ")";
                }
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
                {
                    if (PhotonNetwork.PlayerListOthers[i].NickName == holdName)
                    {
                        howmanySameName += 1;
                        Debug.Log("SameName");
                        sameName = true;
                        ChangedName = true;
                    }
                }
            } while (sameName);
            if (ChangedName)
            {
                PhotonNetwork.NickName += "(" + (char)(64 + howmanySameName) + ")";
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.foundSameName, null, EventManager.AllPeople, SendOptions.SendReliable);
                chatController.onReceiveMessage("Detected someone with the same name changing your name" + PhotonNetwork.CurrentRoom.Name, null, false);
            }
            startOBJs();
        }
        public void startOBJs()
        {
            GameHasStarted = false;
            AmountOfRounds = 6;
            chatController.onReceiveMessage("Number of rounds is " + AmountOfRounds, null, false);
            PhotonNetwork.CurrentRoom.IsOpen = true;
            startGameItems.SetActive(true);
            firedLeaveButton.SetActive(false);
            winCanvas.setWinCanvas = false;
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
                object[] round = new object[] { AmountOfRounds };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.upDateOtherOnGameRounds, round, EventManager.AllPeople, SendOptions.SendReliable);
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
            users[0].NumberOfCards = 0;
            users[0].MissionCards= 0;
            users[0].amountOfMoney = 0;
            users[0].characterScript = null;
            users[0].filled = true;
            users[0].fired = false;
            Player[] listHoldCurrentPlayer = PhotonNetwork.PlayerListOthers;
            for (int i = 0; i < 5; i++)
            {
                if ((i + 1) <= listHoldCurrentPlayer.Length)
                {
                    users[i + 1].playerPhoton = listHoldCurrentPlayer[i];
                    users[i + 1].Nickname = listHoldCurrentPlayer[i].NickName;
                    users[i + 1].ActorID = listHoldCurrentPlayer[i].ActorNumber;
                    users[i + 1].amountOfCred = 0;
                    users[i + 1].amountOfMoney = 0;
                    users[i + 1].NumberOfCards = 0;
                    users[i + 1].MissionCards = 0;
                    users[i + 1].characterScript = null;
                    users[i + 1].filled = true;
                    users[i + 1].attendingOrNot = false;
                    users[i + 1].fired = false;
                }
                else
                {
                    users[i + 1].playerPhoton = null;
                    users[i + 1].Nickname = "Waiting...";
                    users[i + 1].ActorID = 7;
                    users[i + 1].amountOfCred = 0;
                    users[i + 1].amountOfMoney = 0;
                    users[i + 1].NumberOfCards = 0;
                    users[i + 1].MissionCards = 0;
                    users[i + 1].characterScript = null;
                    users[i + 1].filled = false;
                    users[i + 1].attendingOrNot = false;
                    users[i + 1].fired = false;
                }
            }
            setMasterTextColour();
            chatController.setUpDropdownList();
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            chatController.onReceiveMessage(newPlayer.NickName + " has enter this room.", null, false);
            if (PhotonNetwork.CurrentRoom.PlayerCount == NickNameRoom.MaximumPeople)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Room FULL");
            }
            updateNumberOfPlayers();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            chatController.onReceiveMessage(otherPlayer.NickName + " has left this room.", null, false);
            if (otherPlayer.IsMasterClient)
            {
                Debug.Log("Host named " + otherPlayer.NickName + " has left the room");
                Debug.Log("Host is changed to player named " + PhotonNetwork.PlayerList[0].NickName);
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
                chatController.onReceiveMessage("Host has left the room host is changed to "+otherPlayer.NickName, null, false);
                if (!winCanvas.setWinCanvas)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        winCanvas.setInteractablePlayAgainButton = true;
                    }
                }
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
                turn.playerLeft(otherPlayer.ActorNumber);
            }
            setMasterTextColour();
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
        public int findPlayerPosition(int whichPlayer)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].ActorID == whichPlayer)
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
                object[] round = new object[] { AmountOfRounds };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.upDateOtherOnGameRounds, round, EventManager.AllPeople, SendOptions.SendReliable);
            }
            else
            {
                numberOfRounds_input.text = AmountOfRounds.ToString();
            }
            setRoundsButton.GetComponent<Button>().interactable = false;
        }
        public void upDateOtherOnGameRounds(int num_rounds)
        {
            if (AmountOfRounds != num_rounds)
            {
                chatController.onReceiveMessage("Number of rounds is set to " + num_rounds, null, false);
            }
            AmountOfRounds = num_rounds;
            numberOfRounds_input.text = AmountOfRounds.ToString();
            
        }
        public void startGame()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startGame, null, EventManager.AllPeople, SendOptions.SendReliable);
            object[] chatInfo = new object[] { "Game is Starting", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            turn.setArrangementForTurn();
        }
        public void startingGame()
        {
            GameHasStarted = true;
            startGameItems.SetActive(false);
        }
        public void setMyCharacter(CharCardScript whichCharacter)
        {
            users[0].characterScript = whichCharacter;
            object[] sendCharactertToOther = new object[] { PhotonNetwork.LocalPlayer, whichCharacter.character_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setMyChar, sendCharactertToOther, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void setOtherCharacter(Player whichPlayer, int whichCard)
        {
            users[findPlayerPosition(whichPlayer)].characterScript = charCardDeck.cardDeck[whichCard-1];
        }

        public void addMyMoney(int Howmuch)
        {
            users[0].amountOfMoney += Howmuch;
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfMoney };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerMoney, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void subMyMoney(int Howmuch)
        {
            users[0].amountOfMoney -= Howmuch;
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfMoney };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerMoney, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void receiveOtherMoney(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].amountOfMoney = amount;
        }
        public void addMyCred(int Howmuch)
        {
            users[0].amountOfCred += Howmuch;
            if (users[0].amountOfCred <= 0)
            {
                object[] chatInfo = new object[] { "Due to poor mission record "+PhotonNetwork.LocalPlayer.NickName+ " is fired.", null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                youAreFired();
            }
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfCred };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerCred, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void subMyCred(int Howmuch)
        {
            users[0].amountOfCred -= Howmuch;
            if (users[0].amountOfCred <= 0)
            {
                object[] chatInfo = new object[] { "Due to poor mission record " + PhotonNetwork.LocalPlayer.NickName + " is fired.", null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                youAreFired();
            }
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfCred };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerCred, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void receiveOtherCred(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].amountOfCred = amount;
        }
        public void sendAmountOfCards()
        {
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].NumberOfCards };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendAmountOfEntropy, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void receiveOtherAmountOfCards(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].NumberOfCards = amount;
        }
        public void setandsendIfNotAttending()
        {
            users[0].attendingOrNot = false;
            object[] yesOrNOBool = new object[] { PhotonNetwork.LocalPlayer};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.tradeNotAttending, yesOrNOBool, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void receiveOtherNotAttending(Player whichPlayer)
        {
            int PlayerPosition=  findPlayerPosition(whichPlayer);
            users[PlayerPosition].attendingOrNot = false;
            tradeControler.PlayerAttentingChange(PlayerPosition);
        }
        public void setandsendIfAttending()
        {
            users[0].attendingOrNot = true;
            object[] yesOrNOBool = new object[] { PhotonNetwork.LocalPlayer, users[0].missionScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.tradeAttending, yesOrNOBool, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void receiveOtherAttending(Player whichPlayer,int missionCardCode)
        {
            int PlayerPosition = findPlayerPosition(whichPlayer);
            users[PlayerPosition].attendingOrNot = true;
            tradeControler.PlayerAttentingChange(PlayerPosition, missionCardCode);
        }
        public void youAreFired()
        {
            users[0].fired = true;
            firedLeaveButton.SetActive(true);
            drawMission.removeAllCard();
            drawEntropy.removeAllEntropyCard();
            turn.playerLeft(PhotonNetwork.LocalPlayer.ActorNumber);
            object[] playerFired = new object[] { PhotonNetwork.LocalPlayer};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendPlayerFired, playerFired, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveFiredPlayer (Player whichPlayer)
        {
            int playerPosition = findPlayerPosition(whichPlayer);
            users[playerPosition].fired = true;
            users[playerPosition].Nickname = "Player Fired";
            turn.playerLeft(whichPlayer.ActorNumber);
        }
        public void clickOnLeaveGame()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("PlayerLeftRoom... loading scene");
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Changing scene");
            PhotonNetwork.LoadLevel(0);
        }
        public void onReceiveWinner1(int Player1)
        {
            GameHasStarted = false;
            winCanvas.setWinCanvas = true;
            winCanvas.setfirstPlaceHolder = true;
            winCanvas.setsecondPlaceHolder = false;
            winCanvas.setThirdPlaceHolder= false;
            setWinnerInfo1(Player1);
            if (PhotonNetwork.IsMasterClient)
            {
                winCanvas.setInteractablePlayAgainButton = true;
            }
            else
            {
                winCanvas.setInteractablePlayAgainButton = false;
            }
        }
        public void onReceiveWinner2(int Player1, int Player2)
        {
            GameHasStarted = false;
            winCanvas.setWinCanvas = true;
            winCanvas.setfirstPlaceHolder = true;
            winCanvas.setsecondPlaceHolder = true;
            winCanvas.setThirdPlaceHolder = false;
            setWinnerInfo1(Player1);
            setWinnerInfo2(Player2);
            if (PhotonNetwork.IsMasterClient)
            {
                winCanvas.setInteractablePlayAgainButton = true;
            }
            else
            {
                winCanvas.setInteractablePlayAgainButton = false;
            }
        }
        public void onReceiveWinner3(int Player1, int Player2,int Player3)
        {
            GameHasStarted = false;
            winCanvas.setWinCanvas = true;
            winCanvas.setfirstPlaceHolder = true;
            winCanvas.setsecondPlaceHolder = true;
            winCanvas.setThirdPlaceHolder = true;
            setWinnerInfo1(Player1);
            setWinnerInfo2(Player2);
            setWinnerInfo3(Player3);
            if (PhotonNetwork.IsMasterClient)
            {
                winCanvas.setInteractablePlayAgainButton = true;
            }
            else
            {
                winCanvas.setInteractablePlayAgainButton = false;
            }
        }
        public void setWinnerInfo1(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setFirstPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setFirstPlaceNickName = users[position].Nickname;
            if (users[position].characterScript == null)
            {
                winCanvas.setFirstPlaceAvertar = null;
            }
            else
            {
                winCanvas.setFirstPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
            }
        }
        public void setWinnerInfo2(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setSecondPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setSecondPlaceNickName = users[position].Nickname;
            if (users[position].characterScript == null)
            {
                winCanvas.setSecondPlaceAvertar = null;
            }
            else
            {
                winCanvas.setSecondPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
            }
        }
        public void setWinnerInfo3(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setThirdPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setThirdPlaceNickName = users[position].Nickname;
            if (users[position].characterScript == null)
            {
                winCanvas.setThirdPlaceAvertar = null;
            }
            else
            {
                winCanvas.setThirdPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
            }
        }
        public void clickOnPlayAgain()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.resetGame, null, EventManager.AllPeople, SendOptions.SendReliable);
        }
        public void setMasterTextColour()
        {
            for(int i = 0; i < 6; i++)
            {
                if (users[i].filled)
                {
                    if (users[i].playerPhoton.IsMasterClient)
                        users[i].setNickNameColour(Color.green);
                    else
                        users[i].setNickNameColour(Color.black);
                }
                else
                    users[i].setNickNameColour(Color.black);
            }
        }

    }
}
