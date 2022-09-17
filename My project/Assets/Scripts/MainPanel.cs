using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private GameObject itemPanel;

    public void OpenItemPanel()
    {
        itemPanel.SetActive(true);
    }

    public void ExitItemPanel()
    {
        itemPanel.SetActive(false);
    }
}