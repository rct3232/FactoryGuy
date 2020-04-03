using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorAct : MonoBehaviour
{
    public string ProcessorActorName = "";
    public GoodsRecipe.Recipe TargetGoodsRecipe;
    GameObject[] Mover;
    GameObject[] PrevBeltDetector;
    GameObject[] MoverDetector;
    GameObject[] NextBeltDetector;
    GameObject[] PrevBelt;
    GoodsValue GoodsValueCall;
    GoodsRecipe GoodsRecipeCall;
    InstallableObjectAct ObjectActCall;
    TimeManager TimeManagerCall;
    GameObject Goods;
    GameObject ProcessedGoods;
    GameObject CurGoods;
    public bool isInitialized = false;
    public int InputNumber;
    public int WorkTime;
    public int WorkLoadPerDay;
    int RealTimeWorkLoadPerDay;
    int TargetSetTime;
    int ProcessorDirection = -1;
    int[] RegisteredInput;
    bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();;
        Goods = GameObject.Find("Goods");
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        TargetGoodsRecipe = null;

        Mover = new GameObject[InputNumber];
        PrevBelt = new GameObject[InputNumber];
        PrevBeltDetector = new GameObject[InputNumber];
        NextBeltDetector = new GameObject[InputNumber];
        MoverDetector = new GameObject[InputNumber];

        Transform DetectorCarrier = transform.GetChild(2);
        for (int i = 0; i < InputNumber; i++)
        {
            MoverDetector[i] = DetectorCarrier.GetChild(0).GetChild(i).gameObject;
            PrevBeltDetector[i] = DetectorCarrier.GetChild(1).GetChild(i).gameObject;
            NextBeltDetector[i] = DetectorCarrier.GetChild(2).GetChild(i).gameObject;
        }

        RegisteredInput = new int[InputNumber];
        for (int i = 0; i < InputNumber; i++)
        {
            RegisteredInput[i] = -1;
        }

        WorkLoadPerDay = 0;
        RealTimeWorkLoadPerDay = 0;

        TargetSetTime = -1;

        isWaiting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjectActCall.isInstall)
        {
            // If processor does not installed yet
            // Nothing will happen
            GetBelt();

            if(TargetGoodsRecipe != null)
            {
                isInitialized = true;

                for(int i = 0; i < InputNumber; i++)
                {
                    if(Mover[i] == null)
                    {
                        isInitialized = false;
                        break;
                    }
                }

                for(int i = 0; i < InputNumber; i++)
                {
                    if(PrevBelt[i] == null)
                    {
                        isInitialized = false;
                        break;
                    }
                }
            }
            else
            {
                isInitialized = false;
            }

            if(isInitialized)
            {
                if((TimeManagerCall.TimeValue - TargetSetTime) % TimeManagerCall.Day < TimeManagerCall.PlaySpeed)
                {
                    WorkLoadPerDay = RealTimeWorkLoadPerDay;
                    RealTimeWorkLoadPerDay = 0;
                }

                ObjectActCall.IsWorking = true;
                ProcessingGoods();
            }
            else
            {
                WorkLoadPerDay = 0;
                RealTimeWorkLoadPerDay = 0;

                TargetSetTime = -1;

                ObjectActCall.IsWorking = false;
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
            ProcessorDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            ProcessorDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            ProcessorDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            ProcessorDirection = 3;
        }
    }

    bool CheckInstallCondition()
    {
        bool result = true;

        CheckDirection();

        for (int i = 0; i < InputNumber; i++)
        {
            if(MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCall = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(BeltActCall.BeltDirection != ProcessorDirection)
                {
                    result = false;
                    break;
                }

                if(BeltActCall.PrevBelt != null)
                {
                    if(BeltActCall.PrevBelt.GetComponent<BeltAct>().BeltDirection != ProcessorDirection)
                    {
                        result = false;
                        break;
                    }
                }
            }
        }

        return result;
    }

    void GetBelt()
    {
        bool GetAllMover = true;
        for (int i = 0; i < InputNumber; i++)
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
                        // Processor will not work
                        Mover[i] = null;
                        if (BeltActCall.BeltDirection == ProcessorDirection)
                        {
                            BeltActCall.ModuleCondtion = true;   
                        }
                        else
                        {
                            // If detected mover's direction is not same as Processor
                            // Cannot install mover
                            BeltActCall.ModuleCondtion = false;
                        }
                    }
                    else
                    {
                        if (BeltActCall.BeltDirection == ProcessorDirection)
                        {
                            Mover[i] = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                            BeltActCall.ChangeNeedStop(true, gameObject);
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

        for (int i = 0; i < InputNumber; i++)
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
        }
    }

    void GetPrevBelt()
    {
        for (int i = 0; i < InputNumber; i++)
        {
            if (PrevBelt[i] != PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject)
            {
                // If previous belt has been attached (or changed) and prev belt's direction is same as processor's
                // Initialize the belt info and stop the belt
                if (PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
                {
                    BeltAct BeltActCall = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                    if(!BeltActCall.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                    {
                        // If detected belt is not installed
                        // Processor will not work
                        PrevBelt[i] = null;
                        Mover[i].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
                    }
                    else
                    {
                        if (BeltActCall.BeltDirection == Mover[0].GetComponent<BeltAct>().BeltDirection)
                        {
                            PrevBelt[i] = PrevBeltDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                            Mover[i].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
                            PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
                        }
                        else
                        {
                            // If detected belt's direction is not same as Mover
                            // Processor will not work
                            PrevBelt[i] = null;
                            Mover[i].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
                        }
                    }
                }
                else
                {
                    // If there is no previous belt
                    // Processor will not work
                    PrevBelt[i] = null;
                    Mover[i].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
                }
            }
        }
    }

    public void ChangeProcessorActor(string Name)
    {
        ProcessorActorName = Name;
    }

    public void SetTargetGoods(string Name)
    {
        TargetGoodsRecipe = GoodsRecipeCall.GetRecipe(Name);

        TargetSetTime = TimeManagerCall.TimeValue;
    }

    void ProcessingGoods()
    {
        for (int i = 0; i < InputNumber; i++)
        {
            // Check the belt has correct object
            if (Mover[i].GetComponent<BeltAct>().GoodsOnBelt != null)
            {
                for (int j = 0; j < InputNumber; j++)
                {
                    if (RegisteredInput[j] == -1)
                    {
                        if (GoodsValueCall.FindGoodsName(int.Parse(Mover[i].GetComponent<BeltAct>().GoodsOnBelt.name)) == TargetGoodsRecipe.InputName[j])
                        {
                            // Write belt's index to RegisteredInput
                            RegisteredInput[j] = i;
                            break;
                        }
                        else
                        {
                            RegisteredInput[j] = -1;
                        }
                    }
                }
            }
        }

        for(int i = 0; i < InputNumber; i++)
        {
            if (Mover[i].GetComponent<BeltAct>().GoodsOnBelt != null)
            {
                PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
            }
        }

        // Input check routine
        for (int i = 0; i < InputNumber; i++)
        {
            if (Mover[i].GetComponent<BeltAct>().GoodsOnBelt != null)
            {
                if (!Mover[i].GetComponent<BeltAct>().isCenter(-1))
                {
                    return;
                }
                else if (Mover[i].GetComponent<BeltAct>().isCenter(-1))
                {
                    for(int j = 0; j < InputNumber;)
                    {
                        // Check all RegisteredInput
                        if (RegisteredInput[j] == i)
                        {
                            // Correct input. break the check routine
                            break;
                        }
                        j++;
                        if (j == InputNumber)
                        {
                            // Wrong input. Object will be destroy
                            GoodsValueCall.DeleteGoodsArray(int.Parse(Mover[i].GetComponent<BeltAct>().GoodsOnBelt.name));
                            Destroy(Mover[i].GetComponent<BeltAct>().GoodsOnBelt);
                            PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
                            Mover[i].GetComponent<BeltAct>().GoodsOnBelt = null;
                            return;
                        }
                    }
                }
            }
        }

        // Now processing
        if (Mover[0].GetComponent<BeltAct>().GoodsOnBelt != null && ProcessedGoods == null)
        {
            Mover[0].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);

            for(int i = 0; i < InputNumber; i++)
            {
                if(Mover[i].GetComponent<BeltAct>().GoodsOnBelt == null)
                {
                    // If there is empty belt, wait for that
                    return;
                }
            }

            // If there is something one belt and no previous processed object
            // Start processing
            if(!isWaiting)
            {
                if(ObjectActCall.WorkSpeed == 0)
                {
                    return;
                }
                StartCoroutine(Waiter());
            }
            else
            {
                return;
            }
        }
        else if(Mover[0].GetComponent<BeltAct>().GoodsOnBelt != null && ProcessedGoods != null)
        {
            // If there is something on belt and previous processed object
            // Waiting object must not come in processor
            Mover[0].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
        }
        else if(Mover[0].GetComponent<BeltAct>().GoodsOnBelt == null && ProcessedGoods != null)
        {
            // If there is nothing on belt but previous processed object
            // Clear the processed object info
            ProcessedGoods = null;
            for (int i = 0; i < InputNumber; i++)
            {
                // Now waiting object can come in
                Mover[0].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
                // Clear the RegisteredInput
                RegisteredInput[i] = -1;
            }
        }
        else
        {
            // If there is nothing
            Mover[0].GetComponent<BeltAct>().ChangeNeedStop(true, gameObject);
            for (int i = 0; i < InputNumber; i++)
            {
                // Processor is ready for object
                PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
            }
        }
    }

    void ChangingGoods()
    {
        CurGoods = Mover[0].GetComponent<BeltAct>().GoodsOnBelt;
        ProcessedGoods = Goods.GetComponent<GoodsInstantiater>().ChangeGoods(CurGoods, TargetGoodsRecipe.OutputName);
        GoodsValueCall.ChangeQuality(int.Parse(ProcessedGoods.name), Random.Range(0.1f, 0.75f));
        for(int i = 1; i < InputNumber; i++)
        {
            // Destroy all Input except Belt[0]'s
            GoodsValueCall.DeleteGoodsArray(Mover[i].GetComponent<BeltAct>().GoodsOnBelt);
            Destroy(Mover[i].GetComponent<BeltAct>().GoodsOnBelt);
            Mover[i].GetComponent<BeltAct>().GoodsOnBelt = null;
        }

        // Initiating Processed Goods
        GoodsValueCall.ChangeMovingState(int.Parse(ProcessedGoods.name), false);
        // Mover[0].GetComponent<BeltAct>().GoodsOnBelt = ProcessedGoods;

        RealTimeWorkLoadPerDay++;
    }

    public bool DeleteObject()
    {
        for(int i = 0; i < InputNumber; i++)
        {
            if(Mover[i] != null)
                Mover[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
            if(PrevBelt[i] != null)
                PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
        }
        return true;
    }

    IEnumerator Waiter()
    {
        isWaiting = true;

        int counter = 0;
        int LeftTime;
        if(ObjectActCall.WorkSpeed != 0)
        {
            LeftTime = Mathf.CeilToInt(WorkTime / ObjectActCall.WorkSpeed);
        }
        else
        {
            LeftTime = 999999999;
        }
        

        while(0 < LeftTime)
        {
            if(ObjectActCall.WorkSpeed != 0)
            {
                yield return new WaitForFixedUpdate();
                counter += TimeManagerCall.PlaySpeed;
                LeftTime = Mathf.CeilToInt(WorkTime / ObjectActCall.WorkSpeed) - counter;
            }
            yield return null;
        }

        isWaiting = false;

        ChangingGoods();
    }
}
