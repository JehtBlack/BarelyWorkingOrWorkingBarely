using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopulator : MonoBehaviour
{
    public GameObject prefab; // This is our prefab object that will be exposed in the inspector

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        GameObject newObj; // Create GameObject instance

        foreach (var item in SettingController.Instance.SettingsFilter)
        {
            Unlockable unlockable = GameManagerInstance.Instance.GetUnlockable(item);

            // Create new instances of our prefab until we've created as many as we specified
            newObj = (GameObject)Instantiate(prefab, transform);
            SettingItemController SIC = newObj.GetComponent<SettingItemController>();

            SIC.SetSettingItem(item, unlockable.Descriptions[0], unlockable.Cost);
        }
    }
}
