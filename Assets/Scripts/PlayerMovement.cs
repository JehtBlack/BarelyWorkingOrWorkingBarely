using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    delegate float GetHorizontalMoventDelegate();

    RefDependsOn<GetHorizontalMoventDelegate> HorizontalMovement = new RefDependsOn<GetHorizontalMoventDelegate>(GameManagerInstance.UnlockStateID.Possession);

    public CharacterController2D controller2D;
    [Range(0, 100.0f)] public float MovementSpeed = 10.0f;


    private bool Jump = false;
    private float HorizontalMove = 0.0f;

    void Awake() {
        HorizontalMovement.WrappedValue = GetHorizontalMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float GetHorizontalMovement() {
        return Input.GetAxisRaw("Horizontal") * MovementSpeed;
    }

    // Update is called once per frame
    void Update() {
        float moveValue = 0;
        if (HorizontalMovement.WrappedValue != null)
            moveValue = HorizontalMovement.WrappedValue();
        
        HorizontalMove = moveValue;
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller2D.Move((HorizontalMove * Time.fixedDeltaTime), false, Jump);
        Jump = false;
    }
}
