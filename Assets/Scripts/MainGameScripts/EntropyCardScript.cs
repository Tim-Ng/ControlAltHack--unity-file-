using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "EntropyCard")]
public class EntropyCardScript : ScriptableObject
{
    public int EntropyCardID;
    public bool IsBagOfTricks;
    public bool IsLigthingStrikes;
    public string Title;
    public string discription;
    
    public Sprite artwork_info;
    public Sprite artwork_back;
    // Is bag of tricks
    public int Cost;
    public string usage;

    // Is Lighting strikes 
    public bool use_usage;
    public bool UseSucFailLighting;
    public string Success;
    public string Failure;
    public int minus_how_much_cred;
    public string RollVSWhich;
    
}
