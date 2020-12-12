using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UserAreas
{
    public class PlayerInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Avertar, AmountOfCards, Username, HackerCred, Cash;
        public Player playerPhoton { get; set; }
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
        public int characterID { get; set; }
        public bool filled { get; set; }
        
    }
}
