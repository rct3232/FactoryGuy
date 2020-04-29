using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallableObjectAct : MonoBehaviour
{
    public TechRecipe.FacilityInfo Info;
    public FacilityValue.FacilityInfo Value;
    public bool isInstall;
    public bool isBelt;
    GameObject BaseSystem;
    InGameValue ValueCall;
    PanelController PanelControllerCall;
    ClickChecker ClickCheckerCall;
    GroupActivation GroupActivationCall;
    TimeManager TimeManagerCall;
    EconomyValue EconomyValueCall;
    NotificationManager NotificationManagerCall;
    public CompanyValue CompanyValueCall;
    public bool CanInstall;
    public GameObject StructObject;
    public GameObject ObjectDetector;
    public int HeightLevel = 0;
    public float WorkSpeed;
    public bool IsWorking;
    List<GameObject> WholeStructObject = new List<GameObject>();
    List<Color> OriginalStructColor = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        isInstall = false;
        BaseSystem = GameObject.Find("BaseSystem");
        ValueCall = BaseSystem.GetComponent<InGameValue>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        ClickCheckerCall = BaseSystem.GetComponent<ClickChecker>();
        GroupActivationCall = BaseSystem.GetComponent<GroupActivation>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        EconomyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetEconomyValue().GetComponent<EconomyValue>();
        NotificationManagerCall = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        WorkSpeed = 1f;
        IsWorking = false;
        
        GetWholeStructObject();
        GetOriginalStructColor();
        CheckInstallCondition();
    }

    // Update is called once per frame
    void Update()
    {
        ObjectAct();
    }

    void GetWholeStructObject()
    {
        int StartIndex = 0;
        if(isBelt)
        {
            Transform BeltCarrier = transform.GetChild(0).GetChild(0);
            for(int i = 0; i < BeltCarrier.childCount; i++)
            {
                WholeStructObject.Add(BeltCarrier.GetChild(i).GetChild(0).GetChild(0).gameObject);
            }

            if(transform.GetChild(0).childCount > 0)
            {
                StartIndex = 1;
            }
            else
            {
                StartIndex = -1;
            }
        }

        if(StartIndex != -1)
        {
            for(int i = StartIndex; i < transform.GetChild(0).childCount; i++)
            {
                WholeStructObject.Add(transform.GetChild(0).GetChild(i).gameObject);
            }
        }
    }

    void GetOriginalStructColor()
    {
        foreach(var Struct in WholeStructObject)
        {
            OriginalStructColor.Add(Struct.GetComponent<Renderer>().material.color);
        }
    }

    void ObjectAct()
    {
        if (!isInstall)
        {
            if(ValueCall.AttachedOnMouse != gameObject)
            {
                ValueCall.AttachedOnMouse = gameObject;
            }

            if(Info.Type != "Belt" && Info.Type != "VerticalBelt")
            {
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    Destroy(this.gameObject);
                    return;
                }
                if(Input.GetKeyDown(KeyCode.R))
                {
                    this.gameObject.transform.Rotate(new Vector3(0, 90, 0));
                }

                if (ClickCheckerCall.target != null)
                {
                    if (ClickCheckerCall.target.layer == 9)
                    {
                        //Debug.Log("You Hovering " + ClickCheckerCall.target.name);
                        this.gameObject.transform.position = ClickCheckerCall.target.transform.position + new Vector3(0,(1 + HeightLevel * 2),0);
                        if (InstallConditionCheck())
                        {
                            ChangeStructColor(new Color(0.5f, 0.5f, 1f));
                            NotificationManagerCall.SetNote("$ " + Info.Price, new Color(1f,0.2f,0.2f));

                            if (Input.GetMouseButtonDown(0))
                            {
                                Installation();
                            }
                        }
                        else
                        {
                            ChangeStructColor(new Color(1f, 0.5f, 0.5f));
                            if(!GroupActivationCall.CheckActivatedGroup(ClickCheckerCall.target) ||
                            !GroupActivationCall.CheckInGroup(ClickCheckerCall.target, ObjectDetector.transform.localScale, transform.eulerAngles))
                            {
                                NotificationManagerCall.SetNote("You cannot place here! Out of your factory.", new Color(1f,0.2f,0.2f));
                            }
                            else if(ObjectDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                            {
                                NotificationManagerCall.SetNote("You cannot place here! Something is already here.", new Color(1f,0.2f,0.2f));
                            }
                            else if(EconomyValueCall.Balance < Info.Price)
                            {
                                NotificationManagerCall.SetNote("You cannot place here! Not enough money.", new Color(1f,0.2f,0.2f));
                            }
                            else
                            {
                                NotificationManagerCall.SetNote("You cannot place here! Wrong direction.", new Color(1f,0.2f,0.2f));
                            }
                        }
                    }
                }
            }
        }
        else
        {
            CheckWorkSpeed();
            if(ClickCheckerCall.target == StructObject)
            {
                // Debug.Log("Mouse is on " + gameObject.transform.name + " " + gameObject.layer);
                if(ValueCall.ModeBit[1])
                {
                    ChangeStructColor(new Color(1f, 0.5f, 0.5f));
                    if (Input.GetMouseButtonDown(0)) ObjectDelete();
                }
                else
                {
                    ChangeStructColor(new Color(0.5f, 0.5f, 0.5f));
                    if (Input.GetMouseButtonDown(0)) ObjectInteraction();                        
                }
            }
            else
            {
                if(IsWorking)
                {
                    if(WorkSpeed <= 0.5f)
                    {
                        ChangeStructColor(new Color(WorkSpeed,WorkSpeed,WorkSpeed,1f));
                    }
                    else
                    {
                        ChangeStructColor(new Color(0,0,0,0));
                    }
                }
                else
                {
                    ChangeStructColor(new Color(1f,1f,0,1f));
                }
            }
        }
    }

    public bool InstallConditionCheck()
    {
        if (CanInstall && ObjectDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject == null &&
            GroupActivationCall.CheckActivatedGroup(ClickCheckerCall.target) &&
            GroupActivationCall.CheckInGroup(ClickCheckerCall.target, ObjectDetector.transform.localScale, transform.eulerAngles) &&
            EconomyValueCall.Balance >= Info.Price) return true;
        else return false;
    }
    
    public bool Installation()
    {
        if(isInstall) return false;

        gameObject.name = "#" + (CompanyValueCall.GetFacilityValue().GetComponent<FacilityValue>().InstalledFacilityAmount + 1).ToString() + " " + Info.Type;

        ValueCall.AttachedOnMouse = null;
        isInstall = true;
        transform.parent = GameObject.Find("InstalledObject").transform;
        ChangeStructColor(new Color(0,0,0,0));
        
        Value = CompanyValueCall.GetFacilityValue().GetComponent<FacilityValue>().AddFacilityInfo(gameObject);

        if(Info.Type != "Belt" && Info.Type != "VerticalBelt") GameObject.Find("ObjectInstaller").GetComponent<ObjInstantiater>().InstantiateNewObject(Info.Name);

        return true;
    }

    public BeltAct GetBeltAct()
    {
        BeltAct Result = null;

        if(Info.Type == "Belt") Result = transform.GetChild(1).GetChild(0).gameObject.GetComponent<BeltAct>();

        return Result;
    }

    void CheckWorkSpeed()
    {
        float SuppliedElectricRatio = 0f;
        float SuppliedEmployeeRatio = 0f;

        if(Info.ElectricConsum == 0)
        {
            SuppliedElectricRatio = 1f;
        }
        else
        {
            SuppliedElectricRatio = Value.SuppliedElectricity / Info.ElectricConsum;
        }

        if(Info.LaborRequirement == 0)
        {
            SuppliedEmployeeRatio = 1f;
        }
        else
        {
            SuppliedEmployeeRatio = Value.SuppliedLabor / Info.LaborRequirement;
            if(SuppliedEmployeeRatio == 0)
            {
                SuppliedEmployeeRatio = 0.1f;
            }
        }

        WorkSpeed = SuppliedElectricRatio * SuppliedEmployeeRatio;
    }

    void ObjectInteraction()
    {
        PanelControllerCall.DisplayFloatingPanel("ObjectInfoPanel", gameObject);
    }

    public void ChangeStructColor(Color color)
    {
        WholeStructObject.Clear();
        GetWholeStructObject();
        int limit = Mathf.Min(WholeStructObject.Count, OriginalStructColor.Count);
        if(color == new Color(0,0,0,0))
        {
            for(int i = 0; i < limit; i++)
            {
                WholeStructObject[i].GetComponent<Renderer>().material.color = OriginalStructColor[i];
            }
        }
        else
        {
            for(int i = 0; i < limit; i++)
            {
                WholeStructObject[i].GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, OriginalStructColor[i].a);
            }
        }
    }

    void CheckInstallCondition()
    {
        switch (StructObject.layer)
        {
            case 11:
                CanInstall = false;
                break;
            case 18:
                CanInstall = false;
                break;
            case 19:
                CanInstall = false;
                break;
            case 24:
                CanInstall = false;
                break;
            default:
                CanInstall = true;
                break;
        }
    }

    public void ObjectDelete()
    {
        bool CanDelete = false;
        switch (StructObject.layer)
        {
            case 11: //GoodsCreator
                CanDelete = this.GetComponent<GoodsCreater>().DeleteObject();
                break;
            case 23: //Belt
                CanDelete = transform.GetChild(1).GetChild(0).GetComponent<BeltAct>().DeteleBelt();
                break;
            case 16: //Processor
                CanDelete = this.GetComponent<ProcessorAct>().DeleteObject();
                break;
            case 17: //Warehouse
                CanDelete = this.GetComponent<WarehouseObjectAct>().DeleteObject();
                break;
            case 18: //GoodsLoader
                CanDelete = this.GetComponent<GoodsLoaderAct>().DeleteObject();
                break;
            case 19: //Distributor
                CanDelete = this.GetComponent<DistributorAct>().DeleteObject();
                break;
            case 20: //QCU
                CanDelete = this.GetComponent<QCUAct>().DeleteObject();
                break;
            case 21: //Destoryer
                CanDelete = this.GetComponent<DestroyerAct>().DeleteObject();
                break;
            case 22: //Labatory
                CanDelete = this.GetComponent<LabatoryAct>().DeleteObject();
                break;
            case 24: //VerticalBelt
                CanDelete = this.GetComponent<VerticlaBeltAct>().DeleteObject();
                break;
            case 25: //EnergyStorage
                CanDelete = this.GetComponent<EnergyStorageAct>().DeleteObject();
                break;
            case 26: //DayRoom
                CanDelete = this.GetComponent<DayRoomAct>().DeleteObject();
                break;
            case 27: //EnergySupplier
                CanDelete = this.GetComponent<EnergySupplierAct>().DeleteObject();
                break;
            default:
                Debug.Log("Unknown Object Type");
                break;
        }

        if(CanDelete)
        {
            CompanyValueCall.GetFacilityValue().GetComponent<FacilityValue>().DeleteFacilityInfo(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Object delete was rejected");
        }
    }
}
