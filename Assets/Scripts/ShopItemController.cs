using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemController : MonoBehaviour
{

    [SerializeField]
    private TMPro.TextMeshProUGUI Description;
    [SerializeField]
    private TMPro.TextMeshProUGUI Cost;

    private GameManagerInstance.UnlockStateID _UnlockItem;
    private string _Description = "";
    private ulong _Cost = 0;


    // Start is called before the first frame update
    void Start(){}
    // Update is called once per frame
    void Update(){}

    public void BuyRequest()
    {
        if (GameManagerInstance.Instance.GetCurrency() >= _Cost)
        {
            GameManagerInstance.Instance.RemoveCurrency(_Cost);
            GameManagerInstance.Instance.SetUnlockState(_UnlockItem, true);
            Destroy(gameObject);
        }
    }

    public void SetShopItem(GameManagerInstance.UnlockStateID pId, string pDescription = "", ulong pValue = 0)
    {
        _UnlockItem = pId;
        _Description = pDescription;
        _Cost = pValue;

        if (Description != null)
            Description.text = pDescription;
        if (Cost != null)
            Cost.text = pValue.ToString();
    }
}
