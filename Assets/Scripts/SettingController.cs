using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SettingController : MonoBehaviour
{
    public static SettingController Instance;

    private Canvas SettingCanvas;

    public void Awake()
    {
        SettingCanvas = GetComponent<Canvas>();
        SettingCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        SettingCanvas.worldCamera = Camera.main;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Settings"))
            ToggleSettings();
    }

    public void OpenSettings()
    {
        SettingCanvas.enabled = true;
    }
    public void CloseSettings()
    {
        SettingCanvas.enabled = true;
    }
    public void ToggleSettings()
    {
        SettingCanvas.enabled = !SettingCanvas.enabled;
    }
}
