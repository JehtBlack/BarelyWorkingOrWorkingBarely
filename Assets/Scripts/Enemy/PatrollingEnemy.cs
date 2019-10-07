using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour, IEnemyBehaviour {
    private Vector2 Root;

    [SerializeField]
    private bool Ranged;
    [SerializeField]
    private float _AggroRadius;
    [SerializeField]
    private float DeaggroRange;
    [SerializeField]
    private float Power;
    [SerializeField]
    private float _AttackRadius;
    [SerializeField]
    private float _OptimalRange;
    [SerializeField]
    private float _MovementSpeed;

    [SerializeField]
    private List<Vector2> PatrolPoints = new List<Vector2>();

    private int TargetPosition;
    private const float TargetReachedTolerance = 0.1f;

    [SerializeField]
    private GameObject BulletObj;

    Vector2 GetTargetPosition() {

        int GetIdx() {
            bool forwards = (TargetPosition / PatrolPoints.Count) % 2 == 0;

            int ret = TargetPosition % PatrolPoints.Count;
            if (!forwards) {
                ret = (PatrolPoints.Count - 1) - ret;
            }

            return ret;
        }

        int idx = -1;
        if (PatrolPoints.Count > 0) {
            idx = GetIdx();
            float sqTargetTolerance = TargetReachedTolerance * TargetReachedTolerance;
            if ((PatrolPoints[idx] - (Vector2) transform.position).sqrMagnitude <= sqTargetTolerance) {
                TargetPosition++;
                idx = GetIdx();
            }
        }
        return idx == -1 ? Root : PatrolPoints[idx];
    }

    public void SetRootPosition(Vector2 position) {
        Root = position;
        TargetPosition = 0;
    }

    public Vector2 NextDestination(Vector2 currentPosition, bool isAggroed, Vector2 currentPlayerPosition) {
        float sqTargetTolerance = TargetReachedTolerance * TargetReachedTolerance;

        if (!isAggroed) {
            return GetTargetPosition();
        }

        if ((Root - currentPosition).sqrMagnitude > DeaggroRange * DeaggroRange) {
            return GetTargetPosition();
        }

        bool shouldConsiderReposition = Random.value < 0.25f;

        if (shouldConsiderReposition) {
            float repositionThreshold = _OptimalRange * 0.85f;
            repositionThreshold *= repositionThreshold;
            if (Ranged && ((currentPlayerPosition - currentPosition).sqrMagnitude < repositionThreshold)) {
                float xPosTarget = currentPlayerPosition.x - currentPosition.x;
                xPosTarget *= -1; // reverse direction
                return new Vector2(xPosTarget, 0); // move horizontally away from the player to return to optimal range
            }
        }

        return currentPlayerPosition;
    }

    public float AggroRadius() {
        return _AggroRadius;
    }

    public float AttackRadius() {
        return _AttackRadius;
    }

    public float MovementSpeed() {
        return _MovementSpeed;
    }

    public void Attack(IDamageable player, float distanceToPlayer) {

        if (Ranged) {
            GameObject bullet = GameObject.Instantiate(BulletObj, transform.position, Quaternion.identity);
        }
        else {

        }
    }

    void OnDrawGizmosSelected() {

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AttackRadius());

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AggroRadius());

        Gizmos.color = Color.black;
        float size = 0.1f;
        foreach (Vector2 v in PatrolPoints) {
            Gizmos.DrawLine(new Vector3(v.x - size, v.y - size), new Vector3(v.x + size, v.y + size));
            Gizmos.DrawLine(new Vector3(v.x - size, v.y + size), new Vector3(v.x + size, v.y - size));
        }
    }
}
