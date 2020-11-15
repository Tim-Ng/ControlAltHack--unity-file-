using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEntropyCard : MonoBehaviour
{
    [SerializeField] private rollTime RollTime;
    [SerializeField] private MoneyAndPoints moneyAndPoints;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    [SerializeField] private popupcardwindowEntropy popUpEntropy;
    private EntropyCardScript entropyCardScript;
    private List<int> BagofTricksWithSkillChanger =new List<int>{20,23,25,3,21,22,18,5,33,6,9,19,10};
    public void clickOnPlayEntropyCard()
    {
        entropyCardScript = popUpEntropy.GetEntropyCardScript();
        if (entropyCardScript.Cost <= moneyAndPoints.getMyMoneyAmount())
        {
            moneyAndPoints.subMyMoney(entropyCardScript.Cost);
            enoughMoney();
        }
        else
        {
            //cant play card 
        }
    }
    public void enoughMoney()
    {
        if (entropyCardScript.IsBagOfTricks)
        {
            if (entropyCardScript.SkillEffecter)
            {
                if (inSkillChangerList(entropyCardScript.EntropyCardID) )
                {
                    RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease1, entropyCardScript.byHowMuchSkillIncrease1, Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1));
                    if (entropyCardScript.increaseSecondSkill)
                    {
                        RollTime.addSkillChanger(entropyCardScript.whichSkillIncrease2, entropyCardScript.byHowMuchSkillIncrease2, Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1));
                    }
                }
                else
                {
                    Debug.LogError("Skill changer not in list !!!!");
                }
            }
        }
    }
    private bool inSkillChangerList(int whichID)
    {
        foreach (int inList in BagofTricksWithSkillChanger)
        {
            if (inList == whichID)
            {
                return true;
            }
        }
        return false;
    }
}
