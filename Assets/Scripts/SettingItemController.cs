using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingItemController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI Description;
    [SerializeField]
    private GameManagerInstance.UnlockStateID _UnlockItem;
    [SerializeField]
    private string _Description = "";
    [SerializeField]
    private bool IsEnabled = false;
    [SerializeField]
    private GameObject ContainerObject;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerInstance.Instance.UnlockStateChanged += Instance_UnlockStateChanged;
    }

    private void Instance_UnlockStateChanged(GameManagerInstance.UnlockStateID arg1, bool arg2, bool arg3)
    {
        if (arg1 == _UnlockItem)
        {
            IsEnabled = arg3;
            if (ContainerObject != null)
                ContainerObject.SetActive(IsEnabled);
        }
    }

    // Update is called once per frame
    void Update() { }

    public void ToggleEnabled()
    {
        IsEnabled = !IsEnabled;
        bool? value = SettingController.Instance.GetSettable(_UnlockItem);
        SettingController.Instance.SetSettable(_UnlockItem, value.HasValue ? !value.Value : false);
    }

    public void SetSettingItem(GameManagerInstance.UnlockStateID pId, string pDescription = "", ulong pValue = 0)
    {
        _UnlockItem = pId;
        _Description = pDescription;
        IsEnabled = GameManagerInstance.Instance.GetUnlockState(pId);

        if (ContainerObject != null)
            ContainerObject.SetActive(IsEnabled);

        if (Description != null)
            Description.text = pDescription;
    }
}
