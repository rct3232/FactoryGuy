using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject SearchPanel;
    [SerializeField]GameObject FunctionPanel;
    [SerializeField]GameObject InfoPanel;
    [SerializeField]GameObject FatiguePanel;
    [SerializeField]GameObject ExperiencePanel;
    [SerializeField]GameObject FunctionButtonPanel;
    [SerializeField]GameObject ListPanel;
    [SerializeField]GameObject ItemListPanel;
    [SerializeField]GameObject ItemCarrier;
    public int CurrentWorkerIndex = -1;
    EmployeeValue CallEmployeeValue;
    GameObject WorkerImageObject, NameTextObject, AgeTextObject, PayTextObject, AbilityTextObject, FatigueGraphBar, ExperienceGraphBar, PromoteButton, FireButton;
    // Start is called before the first frame update
    void Awake()
    {
        CallEmployeeValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetEmployeeValue().GetComponent<EmployeeValue>();
        
        WorkerImageObject = InfoPanel.transform.GetChild(1).GetChild(0).gameObject;
        NameTextObject = InfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        AgeTextObject = InfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        PayTextObject = InfoPanel.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
        AbilityTextObject = InfoPanel.transform.GetChild(3).GetChild(3).GetChild(1).GetChild(1).gameObject;
        FatigueGraphBar = FatiguePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        ExperienceGraphBar = ExperiencePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        PromoteButton = FunctionButtonPanel.transform.GetChild(1).GetChild(0).gameObject;
        FireButton = FunctionButtonPanel.transform.GetChild(3).GetChild(0).gameObject;
    }

    void Start()
    {

    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize * 8);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, Screen.height - CallPanelController.CurrentUIsize - FunctionPanelSize.y);
        Vector2 ListPanelSize = ListPanel.GetComponent<RectTransform>().sizeDelta;

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);

        for(int i = 0; i < FunctionPanel.transform.childCount; i++) if(i % 2 == 0) FunctionPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);

        InfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 3f);
        for(int i = 0; i < InfoPanel.transform.childCount; i++) if(i % 2 == 0) InfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        InfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
        InfoPanel.transform.GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, 0);
        for(int i = 0; i < InfoPanel.transform.GetChild(3).childCount; i++)
        {
            InfoPanel.transform.GetChild(3).GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
            InfoPanel.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
            InfoPanel.transform.GetChild(3).GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
            if(i + 1 == InfoPanel.transform.GetChild(3).childCount)
            {
                InfoPanel.transform.GetChild(3).GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.45f, 0);
                InfoPanel.transform.GetChild(3).GetChild(i).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.35f, 0);
            }
        }

        FatiguePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        for(int i = 0; i < FatiguePanel.transform.childCount; i++) if(i % 2 == 0) FatiguePanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        FatiguePanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.2f, 0);
        FatiguePanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
        FatiguePanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.7f);
        FatiguePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentUIsize * 0.1f, CallPanelController.CurrentUIsize * 0.1f);
        FatiguePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentUIsize * 0.1f, - CallPanelController.CurrentUIsize * 0.1f);

        ExperiencePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        for(int i = 0; i < ExperiencePanel.transform.childCount; i++) if(i % 2 == 0) ExperiencePanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        ExperiencePanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.2f, 0);
        ExperiencePanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
        ExperiencePanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.7f);
        ExperiencePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentUIsize * 0.1f, CallPanelController.CurrentUIsize * 0.1f);
        ExperiencePanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentUIsize * 0.1f, - CallPanelController.CurrentUIsize * 0.1f);

        FunctionButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        for(int i = 0; i < FunctionButtonPanel.transform.childCount; i++) if(i % 2 == 0) FunctionButtonPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        FunctionButtonPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 5f, 0);
        FunctionButtonPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, CallPanelController.CurrentUIsize * 0.2f);
        FunctionButtonPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentUIsize * 0.2f);
        FunctionButtonPanel.transform.GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.6f, 0);
        FunctionButtonPanel.transform.GetChild(3).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, CallPanelController.CurrentUIsize * 0.2f);
        FunctionButtonPanel.transform.GetChild(3).GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentUIsize * 0.2f);

        ItemListPanel.GetComponent<RectTransform>().sizeDelta = ListPanelSize;
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ListPanelSize.x - (CallPanelController.CurrentEdgePadding * 2), CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, 0, 0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding,0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentEdgePadding);

        ItemListPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.6f, 0);
        ItemListPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
        ItemListPanel.transform.GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.8f, 0);
        ItemListPanel.transform.GetChild(0).GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.8f, 0);

        ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ListPanelSize.x - (CallPanelController.CurrentEdgePadding * 2), CallPanelController.CurrentEdgePadding);
        ItemCarrier.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.6f, 0);
        ItemCarrier.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
        ItemCarrier.transform.GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.8f, 0);
        ItemCarrier.transform.GetChild(0).GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.8f, 0);

        CallPanelController.FontScaling(gameObject);
    }

    public void Initializing()
    {
        ClearInfoPanel();
        DisplayWorkerList();
    }

    public void DisplayWorkerList()
    {
        List<EmployeeValue.EmployeeInfo> EmployList = new List<EmployeeValue.EmployeeInfo>();

        for(int i = 1; i < CallEmployeeValue.EmployeeList.Count; i++)
        {
            EmployList.Add(CallEmployeeValue.EmployeeList[i]);
        }

        for(int i = 0; i < ItemCarrier.transform.childCount; i++)
        {
            ItemCarrier.transform.GetChild(i).gameObject.SetActive(true);
        }

        if(ItemCarrier.transform.childCount < EmployList.Count)
        {
            for(int i = ItemCarrier.transform.childCount; i < EmployList.Count; i++)
            {
                GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
            }
        }
        else if(ItemCarrier.transform.childCount > EmployList.Count)
        {
            for(int i = ItemCarrier.transform.childCount - 1; i >= EmployList.Count; i--)
            {
                ItemCarrier.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < ItemCarrier.transform.childCount; i++)
        {
            if(!ItemCarrier.transform.GetChild(i).gameObject.activeSelf) break;

            ItemCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = EmployList[i].Index.ToString();
            ItemCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text = EmployList[i].BaseInfo.Name;
            ItemCarrier.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Text>().text = EmployList[i].BaseInfo.Salary.ToString();
            ItemCarrier.transform.GetChild(i).GetChild(3).gameObject.GetComponent<Text>().text = EmployList[i].BaseInfo.LaborForce.ToString();
        }

        for(int i = 1; i < ItemCarrier.transform.childCount; i++)
        {
            if(!ItemCarrier.transform.GetChild(i).gameObject.activeSelf) Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }
    }

    public void SelectWorkerItem(GameObject TargetObject)
    {
        int SelectedWorkerIndex = CallEmployeeValue.GetEmployeeIndex(int.Parse(TargetObject.transform.GetChild(0).gameObject.GetComponent<Text>().text));
        
        if(CurrentWorkerIndex != SelectedWorkerIndex)
        {
            DisplayInfo(SelectedWorkerIndex);
        }
    }

    public void DisplayInfo(int Index)
    {
        CurrentWorkerIndex = Index;

        SetHappinessImoji();
        NameTextObject.GetComponent<Text>().text = CallEmployeeValue.EmployeeList[Index].BaseInfo.Name;
        AgeTextObject.GetComponent<Text>().text = "Test";
        PayTextObject.GetComponent<Text>().text = CallEmployeeValue.EmployeeList[Index].BaseInfo.Salary.ToString();
        AbilityTextObject.GetComponent<Text>().text = " x " + CallEmployeeValue.EmployeeList[Index].BaseInfo.LaborForce.ToString();

        FatigueGraphBar.gameObject.GetComponent<Image>().fillAmount = CallEmployeeValue.EmployeeList[Index].FatigueValue / CallEmployeeValue.EmployeeList[Index].BaseInfo.LaborForce;
        ExperienceGraphBar.gameObject.GetComponent<Image>().fillAmount = CallEmployeeValue.EmployeeList[Index].Experience / (CallEmployeeValue.EmployeeList[Index].BaseInfo.LaborForce * 10);

        if(ExperienceGraphBar.GetComponent<Image>().fillAmount >= 1f) PromoteButton.GetComponent<Button>().interactable = true;
        else PromoteButton.GetComponent<Button>().interactable = false;
        FireButton.GetComponent<Button>().interactable = true;
    }

    void SetHappinessImoji()
    {
        string EmojiSpritePath = "GameSystem/InGameUI/Sprite/";
        if(CallEmployeeValue.EmployeeList[CurrentWorkerIndex].Happiness > 0.9f) EmojiSpritePath += "ImojiHappy";
        else if(CallEmployeeValue.EmployeeList[CurrentWorkerIndex].Happiness > 0.7f) EmojiSpritePath += "ImojiGood";
        else if(CallEmployeeValue.EmployeeList[CurrentWorkerIndex].Happiness > 0.4f) EmojiSpritePath += "ImojiNormal";
        else if(CallEmployeeValue.EmployeeList[CurrentWorkerIndex].Happiness > 0.1f) EmojiSpritePath += "ImojiBad";
        else EmojiSpritePath += "ImojiAngry";

        WorkerImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(EmojiSpritePath);
        WorkerImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
    }

    public void SelectWorkerPromote()
    {
        CallEmployeeValue.PromoteEmployee(CurrentWorkerIndex);

        DisplayWorkerList();
    }

    public void SelectWorkerFire()
    {
        CallEmployeeValue.FireEmployee(CurrentWorkerIndex);
    }

    public void UpdateInfoPanel()
    {
        SetHappinessImoji();
        
        AgeTextObject.GetComponent<Text>().text = "Test";
        PayTextObject.GetComponent<Text>().text = CallEmployeeValue.EmployeeList[CurrentWorkerIndex].BaseInfo.Salary.ToString();
        AbilityTextObject.GetComponent<Text>().text = " x " + CallEmployeeValue.EmployeeList[CurrentWorkerIndex].BaseInfo.LaborForce.ToString();

        FatigueGraphBar.GetComponent<Image>().fillAmount = CallEmployeeValue.EmployeeList[CurrentWorkerIndex].FatigueValue / CallEmployeeValue.EmployeeList[CurrentWorkerIndex].BaseInfo.LaborForce;
        ExperienceGraphBar.GetComponent<Image>().fillAmount = CallEmployeeValue.EmployeeList[CurrentWorkerIndex].Experience / (CallEmployeeValue.EmployeeList[CurrentWorkerIndex].BaseInfo.LaborForce * 10);

        if(ExperienceGraphBar.GetComponent<Image>().fillAmount >= 1f) PromoteButton.GetComponent<Button>().interactable = true;
        else PromoteButton.GetComponent<Button>().interactable = false;
    }

    public void ClearInfoPanel()
    {
        WorkerImageObject.GetComponent<Image>().sprite = null;
        WorkerImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,0);

        NameTextObject.GetComponent<Text>().text = "";
        AgeTextObject.GetComponent<Text>().text = "";
        PayTextObject.GetComponent<Text>().text = "";
        AbilityTextObject.GetComponent<Text>().text = "";
    
        FatigueGraphBar.GetComponent<Image>().fillAmount = 0f;
        ExperienceGraphBar.GetComponent<Image>().fillAmount = 0f;
        
        PromoteButton.GetComponent<Button>().interactable = false;
        FireButton.GetComponent<Button>().interactable = false;

        CurrentWorkerIndex = -1;
    }

    public void ClosePanel()
    {
        ClearInfoPanel();
    }
}
