using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowButtons : MonoBehaviour
{
	public GameObject buttons;
	private bool areVisible;
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            areVisible = !areVisible;
            if (areVisible)
                buttons.SetActive(false);
            else
                buttons.SetActive(true);
        }
    }
}


