using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    ClickChecker CallClickChecker;
    bool isInitialize;
    RectTransform PanelRect;
    Vector2 InitialPosition;
    Vector3 OnMapPosition;
    bool DynamicPosition;
    
    void Awake()
    {
        PanelRect = gameObject.GetComponent<RectTransform>();
        CallClickChecker = GameObject.Find("BaseSystem").GetComponent<ClickChecker>();

        isInitialize = false;
    }

    public void Initializing(Vector2 initPos, bool isDyanamicPos)
    {
        isInitialize = true;

        InitialPosition = initPos;
        DynamicPosition = isDyanamicPos;

        if(DynamicPosition) OnMapPosition = CallClickChecker.target.transform.position;

        MovePanel(InitialPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInitialize)
        {
            if(DynamicPosition)
            {
                MovePanel(Camera.main.WorldToScreenPoint(OnMapPosition));
            }
        }
    }

    public void MovePanel(Vector2 TargetPosition)
    {
        Vector2 ScreenEdgePoint = new Vector2(Screen.width, Screen.height);

        if(CallPanelController.CurrentSidePanel != null) ScreenEdgePoint.x -= CallPanelController.CurrentSidePanel.transform.parent.gameObject.GetComponent<RectTransform>().rect.width;

        if(TargetPosition.x < 0 || TargetPosition.x + PanelRect.sizeDelta.x > ScreenEdgePoint.x)
        {
            Vector2 CorrectionValue = new Vector2(0,0);

            if(PanelRect.sizeDelta.x > ScreenEdgePoint.x)
            {
                CorrectionValue.x = ScreenEdgePoint.x - (TargetPosition.x + PanelRect.sizeDelta.x);
            }
            else
            {
                if(TargetPosition.x < 0) CorrectionValue.x = - TargetPosition.x;

                if(TargetPosition.x + PanelRect.sizeDelta.x > ScreenEdgePoint.x) CorrectionValue.x = ScreenEdgePoint.x - (TargetPosition.x + PanelRect.sizeDelta.x);
            }

            TargetPosition += CorrectionValue;
        }
        
        if(TargetPosition.y - PanelRect.sizeDelta.y < CallPanelController.CurrentUIsize || TargetPosition.y > ScreenEdgePoint.y)
        {
            Vector2 CorrectionValue = new Vector2(0,0);

            if(PanelRect.sizeDelta.y > ScreenEdgePoint.y - CallPanelController.CurrentUIsize)
            {
                CorrectionValue.y = ScreenEdgePoint.y - TargetPosition.y;
            }
            else
            {
                if(TargetPosition.y - PanelRect.sizeDelta.y < CallPanelController.CurrentUIsize) CorrectionValue.y = - (TargetPosition.y - PanelRect.sizeDelta.y) + CallPanelController.CurrentUIsize;

                if(TargetPosition.y > ScreenEdgePoint.y) CorrectionValue.y = ScreenEdgePoint.y - TargetPosition.y;
            }

            TargetPosition += CorrectionValue;
        }

        PanelRect.anchoredPosition = TargetPosition;
    }

    public void ClosePanel()
    {
        isInitialize = false;
    }
}
