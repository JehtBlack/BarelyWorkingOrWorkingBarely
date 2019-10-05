using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyAcquisition : MonoBehaviour {
    [SerializeField] 
    private UInt64 Value = 0;
    
    [SerializeField] 
    private AnimationCurve ValueFalloff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    
    [SerializeField] 
    [Range(1, 100)]
    private int MaxCollectionCount = 1;

    private int CollectionCount = 0;
    
    public void TriggerCollection() {

        float t = CollectionCount / (float) MaxCollectionCount;
        float evaluation = Mathf.Lerp(0, MaxCollectionCount, t);

        float valuation = ValueFalloff.Evaluate(evaluation);
        UInt64 currencyValue = (UInt64)Mathf.Abs(Mathf.FloorToInt(Value * valuation));
        GameManagerInstance.Instance.AddCurrency(currencyValue);
        CollectionCount++;
    }
}
