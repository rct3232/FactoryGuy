using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorAct : MonoBehaviour
{
    public int OutputNumber;
    public int DistributorDirection = -1;
    public GameObject[] Mover;
    public GameObject AddonObject;
    public GameObject PrevBelt;
    public GameObject[] NextBelt;
    GameObject[] MoverDetector;
    GameObject[] PrevBeltDetector;
    GameObject TargetGoods;
    GoodsValue GoodsValueCall;
    BeltAct PrevBeltActCall;
    BeltAct MainBeltActCall;
    InstallableObjectAct ObjectActCall;
    public bool GetIndex;
    public bool CanProceed;
    public int DistributeIndex;
    public int MainBeltIndex = -1;
    bool isInitialized;

    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        DistributeIndex = 0;
        AddonObject = null;
        TargetGoods = null;
        GetIndex = false;
        CanProceed = false;
        isInitialized = false;

        Mover = new GameObject[OutputNumber];
        MoverDetector = new GameObject[OutputNumber];
        PrevBeltDetector = new GameObject[OutputNumber];
        NextBelt = new GameObject[OutputNumber];
        PrevBelt = null;

        for (int i = 0; i < OutputNumber; i++)
        {
            // 0 ; Left
            // 1 : Right
            MoverDetector[i] = transform.GetChild(2).GetChild(0).GetChild(i).gameObject;
            PrevBeltDetector[i] = transform.GetChild(2).GetChild(1).GetChild(i).gameObject;
        }

        GetBelt();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjectActCall.isInstall)
        {
            GetBelt();

            isInitialized = true;
            
            if(PrevBelt == null)
                isInitialized = false;

            for(int i = 0; i < OutputNumber; i++)
            {
                if(Mover[i] == null)
                {
                    isInitialized = false;
                    break;
                }

                if(NextBelt[i] == null)
                {
                    isInitialized = false;
                    break;
                }
            }

            if(isInitialized)
            {
                ObjectActCall.IsWorking = true;
                
                if(CanProceed)
                {
                    if(PrevBeltActCall.NeedStop)
                    {
                        PrevBeltActCall.ChangeNeedStop(false, gameObject);
                    }
                    
                    if(MainBeltActCall.GoodsOnBelt != null)
                    {
                        // if(transform.name != "#3 Distributor")
                        //     Debug.Log(transform.name + " is start Distributing");
                        if(MainBeltActCall.GoodsOnBelt == TargetGoods)
                        {
                            if(MainBeltActCall.isCenter(-1))
                            {
                                Distributing();
                            }
                        }
                    }
                }
                else
                {
                    if(PrevBeltActCall.GoodsOnBelt != null)
                    {
                        if(!CanProceed)
                        {
                            WhereToGo();
                        }
                    }
                }
            }
        }
        else
        {
            ObjectActCall.CanInstall = CheckInstallCondition();
        }
    }

    void CheckDirection()
    {
        if (gameObject.transform.eulerAngles.y == 0)
        {
            DistributorDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            DistributorDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            DistributorDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            DistributorDirection = 3;
        }
    }

    bool CheckInstallCondition()
    {
        bool result = true;
        int PrevBeltCount = 0;
        int PrevBeltIndex = -1;

        CheckDirection();

        for (int i = 0; i < OutputNumber; i++)
        {
            if(MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCall = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(BeltActCall.BeltDirection != DistributorDirection)
                {
                    result = false;
                    break;
                }
            }

            if(PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCall = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(BeltActCall.BeltDirection != DistributorDirection)
                {
                    result = false;
                    break;
                }
                else
                {
                    PrevBeltCount++;
                    PrevBeltIndex = i;
                }
            }

            if(PrevBeltCount > 1)
            {
                PrevBeltIndex = -1;
                result = false;
                break;
            }
        }

        if(PrevBeltIndex != -1)
        {
            Transform StructCarrier =  ObjectActCall.StructObject.transform.parent;
            string StructName = "Distributor" + OutputNumber.ToString() + "-" + PrevBeltIndex.ToString();
            if(ObjectActCall.StructObject.name != StructName)
            {
                GameObject newStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/" + StructName), StructCarrier);
                newStruct.name = StructName;
                Destroy(ObjectActCall.StructObject);
                ObjectActCall.StructObject = newStruct;
            }
        }

        return result;
    }

    void GetBelt()
    {
        bool GetAllMover = true;
        for (int i = 0; i < OutputNumber; i++)
        {
            if (Mover[i] != MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject)
            {
                // If mover has been attached (or changed) and mover's direction is same as processor's
                // Initialize the belt info and stop the belt
                if (MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                {
                    BeltAct BeltActCall = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                    if(!BeltActCall.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                    {
                        // If detected belt is not installed
                        // Distributer will not work
                        Mover[i] = null;
                        if (BeltActCall.BeltDirection == DistributorDirection)
                        {
                            BeltActCall.ModuleCondtion = true;   
                        }
                        else
                        {
                            // If detected mover's direction is not same as Distributer
                            // Cannot install mover
                            BeltActCall.ModuleCondtion = false;
                        }
                    }
                    else
                    {
                        if (BeltActCall.BeltDirection == DistributorDirection)
                        {
                            Mover[i] = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                        }
                        else
                        {
                            // If detected mover's direction is not same as Processor
                            // Processor will not work
                            Mover[i] = null;
                        }
                    }
                }
                else
                {
                    // If there is no mover
                    // Processor will not work
                    Mover[i] = null;
                }
            }
        }

        for (int i = 0; i < OutputNumber; i++)
        {
            if(Mover[i] == null)
            {
                GetAllMover = false;
                break;
            }
        }

        if(GetAllMover)
        {
            GetPrevBelt();
            if(PrevBelt != null)
            {
                for(int i = 0; i < OutputNumber; i++)
                {
                    NextBelt[i] = Mover[i].GetComponent<BeltAct>().NextBelt;
                }
            }
        }
    }

    void GetPrevBelt()
    {
        if(PrevBelt != null)
        {
            for (int i = 0; i < Mover.Length; i++)
            {
                if(i == MainBeltIndex)
                {
                    if(PrevBelt != PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject)
                    {
                        if(PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                        {
                            BeltAct BeltActCall = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();

                            if(BeltActCall.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                            {
                                PrevBelt = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                                PrevBeltActCall = BeltActCall;
                                MainBeltActCall = Mover[i].GetComponent<BeltAct>();
                                MainBeltIndex = i;

                                Transform StructCarrier =  ObjectActCall.StructObject.transform.parent;
                                string StructName = "Distributor" + OutputNumber.ToString() + "-" + i.ToString();
                                if(ObjectActCall.StructObject.name != StructName)
                                {
                                    GameObject newStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/" + StructName), StructCarrier);
                                    newStruct.name = StructName;
                                    Destroy(ObjectActCall.StructObject);
                                    ObjectActCall.StructObject = newStruct;
                                }

                                for(int j = 0; j < OutputNumber; j++)
                                {
                                    Mover[j].GetComponent<BeltAct>().PrevBelt = PrevBelt;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // If there is no prevbelt and trying to install new prevbelt
            for (int i = 0; i < Mover.Length; i++)
            {
                if(PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                {
                    BeltAct BeltActCall = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                    if(!BeltActCall.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                    {
                        if (BeltActCall.BeltDirection == DistributorDirection)
                        {
                            if(PrevBelt == null)
                                BeltActCall.ModuleCondtion = true;
                            else
                                BeltActCall.ModuleCondtion = false;
                        }
                        else
                        {
                            BeltActCall.ModuleCondtion = false;
                        }
                    }
                    else
                    {
                        PrevBelt = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                        PrevBeltActCall = BeltActCall;
                        MainBeltActCall = Mover[i].GetComponent<BeltAct>();
                        MainBeltIndex = i;

                        Transform StructCarrier =  ObjectActCall.StructObject.transform.parent;
                        string StructName = "Distributor" + OutputNumber.ToString() + "-" + i.ToString();
                        if(ObjectActCall.StructObject.name != StructName)
                        {
                            GameObject newStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/" + StructName), StructCarrier);
                            newStruct.name = StructName;
                            Destroy(ObjectActCall.StructObject);
                            ObjectActCall.StructObject = newStruct;
                        }
                        
                        for(int j = 0; j < OutputNumber; j++)
                        {
                            Mover[j].GetComponent<BeltAct>().PrevBelt = PrevBelt;
                        }
                    }
                }
            }
        }
    }

    void GetDistributeIndex()
    {
        if(AddonObject == null)
        {
            for(int i = 0; i < Mover.Length; i++)
            {
                int index = DistributeIndex + i;
                if(index >= Mover.Length)
                {
                    index -= Mover.Length;
                }

                BeltAct indexBeltActCall = Mover[index].GetComponent<BeltAct>();

                if(index == MainBeltIndex)
                {
                    BeltAct nextIndexBeltActCall = NextBelt[index].GetComponent<BeltAct>();
                    if(indexBeltActCall.GoodsOnBelt == null)
                    {
                        if(nextIndexBeltActCall.GoodsOnBelt == null)
                        {
                            DistributeIndex = index;
                            CanProceed = true;
                            return;
                        }
                        else
                        {
                            if(!nextIndexBeltActCall.isStop)
                            {
                                DistributeIndex = index;
                                CanProceed = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if(!indexBeltActCall.isStop)
                        {
                            if(!nextIndexBeltActCall.isStop)
                            {
                                DistributeIndex = index;
                                CanProceed = true;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if(indexBeltActCall.GoodsOnBelt == null)
                    {
                        DistributeIndex = index;
                        CanProceed = true;
                        return; 
                    }
                    else
                    {
                        if(!indexBeltActCall.isStop)
                        {
                            DistributeIndex = index;
                            CanProceed = true;
                            return;
                        }
                    }
                }
            }
            CanProceed = false;
            return;
        }
        else
        {
            if(Mover[DistributeIndex].GetComponent<BeltAct>().GoodsOnBelt == null)
            {
                CanProceed = true;
                return;
            }
            else
            {
                CanProceed = false;
                return;
            }
        }
    }

    void WhereToGo()
    {
        PrevBeltActCall.ChangeNeedStop(true, gameObject);
        if(!GetIndex)
        {
            if(PrevBeltActCall.GoodsOnBelt != null)
            {
                if(PrevBeltActCall.GoodsOnBelt != TargetGoods)
                {
                    TargetGoods = PrevBeltActCall.GoodsOnBelt;
                    if (AddonObject != null)
                    {
                        AddonObject.GetComponent<AddonObjectAct>().Tic = true;
                    }
                    else
                    {
                        GetIndex = true;
                    }
                }
            }

            CanProceed = false;
            return;
        }
        else
        {
            GetDistributeIndex();
        }
    }

    void Distributing()
    {
        if(MainBeltIndex != DistributeIndex)
        {
            MainBeltActCall.GoodsOnBelt.transform.position = 
                new Vector3(Mover[DistributeIndex].transform.position.x, Mover[DistributeIndex].transform.position.y + 0.325f, Mover[DistributeIndex].transform.position.z);
            Mover[DistributeIndex].GetComponent<BeltAct>().GoodsOnBelt = MainBeltActCall.GoodsOnBelt;
            MainBeltActCall.GoodsOnBelt = null;

            TargetGoods = null;
            GetIndex = false;
            CanProceed = false;
        }
        else
        {
            TargetGoods = null;
            GetIndex = false;
            CanProceed = false;
        }

        if(AddonObject == null)
        {
            DistributeIndex += 1;
            if(DistributeIndex >= Mover.Length)
            {
                DistributeIndex = 0;
            }
        }
    }

    public bool DeleteObject()
    {
        if(AddonObject != null)
        {
            Destroy(AddonObject);
        }
        
        for(int i = 0; i < OutputNumber; i++)
        {
            if(i != MainBeltIndex)
                Mover[i].GetComponent<BeltAct>().PrevBelt = null;

            Mover[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
        }

        if(PrevBelt != null)
            PrevBeltActCall.ChangeNeedStop(false, gameObject);

        return true;
    }
}
