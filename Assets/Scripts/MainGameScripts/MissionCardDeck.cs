using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCardDeck : MonoBehaviour
{
    public MissionCardScript missionScript1;
    public MissionCardScript missionScript2;
    public MissionCardScript missionScript3;
    public MissionCardScript missionScript4;
    public MissionCardScript missionScript5;
    public MissionCardScript missionScript6;
    public MissionCardScript missionScript7;
    public MissionCardScript missionScript8;
    public MissionCardScript missionScript9;
    public MissionCardScript missionScript10;
    public MissionCardScript missionScript11;
    public MissionCardScript missionScript12;
    public MissionCardScript missionScript13;
    public MissionCardScript missionScript14;
    public MissionCardScript missionScript15;
    public MissionCardScript missionScript16;
    public MissionCardScript missionScript17;
    public MissionCardScript missionScript18;

    private List<MissionCardScript> missionCardDeck = new List<MissionCardScript>();
    public List<MissionCardScript> getMissionCardDeck()
    {
        missionCardDeck.Add(missionScript1);
        missionCardDeck.Add(missionScript2);
        missionCardDeck.Add(missionScript3);
        missionCardDeck.Add(missionScript4);
        missionCardDeck.Add(missionScript5);
        missionCardDeck.Add(missionScript6);
        missionCardDeck.Add(missionScript7);
        missionCardDeck.Add(missionScript8);
        missionCardDeck.Add(missionScript9);
        missionCardDeck.Add(missionScript10);
        missionCardDeck.Add(missionScript11);
        missionCardDeck.Add(missionScript12);
        missionCardDeck.Add(missionScript13);
        missionCardDeck.Add(missionScript14);
        missionCardDeck.Add(missionScript15);
        missionCardDeck.Add(missionScript16);
        missionCardDeck.Add(missionScript17);
        missionCardDeck.Add(missionScript18);
        return missionCardDeck;
    }
}
