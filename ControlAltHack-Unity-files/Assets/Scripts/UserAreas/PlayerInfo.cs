using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using DrawCards;
using Avatars;

namespace UserAreas
{
    /// <summary>
    /// This scripts holds and controls all the elements of the each player datas 
    /// </summary>
    public class PlayerInfo : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// This holds an element of the player info area game objects 
        /// </summary>
        [SerializeField,Header("Game objects")] private GameObject Avertar = null;
        /// <summary>
        /// This holds an element of the player info area game objects 
        /// </summary>
        [SerializeField] private GameObject AmountOfCardArea = null, Username = null, HackerCred = null, Cash = null,EntorpyTemplate = null;
        /// <summary>
        /// This holds the value of the player photon data
        /// </summary>
        private Player photonPlayerInfo = null;
        /// <summary>
        /// This is to get/set the player player photon info
        /// </summary>
        /// <remarks>
        /// This will also set the Avertar of the player 
        /// </remarks>
        public Player playerPhoton {
            get {return photonPlayerInfo; }
            set
            {
                photonPlayerInfo = value;
                if (photonPlayerInfo == null)
                {
                    Avertar.GetComponent<Image>().sprite = null;
                }
                else
                {
                    Avertar.GetComponent<Image>().sprite = AvatarList.AvatarLists[int.Parse((string)photonPlayerInfo.CustomProperties["AvatarCode"])];
                }
            }
        }
        /// <summary>
        /// This is to get the sprite of this player's avertar
        /// </summary>
        public Sprite GetAvertarSprite
        {
            get { return Avertar.GetComponent<Image>().sprite; }
        }
        /// <summary>
        /// This is to set/get the value text UI of the Nickname of the current player 
        /// </summary>
        public string Nickname
        {
            get { return Username.GetComponent<Text>().text; }
            set { Username.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// To get/set the Actor ID of this player
        /// </summary>
        public int ActorID { get; set; }
        /// <summary>
        /// This is to hold the amount of cred of the player 
        /// </summary>
        private int AmountOfCred;
        /// <summary>
        /// Get/set the value of the amout of cred as well as setting the UI for the amount of cred of the player 
        /// </summary>
        public int amountOfCred
        {
            get { return AmountOfCred; }
            set { AmountOfCred = value; HackerCred.GetComponent<Text>().text = AmountOfCred.ToString(); }
        }
        /// <summary>
        /// This is to hold the amount of money this player has
        /// </summary>
        private int AmountOfMoney;
        /// <summary>
        /// Get/set the value of the amout of monay as well as setting the UI for the amount of money of the player 
        /// </summary>
        public int amountOfMoney
        {
            get { return AmountOfMoney; }
            set { AmountOfMoney = value; Cash.GetComponent<Text>().text = "$" + AmountOfMoney.ToString(); }
        }
        /// <summary>
        /// This is to hold the value of the charater script of this player 
        /// </summary>
        private CharCardScript CharScript;
        /// <summary>
        /// This is to get/set the CharCardScript of the player
        /// </summary>
        /// <remarks>
        /// When setting the value this player will turn the button on the avertar to be interactable <br/>
        /// If CharScript == null then false else true as interactable
        /// </remarks>
        public CharCardScript characterScript 
        {
            get 
            { 
                return CharScript; 
            }
            set 
            { 
                CharScript = value;
                Avertar.GetComponent<Button>().interactable = (CharScript != null);
            } 
        }
        /// <summary>
        /// This holds the number of entropy cards that this person has
        /// </summary>
        private int numberOfEntroCards;
        /// <summary>
        /// To set/get the the number of entropy cards
        /// </summary>
        /// <remarks>
        /// When set this will also display the number of cards there is in at the AmountOfCardArea of the player
        /// </remarks>
        public int NumberOfCards 
        {
            get { return numberOfEntroCards; }
            set 
            {
                numberOfEntroCards = value;
                if (!(playerPhoton == PhotonNetwork.LocalPlayer))
                {
                    foreach (Transform child in AmountOfCardArea.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    for (int i = 0; i < numberOfEntroCards; i++)
                    {
                        GameObject missionCard = Instantiate(EntorpyTemplate, transform.position, Quaternion.identity);
                        missionCard.transform.SetParent(AmountOfCardArea.transform, false);
                    }
                }
            }
        }
        /// <summary>
        /// get/set if there is a player in this place or not 
        /// </summary>
        /// <remarks>
        /// True = there is a person else there isn't
        /// </remarks>
        public bool filled { get; set; }
        /// <summary>
        /// This function is called when this script is rendered and will make the avertar button interactable to be false
        /// </summary>
        private void Start()
        {
            Avertar.GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// This is to get/set the number of mission cards 
        /// </summary>
        public int MissionCards { get; set; }
        /// <summary>
        /// This is to get/set the current MissionCardScript
        /// </summary>
        public MissionCardScript missionScript { get; set; }
        /// <summary>
        /// This is to get/set if this player is attending the meeting or not 
        /// </summary>
        public bool attendingOrNot { get; set; }
        /// <summary>
        /// This is to get/set if this player is fired or not
        /// </summary>
        /// <remarks>
        /// True = has been fired <br/>
        /// False = hasn't been fired
        /// </remarks>
        public bool fired { get; set; }
        /// <summary>
        /// This is to set the colour of the NickName as to indicate the host 
        /// </summary>
        /// <param name="whatColor"> What color to change to</param>
        public void setNickNameColour(Color whatColor) => Username.GetComponent<Text>().color = whatColor;
    }
}
