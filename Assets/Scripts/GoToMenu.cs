using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoToMenu : MonoBehaviour {

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
