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
    public readonly UInt64 Cost;

    private Unlockable() { }

    public Unlockable(string description, UInt64 cost)
    {
        Description = description;
        Cost = cost;
    }
}

public class GameManagerInstance : MonoBehaviour {

    // helper defines
    public enum UnlockStateID : UInt16 {
        Possession = 0,
        FloorPlane,
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
        { UnlockStateID.TestItem1, new Unlockable("Tests item 1", 100) },
        { UnlockStateID.TestItem2, new Unlockable("Tests item 2", 200) },
        { UnlockStateID.TestItem3, new Unlockable("Tests item 3", 300) },
        { UnlockStateID.TestItem4, new Unlockable("Tests item 4", 400) },
    };

    [SerializeField]
    private ulong Currency = 0;
    private ulong CurrencyProp
    {
        get => Currency;
        set
        {
            Currency = value;
            CurrencyChanged?.Invoke(Currency);
        }
    }

    [SerializeField]
    private List<bool> UnlockStates = new List<bool>();

    [SerializeField]
    private bool IgnoreUnlockCosts = false;

    public event Action<ulong> CurrencyChanged;
    public event Action<UnlockStateID, bool /*oldState*/, bool /*newState*/> UnlockStateChanged;

    // methods
    public bool GetUnlockState(UnlockStateID id) {
        UInt16 idx = (UInt16)id;
        if (idx > UnlockStates.Count) {
            while (UnlockStates.Count <= idx)
                UnlockStates.Add(false);
        }
        return UnlockStates[(int)idx];
    }

    public void SetUnlockState(UnlockStateID id, bool state) {
        UInt16 idx = (UInt16)id;
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

    public void AddCurrency(UInt64 amount) {
        if (UInt64.MaxValue - amount <= Currency)
            amount = UInt64.MaxValue - Currency;
        CurrencyProp += amount;
    }

    public void RemoveCurrency(UInt64 amount) {
        if (amount > Currency)
            amount = Currency;
        CurrencyProp -= amount;
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
