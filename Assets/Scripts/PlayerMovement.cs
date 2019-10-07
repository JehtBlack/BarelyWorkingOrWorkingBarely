using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour, IDamageable {
    [SerializeField]
    private GameObject TreadObj;
    [SerializeField]
    private GameObject CannonObj;

    delegate float GetHorizontalMoventDelegate();
    delegate void FireWeaponDelegate();

    [NonSerialized]
    private RefDependsOn<GetHorizontalMoventDelegate> HorizontalMovement = new RefDependsOn<GetHorizontalMoventDelegate>(GameManagerInstance.UnlockStateID.Possession);
    private RefDependsOn<FireWeaponDelegate> FireWeapon = new RefDependsOn<FireWeaponDelegate>(GameManagerInstance.UnlockStateID.Cannon);

    private CharacterController2D Controller2D;
    [Range(0, 100.0f)] public float MovementSpeed = 10.0f;


    private bool Jump = false;
    private float HorizontalMove = 0.0f;

    [SerializeField]
    private const float MaxHealth = 100.0f;
    [SerializeField]
    private float Health = MaxHealth;

    void Awake() {
        if (GameManagerInstance.Instance != null)
            GameManagerInstance.Instance.UnlockStateChanged += OnUnlock;
        Controller2D = GetComponent<CharacterController2D>();
        HorizontalMovement.WrappedValue = GetHorizontalMovement;
    }

    // Start is called before the first frame update
    void Start() {
        GameManagerInstance.Instance.UnlockStateChanged += OnUnlock;
    }

    void OnUnlock(GameManagerInstance.UnlockStateID id, bool oldState, bool newState) {
        if (id == GameManagerInstance.UnlockStateID.Possession && !oldState && newState) {
            GameObject child = Instantiate(TreadObj) as GameObject;
            child.transform.parent = transform;
            child.transform.localPosition = TreadObj.transform.localPosition;
        }

        if (id == GameManagerInstance.UnlockStateID.Cannon && !oldState && newState) {
            GameObject child = Instantiate(CannonObj) as GameObject;
            child.transform.parent = transform;
            child.transform.localPosition = CannonObj.transform.localPosition;
            CannonFire fireable = child.GetComponentInChildren<CannonFire>();
            fireable.FacingRightCallback = GetComponent<CharacterController2D>().FacingRight;
            FireWeapon.WrappedValue = fireable.UseWeapon;
        }
    }

    float GetHorizontalMovement() {
        return Input.GetAxisRaw("Horizontal") * MovementSpeed;
    }


    // Update is called once per frame
    void Update() {
        float? moveValue = HorizontalMovement.WrappedValue?.Invoke();
        HorizontalMove = moveValue.HasValue ? moveValue.Value : 0;
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
            FireWeapon.WrappedValue?.Invoke();
    }

    private void FixedUpdate()
    {
        Controller2D.Move((HorizontalMove * Time.fixedDeltaTime), false, Jump);
        Jump = false;
    }

    public float CurrentHealth() {
        return Health;
    }

    public void Damage(float power) {
        Health -= power;
        if (Health <= 0.0f)
            Kill();
    }

    public void HealPercent(float healFactor, bool overcharge) {
        Health += healFactor * MaxHealth;
        if (!overcharge)
            Health = Mathf.Max(Health, MaxHealth);
    }

    public void HealAmount(float healAmount, bool overcharge) {
        Health += healAmount;
        if (!overcharge)
            Health = Mathf.Max(Health, MaxHealth);
    }

    void Kill() {
        GameManagerInstance.Instance.ResetToLastCheckpoint();
    }
}
