using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntropyCardDisplay : MonoBehaviour
{
    public EntropyCardScript entropyData;
    public Text Title;
    public Text discription;
    public Text Cost;
    public Text usage;

    public Image artwork_info;

    void Start()
    {
        Title.text = entropyData.Title;
        discription.text = entropyData.discription;
        usage.text = entropyData.usage;
        Cost.text = entropyData.Cost.ToString();

        artwork_info.sprite = entropyData.artwork_info;
    }
}
