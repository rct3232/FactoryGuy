using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour
{
    GameObject MenuButtonObject;
    GameObject OptionButtonObject;
    List<GameObject> OptionButtonList = new List<GameObject>();
    List<GameObject> ToggleButtonList = new List<GameObject>();
    public bool MenuActive = false;
    public bool OptionActive = false;

    void Awake()
    {
        Initializing();
    }

    void Initializing()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int FunctionButtonCount = (transform.childCount - 2) / 2;

            if(i == 0) OptionButtonObject = transform.GetChild(i).gameObject;
            else if(i < 1 + FunctionButtonCount) OptionButtonList.Add(transform.GetChild(i).gameObject);
            else if(i < 1 + (FunctionButtonCount * 2)) ToggleButtonList.Add(transform.GetChild(i).gameObject);
            else if(i == transform.childCount - 1) MenuButtonObject = transform.GetChild(i).gameObject;
        }
    }

    public void MenuButtonSelect()
    {
        if(MenuActive)
        {
            MenuActive = false;
            OptionActive = false;

            OptionButtonObject.SetActive(false);
            foreach(var Button in ToggleButtonList) Button.SetActive(false);
            foreach(var Button in OptionButtonList) Button.SetActive(false);
        }
        else
        {
            MenuActive = true;
            OptionActive = false;

            OptionButtonObject.SetActive(true);
            foreach(var Button in ToggleButtonList) Button.SetActive(true);
        }
    }

    public void OptionButtonSelect()
    {
        if(OptionActive)
        {
            OptionActive = false;

            foreach(var Button in ToggleButtonList) Button.SetActive(true);
            foreach(var Button in OptionButtonList) Button.SetActive(false);
        }
        else
        {
            OptionActive = true;

            foreach(var Button in ToggleButtonList) Button.SetActive(false);
            foreach(var Button in OptionButtonList) Button.SetActive(true);
        }
    }

    public void SaveButtonSelect()
    {

    }

    public void LoadButtonSelect()
    {

    }

    public void ExitButtonSelect()
    {
        SceneManager.LoadScene("StartScene");
    }
}
