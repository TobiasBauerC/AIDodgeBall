using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public AIAgent agent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            agent.Controller(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            agent.Controller(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            agent.Controller(3);
	}
}
