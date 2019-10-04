using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wrapper for locked data/delegates, data that can only be accessed if the dependency is unlocked in the GameManagerInstance
public abstract class DependsOn {

    private GameManagerInstance.UnlockStateID Dependency;
    
    private DependsOn() { }

    public DependsOn(GameManagerInstance.UnlockStateID dependency) {
        Dependency = dependency;
    }
    public bool IsAvailable() {
        return GameManagerInstance.Instance.GetState(Dependency);
    }
}

public class ValDependsOn<Value> : DependsOn where Value : struct {
    private Nullable<Value> WrappedValue;

    public ValDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) { }
    public Nullable<Value> GetValue() {
        return IsAvailable() ? WrappedValue : null;
    }
}

public class RefDependsOn<Value> : DependsOn where Value : class {
    private Value WrappedValue;

    public RefDependsOn(GameManagerInstance.UnlockStateID dependency) : base(dependency) { }

    public Value GeValue() {
        return IsAvailable() ? WrappedValue : null;
    }
}

public class GameManagerInstance : MonoBehaviour {

    // helper defines
    public enum UnlockStateID : UInt16 {
        Possession = 0,
        FloorPlane,

    }

    // data
    public static GameManagerInstance Instance;

    [NonSerialized]
    private Dictionary<UnlockStateID, UInt64> UnlockCosts = new Dictionary<UnlockStateID, UInt64> {

    };

    private UInt64 Currency = 0;
    private List<bool> UnlockStates = new List<bool>();

    private bool IgnoreUnlockCosts = false;

    // methods
    public bool GetState(UnlockStateID id) {
        UInt16 idx = (UInt16)id;
        if (idx > UnlockStates.Count) {
            while (UnlockStates.Count <= idx)
                UnlockStates.Add(false);
        }
        return UnlockStates[(int)idx];
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
