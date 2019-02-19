using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DontBreakMyGame : MonoBehaviour {

    public EventSystem ES;
    private GameObject storeSelected;

	// Use this for initialization
	void Start () {
        storeSelected = ES.firstSelectedGameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (ES.currentSelectedGameObject)
        {
            if(ES.currentSelectedGameObject != storeSelected)
            {
                ES.SetSelectedGameObject(storeSelected);
            }
            else
            {
                storeSelected = ES.currentSelectedGameObject;
            }
        }
	}
}
