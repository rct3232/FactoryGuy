using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCreater : MonoBehaviour
{
    public string TargetGoodsName;
    public GameObject Mover;
    public GameObject Goods;
    GameObject BaseSystem;
    InGameValue ValueCall;
    GoodsValue GoodsValueCall;
    BeltAct BeltActCall;
    SalesValue SalesValueCall;
    CompanyManager CompanyManagerCall;
    InstallableObjectAct ObjectActCall;
    GameObject MoverDetector;
    GameObject WarehouseDetector;
    int DoorDirection = -1;
    public bool isInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        ValueCall = BaseSystem.GetComponent<InGameValue>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        GoodsValueCall = CompanyManagerCall.GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        Goods = GameObject.Find("Goods");
        SalesValueCall = GameObject.Find("SalesManager").GetComponent<SalesValue>();

        MoverDetector = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        WarehouseDetector = transform.GetChild(2).GetChild(1).GetChild(0).gameObject;

        TargetGoodsName = "None";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.GetComponent<InstallableObjectAct>().isInstall)
        {
            GetBelt();
            isInitialized = true;

            if(TargetGoodsName == "None")
            {
                isInitialized = false;
            }

            if(Mover == null)
            {
                isInitialized = false;
            }

            if(isInitialized)
            {
                CreateGoods();
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
            DoorDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            DoorDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            DoorDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            DoorDirection = 3;
        }
    }

    bool CheckInstallCondition()
    {
        bool result = true;

        CheckDirection();

        if(MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
        {
            BeltAct BeltActCall = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
            if(BeltActCall.BeltDirection - DoorDirection == 2 || BeltActCall.BeltDirection - DoorDirection == -2)
            {
                result = false;
            }

            if(BeltActCall.PrevBelt != null)
            {
                if(BeltActCall.PrevBelt != transform.GetChild(3).GetChild(0).gameObject)
                    result = false;
            }
        }

        if(WarehouseDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject == null)
        {
            result = false;
        }

        return result;
    }

    void GetBelt()
    {
        if (Mover != MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            // If mover has been attached (or changed) and mover's direction is same as Door's
            // Initialize the belt info and stop the belt
            if (MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCallTemp = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(!BeltActCallTemp.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                {
                    // If detected belt is not installed
                    // Door will not work
                    Mover = null;
                    if (BeltActCallTemp.BeltDirection - DoorDirection != 2 && BeltActCallTemp.BeltDirection - DoorDirection != -2)
                    {
                        BeltActCallTemp.ModuleCondtion = true;   
                    }
                    else
                    {
                        // If detected mover's direction is not same as Door
                        // Cannot install mover
                        BeltActCallTemp.ModuleCondtion = false;
                    }
                }
                else
                {
                    if (BeltActCallTemp.BeltDirection - DoorDirection != 2 && BeltActCallTemp.BeltDirection - DoorDirection != -2)
                    {
                        Mover = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;
                        BeltActCall = BeltActCallTemp;
                    }
                    else
                    {
                        // If detected mover's direction is not same as Door
                        // Door will not work
                        Mover = null;
                    }
                }
            }
            else
            {
                // If there is no mover
                // Door will not work
                Mover = null;
            }
        }
    }

    public void SetTargetGoods(string name)
    {
        TargetGoodsName = name;
    }
    
    void CreateGoods()
    {
        if(GoodsValueCall.GetGoodsCount(TargetGoodsName, true) <= 0)
        {
            ObjectActCall.IsWorking = false;

            return;
        }
        
        if(Mover.GetComponent<BeltAct>().GoodsOnBelt == null)
        {
            ObjectActCall.IsWorking = true;

            GameObject newGoods = Goods.GetComponent<GoodsInstantiater>().CreateGoods(TargetGoodsName);
            newGoods.transform.position = new Vector3(Mover.transform.position.x, Mover.transform.position.y + 0.325f, Mover.transform.position.z);
            Mover.GetComponent<BeltAct>().GoodsOnBelt = newGoods;
        }
        else
        {
            ObjectActCall.IsWorking = false;
        }
    }

    public bool DeleteObject()
    {
        return true;
    }
}
