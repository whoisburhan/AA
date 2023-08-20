using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class MyGame : MonoBehaviour {

    public void onReady() {
        Debug.Log("MyGame onReady.");
    }

    public void onSuccess() {
        Debug.Log("MyGame onSuccess.");
    }

    public void onFail() {
        Debug.Log("MyGame onFail.");
    }

    public void ShowRewardAd() { 

    }

    private void Awake()
    {
        Debug.Log("MyGame awake.");
    }

    void Update()
    {
        //Debug.Log("MyGame upadate.");
    }

}