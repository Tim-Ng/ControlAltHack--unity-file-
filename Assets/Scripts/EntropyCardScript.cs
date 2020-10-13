using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "EntropyCard")]
public class EntropyCardScript : ScriptableObject
{
    public string Title;
    public string discription;
    public int Cost;
    public string usage;

    public Sprite artwork_info;

    
}
