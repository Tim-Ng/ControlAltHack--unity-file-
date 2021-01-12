using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using DrawCards;
using Avertars;

namespace UserAreas
{
    public class PlayerInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Avertar = null, AmountOfCardArea = null, Username = null, HackerCred = null, Cash = null,EntorpyTemplate = null;
        private Player photonPlayerInfo = null;
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
                    Avertar.GetComponent<Image>().sprite = AvertarList.AvertarLists[int.Parse((string)photonPlayerInfo.CustomProperties["AvertarCode"])];
                }
            }
        }
        private string nickname;
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; Username.GetComponent<Text>().text = nickname; }
        }
        public int ActorID { get; set; }
        private int AmountOfCred;
        public int amountOfCred
        {
            get { return AmountOfCred; }
            set { AmountOfCred = value; HackerCred.GetComponent<Text>().text = AmountOfCred.ToString(); }
        }
        private int AmountOfMoney;
        public int amountOfMoney
        {
            get { return AmountOfMoney; }
            set { AmountOfMoney = value; Cash.GetComponent<Text>().text = "$" + AmountOfMoney.ToString(); }
        }
        private CharCardScript CharScript;
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
        private int numberOfEntroCards;
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
        public bool filled { get; set; }
        private void Start()
        {
            Avertar.GetComponent<Button>().interactable = false;
        }
        public int MissionCards { get; set; }
        public MissionCardScript missionScript { get; set; }
        public bool attendingOrNot { get; set; }
        public bool fired { get; set; }
        public void setNickNameColour(Color whatColor) => Username.GetComponent<Text>().color = whatColor;
    }
}
