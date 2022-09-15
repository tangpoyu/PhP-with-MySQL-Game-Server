using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private GameObject itemUI;

    public void OpenItemUI()
    {
        itemUI.SetActive(true);
    }
}