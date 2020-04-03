using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAct : MonoBehaviour
{
    InstallableObjectAct ObjectActCall;
    public bool isEnd;
    Vector3[] GoodsVelocity;
    public GameObject GoodsOnBelt;
    public GameObject GoodsOnExit;
    public GameObject DetectedBelt;
    public GameObject NextBelt;
    public GameObject PrevBelt;
    public GameObject BeltDetector;
    public GameObject ModuleDetector;
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
    GoodsValue GoodsValueCall;
    TimeManager TimeManagerCall;
    GameObject StructCarrier;
    public GameObject ParentObject;

    // Start is called before the first frame update
    void Start()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        DetectedBelt = null;
        PrevBelt = null;
        ModuleCondtion = true;
        ParentObject = transform.parent.parent.gameObject;
        ObjectActCall = ParentObject.GetComponent<InstallableObjectAct>();
        int Detectorindex = 0;
        BeltDetector = transform.GetChild(Detectorindex++).gameObject;
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

        if(!isDummyBelt)
            StructCarrier = ParentObject.transform.GetChild(0).GetChild(0).GetChild(BeltIndex).gameObject;
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

        DetectingEnd();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DetectedBelt != GetDetectedObject())
        {
            DetectingEnd();
        }
        else
        {
            if(NextBelt != null && !NextBelt.GetComponent<BeltAct>().ObjectActCall.isInstall && NextBelt != gameObject)
            {
                if(NextBeltDirection != GetDetectedObject().GetComponent<BeltAct>().BeltDirection)
                {
                    // Debug.Log("Checking Direction Change is Working");
                    NextBelt.GetComponent<BeltAct>().ChangeShape(BeltDirection);
                    NextBeltDirection = NextBelt.GetComponent<BeltAct>().BeltDirection;
                }
            }
        }

        if(!ObjectActCall.isInstall)
        {
            CheckDirection();

            if(ObjectActCall.StructObject.layer == 23)
            {
                ChangeHeightLevel();
            }

            if(ModuleDetector.GetComponentInParent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                if(ObjectActCall.CanInstall)
                {
                    ObjectActCall.CanInstall = ModuleCondtion;
                }
            }
        }
        else
        {
            ObjectActCall.IsWorking = true;
            if(NextBelt != null && isEnd)
            {
                if(NextBelt.GetComponent<BeltAct>().ObjectActCall.isInstall)
                    isEnd = false;
            }
            else if(NextBelt == null && !isEnd)
            {
                isEnd = true;
            }

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

    void ChangeHeightLevel()
    {
        int HeightLevel = ObjectActCall.HeightLevel;
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(HeightLevel == 0)
            {
                HeightLevel = 1;
            }
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(HeightLevel == 1)
            {
                HeightLevel = 0;
            }
        }
        ObjectActCall.HeightLevel = HeightLevel;
    }

    GameObject GetDetectedObject()
    {
        if(BeltDetector != null)
        {
            return BeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;
        }
        else
        {
            return null;
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

    public void DetectingEnd()
    {
        DetectedBelt = GetDetectedObject();
        if (GetDetectedObject() == null)
        {
            if(NextBelt != null && NextBelt != gameObject)
            {
                NextBelt.GetComponent<BeltAct>().ChangeShape(-1);
                NextBelt.GetComponent<BeltAct>().PrevBelt = null;
            }
            if(PrevBelt == null)
            {
                ChangeShape(-1);
            }
            // isEnd = true;
            NextBelt = null;
            if(!isDummyBelt)
            {
                if(StructCarrier.transform.GetChild(1).childCount > 0)
                {
                    Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                }
                GameObject BeltStopper = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_End"), StructCarrier.transform.GetChild(1));
                BeltStopper.transform.localScale = new Vector3(50f, 50f, 7.5f);
                BeltStopper.transform.localPosition = new Vector3(0, -0.5f, 0);
            }
            NextBeltDirection = -1;
        }
        else
        {
            if(ObjectActCall.isInstall)
            {
                if(!GetDetectedObject().GetComponent<BeltAct>().ObjectActCall.isInstall)
                {
                    // Installed Belt detect new NextBelt
                    if(NextBelt == null || NextBelt == gameObject)
                    {
                        NextBelt = GetDetectedObject();
                        NextBelt.GetComponent<BeltAct>().PrevBelt = gameObject;
                        NextBelt.GetComponent<BeltAct>().ChangeShape(BeltDirection);
                        if(!isDummyBelt)
                        {
                            if(StructCarrier.transform.GetChild(1).childCount > 0)
                            {
                                Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                            }
                        }
                        NextBeltDirection = NextBelt.GetComponent<BeltAct>().BeltDirection;
                    }
                    else
                    {
                        if(NextBelt.GetComponent<BeltAct>().ObjectActCall.isInstall)
                        {
                            // Debug.Log(ParentObject.name + " has already NextBelt");
                            GetDetectedObject().GetComponent<BeltAct>().ObjectActCall.CanInstall = false;
                            GetDetectedObject().GetComponent<BeltAct>().DetectedBelt = GetDetectedObject();
                        }
                        else
                        {
                            if(NextBelt != GetDetectedObject())
                            {
                                NextBelt.GetComponent<BeltAct>().ChangeShape(-1);
                                NextBeltDirection = -1;
                                //NextBelt.GetComponent<BeltAct>().PrevBelt = null;
                                // This part should be implemented in NextBelt's object act script
                            }
                            NextBelt = GetDetectedObject();
                            NextBelt.GetComponent<BeltAct>().PrevBelt = gameObject;
                            NextBelt.GetComponent<BeltAct>().ChangeShape(BeltDirection);
                            NextBeltDirection = NextBelt.GetComponent<BeltAct>().BeltDirection;
                        }
                    }
                }
                else
                {
                    // Debug.Log("Something is going wrong in " + ParentObject.name + "'s DetectingEnd(). Duplicated NextBelt");
                }
            }
            else
            {
                if(GetDetectedObject().GetComponent<BeltAct>().PrevBelt == null)
                {
                    if(NextBelt != null && NextBelt != gameObject)
                    {
                        NextBelt.GetComponent<BeltAct>().ChangeShape(-1);
                        NextBelt.GetComponent<BeltAct>().PrevBelt = null;
                    }
                    if(GetDetectedObject().GetComponent<BeltAct>().ModuleDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                    {
                        if(GetDetectedObject().GetComponent<BeltAct>().BeltDirection != BeltDirection)
                        {
                            // isEnd = true;
                            NextBelt = gameObject;
                            ObjectActCall.CanInstall = false;
                            if(!isDummyBelt)
                            {
                                if(StructCarrier.transform.GetChild(1).childCount > 0)
                                {
                                    Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                                }
                                GameObject BeltStopper = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_End"), StructCarrier.transform.GetChild(1));
                                BeltStopper.transform.localScale = new Vector3(50f, 50f, 7.5f);
                                BeltStopper.transform.localPosition = new Vector3(0, -0.5f, 0);
                            }
                            NextBeltDirection = -1;
                        }
                        else
                        {
                            NextBelt = GetDetectedObject();
                            NextBelt.GetComponent<BeltAct>().PrevBelt = gameObject;
                            NextBelt.GetComponent<BeltAct>().ChangeShape(BeltDirection);
                            // isEnd = false;
                            if(!isDummyBelt)
                            {
                                if(StructCarrier.transform.GetChild(1).childCount > 0)
                                {
                                    Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                                }
                            }
                            NextBeltDirection = NextBelt.GetComponent<BeltAct>().BeltDirection;
                        }
                    }
                    else
                    {
                        NextBelt = GetDetectedObject();
                        NextBelt.GetComponent<BeltAct>().PrevBelt = gameObject;
                        NextBelt.GetComponent<BeltAct>().ChangeShape(BeltDirection);
                        // isEnd = false;
                        if(!isDummyBelt)
                        {
                            if(StructCarrier.transform.GetChild(1).childCount > 0)
                            {
                                Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                            }
                        }
                        NextBeltDirection = NextBelt.GetComponent<BeltAct>().BeltDirection;
                    }
                }
                else
                {
                    // Debug.Log(GetDetectedObject().GetComponent<BeltAct>().ParentObject.name + " has already PrevBelt");
                    if(NextBelt != null && NextBelt != gameObject)
                    {
                        NextBelt.GetComponent<BeltAct>().ChangeShape(-1);
                        NextBelt.GetComponent<BeltAct>().PrevBelt = null;
                    }
                    // isEnd = true;
                    NextBelt = gameObject;
                    ObjectActCall.CanInstall = false;
                    if(!isDummyBelt)
                    {
                        if(StructCarrier.transform.GetChild(1).childCount > 0)
                        {
                            Destroy(StructCarrier.transform.GetChild(1).GetChild(0).gameObject);
                        }
                        GameObject BeltStopper = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_End"), StructCarrier.transform.GetChild(1));
                        BeltStopper.transform.localScale = new Vector3(50f, 50f, 7.5f);
                        BeltStopper.transform.localPosition = new Vector3(0, -0.5f, 0);
                    }
                    NextBeltDirection = -1;
                }
            }
        }
    }

    public void ChangeShape(int PrevBeltDirection)
    {
        if(!isDummyBelt)
        {
            if(PrevBeltDirection == -1)
            {
                if(NextBelt == null || NextBelt == gameObject)
                {
                    // Debug.Log(ParentObject.name + "'s has no PrevBelt");
                    ObjectActCall.CanInstall = true;
                }

                for(int i = StructCarrier.transform.GetChild(0).childCount - 1; i >= 0; i--)
                {
                    Destroy(StructCarrier.transform.GetChild(0).GetChild(i).gameObject);
                }
                GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Straight"), StructCarrier.transform.GetChild(0));
                BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                if(ObjectActCall.StructObject.layer == 23)
                {
                    ObjectActCall.StructObject = BeltStruct;
                }
            }
            else
            {
                if(PrevBeltDirection != BeltDirection)
                {
                    if(Mathf.Abs(PrevBeltDirection - BeltDirection) == 2)
                    {
                        // Debug.Log(ParentObject.name + "'s PrevBelt has opposite direction");
                        ObjectActCall.CanInstall = false;
                    }
                    else
                    {
                        if(PrevBeltDirection - BeltDirection == 1 || PrevBeltDirection - BeltDirection == -3)
                        {
                            // Debug.Log(ParentObject.name + " bended to right");
                            ObjectActCall.CanInstall = true;

                            for(int i = StructCarrier.transform.GetChild(0).childCount - 1; i >= 0; i--)
                            {
                                Destroy(StructCarrier.transform.GetChild(0).GetChild(i).gameObject);
                            }
                            GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Bended_Right"), StructCarrier.transform.GetChild(0));
                            BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                            BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                            BeltStruct.transform.Rotate(0,0,180);
                            if(ObjectActCall.StructObject.layer == 23)
                            {
                                ObjectActCall.StructObject = BeltStruct;
                            }
                        }
                        else if(PrevBeltDirection - BeltDirection == 3 || PrevBeltDirection - BeltDirection == -1)
                        {
                            // Debug.Log(ParentObject.name + " bended to left");
                            ObjectActCall.CanInstall = true;

                            for(int i = StructCarrier.transform.GetChild(0).childCount - 1; i >= 0; i--)
                            {
                                Destroy(StructCarrier.transform.GetChild(0).GetChild(i).gameObject);
                            }
                            GameObject BeltStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/Belt_Bended_Left"), StructCarrier.transform.GetChild(0));
                            BeltStruct.transform.localScale = new Vector3(50f, 50f, 7.5f);
                            BeltStruct.transform.localPosition = new Vector3(0, -0.5f, 0);
                            BeltStruct.transform.Rotate(0,0,180);
                            if(ObjectActCall.StructObject.layer == 23)
                            {
                                ObjectActCall.StructObject = BeltStruct;
                            }
                        }
                        else
                        {
                            // Debug.Log(ParentObject.name + " has invailed direction");
                        }
                    }
                }
                else
                {
                    // Debug.Log(ParentObject.name + "'s PrevBelt has same direction");
                    ObjectActCall.CanInstall = true;

                    for(int i = StructCarrier.transform.GetChild(0).childCount - 1; i >= 0; i--)
                    {
                        Destroy(StructCarrier.transform.GetChild(0).GetChild(i).gameObject);
                    }
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
            NextBelt.GetComponent<BeltAct>().ChangeShape(-1);
            NextBelt.GetComponent<BeltAct>().PrevBelt = null;
        }
        if(PrevBelt != null)
        {
            PrevBelt.GetComponent<BeltAct>().BeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject = null;
            PrevBelt.GetComponent<BeltAct>().NextBelt = null;
            PrevBelt.GetComponent<BeltAct>().DetectingEnd();
        }
        if(GoodsOnBelt != null)
        {
            GoodsValueCall.DeleteGoodsArray(GoodsOnBelt);
            Destroy(GoodsOnBelt);
        }

        return true;
    }
}
