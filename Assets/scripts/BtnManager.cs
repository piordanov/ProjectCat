using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour {


    public void NewGameBtn()
    {
        Debug.Log("New Game Starting");
        SceneManager.LoadScene("mainscene");
    }

    public void ControlsBtn()
    {
        Debug.Log("Going to Controls");
        SceneManager.LoadScene("controlscene");
    }

    public void ExitBtn()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
