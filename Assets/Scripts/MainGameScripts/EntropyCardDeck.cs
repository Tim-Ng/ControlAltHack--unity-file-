using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyCardDeck : MonoBehaviour
{
    //Bag Of Tricks
    public EntropyCardScript entropyCardBagScript1;
    public EntropyCardScript entropyCardBagScript2;
    public EntropyCardScript entropyCardBagScript3;
    public EntropyCardScript entropyCardBagScript4;
    public EntropyCardScript entropyCardBagScript13;
    public EntropyCardScript entropyCardBagScript14;
    public EntropyCardScript entropyCardBagScript15;
    public EntropyCardScript entropyCardBagScript16;
    public EntropyCardScript entropyCardBagScript17;
    public EntropyCardScript entropyCardBagScript18;
    public EntropyCardScript entropyCardBagScript19;
    //Lighting
    public EntropyCardScript entropyCardLightingScript1;
    public EntropyCardScript entropyCardLightingScript2;
    public EntropyCardScript entropyCardLightingScript3;
    public EntropyCardScript entropyCardLightingScript4;
    public EntropyCardScript entropyCardLightingScript5;
    public EntropyCardScript entropyCardLightingScript6;
    public EntropyCardScript entropyCardLightingScript7;
    //Extensive Experience
    public EntropyCardScript entropyCardExtensiveScript1;
    public EntropyCardScript entropyCardExtensiveScript2;
    public EntropyCardScript entropyCardExtensiveScript3;
    public EntropyCardScript entropyCardExtensiveScript4;
    public EntropyCardScript entropyCardExtensiveScript5;
    //to mix around
    public EntropyCardScript entropyCardBagScript5;
    public EntropyCardScript entropyCardBagScript6;
    public EntropyCardScript entropyCardBagScript7;
    public EntropyCardScript entropyCardBagScript8;
    public EntropyCardScript entropyCardBagScript9;
    public EntropyCardScript entropyCardBagScript10;
    public EntropyCardScript entropyCardBagScript11;
    public EntropyCardScript entropyCardBagScript12;
    //shared fate
    public EntropyCardScript entropyChardSharedScript1;
    public EntropyCardScript entropyChardSharedScript2;
    

    List<EntropyCardScript> entropyCardDeck = new List<EntropyCardScript>();
    public List<EntropyCardScript> getentropyCards()
    {
        entropyCardDeck.Add(entropyCardBagScript1);
        entropyCardDeck.Add(entropyCardBagScript2);
        entropyCardDeck.Add(entropyCardBagScript3);
        entropyCardDeck.Add(entropyCardBagScript4);
        entropyCardDeck.Add(entropyCardBagScript5);
        entropyCardDeck.Add(entropyCardBagScript6);
        entropyCardDeck.Add(entropyCardBagScript7);
        entropyCardDeck.Add(entropyCardBagScript8);
        entropyCardDeck.Add(entropyCardBagScript9);
        entropyCardDeck.Add(entropyCardBagScript10);
        entropyCardDeck.Add(entropyCardBagScript11);
        entropyCardDeck.Add(entropyCardBagScript12);
        entropyCardDeck.Add(entropyCardBagScript13);
        entropyCardDeck.Add(entropyCardBagScript14);
        entropyCardDeck.Add(entropyCardBagScript15);
        entropyCardDeck.Add(entropyCardBagScript16);
        entropyCardDeck.Add(entropyCardBagScript17);
        entropyCardDeck.Add(entropyCardBagScript18);
        entropyCardDeck.Add(entropyCardBagScript19);

        entropyCardDeck.Add(entropyCardLightingScript1);
        entropyCardDeck.Add(entropyCardLightingScript2);
        entropyCardDeck.Add(entropyCardLightingScript3);
        entropyCardDeck.Add(entropyCardLightingScript4);
        entropyCardDeck.Add(entropyCardLightingScript5);
        entropyCardDeck.Add(entropyCardLightingScript6);
        entropyCardDeck.Add(entropyCardLightingScript7);
        
        entropyCardDeck.Add(entropyCardExtensiveScript1);
        entropyCardDeck.Add(entropyCardExtensiveScript2);
        entropyCardDeck.Add(entropyCardExtensiveScript3);
        entropyCardDeck.Add(entropyCardExtensiveScript4);
        entropyCardDeck.Add(entropyCardExtensiveScript5);

        entropyCardDeck.Add(entropyChardSharedScript1);
        entropyCardDeck.Add(entropyChardSharedScript2);

        return entropyCardDeck;
    }
}
