using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class DrawCharacterCard : MonoBehaviour
{
    public GameObject PlayerArea;
    public GameObject OponentArea1;
    public GameObject OponentArea2;
    public GameObject OponentArea3;
    public GameObject OponentArea4;
    public GameObject OponentArea5;
    public GameObject CharCard1;
    public GameObject button;

    void Start()
    {
    }
    public void OnClick()
    {
        for (var i = 0; i < 3; i++)
        {
            GameObject characterPlayerCard1 = Instantiate(CharCard1, transform.position, Quaternion.identity);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
        }
        button.SetActive(false);

    }
}
