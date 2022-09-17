using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    private string itemId;
    private Web web;
    [SerializeField] TMP_Text itemName, coin, description;
    [SerializeField] Image image;
    

    public string ItemId { get => itemId; set => itemId = value; }
    public TMP_Text ItemName { get => itemName; set => itemName = value; }
    public TMP_Text Coin { get => coin; set => coin = value; }
    public TMP_Text Description { get => description; set => description = value; }

    private void Awake()
    {
        web = GetComponentInParent<Web>();
    }

    public void Initialize(string itemId,string itenName,string coin,string description)
    {
        this.ItemId = itemId;
        this.ItemName.text = itenName;
        this.Coin.text = "$." + coin;
        this.Description.text = description;
    }

    public void Sell()
    {
        web.SellItem(itemId);
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
