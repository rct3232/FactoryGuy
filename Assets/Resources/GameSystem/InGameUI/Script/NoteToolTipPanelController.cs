using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteToolTipPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public NotificationManager CallNotificationManager;
    public GameObject TextBox;
    
    void Awake()
    {
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();

        TextBox = transform.GetChild(0).gameObject;
    }

    public void Scaling()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.4f);
        gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.4f);

        CallPanelController.FontScaling(gameObject);
    }

    public void DisplayNote()
    {
        NotificationManager.NoteInfo Note = CallNotificationManager.Note;
        if(gameObject.activeSelf)
        {
            if(Note.Content != "")
            {
                if(TextBox.GetComponent<Text>().text != Note.Content)
                {
                    TextBox.GetComponent<Text>().text = Note.Content;
                    TextBox.GetComponent<Text>().color = Note.NoteColor;

                    Canvas.ForceUpdateCanvases();
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(TextBox.GetComponent<RectTransform>().rect.width + (CallPanelController.CurrentEdgePadding * 2f), gameObject.GetComponent<RectTransform>().rect.height);
                }
            }
            else
            {
                transform.parent.gameObject.GetComponentInParent<ToolTipPanelController>().ClosePanel();
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, gameObject.GetComponent<RectTransform>().rect.height);
                gameObject.SetActive(false);
            }
        }
        else
        {
            if(Note.Content != "")
            {
                transform.parent.gameObject.GetComponentInParent<ToolTipPanelController>().Initializing(Input.mousePosition, true, false, gameObject);
                gameObject.SetActive(true);

                TextBox.GetComponent<Text>().text = Note.Content;
                TextBox.GetComponent<Text>().color = Note.NoteColor;

                Canvas.ForceUpdateCanvases();
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(TextBox.GetComponent<RectTransform>().rect.width + (CallPanelController.CurrentEdgePadding * 2f), gameObject.GetComponent<RectTransform>().rect.height);
            }
        }
    }
}
