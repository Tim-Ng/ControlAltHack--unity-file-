using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class DrawCharacterCard : MonoBehaviour
{
    public GameObject PlayerArea;
    public GameObject cardTemplate;
    public CharCardScript CharCard1;
    public GameObject button;
    private int x;

    List<CharCardScript> cardsInfo = new List<CharCardScript>();

    void Start()
    {
        cardsInfo.Add(CharCard1);
        /*cardsInfo.Add(CharCard2);
        cardsInfo.Add(CharCard3);
        cardsInfo.Add(CharCard4);
        cardsInfo.Add(CharCard5);
        cardsInfo.Add(CharCard6);
        cardsInfo.Add(CharCard7);
        cardsInfo.Add(CharCard8);
        cardsInfo.Add(CharCard9);
        cardsInfo.Add(CharCard10);
        cardsInfo.Add(CharCard11);
        cardsInfo.Add(CharCard12);*/
    }

    public void OnClick()
    {
        for (var i = 0; i < 3; i++)
        {
            x =Random.Range(0, (cardsInfo.Count-1));
            GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().CharCard = cardsInfo[x];
            characterPlayerCard1.GetComponent<CharacterCardDispaly>().FrontSide.SetActive(true);
            characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            //cardsInfo.Remove(cardsInfo[x]);
        }
        button.SetActive(false);
    }
}
