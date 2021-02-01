using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace DrawCards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "EntropyCard")]
    public class EntropyCardScript : ScriptableObject
    {
        public int EntropyCardID;
        public string Title;

        public Sprite artwork_info;
        public Sprite artwork_back;

        public int Cost;

        public bool SkillEffecter;
        public AllJobs whichSkillIncrease1;
        public int byHowMuchSkillIncrease1;

        public bool AnotherSecondSkill;
        public AllJobs whichSkillIncrease2;
        public int byHowMuchSkillIncrease2;

        public bool UseSucFailLighting;
        public AllJobs RollVSWhich;
        public string Success;
        public int add_how_much_cred;

        public string Failure;
        public int minus_how_much_cred;

        public bool use_usage;
        public string usageDiscription;

        public bool removeAfterPlay;
    }
}
