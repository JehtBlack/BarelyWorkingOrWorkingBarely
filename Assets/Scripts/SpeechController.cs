using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SpeechController : MonoBehaviour
{

    public static SpeechController Instance;

    private Canvas SpeechCanvas;

    [SerializeField]
    public GameObject ContainerObj;
    [SerializeField]
    private string[] TextMarks;

    private int Index = 0;
    private string CurrentString;
    [SerializeField]
    private TMPro.TextMeshProUGUI TextArea;

    public float WaitTimeBetweenChars = 0.1f;
    private bool CR_Run = false;

    public event Action<bool> SpeechEnded;

    public void Awake()
    {
        SpeechCanvas = GetComponent<Canvas>();
        SpeechCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        SpeechCanvas.worldCamera = Camera.main;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //TypeWriter = TypeWriterRoutine();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeechCanvas.enabled)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                if (CR_Run)
                    SkipTypeWriter();
                else
                    Next();
            }
        }
    }

    public void SetDialogue(string[] Text)
    {
        Index = 0;

        TextMarks = Text;
        
    }

    public void StartDialogue()
    {
        if (TextMarks.Length > Index)
        {
            CurrentString = TextMarks[Index];
            StartCoroutine("TypeWriter");
        }else
        {
            Close();
        }
    }

    public void SkipTypeWriter()
    {
        if (TextMarks.Length > Index)
        {
            StopCoroutine("TypeWriter");
            CR_Run = false;
            if (TextArea != null)
            {
                TextArea.text = CurrentString;
            }
        }
    }
    
    public void Next()
    {
        if (!CR_Run)
        {
            if (TextArea != null)
            {
                TextArea.text = "";
            }
            StopCoroutine("TypeWriter");
            Index += 1;
            StartDialogue();
        }
    }

    public void Open()
    {
        if(SpeechCanvas != null && ContainerObj != null)
        {
            ContainerObj.SetActive(true);
            SpeechCanvas.enabled = true;
        }
    }

    public void Close()
    {
        if (SpeechCanvas != null && ContainerObj != null)
        {
            ContainerObj.SetActive(false);

            SpeechCanvas.enabled = false;
        }
        SpeechEnded?.Invoke(true);
    }

    private void NexCharacter(int StringIndex)
    {
        if(TextArea != null)
        {
            TextArea.text += CurrentString[StringIndex];
        }
    }

    public bool GetOpen()
    {
        if (SpeechCanvas != null)
        {
            return SpeechCanvas.enabled;
        }
        return false;
    }

    IEnumerator TypeWriter()
    {
        CR_Run = true;
        for (int i = 0; i < CurrentString.Length ; i++ )
        {
            NexCharacter(i);
            yield return new WaitForSeconds(WaitTimeBetweenChars);
        }
        CR_Run = false;
    }
}
