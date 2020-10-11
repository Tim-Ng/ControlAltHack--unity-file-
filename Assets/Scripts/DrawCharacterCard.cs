using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;

public class DrawCharacterCard : MonoBehaviour
{
    public GameObject PlayerArea1;
    public GameObject OponentArea1;
    public GameObject OponentArea2;
    public GameObject OponentArea3;
    public GameObject OponentArea4;
    public GameObject OponentArea5;
    public GameObject CharCard1;

    void Start()
    {

    }
    public void OnClick()
    {
        for (var i = 0; i < 3; i++)
        {
            GameObject charactedPlayerCard1 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject charactedPlayerCard2 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject charactedPlayerCard3 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject charactedPlayerCard4 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject charactedPlayerCard5 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject charactedPlayerCard6 = Instantiate(CharCard1, new Vector3(0, 0, 0), Quaternion.identity);
            charactedPlayerCard1.transform.SetParent(PlayerArea1.transform, false);
            charactedPlayerCard2.transform.SetParent(OponentArea1.transform, false);
            charactedPlayerCard3.transform.SetParent(OponentArea2.transform, false);
            charactedPlayerCard4.transform.SetParent(OponentArea3.transform, false);
            charactedPlayerCard5.transform.SetParent(OponentArea4.transform, false);
            charactedPlayerCard6.transform.SetParent(OponentArea5.transform, false);
        }
    }
}
