using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(UnityEngine.PostProcessing.PostProcessingBehaviour))]
public class CameraController : MonoBehaviour {
    [SerializeField] 
    private UnityEngine.PostProcessing.PostProcessingProfile GreyScaleProfile;

    // Start is called before the first frame update
    void Start() {

        if (!GameManagerInstance.Instance.GetUnlockState(GameManagerInstance.UnlockStateID.ColourVision))
            GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = GreyScaleProfile;

        SettingController.Instance.OnSettingChanged += OnUnlock;
    }

    void OnUnlock(GameManagerInstance.UnlockStateID id, bool oldState, bool newState) {
        if (id == GameManagerInstance.UnlockStateID.ColourVision && newState != oldState) {
            if (newState)
                GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = null;
            else
                GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = GreyScaleProfile;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
