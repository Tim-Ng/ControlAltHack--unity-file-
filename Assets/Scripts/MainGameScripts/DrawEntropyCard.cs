using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEntropyCard : MonoBehaviour
{
    public GameObject EntropycardTemplate;
    public GameObject PlayerArea;
    public GameObject OponentArea1;
    public int OponentArea1_cards;
    public GameObject OponentArea2;
    public int OponentArea2_cards;
    public GameObject OponentArea3;
    public int OponentArea3_cards;
    public GameObject OponentArea4;
    public int OponentArea4_cards;
    public GameObject OponentArea5;
    public int OponentArea5cards;
    public GameObject dummy_entropy_card;

    //inputmultiple cards
    public EntropyCardScript entropycardBag1;
    public EntropyCardScript entropycardLigting1;
    private List<EntropyCardScript> entropycards = new List<EntropyCardScript>(); 


    private int x;

    private void Start()
    {
        //add all cards to a list here
        entropycards.Add(entropycardBag1);
        entropycards.Add(entropycardLigting1);
    }
    public void distribute_entropycard(int how_many)
    {
        for (var i = 0; i < how_many; i++)
        {
            x = Random.Range(0, (entropycards.Count));
            GameObject entropyCard = Instantiate(EntropycardTemplate, transform.position, Quaternion.identity);
            entropyCard.GetComponent<EntropyCardDisplay>().entropyData = entropycards[x];
            entropyCard.GetComponent<EntropyCardDisplay>().FrontSide.SetActive(true);
            entropyCard.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            entropyCard.transform.SetParent(PlayerArea.transform, false);
            //entropycards.Remove(entropycards[x]);
        }
    }

    //keep checking opponent amount

    public void setOponentArea1_card(int amount)
    {
        OponentArea1_cards = amount;
    }
    public void setOponentArea2_card(int amount)
    {
        OponentArea1_cards = amount;
    }
    public void setOponentArea3_card(int amount)
    {
        OponentArea1_cards = amount;
    }
    public void setOponentArea4_card(int amount)
    {
        OponentArea1_cards = amount;
    }
    public void setOponentArea5_card(int amount)
    {
        OponentArea1_cards = amount;
    }
}
