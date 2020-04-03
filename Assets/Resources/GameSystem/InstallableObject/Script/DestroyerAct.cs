using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerAct : MonoBehaviour
{
    GameObject Mover;
    BeltAct BeltActCall;
    GoodsValue GoodsValueCall;
    InstallableObjectAct ObjectActCall;
    TimeManager TimeManagerCall;
    GameObject MoverDetector;
    GameObject PrevBeltDetector;
    GameObject PrevBelt;
    public int WorkTime;
    bool isWaiting;
    bool isInitialized;
    int DestroyerDirection = -1;
    // Start is called before the first frame update
    void Start()
    {
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        isWaiting = false;
        isInitialized = false;

        MoverDetector = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        PrevBeltDetector = transform.GetChild(2).GetChild(1).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall)
        {
            GetBelt();
            
            isInitialized = true;
            ObjectActCall.IsWorking = true;

            if(Mover == null)
                isInitialized = false;
            if(PrevBelt == null)
                isInitialized = false;
            if(isInitialized)
            {
                BeltActCall.ChangeNeedStop(true, gameObject);
                if(!isWaiting)
                {
                    if(BeltActCall.GoodsOnBelt != null)
                    {
                        if (GoodsValueCall.CheckMovingState(BeltActCall.GoodsOnBelt) == 0)
                        {
                            if(ObjectActCall.WorkSpeed != 0)
                            {
                                StartCoroutine(Waiter(Mathf.FloorToInt(WorkTime / ObjectActCall.WorkSpeed)));
                            }
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
            DestroyerDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            DestroyerDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            DestroyerDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            DestroyerDirection = 3;
        }
    }

    bool CheckInstallCondition()
    {
        bool result = true;

        CheckDirection();

        if(MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
        {
            BeltAct BeltActCall = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
            if(BeltActCall.BeltDirection != DestroyerDirection)
            {
                result = false;
            }

            if(BeltActCall.PrevBelt != null)
            {
                if(BeltActCall.PrevBelt.GetComponent<BeltAct>().BeltDirection != DestroyerDirection)
                {
                    result = false;
                }
            }
        }

        return result;
    }

    void GetBelt()
    {
        if (Mover != MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            // If mover has been attached (or changed) and mover's direction is same as Destoryer's
            // Initialize the belt info and stop the belt
            if (MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCallTemp = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(!BeltActCallTemp.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                {
                    // If detected belt is not installed
                    // Destoryer will not work
                    Mover = null;
                    if (BeltActCallTemp.BeltDirection == DestroyerDirection)
                    {
                        BeltActCallTemp.ModuleCondtion = true;   
                    }
                    else
                    {
                        // If detected mover's direction is not same as Destoryer
                        // Cannot install mover
                        BeltActCallTemp.ModuleCondtion = false;
                    }
                }
                else
                {
                    if (BeltActCallTemp.BeltDirection == DestroyerDirection)
                    {
                        Mover = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;
                        BeltActCall = BeltActCallTemp;
                    }
                    else
                    {
                        // If detected mover's direction is not same as Destoryer
                        // Destoryer will not work
                        Mover = null;
                    }
                }
            }
            else
            {
                // If there is no mover
                // Destoryer will not work
                Mover = null;
            }
        }

        if(Mover != null)
        {
            GetPrevBelt();
        }
    }

    void GetPrevBelt()
    {
        if (PrevBelt != PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            // If previous belt has been attached (or changed) and prev belt's direction is same as processor's
            // Initialize the belt info and stop the belt
            if (PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCall = PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(!BeltActCall.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                {
                    // If detected belt is not installed
                    // Processor will not work
                    PrevBelt = null;
                }
                else
                {
                    if (BeltActCall.BeltDirection == Mover.GetComponent<BeltAct>().BeltDirection)
                    {
                        PrevBelt = PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;
                    }
                    else
                    {
                        // If detected belt's direction is not same as Mover
                        // Processor will not work
                        PrevBelt = null;
                    }
                }
            }
            else
            {
                // If there is no previous belt
                // Processor will not work
                PrevBelt = null;
            }
        }
    }

    void DestoryGoods()
    {
        GoodsValueCall.DeleteGoodsArray(BeltActCall.GoodsOnBelt);
        Destroy(BeltActCall.GoodsOnBelt);
        BeltActCall.GoodsOnBelt = null;
    }

    public bool DeleteObject()
    {
        if(Mover != null)
            Mover.GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
        
        return true;
    }

    IEnumerator Waiter(int Time)
    {
        isWaiting = true;

        int counter = 0;
        int LeftTime = Mathf.CeilToInt(WorkTime / ObjectActCall.WorkSpeed);

        while(0 < LeftTime)
        {
            if(ObjectActCall.WorkSpeed != 0)
            {
                counter += TimeManagerCall.PlaySpeed;
                LeftTime = LeftTime = Mathf.CeilToInt((WorkTime - counter) / ObjectActCall.WorkSpeed);
            }
            yield return null;
        }

        isWaiting = false;
        
        DestoryGoods();
    }
}
