using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterMenuController : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public float UIBasicScale;

    // Start is called before the first frame update
    void Start()
    {
        UIBasicScale = 50f * TopValue.TopValueSingleton.UIScale;

        MainMenuPanel.GetComponent<MainMenuController>().CallOuterMenuController = this;

        MainMenuPanel.GetComponent<MainMenuController>().Scaling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
