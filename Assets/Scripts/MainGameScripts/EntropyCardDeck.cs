using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyCardDeck : MonoBehaviour
{
    public EntropyCardScript entropyCardScript1;
    public EntropyCardScript entropyCardScript2;
    public EntropyCardScript entropyCardScript3;
    public EntropyCardScript entropyCardScript4;
    public EntropyCardScript entropyCardScript5;
    public EntropyCardScript entropyCardScript6;
    public EntropyCardScript entropyCardScript7;
    public EntropyCardScript entropyCardScript8;
    public EntropyCardScript entropyCardScript9;
    public EntropyCardScript entropyCardScript10;
    public EntropyCardScript entropyCardScript11;

    List<EntropyCardScript> entropyCardDeck = new List<EntropyCardScript>();
    public List<EntropyCardScript> getentropyCards()
    {
        entropyCardDeck.Add(entropyCardScript1);
        entropyCardDeck.Add(entropyCardScript2);
        entropyCardDeck.Add(entropyCardScript3);
        entropyCardDeck.Add(entropyCardScript4);
        entropyCardDeck.Add(entropyCardScript5);
        entropyCardDeck.Add(entropyCardScript6);
        entropyCardDeck.Add(entropyCardScript7);
        entropyCardDeck.Add(entropyCardScript8);
        entropyCardDeck.Add(entropyCardScript9);
        entropyCardDeck.Add(entropyCardScript10);
        entropyCardDeck.Add(entropyCardScript11);
        return entropyCardDeck;
    }
}
