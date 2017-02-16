using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonToggle : MonoBehaviour
{
	// Update is called once per frame
	private void Update () {
	    if (Input.GetKeyDown(KeyCode.Tab))
	    {
	        gameObject.GetComponent<Cannon>().enabled = !gameObject.GetComponent<Cannon>().enabled;

	    }
	}
}
