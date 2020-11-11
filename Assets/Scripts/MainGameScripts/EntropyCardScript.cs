using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "EntropyCard")]
public class EntropyCardScript : ScriptableObject
{
    public int EntropyCardID;
    public bool IsBagOfTricks;
    public bool IsLigthingStrikes;
    public bool IsSharedFate;//added by goh
    public bool IsExtensiveExperience;//added by goh
    public string Title;
    public string discription;
    
    public Sprite artwork_info;
    public Sprite artwork_back;
    // Is bag of tricks
    public int Cost;
    public bool SkillEffecter;
    public string whichSkillIncrease1;
    public int byHowMuchSkillIncrease1;
    public bool increaseSecondSkill;
    public string whichSkillIncrease2;
    public int byHowMuchSkillIncrease2;
    public string usage;

    // Is Lighting strikes 
    public bool use_usage;
    public bool UseSucFailLighting;
    public string Success;
    public int add_how_much_cred;//added by goh
    public string Failure;
    public int minus_how_much_cred;
    public string RollVSWhich;
    
}
