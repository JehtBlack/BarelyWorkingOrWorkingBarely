using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SettingController : MonoBehaviour
{
    public static SettingController Instance;

    private Canvas SettingCanvas;

    [NonSerialized]
    public readonly List<GameManagerInstance.UnlockStateID> SettingsFilter = new List<GameManagerInstance.UnlockStateID> {
        GameManagerInstance.UnlockStateID.ColourVision
    };

    [NonSerialized] 
    public readonly Dictionary<GameManagerInstance.UnlockStateID, bool> Settables = new Dictionary<GameManagerInstance.UnlockStateID, bool>();

    public event Action<GameManagerInstance.UnlockStateID, bool /*oldState*/, bool /*newState*/> OnSettingChanged;

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

    void Start() {
        GameManagerInstance.Instance.UnlockStateChanged += OnUnlock;
    }

    public void SetSettable(GameManagerInstance.UnlockStateID id, bool state) {
        // if id must alter other settables check here and turn off other settings

        bool oldState = false;
        if (Settables.ContainsKey(id))
            oldState = Settables[id];

        Settables[id] = state;
        OnSettingChanged?.Invoke(id, oldState, Settables[id]);
    }

    public bool? GetSettable(GameManagerInstance.UnlockStateID id) {
        if (!Settables.ContainsKey(id))
            return null;
        return Settables[id];
    }

    void OnUnlock(GameManagerInstance.UnlockStateID id, bool oldState, bool newState) {
        if (SettingsFilter.Contains(id)) {
            SetSettable(id, newState);
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
