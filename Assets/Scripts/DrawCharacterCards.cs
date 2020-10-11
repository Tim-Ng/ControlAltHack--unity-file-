using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;

public class DrawCharacterCards : MonoBehaviour
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
        GameObject characterCard = Instantiate(CharCard1, new Vector3(0,0,0),Quaternion.identity);
    }
}
