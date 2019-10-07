using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    float CurrentHealth();
    void Damage(float power);
    void HealPercent(float healFactor, bool overcharge);
    void HealAmount(float healAmount, bool overcharge);
}
