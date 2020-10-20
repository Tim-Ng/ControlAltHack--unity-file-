using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card",menuName ="CharacterCard")]
public class CharCardScript : ScriptableObject
{
    public string character_code;
    public string character_card_name;
    public string discription;

    public Sprite artwork_back;
    public Sprite artwork_front;
    public Sprite char_info;

    public int input_hardware;
    public int input_cryptanalysis;
    public int input_network;
    public int input_conections;
    public int input_socal;
    public int input_kitchen;
    public int input_software;
    public int input_special;

    public string ad_disadvantages;

    public string special;
    public Sprite image_special;
    public Sprite image_Avertar;
}
