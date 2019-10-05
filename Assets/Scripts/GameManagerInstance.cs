﻿using System;
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

    [SerializeField]
    private UInt64 Currency = 0;
    [SerializeField]
    private List<bool> UnlockStates = new List<bool>();

    [SerializeField]
    private bool IgnoreUnlockCosts = false;

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

        UnlockStates[idx] = state;
    }

    public UInt64 GetUnlockCost(UnlockStateID id) {
        if (!UnlockCosts.ContainsKey(id)) {
            Debug.LogError(string.Format("Unlock State ID {0} cost not defined!", id.ToString("g")));
            return 0;
        }

        if (IgnoreUnlockCosts)
            return 0;

        return UnlockCosts[id];
    }

    public void AddCurrency(UInt64 amount) {
        if (UInt64.MaxValue - amount <= Currency)
            amount = UInt64.MaxValue - Currency;
        Currency += amount;
    }

    public void RemoveCurrency(UInt64 amount) {
        if (amount > Currency)
            amount = Currency;
        Currency -= amount;
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