using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CurrencyAcquisition), typeof(Collider2D))]
public class ColliderCollectible : MonoBehaviour {
    private CurrencyAcquisition acquisitionTrigger;
    void Awake() {
        acquisitionTrigger = gameObject.GetComponent<CurrencyAcquisition>();
    }
    void OnCollisionEnter2D(Collision2D col) {
        acquisitionTrigger.TriggerCollection();
    }
}
