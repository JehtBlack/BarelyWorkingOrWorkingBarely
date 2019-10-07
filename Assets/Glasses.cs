using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glasses : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GameManagerInstance.Instance.UnlockStateChanged += Instance_UnlockStateChanged;
    }

    private void Instance_UnlockStateChanged(GameManagerInstance.UnlockStateID arg1, bool arg2, bool arg3)
    {
        if(arg1 == GameManagerInstance.UnlockStateID.ColourVision)
        {
            if(arg3)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
