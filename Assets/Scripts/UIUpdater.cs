using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdater: MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI CurrencyUI;

    private void Start()
    {
        GameManagerInstance.Instance.CurrencyChanged += Instance_CurrencyChanged; 
    }

    private void Instance_CurrencyChanged(ulong obj)
    {
        if(CurrencyUI != null)
             CurrencyUI.text = obj.ToString();
    }
}
