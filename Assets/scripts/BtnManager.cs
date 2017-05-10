using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour {
    int control = 0;
    public GameObject arrow;
    public void NewGameBtn()
    {
        Debug.Log("New Game Starting");
        SceneManager.LoadScene("mainscene");
    }

    public void ControlsBtn()
    {
        control++;
        Debug.Log("Going to Controls");
        //SceneManager.LoadScene("controlscene");
        if (control % 2 == 0)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
        }
    }

    public void ExitBtn()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
