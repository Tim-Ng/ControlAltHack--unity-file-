using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using DrawCards;

namespace UserAreas
{
    public class PlayerInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Avertar = null, AmountOfCards = null, Username = null, HackerCred = null, Cash = null;
        public Player playerPhoton {get; set;}
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
                Avertar.GetComponent<Image>().sprite = CharScript.image_Avertar;
                Avertar.GetComponent<Button>().interactable = true;
            } 
        }
        public bool filled { get; set; }
        private void Start()
        {
            Avertar.GetComponent<Button>().interactable = false;
        }

    }
}
