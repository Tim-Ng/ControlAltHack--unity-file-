using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrawCards
{
    public class drawCharacterCard : MonoBehaviour
    {
        private List<int> characterCardID = new List<int>();
        [SerializeField] private GameObject cardArea,cardTemplate;
        [SerializeField]
        private CharCardScript
            card1,
            card2,
            card3,
            card4,
            card5,
            card6,
            card7,
            card8,
            card9,
            card10,
            card11,
            card12,
            card13,
            card14,
            card15,
            card16;
        private void Start()
        {
            characterCardID.Clear();
            characterCardID.Add(1);
            characterCardID.Add(2);
            characterCardID.Add(3);
            characterCardID.Add(4);
            characterCardID.Add(5);
            characterCardID.Add(6);
            characterCardID.Add(7);
            characterCardID.Add(8);
            characterCardID.Add(9);
            characterCardID.Add(10);
            characterCardID.Add(11);
            characterCardID.Add(12);
            characterCardID.Add(13);
            characterCardID.Add(14);
            characterCardID.Add(15);
            characterCardID.Add(16);
        }
        public static void drawCard(int number)
        {

        }
    }
}