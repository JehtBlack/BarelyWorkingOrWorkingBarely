using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller2D;
    [Range(0, 100.0f)] public float MovementSpeed = 10.0f;

    private bool CanMove = false;
    private bool Jump = false;
    private float HorizontalMove = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameManagerInstance.Instance.GetUnlockState(GameManagerInstance.UnlockStateID.Possession))
            CanMove = false;
        else
            CanMove = true;

        GameManagerInstance.Instance.UnlockStateChanged += OnUnlock;

    }

    void OnUnlock(GameManagerInstance.UnlockStateID id, bool oldState, bool newState)
    {
        if (id == GameManagerInstance.UnlockStateID.Possession && newState && !oldState)
            CanMove = true;
        else
            CanMove = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (CanMove)
        {
            HorizontalMove = Input.GetAxisRaw("Horizontal") * MovementSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                Jump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        controller2D.Move((HorizontalMove * Time.fixedDeltaTime), false, Jump);
        Jump = false;
    }
}
