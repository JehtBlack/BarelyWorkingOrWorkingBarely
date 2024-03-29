﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ShopController : MonoBehaviour {

    public static ShopController Instance;

    private Canvas ShopCanvas;

    public event Action<bool> ShopOpen;

    public void Awake() {
        ShopCanvas = GetComponent<Canvas>();
        ShopCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        ShopCanvas.worldCamera = Camera.main;

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
        
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Shop"))
        //    gameObject.GetComponent<Canvas>().enabled = !gameObject.GetComponent<Canvas>().enabled;
    }

    public void OpenShop()
    {
        ShopOpen?.Invoke(true);
        ShopCanvas.enabled = true;
    }
    public void CloseShop()
    {
        ShopOpen?.Invoke(true);
        ShopCanvas.enabled = true;
    }
    public void ToggleShop()
    {
        ShopOpen?.Invoke(!ShopCanvas.enabled);
        ShopCanvas.enabled = !ShopCanvas.enabled;
    }
    public bool GetOpen()
    {
        if(ShopCanvas != null)
        {
            return ShopCanvas.enabled;
        }
        return false;
    }
}
