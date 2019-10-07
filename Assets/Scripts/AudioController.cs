using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource ShopMusic;
    public AudioSource GameMusic;

    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        ShopController.Instance.ShopOpen += Instance_ShopOpen;
        GameManagerInstance.Instance.UnlockStateChanged += OnUnlock;
    }

    void OnUnlock(GameManagerInstance.UnlockStateID id, bool oldState, bool newState)
    {
        if (id == GameManagerInstance.UnlockStateID.Hearing)
        {
            if (newState)
            {
                if (ShopController.Instance.GetOpen())
                {
                    ShopMusic.Play();
                }
                else
                {
                    GameMusic.Play();
                }
            }else
            {
                GameMusic.Pause();
                ShopMusic.Pause();
            }
        }
    }

    private void Instance_ShopOpen(bool obj)
    {
        if (GameManagerInstance.Instance.GetUnlockState(GameManagerInstance.UnlockStateID.Hearing))
        {
            if (obj)
            {
                GameMusic.Pause();
                ShopMusic.Play();
            }
            else
            {
                GameMusic.Play();
                ShopMusic.Pause();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
