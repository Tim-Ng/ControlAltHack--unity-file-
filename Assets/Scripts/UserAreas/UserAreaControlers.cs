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
    /// <summary>
    /// This script control the values of all user areas as well as the winner canvas
    /// </summary>
    public class UserAreaControlers : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// This is to hold the script for the winCanvasController
        /// </summary>
        [SerializeField,Header("Win canvas script")] private winCanvasController winCanvas = null;
        /// <summary>
        /// This is the script to control the game objects of the current player
        /// </summary>
        [SerializeField, Header("Player Area")] private PlayerInfo thisUserArea = null;
        /// <summary>
        /// This is the script to control the game objects of the other players 
        /// </summary>
        [SerializeField] private PlayerInfo userArea1 = null, userArea2 = null, userArea3 = null, userArea4 = null, userArea5 = null;
        /// <summary>
        /// This is the input feild 
        /// </summary>
        [SerializeField,Space(10)] private TMP_InputField numberOfRounds_input = null;
        
        /// <summary>
        /// The text for the UI for the number of people current in room 
        /// </summary>
        [SerializeField,Header("UI for texts")] private Text numberOfPeople = null;
        /// <summary>
        /// This is the gameobject for the UI to display the current Room name
        /// </summary>
        [SerializeField] private GameObject roomCode = null;
        /// <summary>
        /// The button to set the number of rounds 
        /// </summary>
        [SerializeField] private GameObject setRoundsButton = null;
        /// <summary>
        /// The button for the starting game
        /// </summary>
        [SerializeField] private GameObject startGameButton = null;
        /// <summary>
        /// The button for the leave button after getting fired
        /// </summary>
        [SerializeField] private GameObject firedLeaveButton = null;
        /// <summary>
        /// The game object that holds all the game object elements of the start game items
        /// </summary>
        [SerializeField, Space(20)] private GameObject startGameItems = null;
        /// <summary>
        /// This is the game object where this script is attatched to.
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// Holds the script TurnManager
        /// </summary>
        private TurnManager turn = null;
        /// <summary>
        /// Holds the script EventHandeler
        /// </summary>
        private EventHandeler EventManager = null;
        /// <summary>
        /// Holds the script TradeControler
        /// </summary>
        private TradeControler tradeControler= null;
        /// <summary>
        /// Holds the script drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntropy = null;
        /// <summary>
        /// Holds the script drawMissionCard
        /// </summary>
        private drawMissionCard drawMission= null;
        /// <summary>
        /// Holds the script ChatController
        /// </summary>
        private ChatController chatController = null;
        /// <summary>
        /// This holds the value for the number of rounds 
        /// </summary>
        [HideInInspector]public int AmountOfRounds;
        /// <summary>
        /// To get/set if the game has started or not
        /// </summary>
        /// <remarks>
        /// If its true then has started <br/?
        /// Else the game hasn't started
        /// </remarks>
        public static bool GameHasStarted { get; set; }
        /// <summary>
        /// This holds a list of the PlayerInfo scripts 
        /// </summary>
        /// <remarks>
        /// 0 = This current player <br/>
        /// 1 =  1st other player <br/>
        /// 2 =  2nd other player <br/>
        /// 3 =  3rd other player <br/>
        /// 4 =  4th other player <br/>
        /// 5 =  5th other player <br/>
        /// </remarks>
        [HideInInspector]public List<PlayerInfo> users = new List<PlayerInfo>();
        /// <summary>
        /// This function will run when the script is rendered
        /// </summary>
        /// <remarks>
        /// This will set up the scripts we needed <br/> 
        /// The number of players,room name and otherplayer infomation will be setup
        /// </remarks>
        private void Start()
        {
            ScriptsODJ = gameObject;
            turn = ScriptsODJ.GetComponent<TurnManager>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            tradeControler = ScriptsODJ.GetComponent<TradeControler>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            chatController = ScriptsODJ.GetComponent<ChatController>();
            chatController.onReceiveMessage("You have joined the room :\""+PhotonNetwork.CurrentRoom.Name+"\"", null, false);
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
        /// <summary>
        /// This functoin is used to set up the start objects and set to default 
        /// </summary>
        public void startOBJs()
        {
            GameHasStarted = false;
            AmountOfRounds = 6;
            chatController.onReceiveMessage("Number of rounds is " + AmountOfRounds, null, false);
            PhotonNetwork.CurrentRoom.IsOpen = true;
            startGameItems.SetActive(true);
            firedLeaveButton.SetActive(false);
            winCanvas.setWinCanvas = false;
            roomCode.GetComponent<Text>().text = "\""+PhotonNetwork.CurrentRoom.Name+ "\"" ;
            users.Clear();
            users.Add(thisUserArea);
            users.Add(userArea1);
            users.Add(userArea2);
            users.Add(userArea3);
            users.Add(userArea4);
            users.Add(userArea5);
            updateNumberOfPlayers();
        }
        /// <summary>
        /// The function is used to update the number of player text as well as setting up all the infomation for the other player with their respective UI [PlayerInfo]
        /// </summary>
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
        /// <summary>
        /// This function is called when a player has entered the room
        /// </summary>
        /// <remarks>
        /// Send info to the chat as well as closing the room if too many people and call function updateNumberOfPlayers
        /// </remarks>
        /// <param name="newPlayer">The joining player data</param>
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
        /// <summary>
        /// This function is called when a player left the room
        /// </summary>
        /// <remarks>
        /// Will look if the game has started of not and act accordingly
        /// </remarks>
        /// <param name="otherPlayer">The joining player data</param>
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
        /// <summary>
        /// This function is used to find a player position using their Player data
        /// </summary>
        /// <param name="whichPlayer">The player's data to find</param>
        /// <returns>The position in the users list </returns>
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
        /// <summary>
        /// This function is used to find a player position using their Player actor ID
        /// </summary>
        /// <param name="whichPlayer">The player's actor ID</param>
        /// <returns>The position in the users list </returns>
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
        /// <summary>
        /// This is to check if the input of the number of rounds is usable 
        /// </summary>
        /// <remarks>
        /// The round input cannot be a string and if its integer than cannot be lesser than 6
        /// </remarks>
        /// <returns>
        /// True if can use <br/>
        /// False if cannot use 
        /// </returns>
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
        /// <summary>
        /// This function is called when there is a change in the text input for the number of rounds
        /// </summary>
        /// <remarks>
        /// Will make the setRoundsButton to make the interactable to true
        /// </remarks>
        public void whenRoundsInputChange() => setRoundsButton.GetComponent<Button>().interactable = true;
        /// <summary>
        /// This is when the button setRoundsButton is pressed
        /// </summary>
        /// <remarks>
        /// If can use the input then the new round number will be sent to the other players <br/>
        /// If the number of rounds will be set to the number before the new input
        /// </remarks>
        public void onSetRoundInputButton()
        {
            if (ifRoundInputCanUse())
            {
                AmountOfRounds = int.Parse(numberOfRounds_input.text);
                numberOfRounds_input.text = AmountOfRounds.ToString();
                object[] chatInfo = new object[] { "Number of round is changed to :" + AmountOfRounds, null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                object[] round = new object[] { AmountOfRounds };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.upDateOtherOnGameRounds, round, EventManager.AllPeople, SendOptions.SendReliable);
            }
            else
            {
                numberOfRounds_input.text = AmountOfRounds.ToString();
            }
            setRoundsButton.GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// This will be used to update the number of rounds and the UI
        /// </summary>
        /// <param name="num_rounds">The new value of number of rounds</param>
        public void upDateOtherOnGameRounds(int num_rounds)
        {
            AmountOfRounds = num_rounds;
            numberOfRounds_input.text = AmountOfRounds.ToString();
        }
        /// <summary>
        /// This function is called when the button for the start game is pressed. This will send info to all the players so set up the game
        /// </summary>
        public void startGame()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startGame, null, EventManager.AllPeople, SendOptions.SendReliable);
            object[] chatInfo = new object[] { "Game is Starting", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            turn.setArrangementForTurn();
        }
        /// <summary>
        /// This function is called is when the game has started
        /// </summary>
        public void startingGame()
        {
            GameHasStarted = true;
            startGameItems.SetActive(false);
        }
        /// <summary>
        /// This function is to set this current player's character.The info will be sent to other players
        /// </summary>
        /// <param name="whichCharacter"></param>
        public void setMyCharacter(CharCardScript whichCharacter)
        {
            users[0].characterScript = whichCharacter;
            object[] sendCharactertToOther = new object[] { PhotonNetwork.LocalPlayer, whichCharacter.character_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setMyChar, sendCharactertToOther, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is called when other player's character is set. 
        /// </summary>
        /// <param name="whichPlayer">The player that has choosed their character</param>
        /// <param name="whichCard">The card's player ID</param>
        public void setOtherCharacter(Player whichPlayer, int whichCard)
        {
            users[findPlayerPosition(whichPlayer)].characterScript = charCardDeck.cardDeck[whichCard-1];
        }
        /// <summary>
        /// This function is to add this player's money. Send the new number of money to everyone esle
        /// </summary>
        /// <param name="Howmuch">How much to increase</param>
        public void addMyMoney(int Howmuch)
        {
            users[0].amountOfMoney += Howmuch;
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfMoney };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerMoney, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is to sub this player's money. Send the new number of money to everyone esle
        /// </summary>
        /// <param name="Howmuch">How much to decrease</param>
        public void subMyMoney(int Howmuch)
        {
            users[0].amountOfMoney -= Howmuch;
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].amountOfMoney };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.playerMoney, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is called when there is an update in the amount of money another player has 
        /// </summary>
        /// <param name="whichPlayer">The player with money change</param>
        /// <param name="amount">The amount of money </param>
        public void receiveOtherMoney(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].amountOfMoney = amount;
        }
        /// <summary>
        /// This function is used to add the current player's hacker cred, and send the new value to other players 
        /// </summary>
        /// <param name="Howmuch">Increase by how much</param>
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
        /// <summary>
        /// This function is used to sub the current player's hacker cred, and send the new value to other players 
        /// </summary>
        /// <param name="Howmuch">decrease by how much</param>
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
        /// <summary>
        /// This function is called when a player updates their hacker cred 
        /// </summary>
        /// <param name="whichPlayer">The player with hacker cred change</param>
        /// <param name="amount">The new amount </param>
        public void receiveOtherCred(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].amountOfCred = amount;
        }
        /// <summary>
        /// This is used to send everyone else the current number of entropy cards this player has 
        /// </summary>
        public void sendAmountOfCards()
        {
            object[] amount = new object[] { PhotonNetwork.LocalPlayer, users[0].NumberOfCards };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendAmountOfEntropy, amount, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This function is called when a player sent their number of cards 
        /// </summary>
        /// <param name="whichPlayer">The player that sent the data</param>
        /// <param name="amount">The amount of entorpy </param>
        public void receiveOtherAmountOfCards(Player whichPlayer, int amount)
        {
            users[findPlayerPosition(whichPlayer)].NumberOfCards = amount;
        }
        /// <summary>
        /// This is to send everyone that this current player has either done meeting or not attending the meeting 
        /// </summary>
        /// <param name="DoneTrading">
        /// If true then done trading <br/>
        /// Else not done trading and is not attending
        /// </param>
        public void setandsendIfNotAttending(bool DoneTrading)
        {
            users[0].attendingOrNot = false;
            object[] yesOrNOBool = new object[] { PhotonNetwork.LocalPlayer, DoneTrading };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.tradeNotAttending, yesOrNOBool, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is on receiving if another player is not attending the meeting or done with trading 
        /// </summary>
        /// <param name="whichPlayer">The player who sent the info</param>
        /// <param name="clickedOnDoneTrading">The info its not attending or done trading</param>
        public void receiveOtherNotAttending(Player whichPlayer,bool clickedOnDoneTrading)
        {
            int PlayerPosition=  findPlayerPosition(whichPlayer);
            users[PlayerPosition].attendingOrNot = false;
            tradeControler.PlayerAttentingChange(PlayerPosition, clickedOnDoneTrading);
        }
        /// <summary>
        /// This is to send everyone that this current player is attending the meeting
        /// </summary>
        public void setandsendIfAttending()
        {
            users[0].attendingOrNot = true;
            object[] yesOrNOBool = new object[] { PhotonNetwork.LocalPlayer, users[0].missionScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.tradeAttending, yesOrNOBool, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is on receiving if another player is attending the meeting 
        /// </summary>
        /// <param name="whichPlayer">The player who sent the info</param>
        /// <param name="missionCardCode">Mission card info</param>
        public void receiveOtherAttending(Player whichPlayer,int missionCardCode)
        {
            int PlayerPosition = findPlayerPosition(whichPlayer);
            users[PlayerPosition].attendingOrNot = true;
            tradeControler.PlayerAttentingChange(PlayerPosition, missionCardCode);
        }
        /// <summary>
        /// If this player is fired this function is called
        /// </summary>
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
        /// <summary>
        /// If another player if fired this function is called
        /// </summary>
        public void onReceiveFiredPlayer (Player whichPlayer)
        {
            int playerPosition = findPlayerPosition(whichPlayer);
            users[playerPosition].fired = true;
            users[playerPosition].Nickname = "Player Fired";
            turn.playerLeft(whichPlayer.ActorNumber);
        }
        /// <summary>
        /// This function is used to leave the room
        /// </summary>
        public void clickOnLeaveGame()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("PlayerLeftRoom... loading scene");
        }
        /// <summary>
        /// This function is called if this current player has left the room and set the scene to the nickname and hostJoin scene
        /// </summary>
        public override void OnLeftRoom()
        {
            Debug.Log("Changing scene");
            PhotonNetwork.LoadLevel(0);
        }
        /// <summary>
        /// This function is used if only one person is in room after the game ended and set up the winner canvas
        /// </summary>
        /// <param name="Player1"> 1st place player actor ID </param>
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
        /// <summary>
        /// This function is used if only two people is in room after the game ended and set up the winner canvas
        /// </summary>
        /// <param name="Player1"> 1st place player actor ID </param>
        /// <param name="Player2"> 2nd place player actor ID </param>
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
        /// <summary>
        /// This function is used if more than 3 people is in room after the game ended and set up the winner canvas
        /// </summary>
        /// <param name="Player1"> 1st place player actor ID </param>
        /// <param name="Player2"> 2nd place player actor ID </param>
        /// <param name="Player3"> 3rd place player actor ID</param>
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
        /// <summary>
        /// Setting the UI and infomation of the 1st place area
        /// </summary>
        /// <param name="PlayerID"> The 1st place actor ID </param>
        public void setWinnerInfo1(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setFirstPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setFirstPlaceNickName = users[position].Nickname;
            winCanvas.setFirstPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
        }
        /// <summary>
        /// Setting the UI and infomation of the 2nd place area
        /// </summary>
        /// <param name="PlayerID"> The 2nd place actor ID </param>
        public void setWinnerInfo2(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setSecondPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setSecondPlaceNickName = users[position].Nickname;
            winCanvas.setSecondPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
        }
        /// <summary>
        /// Setting the UI and infomation of the 3rd place area
        /// </summary>
        /// <param name="PlayerID"> The 3rd place actor ID </param>
        public void setWinnerInfo3(int PlayerID)
        {
            int position = findPlayerPosition(PlayerID);
            winCanvas.setThirdPlaceAmountOfCred = users[position].amountOfCred;
            winCanvas.setThirdPlaceNickName = users[position].Nickname;
            winCanvas.setThirdPlaceAvertar = AvertarList.AvertarLists[int.Parse((string)users[position].playerPhoton.CustomProperties["AvertarCode"])];
        }
        /// <summary>
        /// This button is called when the player again button is clicked on 
        /// </summary>
        public void clickOnPlayAgain()
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.resetGame, null, EventManager.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to set the colour of the nickname of the host to green
        /// </summary>
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
