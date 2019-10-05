using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
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
    public Value? WrappedValue {
        get { return IsAvailable() ? WrappedValue : null; }
        set { WrappedValue = value; }
    }

    public ValDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) {
        WrappedValue = null;
    }
}

public class RefDependsOn<Value> : DependsOn where Value : class {
    public Value WrappedValue {
        get { return IsAvailable() ? WrappedValue : null; }
        set { WrappedValue = value; }
    }

    public RefDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) {
        WrappedValue = null;
    }
}

public class Unlockable
{
    public readonly string Description;
    public readonly ulong Cost;

    private Unlockable() { }

    public Unlockable(string description, ulong cost)
    {
        Description = description;
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

    // data
    public static GameManagerInstance Instance;

    [NonSerialized]
    private Dictionary<UnlockStateID, Unlockable> UnlockCosts = new Dictionary<UnlockStateID, Unlockable> {
        { UnlockStateID.Possession, new Unlockable("Possess the little guy", 0) },
        { UnlockStateID.ColourVision, new Unlockable("You like colour right?", 100) },
        { UnlockStateID.FloorPlane, new Unlockable("Floors are important aren't they?", 100) },
        { UnlockStateID.TestItem1, new Unlockable("Tests item 1", 100) },
        { UnlockStateID.TestItem2, new Unlockable("Tests item 2", 200) },
        { UnlockStateID.TestItem3, new Unlockable("Tests item 3", 300) },
        { UnlockStateID.TestItem4, new Unlockable("Tests item 4", 400) },
    };

    [SerializeField] 
    private ulong Currency = 0;

    [SerializeField]
    private List<bool> UnlockStates = new List<bool>();

    [SerializeField]
    private bool IgnoreUnlockCosts = false;

    public event Action<ulong> CurrencyChanged;
    public event Action<UnlockStateID, bool /*oldState*/, bool /*newState*/> UnlockStateChanged;

    // methods
    public bool GetUnlockState(UnlockStateID id) {
        ushort idx = (ushort)id;
        if (idx > UnlockStates.Count) {
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

    public Unlockable GetUnlockable(UnlockStateID id) {
        if (!UnlockCosts.ContainsKey(id)) {
            Debug.LogError(string.Format("Unlock State ID {0} cost not defined!", id.ToString("g")));
            return new Unlockable("We forgot to add this one", 0);
        }

        if (IgnoreUnlockCosts) {
            return new Unlockable(UnlockCosts[id].Description, 0);
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

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
