using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPopulator : MonoBehaviour
{
    public GameObject prefab; // This is our prefab object that will be exposed in the inspector

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        GameObject newObj; // Create GameObject instance

        Dictionary<GameManagerInstance.UnlockStateID, Unlockable> UnlockCosts = GameManagerInstance.Instance.GetUnlockCosts();

        foreach (var item in UnlockCosts)
        {
            Unlockable unlockable = item.Value;

            // Create new instances of our prefab until we've created as many as we specified
            newObj = (GameObject)Instantiate(prefab, transform);
            ShopItemController SIC = newObj.GetComponent<ShopItemController>();

            SIC.SetShopItem(item.Key, unlockable.Description, unlockable.Cost);
        }

            // Randomize the color of our image
            //newObj.GetComponent().color = Random.ColorHSV();
    }
}
