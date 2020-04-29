using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAct : MonoBehaviour
{
    public InstallableObjectAct ObjectActCall;
    public bool isEnd;
    Vector3[] GoodsVelocity;
    public List<GameObject> BeltInstallList = new List<GameObject>();
    public GameObject VerticalBelt = null;
    public int[] BeltInstallFirstPos = new int[2];
    public int DirectionSign = 0;
    public int Amount = 0;
    public int VerticalDistance = 0;
    bool InstallStandBy = false;
    public GameObject GoodsOnBelt;
    public GameObject GoodsOnExit;
    public GameObject NextBelt;
    public GameObject PrevBelt;
    public GameObject ModuleDetector;
    GameObject DetectedObject;
    GameObject FrontDetector;
    List<GameObject> RearDetector = new List<GameObject>();
    public int BeltDirection = -1;
    public bool NeedStop = false;
    public bool isStop = false;
    public bool isDummyBelt = false;
    public int BeltIndex;
    public int NextBeltDirection;
    public float BeltSpeed;
    public bool ModuleCondtion;
    GameObject BaseSystem;
    InGameValue InGameValueCall;
    ClickChecker ClickCheckerCall;
    GroupActivation GroupActivationCall;
    GoodsValue GoodsValueCall;
    TimeManager TimeManagerCall;
    ObjInstantiater ObjInstantiaterCall;
    GameObject StructCarrier;
    GameObject ObjectInstaller;
    public GameObject ParentObject;

    // Start is called before the first frame update
    void Awake()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        InGameValueCall = BaseSystem.GetComponent<InGameValue>();
        ClickCheckerCall = BaseSystem.GetComponent<ClickChecker>();
        GroupActivationCall = BaseSystem.GetComponent<GroupActivation>();
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        ObjectInstaller = GameObject.Find("ObjectInstaller");
        ObjInstantiaterCall = ObjectInstaller.GetComponent<ObjInstantiater>();

        PrevBelt = null;
        ModuleCondtion = true;
        ParentObject = transform.parent.parent.gameObject;
        ObjectActCall = ParentObject.GetComponent<InstallableObjectAct>();
        int Detectorindex = 0;
        ModuleDetector = transform.GetChild(Detectorindex++).gameObject;
        FrontDetector = transform.GetChild(Detectorindex++).gameObject;
        for(int i = 0; Detectorindex < transform.childCount; i++)
        {
            RearDetector.Add(transform.GetChild(Detectorindex++).gameObject);
        }
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i).gameObject == gameObject)
            {
                BeltIndex = i;
            }
        }

        CheckDirection();

        if(!isDummyBelt) StructCarrier = ParentObject.transform.GetChild(0).GetChild(0).GetChild(BeltIndex).gameObject;

        NextBeltDirection = -1;
        GoodsVelocity = new Vector3[4];
        float CurrentBeltSpeed = (float)TimeManagerCall.PlaySpeed * ObjectActCall.WorkSpeed * BeltSpeed;
        for (int i = 0; i< 4; i++)
        {
            switch (i)
            {
                case 0:
                    GoodsVelocity[i] = new Vector3(-CurrentBeltSpeed, 0, 0);
                    break;
                case 1:
                    GoodsVelocity[i] = new Vector3(0, 0, CurrentBeltSpeed);
                    break;
                case 2:
                    GoodsVelocity[i] = new Vector3(CurrentBeltSpeed, 0, 0);
                    break;
                case 3:
                    GoodsVelocity[i] = new Vector3(0, 0, -CurrentBeltSpeed);
                    break;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!ObjectActCall.isInstall)
        {
            if(!InstallStandBy) BeltInstall();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.IsWorking)
        {
            DetectPlaySpeedChange();
            GetGoodsOnBelt();

            if(GoodsOnBelt != null)
            {
                if(GoodsValueCall.CheckMovingState(int.Parse(GoodsOnBelt.name)) == 1)
                {
                    if(PrevBelt != null)
                    {
                        int PrevBeltDirection = PrevBelt.GetComponent<BeltAct>().BeltDirection;
                        if(isCenter(PrevBeltDirection))
                        {
                            GoodsOnBelt.GetComponent<Rigidbody>().velocity = GoodsVelocity[BeltDirection];
                        }
                        else
                        {
                            GoodsOnBelt.GetComponent<Rigidbody>().velocity = GoodsVelocity[PrevBeltDirection];
                        }
                    }
                    else
                    {
                        GoodsOnBelt.GetComponent<Rigidbody>().velocity = GoodsVelocity[BeltDirection];
                    }
                }

                if(!NeedStop && GoodsOnExit != GoodsOnBelt)
                {
                    MovingGoods();
                }
                else
                {
                    if(GoodsValueCall.CheckMovingState(int.Parse(GoodsOnBelt.name)) == 1 && GoodsOnExit != GoodsOnBelt)
                    {
                        CenterStop();
                    }
                }
            }
        }
    }

    void DetectPlaySpeedChange()
    {
        float CurrentBeltSpeed = (float)TimeManagerCall.PlaySpeed * ObjectActCall.WorkSpeed * BeltSpeed; 
        if(CurrentBeltSpeed != GoodsVelocity[2].x)
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        GoodsVelocity[i] = new Vector3(-CurrentBeltSpeed, 0, 0);
                        break;
                    case 1:
                        GoodsVelocity[i] = new Vector3(0, 0, CurrentBeltSpeed);
                        break;
                    case 2:
                        GoodsVelocity[i] = new Vector3(CurrentBeltSpeed, 0, 0);
                        break;
                    case 3:
                        GoodsVelocity[i] = new Vector3(0, 0, -CurrentBeltSpeed);
                        break;
                }
            }
        }
    }

    public void BeltInstall()
    {
        DetectedObject = ObjectActCall.ObjectDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;

        if(Input.GetMouseButtonUp(0))
        {
            ExcuteInstallation();

            return;
        }

        if(Input.GetMouseButton(1))
        {
            if(BeltInstallList.Count > 0)
            {
                ResetInstallList();
            }
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            if(BeltInstallList.Count > 0)
            {
                ResetInstallList();
            }
            else
            {
                ResetInstallList();
                ObjectActCall.ObjectDelete();
                return;
            }
        }

        if(BeltInstallList.Count == 0)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetInstallList();
                ParentObject.transform.Rotate(new Vector3(0, 90, 0));
            }
            CheckDirection();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if(ObjectActCall.HeightLevel == 0)
            {
                if(InstallVerticalBelt())
                {
                    ObjectActCall.HeightLevel = 1;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(ObjectActCall.HeightLevel == 1)
            {
                if(InstallVerticalBelt())
                {
                    ObjectActCall.HeightLevel = 0;
                }
            }
        }

        if (CheckInFactory())
        {
            if(BeltInstallList.Count <= 0)
            {
                ParentObject.transform.position = ClickCheckerCall.target.transform.position + new Vector3(0,(1 + ObjectActCall.HeightLevel * 2),0);
                if(DetectedObject != null)
                {
                    if(DetectedObject.GetComponent<InstallableObjectAct>() != null)
                    {
                        if(DetectedObject.GetComponent<InstallableObjectAct>().Info.Type == "Belt")
                        {
                            ObjectActCall.ChangeStructColor(new Color(0.5f, 0.5f, 1f));
                            ObjectActCall.CanInstall = true;
                            if(Input.GetMouseButtonDown(0))
                            {
                                BeltInstallFirstPos = GroupActivationCall.GetTilePos(ClickCheckerCall.target);
                                BeltInstallList.Add(DetectedObject);
                                InstallStandBy = true;

                                InGameValueCall.AttachedOnMouse = null;
                                GameObject NextBelt = ObjInstantiaterCall.InstantiateNewObject(ObjectActCall.Info.Name);
                                BeltAct NextBeltAct = NextBelt.GetComponent<InstallableObjectAct>().GetBeltAct();
                                NextBelt.transform.rotation = ParentObject.transform.rotation;
                                NextBelt.GetComponent<InstallableObjectAct>().HeightLevel = ObjectActCall.HeightLevel;
                                NextBeltAct.BeltDirection = BeltDirection;
                                NextBeltAct.BeltInstallList = BeltInstallList;
                                NextBeltAct.BeltInstallFirstPos = BeltInstallFirstPos;

                                ObjectActCall.ObjectDelete();

                                return;
                            }
                        }
                        else
                        {
                            // Something is already here
                            ObjectActCall.ChangeStructColor(new Color(1f, 0.5f, 0.5f));
                            ObjectActCall.CanInstall = false;
                        }
                    }
                    else
                    {
                        // Something is already here
                        ObjectActCall.ChangeStructColor(new Color(1f, 0.5f, 0.5f));
                        ObjectActCall.CanInstall = false;
                    }
                }
                else
                {
                    ObjectActCall.ChangeStructColor(new Color(0.5f, 0.5f, 1f));
                    ObjectActCall.CanInstall = true;
                    if(Input.GetMouseButtonDown(0))
                    {
                        BeltInstallFirstPos = GroupActivationCall.GetTilePos(ClickCheckerCall.target);
                        BeltInstallList.Add(ParentObject);
                        InstallStandBy = true;

                        InGameValueCall.AttachedOnMouse = null;
                        GameObject NextBelt = ObjInstantiaterCall.InstantiateNewObject(ObjectActCall.Info.Name);
                        BeltAct NextBeltAct = NextBelt.GetComponent<InstallableObjectAct>().GetBeltAct();
                        NextBelt.transform.rotation = ParentObject.transform.rotation;
                        NextBelt.GetComponent<InstallableObjectAct>().HeightLevel = ObjectActCall.HeightLevel;
                        NextBeltAct.BeltDirection = BeltDirection;
                        NextBeltAct.BeltInstallList = BeltInstallList;
                        NextBeltAct.BeltInstallFirstPos = BeltInstallFirstPos;

                        return;
                    }
                }
            }
            else
            {
                if(Input.GetMouseButton(0))
                {
                    ObjectActCall.CanInstall = true;
                    AppendInstallList();
                }
            }
        }
        else
        {
            ObjectActCall.ChangeStructColor(new Color(1f, 0.5f, 0.5f));
            ObjectActCall.CanInstall = false;
        }

        GameObject ContactBelt = ParentObject;
        if(DetectedObject != null)
        {
            if(DetectedObject.GetComponent<InstallableObjectAct>() != null)
            {
                InstallableObjectAct TargetObjectAct = DetectedObject.GetComponent<InstallableObjectAct>();
                if(TargetObjectAct.Info.Type == "Belt")
                {
                    ContactBelt = DetectedObject;
                }
            }
        }

        List<GameObject> ActivatedBeltList = new List<GameObject>();
        for(int i = 0; i < BeltInstallList.Count - 1; i++)
        {
            if(BeltInstallList[i].activeSelf) ActivatedBeltList.Add(BeltInstallList[i]);
        }
        ActivatedBeltList.Add(ContactBelt);


        if(ActivatedBeltList.Count > 0)
        {
            int ExistBeltCount = 0;
            if(ActivatedBeltList[0].GetComponent<InstallableObjectAct>().isInstall) ExistBeltCount++;
            if(ActivatedBeltList[ActivatedBeltList.Count - 1].GetComponent<InstallableObjectAct>().isInstall) ExistBeltCount++;

            if(ObjectActCall.CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().Balance < ObjectActCall.Info.Price * (ActivatedBeltList.Count - ExistBeltCount)) ObjectActCall.CanInstall = false;

            if(ObjectActCall.CanInstall)
            {
                for(int i = 1; i < ActivatedBeltList.Count - 1; i++)
                {
                    InstallableObjectAct TargetObjectAct = BeltInstallList[i].GetComponent<InstallableObjectAct>();
                    if(!TargetObjectAct.InstallConditionCheck())
                    {
                        ObjectActCall.CanInstall = false;
                        break;
                    }
                }
            }

            for(int i = 0; i < ObjectInstaller.transform.childCount; i++)
            {
                GameObject TargetObject = ObjectInstaller.transform.GetChild(i).gameObject;
                if(TargetObject.GetComponent<InstallableObjectAct>().isInstall) continue;

                InstallableObjectAct TargetObjectAct = TargetObject.GetComponent<InstallableObjectAct>();
                if(ObjectActCall.CanInstall) TargetObjectAct.ChangeStructColor(new Color(0.5f, 0.5f, 1f));
                else TargetObjectAct.ChangeStructColor(new Color(1f, 0.5f, 0.5f));
            }
        }
    }

    bool CheckInFactory()
    {
        if (ClickCheckerCall.target != null)
        {
            if (ClickCheckerCall.target.layer == 9)
            {
                if(GroupActivationCall.CheckActivatedGroup(ClickCheckerCall.target) && GroupActivationCall.CheckInGroup(ClickCheckerCall.target, ObjectActCall.ObjectDetector.transform.localScale, transform.eulerAngles))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    void AppendInstallList()
    {
        int[] TilePos = GroupActivationCall.GetTilePos(ClickCheckerCall.target);
        int Distance = 0;
        int PositionSign;
        string Axis = null;
        int[] NewBeltPos;

        if(VerticalBelt != null) VerticalDistance = 1;

        switch(BeltDirection)
        {
            case 0 : case 2:
            Axis = "x";
            Distance = TilePos[0] - BeltInstallFirstPos[0];
            break;
            case 1 : case 3:
            Axis = "y";
            Distance = TilePos[1] - BeltInstallFirstPos[1];
            break;
        }

        Amount = Mathf.Abs(Distance);

        if(Mathf.FloorToInt(BeltDirection / 2) + (BeltDirection % 2) == 1)
        {
            if(Distance > 0) DirectionSign = -1;
            else DirectionSign = 1;
        }
        else
        {
            if(Distance >= 0) DirectionSign = 1;
            else DirectionSign = -1;
        }

        if(DirectionSign != 1 && DirectionSign != -1) return;

        if(Distance > 0) PositionSign = 1;
        else PositionSign = -1;

        GameObject ContactBelt = ParentObject;
        if(DetectedObject != null && DetectedObject != BeltInstallList[0])
        {
            if(DetectedObject.GetComponent<InstallableObjectAct>() != null)
            {
                InstallableObjectAct TargetObjectAct = DetectedObject.GetComponent<InstallableObjectAct>();
                if(TargetObjectAct.Info.Type == "Belt")
                {
                    ContactBelt = DetectedObject;
                }
            }
        }

        if(ContactBelt == ParentObject)
        {
            ParentObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ParentObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(BeltInstallList[BeltInstallList.Count - 1] != ParentObject) BeltInstallList.Add(ParentObject);

        for(int i = 0; i < BeltInstallList.Count; i++)
        {
            BeltAct TargetBeltAct = BeltInstallList[i].GetComponent<InstallableObjectAct>().GetBeltAct();
            if(DirectionSign == 1)
            {
                if(TargetBeltAct.NextBelt != null)
                {
                    BeltAct TargetBeltNextAct = TargetBeltAct.NextBelt.GetComponent<BeltAct>();
                    if(TargetBeltNextAct.PrevBelt != null) if(!TargetBeltNextAct.PrevBelt.GetComponent<BeltAct>().ObjectActCall.isInstall) TargetBeltNextAct.PrevBelt = null;
                    if(!TargetBeltNextAct.ObjectActCall.isInstall) TargetBeltAct.NextBelt = null;
                }
            }
            else
            {
                if(TargetBeltAct.PrevBelt != null)
                {
                    BeltAct TargetBeltPrevAct = TargetBeltAct.PrevBelt.GetComponent<BeltAct>();
                    if(TargetBeltPrevAct.NextBelt != null) if(!TargetBeltPrevAct.NextBelt.GetComponent<BeltAct>().ObjectActCall.isInstall) TargetBeltPrevAct.NextBelt = null;
                    if(!TargetBeltPrevAct.ObjectActCall.isInstall) TargetBeltAct.PrevBelt = null;
                }
            }

            if(i != 0 && i != BeltInstallList.Count - 1) BeltInstallList[i].SetActive(false);
        }

        if(Axis == "x") NewBeltPos = new int[2] {BeltInstallFirstPos[0] + (Amount * PositionSign), BeltInstallFirstPos[1]};
        else NewBeltPos = new int[2] {BeltInstallFirstPos[0], BeltInstallFirstPos[1] + (Amount * PositionSign)};
        ParentObject.transform.position = GroupActivationCall.GetTilePhysicsPos(NewBeltPos) + new Vector3(0,(1 + ObjectActCall.HeightLevel * 2),0);

        if(Amount + VerticalDistance == 0)
        {
            InGameValueCall.AttachedOnMouse = ParentObject;

            return;
        }

        List<GameObject> ActivatedBeltList = new List<GameObject>();
        ActivatedBeltList.Add(BeltInstallList[0]);

        for(int i = 1; i < Amount + VerticalDistance; i++)
        {
            if(Axis == "x") NewBeltPos = new int[2] {BeltInstallFirstPos[0] + ((i - VerticalDistance) * PositionSign), BeltInstallFirstPos[1]};
            else NewBeltPos = new int[2] {BeltInstallFirstPos[0], BeltInstallFirstPos[1] + ((i - VerticalDistance) * PositionSign)};            

            GameObject TargetBelt;
            if(i >= BeltInstallList.Count - 1)
            {
                InGameValueCall.AttachedOnMouse = null;
                TargetBelt = ObjInstantiaterCall.InstantiateNewObject(ObjectActCall.Info.Name);

                BeltInstallList.Insert(i, TargetBelt);
            }
            else
            {
                BeltInstallList[i].SetActive(true);
                TargetBelt = BeltInstallList[i];
            }
            InstallableObjectAct TargetBeltObjectAct = TargetBelt.GetComponent<InstallableObjectAct>();
            BeltAct TargetBeltAct = TargetBelt.GetComponent<InstallableObjectAct>().GetBeltAct();

            TargetBeltAct.BeltDirection = BeltDirection;
            TargetBeltObjectAct.HeightLevel = ObjectActCall.HeightLevel;
            TargetBelt.transform.position = GroupActivationCall.GetTilePhysicsPos(NewBeltPos) + new Vector3(0,(1 + TargetBeltObjectAct.HeightLevel * 2),0);
            TargetBelt.transform.rotation = ParentObject.transform.rotation;
            TargetBeltAct.InstallStandBy = true;
            
            TargetBeltAct.BeltInstallList = BeltInstallList;
            TargetBeltAct.BeltInstallFirstPos = BeltInstallFirstPos;

            ActivatedBeltList.Add(TargetBelt);
        }

        ActivatedBeltList.Add(ContactBelt);

        BeltAct FirstBeltAct, LastBeltAct;
        if(DirectionSign == 1)
        {
            FirstBeltAct = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct();
            LastBeltAct = ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct();
        }
        else
        {
            FirstBeltAct = ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct();
            LastBeltAct = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct();
        }

        if(FirstBeltAct.ObjectActCall.isInstall)
        {
            if(FirstBeltAct.NextBelt != null)
            {
                if(FirstBeltAct.NextBelt.GetComponent<BeltAct>().ObjectActCall.isInstall && !FirstBeltAct.isEnd)
                {
                    ObjectActCall.CanInstall = false;
                }
            }
            if(FirstBeltAct.PrevBelt != null)
            {
                if(Mathf.Abs(FirstBeltAct.BeltDirection - FirstBeltAct.PrevBelt.GetComponent<BeltAct>().BeltDirection) == 2)
                {
                    ObjectActCall.CanInstall = false;
                }
            }
        }
        if(LastBeltAct.ObjectActCall.isInstall)
        {
            if(LastBeltAct.PrevBelt != null)
            {
                if(LastBeltAct.PrevBelt.GetComponent<BeltAct>().ObjectActCall.isInstall && !LastBeltAct.PrevBelt.GetComponent<BeltAct>().isEnd)
                {
                    ObjectActCall.CanInstall = false;
                }
            }
            if(LastBeltAct.NextBelt != null)
            {
                if(Mathf.Abs(LastBeltAct.BeltDirection - LastBeltAct.NextBelt.GetComponent<BeltAct>().BeltDirection) == 2)
                {
                    ObjectActCall.CanInstall = false;
                }
            }
        }

        if(ObjectActCall.CanInstall)
        {
            if(DirectionSign == 1)
            {
                ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                ActivatedBeltList[1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);

                ActivatedBeltList[ActivatedBeltList.Count - 2].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[ActivatedBeltList.Count - 1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                ActivatedBeltList[ActivatedBeltList.Count - 1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[ActivatedBeltList.Count - 2].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);

                ActivatedBeltList[0].transform.rotation = ParentObject.transform.rotation;
                ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().CheckDirection();
            }
            else
            {
                ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                ActivatedBeltList[1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);

                ActivatedBeltList[ActivatedBeltList.Count - 2].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[ActivatedBeltList.Count - 1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                ActivatedBeltList[ActivatedBeltList.Count - 1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[ActivatedBeltList.Count - 2].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                
                ActivatedBeltList[0].transform.rotation = ParentObject.transform.rotation;
                ActivatedBeltList[0].GetComponent<InstallableObjectAct>().GetBeltAct().CheckDirection();
            }
        }
        
        InGameValueCall.AttachedOnMouse = ParentObject;
    }

    bool InstallVerticalBelt()
    {
        if(BeltInstallList.Count <= 0) return true;

        if(VerticalBelt == null)
        {
            InGameValueCall.AttachedOnMouse = null;
            VerticalBelt = ObjInstantiaterCall.InstantiateNewObject("Elevator");
            InGameValueCall.AttachedOnMouse = ParentObject;

            if(VerticalBelt == null) return false;
            
            VerticalBelt.transform.position = BeltInstallList[0].transform.position + new Vector3(0,ObjectActCall.HeightLevel * -2,0);
            VerticalBelt.transform.rotation = BeltInstallList[0].transform.rotation;

            return true;
        }
        else
        {
            VerticalBelt.GetComponent<InstallableObjectAct>().ObjectDelete();
            VerticalBelt = null;

            return true;
        }
    }

    void ResetInstallList()
    {
        if(BeltInstallList.Count == 0) return;

        for(int i = BeltInstallList.Count - 2; i >= 0; i--)
        {
            GameObject Target = BeltInstallList[i];
            BeltAct TargetBeltAct = Target.GetComponent<InstallableObjectAct>().GetBeltAct();

            if(!TargetBeltAct.ObjectActCall.isInstall)
            {
                Target.SetActive(true);
                Target.GetComponent<InstallableObjectAct>().ObjectDelete();
            }
            else
            {
                if(DirectionSign == 1)
                {
                    if(TargetBeltAct.NextBelt != null)
                    {
                        if(TargetBeltAct.isEnd)
                        {
                            TargetBeltAct.NextBelt.GetComponent<BeltAct>().ChangePrevBelt(null);
                            TargetBeltAct.ChangeNextBelt(null);
                        }
                    }
                    else TargetBeltAct.ChangeNextBelt(null);
                }
                else
                {
                    if(TargetBeltAct.PrevBelt != null)
                    {
                        if(TargetBeltAct.PrevBelt.GetComponent<BeltAct>().isEnd)
                        {
                            TargetBeltAct.PrevBelt.GetComponent<BeltAct>().ChangeNextBelt(null);
                            TargetBeltAct.ChangePrevBelt(null);
                        }
                    }
                    else TargetBeltAct.ChangePrevBelt(null);
                }
            }
        }

        ParentObject.transform.GetChild(0).gameObject.SetActive(true);

        BeltInstallList = new List<GameObject>();
        BeltInstallFirstPos = new int[2];
        
        InGameValueCall.AttachedOnMouse = ParentObject;

        if(VerticalBelt != null)
        {
            VerticalBelt.GetComponent<InstallableObjectAct>().ObjectDelete();
            VerticalBelt = null;
        }
    }

    void ExcuteInstallation()
    {
        if(BeltInstallList.Count <= 0) return;
        
        if(!ObjectActCall.CanInstall)
        {
            ResetInstallList();
            return;
        }

        GameObject ContactBelt = ParentObject;
        if(DetectedObject != null)
        {
            if(DetectedObject.GetComponent<InstallableObjectAct>() != null)
            {
                InstallableObjectAct TargetObjectAct = DetectedObject.GetComponent<InstallableObjectAct>();
                if(TargetObjectAct.Info.Type == "Belt")
                {
                    ContactBelt = DetectedObject;
                }
            }
        }

        if(ContactBelt != ParentObject && Amount == 0)
        {
            ResetInstallList();
            return;
        }

        if(Amount + VerticalDistance != 0)
        {
            List<GameObject> ActivatedBeltList = new List<GameObject>();
            for(int i = 0; i < BeltInstallList.Count - 1; i++)
            {
                if(BeltInstallList[i].activeSelf) ActivatedBeltList.Add(BeltInstallList[i]);
            }
            ActivatedBeltList.Add(ContactBelt);

            for(int i = 0; i < ActivatedBeltList.Count - 1; i++)
            {
                if(DirectionSign == 1)
                {
                    ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[i + 1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                    ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().isEnd = false;
                    ActivatedBeltList[i + 1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                }
                else
                {
                    ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().ChangePrevBelt(ActivatedBeltList[i + 1].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                    ActivatedBeltList[i + 1].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject);
                    ActivatedBeltList[i + 1].GetComponent<InstallableObjectAct>().GetBeltAct().isEnd = false;
                }

                if(ActivatedBeltList[i].GetComponent<InstallableObjectAct>().isInstall) continue;
                
                ActivatedBeltList[i].GetComponent<InstallableObjectAct>().Installation();
                ActivatedBeltList[i].GetComponent<InstallableObjectAct>().IsWorking = true;
                ActivatedBeltList[i].GetComponent<InstallableObjectAct>().GetBeltAct().InstallStandBy = false;
            }
        }
        else BeltInstallList[0].GetComponent<InstallableObjectAct>().ObjectDelete();
        
        ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct().InstallStandBy = false;
        ContactBelt.GetComponent<InstallableObjectAct>().IsWorking = true;
        ContactBelt.GetComponent<InstallableObjectAct>().Installation();

        if(DirectionSign == 1)
        {
            if(ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct().NextBelt == null)
            {
                ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(null);
            }
            else
            {
                ContactBelt.GetComponent<InstallableObjectAct>().GetBeltAct().isEnd = false;
            }
        }
        else
        {
            if(BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().NextBelt == null)
            {
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().ChangeNextBelt(null);
            }
            else
            {
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().isEnd = false;
            }
        }

        if(VerticalBelt != null)
        {
            if(DirectionSign == 1)
            {
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().VerticalBelt = VerticalBelt;
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().NextBelt.GetComponent<BeltAct>().VerticalBelt = VerticalBelt;

                VerticalBelt.GetComponent<VerticlaBeltAct>().PrevBelt = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject;
                VerticalBelt.GetComponent<VerticlaBeltAct>().Mover = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().NextBelt;
            }
            else
            {
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().VerticalBelt = VerticalBelt;
                BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().PrevBelt.GetComponent<BeltAct>().VerticalBelt = VerticalBelt;

                VerticalBelt.GetComponent<VerticlaBeltAct>().PrevBelt = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().PrevBelt;
                VerticalBelt.GetComponent<VerticlaBeltAct>().Mover = BeltInstallList[0].GetComponent<InstallableObjectAct>().GetBeltAct().gameObject;
            }

            VerticalBelt.GetComponent<InstallableObjectAct>().Installation();
            VerticalBelt.GetComponent<VerticlaBeltAct>().Initializing();
            VerticalBelt = null;
        }
        
        BeltInstallList = new List<GameObject>();
        BeltInstallFirstPos = new int[2];
        
        GameObject NewBelt;
        if(ContactBelt == ParentObject) NewBelt = ObjInstantiaterCall.InstantiateNewObject(ObjectActCall.Info.Name);
        else NewBelt = ParentObject;

        NewBelt.transform.GetChild(0).gameObject.SetActive(true);
        NewBelt.transform.rotation = ParentObject.transform.rotation;
        NewBelt.GetComponent<InstallableObjectAct>().HeightLevel = ObjectActCall.HeightLevel;
        NewBelt.GetComponent<InstallableObjectAct>().GetBeltAct().CheckDirection();
    }

    public void ChangePrevBelt(GameObject TargetBelt)
    {
        PrevBelt = TargetBelt;
        
        if(PrevBelt == null && NextBelt != null)
        {
            ParentObject.transform.rotation = NextBelt.GetComponent<BeltAct>().ParentObject.transform.rotation;
            CheckDirection();
        }

        ChangeShape();
    }

    public void ChangeNextBelt(GameObject TargetBelt)
    {
        NextBelt = TargetBelt;

        if(NextBelt == null)
        {
            if(StructCarrier.transform.GetChild(1).childCount > 1)
            {
                for(int i = StructCarrier.transform.GetChild(1).childCount - 1; i >= 1; i--)
                {
                    Destroy(StructCarrier.transform.GetChild(1).GetChild(i).gameObject);
                }
            }
            else if(StructCarrier.transform.GetChild(1).childCount == 0)
            {
                GameObject Stopper = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_End"), StructCarrier.transform.GetChild(1));
                Stopper.transform.localScale = new Vector3(50f, 50f, 7.5f);
                Stopper.transform.localPosition = new Vector3(0, -0.5f, 0);
            }
        }
        else
        {
            if(StructCarrier.transform.GetChild(1).childCount > 0)
            {
                for(int i = StructCarrier.transform.GetChild(1).childCount - 1; i >= 0; i--)
                {
                    Destroy(StructCarrier.transform.GetChild(1).GetChild(i).gameObject);
                }
            }
        }
        
        if(NextBelt == null && PrevBelt != null)
        {
            ParentObject.transform.rotation = PrevBelt.GetComponent<BeltAct>().ParentObject.transform.rotation;
            CheckDirection();
        }

        ChangeShape();
    }

    public void ChangeShape()
    {
        int PrevBeltDirection = -1;
        if(PrevBelt != null) PrevBeltDirection = PrevBelt.GetComponent<BeltAct>().BeltDirection;

        if(!isDummyBelt)
        {
            if(StructCarrier.transform.GetChild(0).childCount > 1)
            {
                for(int i = StructCarrier.transform.GetChild(0).childCount - 1; i >= 1; i--)
                {
                    Destroy(StructCarrier.transform.GetChild(0).GetChild(i).gameObject);
                }
            }

            if(PrevBeltDirection == -1)
            {
                if(StructCarrier.transform.GetChild(0).GetChild(0).name != "Belt_Straight")
                {
                    Destroy(StructCarrier.transform.GetChild(0).GetChild(0).gameObject);
                    GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Straight"), StructCarrier.transform.GetChild(0));
                    BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                    BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                    if(ObjectActCall.StructObject.layer == 23)
                    {
                        ObjectActCall.StructObject = BeltStruct;
                    }
                }
            }
            else
            {
                if(PrevBeltDirection != BeltDirection)
                {
                    if(PrevBeltDirection - BeltDirection == 1 || PrevBeltDirection - BeltDirection == -3)
                    {
                        if(StructCarrier.transform.GetChild(0).GetChild(0).name != "Belt_Bended_Right")
                        {
                            Destroy(StructCarrier.transform.GetChild(0).GetChild(0).gameObject);
                            GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Bended_Right"), StructCarrier.transform.GetChild(0));
                            BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                            BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                            BeltStruct.transform.Rotate(0,0,180);
                            if(ObjectActCall.StructObject.layer == 23)
                            {
                                ObjectActCall.StructObject = BeltStruct;
                            }
                        }
                        
                    }
                    else if(PrevBeltDirection - BeltDirection == 3 || PrevBeltDirection - BeltDirection == -1)
                    {
                        if(StructCarrier.transform.GetChild(0).GetChild(0).name != "Belt_Bended_Left")
                        {
                            Destroy(StructCarrier.transform.GetChild(0).GetChild(0).gameObject);
                            GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Bended_Left"), StructCarrier.transform.GetChild(0));
                            BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                            BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                            BeltStruct.transform.Rotate(0,0,180);
                            if(ObjectActCall.StructObject.layer == 23)
                            {
                                ObjectActCall.StructObject = BeltStruct;
                            }
                        }
                    }
                }
                else
                {
                    if(StructCarrier.transform.GetChild(0).GetChild(0).name != "Belt_Straight")
                    {
                        Destroy(StructCarrier.transform.GetChild(0).GetChild(0).gameObject);
                        GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Straight"), StructCarrier.transform.GetChild(0));
                        BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                        BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                        BeltStruct.transform.Rotate(0,0,180);
                        if(ObjectActCall.StructObject.layer == 23)
                        {
                            ObjectActCall.StructObject = BeltStruct;
                        }
                    }
                }
            }
        }
    }

    void CheckDirection()
    {
        if (ParentObject.transform.eulerAngles.y == 0)
        {
            BeltDirection = 0;
        }
        else if (ParentObject.transform.eulerAngles.y == 90)
        {
            BeltDirection = 1;
        }
        else if (ParentObject.transform.eulerAngles.y == 180)
        {
            BeltDirection = 2;
        }
        else if (ParentObject.transform.eulerAngles.y == 270)
        {
            BeltDirection = 3;
        }
    }

    void GetGoodsOnBelt()
    {
        for(int i = 0; i < RearDetector.Count; i++)
        {
            if(RearDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                GoodsOnBelt = RearDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                // Debug.Log(GoodsOnBelt.name + " is on " + ParentObject.name);
                break;
            }
        }
        
        if(FrontDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null && GoodsOnBelt != null)
        {
            if(FrontDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.name == GoodsOnBelt.name)
            {
                GoodsOnExit = GoodsOnBelt;
            }
        }
        
        if(FrontDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject == null && GoodsOnExit != null)
        {
            // Debug.Log(GoodsOnBelt.name + " leave " + ParentObject.name);
            GoodsOnBelt = null;
            GoodsOnExit = null;
        }

        if(GoodsOnBelt == null)
        {
            GoodsOnExit = null;
        }
    }

    public void ChangeNeedStop(bool State, GameObject Object)
    {
        if(NeedStop != State)
        {
            NeedStop = State;
            // Debug.Log(Object.name + " Changed " + transform.parent.parent.name + " belt's NeedStop State to " + State);
        }
    }

    void MovingGoods()
    {
        // If this Belt is End Belt
        if (isEnd)
        {
            if (GoodsValueCall.CheckMovingState(int.Parse(GoodsOnBelt.name)) == 1)
            {
                CenterStop();
            }
        }
        // If this isnt End Belt
        else
        {
            // If next Belt has goods already
            if (NextBelt.GetComponent<BeltAct>().GoodsOnBelt != null)
            {
                if(NextBelt.GetComponent<BeltAct>().GoodsOnBelt != GoodsOnBelt)
                {
                    if (GoodsValueCall.CheckMovingState(int.Parse(GoodsOnBelt.name)) == 1)
                    {
                        CenterStop();
                    }
                }
            }
            // if next Belt is empty
            else
            {
                // If Goods is already moving
                if (GoodsValueCall.CheckMovingState(int.Parse(GoodsOnBelt.name)) == 1)
                {
                    if(PrevBelt != null)
                    {
                        if(PrevBelt.GetComponent<BeltAct>().BeltDirection != BeltDirection)
                        {
                            // Debug.Log(GoodsOnBelt.name + " is Now in Corner of " + ParentObject.name);
                            if(isCenter(PrevBelt.GetComponent<BeltAct>().BeltDirection))
                            {
                                // Debug.Log(GoodsOnBelt.name + " is Center in " + ParentObject.name + "'s Change Direction");
                                GoodsOnBelt.GetComponent<Rigidbody>().velocity = GoodsVelocity[BeltDirection];
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    
                }
                // If Goods isnt moving now
                else
                {
                    GoodsOnBelt.GetComponent<Rigidbody>().velocity = GoodsVelocity[BeltDirection];
                    GoodsValueCall.ChangeMovingState(int.Parse(GoodsOnBelt.name), true);
                    isStop = false;
                }
            }
        }
    }

    public bool isCenter(int PrevDirection)
    {
        if(PrevDirection == -1)
        {
            PrevDirection = BeltDirection;
        }

        switch (PrevDirection)
        {
            case 0:
                if (GoodsOnBelt.transform.position.x > transform.position.x)
                {
                    return false;
                }
                break;
            case 2:
                if (GoodsOnBelt.transform.position.x < transform.position.x)
                {
                    return false;
                }
                break;
            case 1:
                if (GoodsOnBelt.transform.position.z < transform.position.z)
                {
                    return false;
                }
                break;
            case 3:
                if (GoodsOnBelt.transform.position.z > transform.position.z)
                {
                    return false;
                }
                break;
        }

        return true;
    }

    void CenterStop()
    {
        if(PrevBelt != null)
        {
            if (isCenter(PrevBelt.GetComponent<BeltAct>().BeltDirection))
            {
                GoodsOnBelt.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GoodsValueCall.ChangeMovingState(int.Parse(GoodsOnBelt.name), false);
                isStop = true;
                // Debug.Log(GoodsOnBelt.name + " is Center in " + ParentObject.name + "'s CenterStop");
            }
        }
        else
        {
            if(isCenter(BeltDirection))
            {
                GoodsOnBelt.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GoodsValueCall.ChangeMovingState(int.Parse(GoodsOnBelt.name), false);
                isStop = true;
                // Debug.Log(GoodsOnBelt.name + " is Center in " + ParentObject.name + "'s CenterStop");
            }
        }
    }

    public bool DeteleBelt()
    {
        if(NextBelt != null)
        {
            NextBelt.GetComponent<BeltAct>().ChangePrevBelt(null);
        }
        if(PrevBelt != null)
        {
            PrevBelt.GetComponent<BeltAct>().ChangeNextBelt(null);
        }
        if(GoodsOnBelt != null)
        {
            GoodsValueCall.DeleteGoodsArray(GoodsOnBelt);
            Destroy(GoodsOnBelt);
        }
        if(VerticalBelt != null)
        {
            VerticalBelt.GetComponent<InstallableObjectAct>().ObjectDelete();
        }

        return true;
    }
}
