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
        GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = GreyScaleProfile;
    }

    // Update is called once per frame
    void Update() {

    }
}
