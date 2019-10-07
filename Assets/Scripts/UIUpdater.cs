using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdater: MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI CurrencyUI;

    private bool subscribed = false;

    private void Awake() {
        subscribed = false;
    }

    private void Instance_CurrencyChanged(ulong obj)
    {
        if(CurrencyUI != null)
             CurrencyUI.text = obj.ToString();
    }

    void Update() {
        if (!subscribed) {
            GameManagerInstance.Instance.CurrencyChanged += Instance_CurrencyChanged;
            subscribed = true;
        }
    }
}
