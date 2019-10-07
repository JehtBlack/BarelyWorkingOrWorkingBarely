using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// Wrapper for locked data/delegates, data that can only be accessed if the dependency is unlocked in the GameManagerInstance
public abstract class DependsOn {

    private GameManagerInstance.UnlockStateID Dependency;
    
    private DependsOn() { }

    public DependsOn(GameManagerInstance.UnlockStateID dependency) {
        Dependency = dependency;
    }
    public bool IsAvailable() {
        return GameManagerInstance.Instance.GetUnlockState(Dependency);
    }
}

public class ValDependsOn<Value> : DependsOn where Value : struct {
    private Value? _WrappedValue;
    public Value? WrappedValue {
        get { return IsAvailable() ? _WrappedValue : null; }
        set { _WrappedValue = value; }
    }

    public ValDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) {
        WrappedValue = null;
    }

    public ValDependsOn(GameManagerInstance.UnlockStateID dependency, Value value) : base(dependency) {
        WrappedValue = value;
    }
}

public class RefDependsOn<Value> : DependsOn where Value : class {
    private Value _WrappedValue;

    public Value WrappedValue {
        get { return IsAvailable() ? _WrappedValue : null; }
        set { _WrappedValue = value; }
    }

    public RefDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) {
        WrappedValue = null;
    }

    public RefDependsOn(GameManagerInstance.UnlockStateID dependency, Value value) : base(dependency) {
        WrappedValue = value;
    }
}


class WorldState {
    public float PlayerHealth;
    public List<bool> UnlockStates;
    public ulong PlayerCurrency;
    public Dictionary<GameManagerInstance.UnlockStateID, bool> Settings;
    public Vector2 CheckPointPosition;
}

public class Unlockable
{
    public readonly string[] Descriptions;
    public readonly ulong Cost;

    private Unlockable() { }

    public Unlockable(string[] descriptions, ulong cost)
    {
        Descriptions = descriptions;
        Cost = cost;
    }
}

public class GameManagerInstance : MonoBehaviour {

    // helper defines
    public enum UnlockStateID : ushort {
        Possession = 0,
        FloorPlane,
        ColourVision,
        TestItem1,
        TestItem2,
        TestItem3,
        TestItem4
    }

    public enum SoundEffect {
        Test,
    }

    // data
    public static GameManagerInstance Instance;

    [NonSerialized] 
    public readonly Dictionary<SoundEffect, string> SoundEffectSettings = new Dictionary<SoundEffect, string> {
        { SoundEffect.Test, "1,.5,,.1711,,.0495,.3,.7466,.3297,-.1746,,,,,,,,,,,.2402,.1022,,,,1,,,.1853,,," }
    };

    [NonSerialized]
    private Dictionary<UnlockStateID, Unlockable> UnlockCosts = new Dictionary<UnlockStateID, Unlockable> {
        { UnlockStateID.Possession, new Unlockable(new [] { "Possess the little guy" }, 0) },
        { UnlockStateID.ColourVision, new Unlockable(new [] { "You like colour right?" }, 100) },
        { UnlockStateID.FloorPlane, new Unlockable(new [] { "Floors are important aren't they?" }, 100) },
        { UnlockStateID.TestItem1, new Unlockable(new [] { "Tests item 1" }, 100) },
        { UnlockStateID.TestItem2, new Unlockable(new [] { "Tests item 2" }, 200) },
        { UnlockStateID.TestItem3, new Unlockable(new [] { "Tests item 3" }, 300) },
        { UnlockStateID.TestItem4, new Unlockable(new [] { "Tests item 4" }, 400) },
    };

    [SerializeField] 
    private ulong Currency = 0;

    [SerializeField]
    private List<bool> UnlockStates = new List<bool>();

    public event Action<ulong> CurrencyChanged;
    public event Action<UnlockStateID, bool /*oldState*/, bool /*newState*/> UnlockStateChanged;

    private WorldState LastWorldState;

    // Editor stuff
    [Header("Debug Settings")]
    public bool IgnoreUnlockCosts = false;
    public UnlockStateID UnlockID;

    // methods
    public bool GetUnlockState(UnlockStateID id) {
        ushort idx = (ushort)id;
        if (idx >= UnlockStates.Count) {
            while (UnlockStates.Count <= idx)
                UnlockStates.Add(false);
        }

        return UnlockStates[(int)idx];
    }

    public void SetUnlockState(UnlockStateID id, bool state) {
        ushort idx = (ushort)id;
        if (idx > UnlockStates.Count)
        {
            while (UnlockStates.Count <= idx)
                UnlockStates.Add(false);
        }

        bool oldState = UnlockStates[idx];
        UnlockStates[idx] = state;

        UnlockStateChanged?.Invoke(id, oldState, state);
    }

    public Dictionary<UnlockStateID, Unlockable> GetUnlockCosts()
    {
        return UnlockCosts;
    }

    public Unlockable GetUnlockable(UnlockStateID id) {
        if (!UnlockCosts.ContainsKey(id)) {
            Debug.LogError(string.Format("Unlock State ID {0} cost not defined!", id.ToString("g")));
            return new Unlockable(new[] { "We forgot to add this one" }, 0);
        }

        if (IgnoreUnlockCosts) {
            return new Unlockable(UnlockCosts[id].Descriptions, 0);
        }

        return UnlockCosts[id];
    }

    public UInt64 GetCurrency() {
        return Currency;
    }

    public void AddCurrency(ulong amount) {
        if (ulong.MaxValue - amount <= Currency)
            amount = ulong.MaxValue - Currency;
        Currency += amount;
        CurrencyChanged?.Invoke(Currency);
    }

    public void RemoveCurrency(ulong amount) {
        if (amount > Currency)
            amount = Currency;
        Currency -= amount;
        CurrencyChanged?.Invoke(Currency);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public SfxrSynth GetSoundEffect(SoundEffect sfx) {
        if (!SoundEffectSettings.ContainsKey(sfx)) {
            Debug.LogError("Requested an unimplemented sound effect!");
            return new SfxrSynth();
        }

        SfxrSynth synth = new SfxrSynth();
        synth.parameters.SetSettingsString(SoundEffectSettings[sfx]);
        return synth;
    }

    public void CacheWorldState(Vector2 checkpointPosition) {
        WorldState caching = new WorldState();
        float? hp = GameObject.FindWithTag("Player").GetComponent<IDamageable>()?.CurrentHealth();
        caching.PlayerHealth = hp.HasValue ? hp.Value : 100.0f;
        caching.PlayerCurrency = Currency;
        caching.UnlockStates = UnlockStates;
        caching.Settings = SettingController.Instance.Settables;
        caching.CheckPointPosition = checkpointPosition;

        LastWorldState = caching;
    }

    public void ResetToLastCheckpoint() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        Currency = LastWorldState.PlayerCurrency;

        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<IDamageable>()?.HealAmount(LastWorldState.PlayerHealth, true);
        player.transform.position = LastWorldState.CheckPointPosition;

        for (int i = 0; i < LastWorldState.UnlockStates.Count; ++i) {
            UnlockStateID id = (UnlockStateID)i;
            SetUnlockState(id, LastWorldState.UnlockStates[i]);
        }

        foreach (var setting in LastWorldState.Settings) {
            SettingController.Instance.SetSettable(setting.Key, setting.Value);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
