using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorAct : MonoBehaviour
{
    public string ProcessorActorName = "";
    public GoodsRecipe.Recipe TargetGoodsRecipe;
    GameObject[] Mover;
    GameObject[] MoverDetector;
    GameObject[] PrevBelt;
    GameObject NextBelt;
    int MainBeltIndex = -1;
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
        MoverDetector = new GameObject[InputNumber];

        Transform DetectorCarrier = transform.GetChild(2);
        for (int i = 0; i < InputNumber; i++)
        {
            MoverDetector[i] = DetectorCarrier.GetChild(0).GetChild(i).gameObject;
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
        bool Result = true;
        CheckDirection();

        int NextBeltCount = 0;
        for (int i = 0; i < InputNumber; i++)
        {
            if(MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct TargetBeltAct = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(TargetBeltAct.BeltDirection != ProcessorDirection)
                {
                    Result = false;
                    break;
                }

                if(TargetBeltAct.PrevBelt != null)
                {
                    if(TargetBeltAct.PrevBelt.GetComponent<BeltAct>().BeltDirection != ProcessorDirection)
                    {
                        Result = false;
                        break;
                    }
                }
            }
            else
            {
                Result = false;
                break;
            }
        }

        if(NextBeltCount > InputNumber) Result = false;

        return Result;
    }

    void GetBelt()
    {
        bool CanInitialize = true;

        NextBelt = null;
        for(int i = 0; i < InputNumber; i++)
        {
            if(MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                if(MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>().ObjectActCall.isInstall)
                {
                    Mover[i] = MoverDetector[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;
                    BeltAct TargetBeltAct = Mover[i].GetComponent<BeltAct>();
                    TargetBeltAct.ModuleObject = gameObject;

                    PrevBelt[i] = TargetBeltAct.PrevBelt;
                    if(PrevBelt[i] == null) CanInitialize = false;

                    if(TargetBeltAct.NextBelt != null)
                    {
                        NextBelt = TargetBeltAct.NextBelt;
                        MainBeltIndex = i;
                    }
                }   
            }
        }

        for(int i = 0; i < InputNumber; i++)
        {
            Mover[i].GetComponent<BeltAct>().ChangeNextBelt(NextBelt);
            if(NextBelt != null) Mover[i].GetComponent<BeltAct>().isEnd = false;
        }

        if(NextBelt == null)
        {
            CanInitialize = false;
        }

        isInitialized = CanInitialize;
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
        CurGoods = Mover[MainBeltIndex].GetComponent<BeltAct>().GoodsOnBelt;
        ProcessedGoods = Goods.GetComponent<GoodsInstantiater>().ChangeGoods(CurGoods, TargetGoodsRecipe.OutputName);
        GoodsValueCall.ChangeQuality(int.Parse(ProcessedGoods.name), Random.Range(0.1f, 0.75f));
        for(int i = 0; i < InputNumber; i++)
        {
            if(i == MainBeltIndex) continue;

            // Destroy all Input except Mainbelt
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
            if(i != MainBeltIndex)
            {
                Mover[i].GetComponent<BeltAct>().NextBelt = null;
                Mover[i].GetComponent<BeltAct>().isEnd = true;
            }

            Mover[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
            Mover[i].GetComponent<BeltAct>().ModuleObject = null;

            if(PrevBelt[i] != null) PrevBelt[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
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
