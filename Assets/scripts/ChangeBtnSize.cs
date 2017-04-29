using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBtnSize : MonoBehaviour {
    void OnMouseOver()
    {
        //Debug.Log("Meehika is smart");
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(180,40);//rect.Set(-93, -2.41f, 160, 30);
    }
    void OnMouseExit()
    {
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
    }
}