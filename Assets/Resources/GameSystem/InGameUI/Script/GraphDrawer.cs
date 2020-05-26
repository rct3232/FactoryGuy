using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphDrawer : MonoBehaviour
{
    PanelController CallPanelController;
    GameObject GraphCarrier = null;
    GameObject xAxisIndexCarrier;
    GameObject yAxisIndexCarrier;
    float GraphHeight;
    float xDistance;
    float yMaximum;
    float CorrectionValue;

    // Start is called before the first frame update
    void Awake()
    {
        CallPanelController = GameObject.Find("Canvas").GetComponent<PanelController>();
    }

    void Start()
    {
        
    }

    public void DrawLineGraph(float[] YaxisIndexValue, float StandardValue, List<string> XaxisIndexData, float YaxisIndexGap, List<float>[] Data, Color[] GraphColor)
    {
        GraphCarrier = transform.GetChild(0).gameObject;
        xAxisIndexCarrier = transform.GetChild(1).gameObject;
        yAxisIndexCarrier = transform.GetChild(2).gameObject;

        GraphHeight = GraphCarrier.GetComponent<RectTransform>().sizeDelta.y - CallPanelController.CurrentEdgePadding;
        xDistance = GraphCarrier.GetComponent<RectTransform>().sizeDelta.x / (float)XaxisIndexData.Count;

        CorrectionValue = 0;

        if(GraphColor == null)
        {
            GraphColor = new Color[Data.Length];
            for(int i = 0; i < Data.Length; i++)
            {
                GraphColor[i] = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f), 1f);
            }
        }

        List<float>[] CorrectedData = new List<float>[Data.Length];
        float[] DataMin = new float[Data.Length];
        float[] DataMax = new float[Data.Length];
        float[] CorrectedDataMin = new float[Data.Length];
        float[] CorrectedDataMax = new float[Data.Length];

        for(int i = 0; i < Data.Length; i++)
        {
            DataMin[i] = Mathf.Min(Data[i].ToArray());
            DataMax[i] = Mathf.Max(Data[i].ToArray());
        }

        if(YaxisIndexValue != null)
        {
            if(YaxisIndexValue[0] > Mathf.Min(DataMin))
            {
                YaxisIndexValue[0] = Mathf.Min(DataMin);
            }

            CorrectionValue = - YaxisIndexValue[0];
        }

        for(int i = 0; i < Data.Length; i++)
        {
            CorrectedData[i] = new List<float>();
            foreach(var Value in Data[i])
            {
                CorrectedData[i].Add(Value + CorrectionValue);
            }
        }

        for(int i = 0; i < CorrectedData.Length; i++)
        {
            CorrectedDataMin[i] = Mathf.Min(CorrectedData[i].ToArray());
            CorrectedDataMax[i] = Mathf.Max(CorrectedData[i].ToArray());
        }

        if(YaxisIndexValue != null)
        {
            if(YaxisIndexValue[1] < Mathf.Max(DataMax))
            {
                yMaximum = Mathf.Max(CorrectedDataMax);
            }
            else
            {
                yMaximum = YaxisIndexValue[1] + CorrectionValue;
            }
        }
        else
        {
            yMaximum = Mathf.Max(CorrectedDataMax);
        }

        for(int i = 0; i < Data.Length; i++) ShowLineGraph(CorrectedData[i], GraphColor[i]);

        CreateLineGraphIndex(StandardValue, XaxisIndexData, YaxisIndexGap);
    }

    void ShowLineGraph(List<float> Data, Color GraphColor)
    {
        GameObject LastDot = null;
        for(int i = 0; i < Data.Count; i++)
        {
            float xPosition = i * xDistance;

            float yPerspectiveValue = 0.5f;
            if(yMaximum != 0) yPerspectiveValue = Data[i] / yMaximum;
            float yPosition = (yPerspectiveValue * GraphHeight) + (CallPanelController.CurrentEdgePadding * 0.5f);
            GameObject NewDot = CreateValueDot(new Vector2(xPosition, yPosition), GraphColor);
            if(LastDot != null)
            {
                CreateConnection(LastDot.GetComponent<RectTransform>().anchoredPosition, NewDot.GetComponent<RectTransform>().anchoredPosition, GraphColor);
            }
            LastDot = NewDot;
        }
    }

    void CreateLineGraphIndex(float StandardValue, List<string> XaxisIndexData, float YaxisIndexGap)
    {
        for(int i = 0; i < XaxisIndexData.Count; i++)
        {
            GameObject newIndex = new GameObject("Index", typeof(Text));
            newIndex.transform.SetParent(xAxisIndexCarrier.transform, false);
            newIndex.GetComponent<Text>().text = XaxisIndexData[i];
            newIndex.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            newIndex.AddComponent<ContentSizeFitter>();
            newIndex.GetComponent<ContentSizeFitter>().horizontalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            newIndex.GetComponent<ContentSizeFitter>().verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained;
            RectTransform IndexRect = newIndex.GetComponent<RectTransform>();
            IndexRect.sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            IndexRect.anchorMin = new Vector2(0,0);
            IndexRect.anchorMax = new Vector2(0,0);
            IndexRect.anchoredPosition = new Vector2(i * xDistance, CallPanelController.CurrentEdgePadding * 0.5f);
        }

        float StandardIndexPosition = float.NaN;
        float CorrectedStandardValue = float.NaN;
        if(StandardValue != float.NaN)
        {
            CorrectedStandardValue = StandardValue + CorrectionValue;
            if(CorrectedStandardValue >= 0 && CorrectedStandardValue <= yMaximum)
            {
                StandardIndexPosition = ((CorrectedStandardValue / yMaximum) * GraphHeight) + (CallPanelController.CurrentEdgePadding * 0.5f);
            }
        }

        int yIndexCount = Mathf.FloorToInt((GraphHeight + CallPanelController.CurrentEdgePadding) / (CallPanelController.CurrentEdgePadding * 2f));
        List<float> YaxisIndexData = new List<float>();
        if(!float.IsNaN(YaxisIndexGap))
        {
            if(Mathf.FloorToInt(yMaximum / YaxisIndexGap) > yIndexCount)
            {
                int GapCorrectionValue = Mathf.CeilToInt(yMaximum / YaxisIndexGap) / yIndexCount;
                yIndexCount = Mathf.FloorToInt(yMaximum / YaxisIndexGap / GapCorrectionValue);
                YaxisIndexGap *= GapCorrectionValue;
            }
            else
            {
                yIndexCount = Mathf.FloorToInt(yMaximum / YaxisIndexGap);
            }
        }
        else
        {
            YaxisIndexGap = yMaximum / (float)(yIndexCount - 1);
        }

        int DigitCorrectionValue = 1;
        for(int i = 0; i < yIndexCount; i++)
        {
            float Value = (i * YaxisIndexGap) - CorrectionValue;
            if(Value / 1000f > 10f)
            {
                for(;Value < 100000f;)
                {
                    Value *= 0.1f;
                    DigitCorrectionValue *= 10;
                }
                Value = Mathf.CeilToInt(Value);
                Value *= 0.1f;
                DigitCorrectionValue *= 10;
            }
            else
            {
                Value *= 10;
                Value = Mathf.CeilToInt(Value);
                Value *= 0.1f;
            }
            YaxisIndexData.Add(Value);
        }

        if(!float.IsNaN(StandardIndexPosition))
        {
            GameObject newIndex = new GameObject("StandardIndex", typeof(Text));
            newIndex.transform.SetParent(yAxisIndexCarrier.transform, false);
            newIndex.GetComponent<Text>().text = StandardValue.ToString();
            newIndex.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            newIndex.GetComponent<Text>().fontSize = CallPanelController.GetFontSize(3);
            newIndex.AddComponent<ContentSizeFitter>();
            newIndex.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            newIndex.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            RectTransform IndexRect = newIndex.GetComponent<RectTransform>();
            IndexRect.sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
            IndexRect.anchorMin = new Vector2(0,0);
            IndexRect.anchorMax = new Vector2(0,0);
            IndexRect.pivot = new Vector2(0,0.5f);
            IndexRect.anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding * 0.25f, StandardIndexPosition);
        }

        float LastIndexPosition = float.NaN;
        for(int i = 0; i < YaxisIndexData.Count; i++)
        {
            float yPerspectivePosition = 0.5f;
            if(yMaximum != 0) yPerspectivePosition = (YaxisIndexData[i] + CorrectionValue) / yMaximum;
            float yPosition = (yPerspectivePosition * GraphHeight) + (CallPanelController.CurrentEdgePadding * 0.5f);

            if(!float.IsNaN(LastIndexPosition)) if(LastIndexPosition - yPosition < CallPanelController.CurrentEdgePadding * 0.25f) continue;

            GameObject newIndex = new GameObject("Index", typeof(Text));
            newIndex.transform.SetParent(yAxisIndexCarrier.transform, false);
            newIndex.GetComponent<Text>().text = YaxisIndexData[i].ToString();
            newIndex.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            newIndex.GetComponent<Text>().fontSize = CallPanelController.GetFontSize(3);
            newIndex.AddComponent<ContentSizeFitter>();
            newIndex.GetComponent<ContentSizeFitter>().horizontalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            newIndex.GetComponent<ContentSizeFitter>().verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained;
            RectTransform IndexRect = newIndex.GetComponent<RectTransform>();
            IndexRect.sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
            IndexRect.anchorMin = new Vector2(0,0);
            IndexRect.anchorMax = new Vector2(0,0);
            IndexRect.pivot = new Vector2(0,0.5f);
            
            IndexRect.anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding * 0.25f, yPosition);

            if(!float.IsNaN(StandardIndexPosition))
            {
                if(Mathf.Abs(IndexRect.anchoredPosition.y - StandardIndexPosition) < CallPanelController.CurrentEdgePadding * 0.5f)
                {
                    Destroy(newIndex);
                }
            }

            LastIndexPosition = yPosition;
        }
    }

    public void DrawPieGraph(List<string> DataIndex, List<float> Data, Color[] GraphColor)
    {
        GraphCarrier = gameObject;
        xAxisIndexCarrier = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        GraphHeight = GraphCarrier.GetComponent<RectTransform>().sizeDelta.y;

        if(GraphColor == null)
        {
            GraphColor = new Color[Data.Count];
            for(int i = 0; i < Data.Count; i++)
            {
                GraphColor[i] = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f), 1f);
            }
        }

        float[] DataPercentage = new float[Data.Count];
        float zRotation = 0;

        float TotalValue = 0;
        foreach(var Value in Data)
        {
            TotalValue += Value;
        }

        if(TotalValue <= 0)
        {
            GameObject NoDataText = new GameObject("Text", typeof(Text));
            NoDataText.transform.SetParent(GraphCarrier.transform, false);
            NoDataText.GetComponent<Text>().text = "No Data";
            NoDataText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            NoDataText.GetComponent<Text>().fontSize = CallPanelController.GetFontSize(1);
            NoDataText.AddComponent<ContentSizeFitter>();
            NoDataText.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            NoDataText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            RectTransform NoDataTextRect = NoDataText.GetComponent<RectTransform>();
            NoDataTextRect.sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            NoDataTextRect.anchorMin = new Vector2(0.5f, 0.5f);
            NoDataTextRect.anchorMax = new Vector2(0.5f, 0.5f);
            NoDataTextRect.anchoredPosition = new Vector2(0, 0);

            return;
        }

        for(int i = 0; i < DataPercentage.Length; i++)
        {
            DataPercentage[i] = Data[i] / TotalValue;
        }

        for(int i = 0; i < DataPercentage.Length; i++)
        {
            CreatePie(DataIndex[i], DataPercentage[i], zRotation, GraphColor[i]);
            zRotation -= DataPercentage[i] * 360f;
        }
    }

    void CreatePie(string DataIndex, float FillAmount, float RotationValue, Color PieColor)
    {
        GameObject newPie = new GameObject("Pie", typeof(Image));
        newPie.transform.SetParent(GraphCarrier.transform, false);
        newPie.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/Circle");
        newPie.GetComponent<Image>().color = PieColor;
        newPie.GetComponent<Image>().type = Image.Type.Filled;
        newPie.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
        newPie.GetComponent<Image>().fillOrigin = (int)Image.Origin360.Top;
        newPie.GetComponent<Image>().fillAmount = FillAmount;
        RectTransform PieRect = newPie.GetComponent<RectTransform>();
        PieRect.sizeDelta = new Vector2(GraphHeight, GraphHeight);
        PieRect.anchorMin = new Vector2(0.5f, 0.5f);
        PieRect.anchorMax = new Vector2(0.5f, 0.5f);
        PieRect.pivot = new Vector2(0.5f, 0.5f);
        PieRect.anchoredPosition = new Vector2(0,0);
        newPie.transform.rotation = Quaternion.Euler(0,0,RotationValue);

        if(FillAmount > 0f) CreatePieIndex(newPie, - (FillAmount * 180f) + RotationValue, DataIndex);
    }

    void CreatePieIndex(GameObject Pie, float RotationValue, string DataIndex)
    {
        GameObject newIndexPanel = new GameObject("IndexPanel", typeof(Image));
        newIndexPanel.transform.SetParent(xAxisIndexCarrier.transform, false);
        newIndexPanel.GetComponent<Image>().color = new Color(0,0,0,0);
        RectTransform PanelRect = newIndexPanel.GetComponent<RectTransform>();
        PanelRect.pivot = new Vector2(0, 0);
        PanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        PanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        PanelRect.anchoredPosition = new Vector2(0, 0);
        PanelRect.sizeDelta = new Vector2(0, GraphHeight / 2f);
        newIndexPanel.transform.eulerAngles = new Vector3(0,0,RotationValue);
        // Debug.Log(newIndexPanel.transform.eulerAngles.z + " " + DataIndex);

        GameObject newIndex = new GameObject("IndexText", typeof(Text));
        newIndex.transform.SetParent(newIndexPanel.transform, false);
        newIndex.GetComponent<Text>().text = DataIndex;
        newIndex.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        newIndex.GetComponent<Text>().fontSize = CallPanelController.GetFontSize(3);
        if(Pie.GetComponent<Image>().color.r + Pie.GetComponent<Image>().color.g + Pie.GetComponent<Image>().color.b > 1.5F)
        {
            newIndex.GetComponent<Text>().color = new Color(0, 0, 0, 1f);
        }
        else
        {
            newIndex.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
        }
        newIndex.AddComponent<ContentSizeFitter>();
        newIndex.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        newIndex.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        RectTransform IndexRect = newIndex.GetComponent<RectTransform>();
        IndexRect.sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
        IndexRect.anchorMin = new Vector2(0.5f, 0.5f);
        IndexRect.anchorMax = new Vector2(0.5f, 0.5f);
        IndexRect.pivot = new Vector2(0.5f, 0.5f);
        IndexRect.anchoredPosition = new Vector2(0, 0);
        newIndex.transform.rotation = Quaternion.Euler(0, 0, - Pie.transform.rotation.z);
    }

    public void LineGraphClear()
    {
        if(GraphCarrier == null) return;

        for(int i = GraphCarrier.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(GraphCarrier.transform.GetChild(i).gameObject);
        }
        for(int i = xAxisIndexCarrier.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(xAxisIndexCarrier.transform.GetChild(i).gameObject);
        }
        for(int i = yAxisIndexCarrier.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(yAxisIndexCarrier.transform.GetChild(i).gameObject);
        }
    }

    public void PieGraphClear()
    {
        if(GraphCarrier == null) return;

        for(int i = 0; i < GraphCarrier.transform.childCount; i++)
        {
            Destroy(GraphCarrier.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < xAxisIndexCarrier.transform.childCount; i++)
        {
            Destroy(xAxisIndexCarrier.transform.GetChild(i).gameObject);
        }
    }

    public void DrawProgressGraph(float[] YaxisIndexValue, List<string> XaxisIndexData, List<float>[] Data, Color[] GraphColor)
    {
        GraphCarrier = transform.GetChild(0).gameObject;
        xAxisIndexCarrier = transform.GetChild(1).gameObject;
        yAxisIndexCarrier = transform.GetChild(2).gameObject;

        GraphHeight = GraphCarrier.GetComponent<RectTransform>().sizeDelta.y - CallPanelController.CurrentEdgePadding;
        xDistance = GraphCarrier.GetComponent<RectTransform>().sizeDelta.x / (CallPanelController.CurrentUIsize * 0.5f);

        CorrectionValue = 0;

        if(GraphColor == null)
        {
            GraphColor = new Color[Data.Length];
            for(int i = 0; i < Data.Length; i++)
            {
                GraphColor[i] = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f), 1f);
            }
        }

        List<float>[] CorrectedData = new List<float>[Data.Length];
        float[] DataMin = new float[Data.Length];
        float[] DataMax = new float[Data.Length];
        float[] CorrectedDataMin = new float[Data.Length];
        float[] CorrectedDataMax = new float[Data.Length];

        for(int i = 0; i < Data.Length; i++)
        {
            DataMin[i] = Mathf.Min(Data[i].ToArray());
            DataMax[i] = Mathf.Max(Data[i].ToArray());
        }

        CorrectionValue = - Mathf.Min(DataMin);
        if(YaxisIndexValue != null)
        {
            if(YaxisIndexValue[0] > Mathf.Min(DataMin))
            {
                YaxisIndexValue[0] = Mathf.Min(DataMin);
            }
        }

        for(int i = 0; i < Data.Length; i++)
        {
            CorrectedData[i] = new List<float>();
            foreach(var Value in Data[i])
            {
                CorrectedData[i].Add(Value + CorrectionValue);
            }
        }

        for(int i = 0; i < CorrectedData.Length; i++)
        {
            CorrectedDataMin[i] = Mathf.Min(CorrectedData[i].ToArray());
            CorrectedDataMax[i] = Mathf.Max(CorrectedData[i].ToArray());
        }

        if(YaxisIndexValue != null)
        {
            if(YaxisIndexValue[1] < Mathf.Max(DataMax))
            {
                yMaximum = Mathf.Max(CorrectedDataMax);
            }
            else
            {
                yMaximum = YaxisIndexValue[1] + CorrectionValue;
            }
        }
        else
        {
            yMaximum = Mathf.Max(CorrectedDataMax);
        }

        for(int i = 0; i < Data.Length; i++) ShowProgressGraph(CorrectedData[i], GraphColor[i]);

        CreateLineGraphIndex(float.NaN, XaxisIndexData, float.NaN);
    }

    void ShowProgressGraph(List<float> Data, Color GraphColor)
    {
        GameObject LastDot = null;
        for(int i = Data.Count - 1; i >= 0; i--)
        {
            float xPosition = GraphCarrier.GetComponent<RectTransform>().sizeDelta.x - ((Data.Count - 1 - i) * xDistance);
            float yPosition;
            GameObject NewDot;

            if(i < Data.Count - 1)
            {
                if(Data[i] != Data[i + 1])
                {
                    yPosition = LastDot.GetComponent<RectTransform>().anchoredPosition.y;
                    NewDot = CreateValueDot(new Vector2(xPosition, yPosition), new Color(0,0,0,0));

                    CreateConnection(LastDot.GetComponent<RectTransform>().anchoredPosition, NewDot.GetComponent<RectTransform>().anchoredPosition, GraphColor);
                    LastDot = NewDot;
                }
            }
            
            float yPerspectiveValue = 0.5f;
            if(yMaximum != 0) yPerspectiveValue = Data[i] / yMaximum;

            yPosition = (yPerspectiveValue * GraphHeight) + (CallPanelController.CurrentEdgePadding * 0.5f);
            NewDot = CreateValueDot(new Vector2(xPosition, yPosition), new Color(0,0,0,0));
            
            if(LastDot != null)
            {
                CreateConnection(LastDot.GetComponent<RectTransform>().anchoredPosition, NewDot.GetComponent<RectTransform>().anchoredPosition, GraphColor);
            }
            LastDot = NewDot;
        }
    }

    void CreateProgressGraphIndex(List<string> XaxisIndexData, float[] YaxisIndexValue)
    {

    }

    GameObject CreateValueDot(Vector2 Position, Color DotColor)
    {
        GameObject newDot = new GameObject("Dot", typeof(Image));
        newDot.transform.SetParent(GraphCarrier.transform, false);
        newDot.GetComponent<Image>().sprite = null;
        newDot.GetComponent<Image>().color = DotColor;
        RectTransform DotRect = newDot.GetComponent<RectTransform>();
        DotRect.anchoredPosition = Position;
        DotRect.sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding * 0.5f, CallPanelController.CurrentEdgePadding * 0.5f);
        DotRect.anchorMin = new Vector2(0,0);
        DotRect.anchorMax = new Vector2(0,0);

        return newDot;
    }

    void CreateConnection(Vector2 DotPositionA, Vector2 DotPositionB, Color LineColor)
    {
        GameObject newConnection = new GameObject("Connection", typeof(Image));
        newConnection.transform.SetParent(GraphCarrier.transform, false);
        newConnection.GetComponent<Image>().sprite = null;
        newConnection.GetComponent<Image>().color = LineColor;
        RectTransform ConnectionRect = newConnection.GetComponent<RectTransform>();
        Vector2 Direction = (DotPositionB - DotPositionA).normalized;
        float Distance = Vector2.Distance(DotPositionA, DotPositionB);
        ConnectionRect.sizeDelta = new Vector2(Distance, CallPanelController.CurrentEdgePadding * 0.1f);
        ConnectionRect.anchorMin = new Vector2(0,0);
        ConnectionRect.anchorMax = new Vector2(0,0);
        ConnectionRect.anchoredPosition = DotPositionA + Direction * Distance * 0.5f;
        ConnectionRect.localEulerAngles = new Vector3(0,0,Mathf.Atan2(Direction.y, Direction.x)*Mathf.Rad2Deg);
    }
}
