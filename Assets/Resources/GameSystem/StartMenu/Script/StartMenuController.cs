using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject StartButton;
    int CurrentMapSizeRatio;

    void CheckStartAvaliable()
    {
        if(CurrentMapSizeRatio > 0) StartButton.GetComponent<Button>().interactable = true;
        else StartButton.GetComponent<Button>().interactable = false;
    }

    public void Initializing()
    {
        CurrentMapSizeRatio = 0;
        CheckStartAvaliable();
    }

    public void MapSizeButtonSelect(int MapRatio)
    {
        CurrentMapSizeRatio = MapRatio;

        CheckStartAvaliable();
    }

    public void StartButtonSelect()
    {
        TopValue.TopValueSingleton.MapSize = 4 * CurrentMapSizeRatio;
        SceneManager.LoadScene("InGameScene");
    }
}
