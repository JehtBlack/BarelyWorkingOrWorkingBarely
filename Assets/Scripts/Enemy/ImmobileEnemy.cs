using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ImmobileEnemy : MonoBehaviour, IEnemyBehaviour {
    private Vector2 Root;

    [SerializeField]
    private bool Ranged;
    [SerializeField] 
    private float _AggroRadius;
    [SerializeField] 
    private float Power;
    [SerializeField]
    private float _AttackRadius;
    
    public void SetRootPosition(Vector2 position) {
        Root = position;
    }

    public Vector2 NextDestination(Vector2 currentPosition, bool isAggroed, Vector2 currentPlayerPosition) {
        return Root;
    }

    public float AggroRadius() {
        return _AggroRadius;
    }

    public float AttackRadius() {
        return _AttackRadius;
    }

    public float MovementSpeed() {
        return 0;
    }

    public void Attack(IDamageable player, float distanceToPlayer) {

        if (Ranged) {

        }
        else {
            
        }
    }

    void OnDrawGizmosSelected() {

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AttackRadius());

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AggroRadius());
    }
}
