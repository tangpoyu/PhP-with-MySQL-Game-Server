using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text username, level, coin;

    public void Initialize(string username, string level, string coin)
    {
        this.username.text = username;
        this.level.text = "LV." + level;
        this.coin.text = "$" + coin;
    }
}
