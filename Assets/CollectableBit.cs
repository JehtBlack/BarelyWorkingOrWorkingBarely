using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBit : MonoBehaviour, ICollectible
{
    [SerializeField]
    private ulong CurrencyValue = 0;

    public void Collected()
    {
        GameManagerInstance.Instance.AddCurrency(CurrencyValue);

        Destroy(gameObject);
    }
}
