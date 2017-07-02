using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class Door : MonoBehaviour {

    public bool locked;

    // Use this for initialization
    void Start () {
        
        FakeWorld.AddWorldObject<Door>(gameObject.name,this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
