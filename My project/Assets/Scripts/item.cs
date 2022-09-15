using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class item : MonoBehaviour
{
    [SerializeField] TMP_Text itemName, coin, description;
    
   
    public void Initialize(string itenName,string coin,string description)
    {
        this.itemName.text = itenName;
        this.coin.text = coin + "$";
        this.description.text = description;
    }
}
