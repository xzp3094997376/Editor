using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnumStart : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

        Debug.Log("Start");
        yield return 1;
        Debug.Log("end");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update");
	}
}
