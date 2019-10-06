using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    private GameObject TreadObj;

    delegate float GetHorizontalMoventDelegate();

    [NonSerialized]
    private RefDependsOn<GetHorizontalMoventDelegate> HorizontalMovement = new RefDependsOn<GetHorizontalMoventDelegate>(GameManagerInstance.UnlockStateID.Possession);

    private CharacterController2D Controller2D;
    [Range(0, 100.0f)] public float MovementSpeed = 10.0f;


    private bool Jump = false;
    private float HorizontalMove = 0.0f;

    void Awake() {
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
    }

    private void FixedUpdate()
    {
        Controller2D.Move((HorizontalMove * Time.fixedDeltaTime), false, Jump);
        Jump = false;
    }
}
