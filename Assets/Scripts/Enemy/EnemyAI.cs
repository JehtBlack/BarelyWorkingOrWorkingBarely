using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class EnemyAI : MonoBehaviour, IDamageable {
    private IEnemyBehaviour Behaviour;

    private Vector2 ActiveDestination;
    private GameObject Player;

    [SerializeField]
    private const float MaxHealth = 10.0f;
    [SerializeField]
    private float Health = MaxHealth;

    private CharacterController2D Controller2D;
    private bool Jump = false;
    private float HorizontalMove = 0.0f;

    bool BehaviourAttached() {
        return Behaviour != null;
    }

    void GetBehaviour() {
        Behaviour = GetComponent<IEnemyBehaviour>();

        if (BehaviourAttached()) {
            Behaviour.SetRootPosition(transform.position);
        }
    }

    void Awake() {
        GetBehaviour();
    }

    // Start is called before the first frame update
    void Start() {
        ActiveDestination = transform.position;
        Player = GameObject.FindWithTag("Player");

        GetBehaviour();
        Controller2D = GetComponent<CharacterController2D>();
    }

    Vector2 ToPlayer() {
        return Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (!BehaviourAttached())
            return;

        bool aggroed = false;

        Vector2 rayDir = ToPlayer();
        int checkLayers = LayerMask.GetMask("Player", "Obstructions");
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, rayDir, Mathf.Infinity, checkLayers);
        aggroed = hit.transform == Player.transform && hit.distance <= Behaviour.AggroRadius();

        Vector2 moveTo = Behaviour.NextDestination(transform.position, aggroed, Player.transform.position);
        moveTo -= (Vector2)transform.position;
        moveTo.Normalize();
        HorizontalMove = moveTo.x * Behaviour.MovementSpeed();

        if (aggroed) {
            hit = Physics2D.Raycast(transform.position, rayDir, Mathf.Infinity, checkLayers);
            if (hit.transform == Player.transform && hit.distance <= Behaviour.AttackRadius()) {
                IDamageable playerDamageable = Player.GetComponent<IDamageable>();
                if (playerDamageable == null) {
                    Debug.LogError("Player is not damageable.");
                    return;
                }

                Behaviour.Attack(playerDamageable, hit.distance);
            }
        }
    }

    void FixedUpdate() {
        Controller2D.Move((HorizontalMove * Time.fixedDeltaTime), false, Jump);
    }
    public float CurrentHealth() {
        return Health;
    }

    public void Damage(float power) {
        Health -= power;
        if (Health <= 0.0f)
            Kill();
    }

    public void HealPercent(float healFactor, bool overcharge)
    {
        Health += healFactor * MaxHealth;
        if (!overcharge)
            Health = Mathf.Max(Health, MaxHealth);
    }

    public void HealAmount(float healAmount, bool overcharge)
    {
        Health += healAmount;
        if (!overcharge)
            Health = Mathf.Max(Health, MaxHealth);
    }

    void Kill() {

    }
}
