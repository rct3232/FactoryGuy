using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public OuterMenuController CallOuterMenuController;
    public GameObject StartMenu;

    void Start()
    {
        
    }

    // Start is called before the first frame update
    public void Scaling()
    {
        Vector2 ButtonSize = new Vector2(CallOuterMenuController.UIBasicScale * 4f, CallOuterMenuController.UIBasicScale);
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = ButtonSize;
        }

        float ButtonSpacing = CallOuterMenuController.UIBasicScale * 2f;
        if((ButtonSpacing * (transform.childCount - 1)) + (ButtonSize.y * transform.childCount) > Screen.height) ButtonSpacing = (Screen.height - (ButtonSize.y * transform.childCount)) / (transform.childCount - 1);
        gameObject.GetComponent<VerticalLayoutGroup>().spacing = ButtonSpacing;
    }

    public void StartButtonSelect()
    {
        StartMenu.SetActive(!StartMenu.activeSelf);
        StartMenu.GetComponent<StartMenuController>().Initializing();
    }

    public void LoadButtonSelect()
    {

    }

    public void OptionButtonSelect()
    {

    }

    public void QuitButtonSelect()
    {

    }
}
