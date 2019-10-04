using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInstance : MonoBehaviour {

    // helper defines
    public enum UnlockStateID : UInt16 {

    }

    // data
    public static GameManagerInstance Instance;

    [NonSerialized]
    private Dictionary<UnlockStateID, UInt64> UnlockCosts = new Dictionary<UnlockStateID, UInt64> {

    };

    private UInt64 Currency = 0;
    private List<bool> UnlockStates = new List<bool>();

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
