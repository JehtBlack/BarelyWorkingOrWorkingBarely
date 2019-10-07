using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehaviour {

    void SetRootPosition(Vector2 position);
    Vector2 NextDestination(Vector2 currentPosition, bool isAggroed, Vector2 currentPlayerPosition);
    float AggroRadius();
    float AttackRadius();
    float MovementSpeed();
    void Attack(IDamageable player, float distanceToPlayer);
}
