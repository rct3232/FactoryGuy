﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinerAct : MonoBehaviour
{
    public int InputNumber;        //들어오는 방향 개수
    public int JoinerDirection = -1;
    public bool CanProceed;
    bool isInitialized;
    public GameObject AddonObject;
    InstallableObjectAct ObjectActCall;

    public GameObject[] DetectedObject;
    GameObject[] DetectSensor;     //벨트 확인 센서오브젝트들


    public GameObject PrevBelt;
    public GameObject[] PrevBelt2;      //새로운 이전벨트(여러개)
    public GameObject[] NextBelt;
    public GameObject NextBelt2;        //새로운 출력벨트(1)

    BeltAct NextBeltActCall;
    BeltAct MainBeltActCall;

    GameObject TargetGoods;
    GoodsValue GoodsValueCall;

    public int CurrentJoinIndex;           //현재 진행중인 합칠 오브젝트 인덱스
    public bool NoNeedCheckCurrentIndex;           //?
    public int MainBeltIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();           //InstallableObjectAct 호출
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();       //GoodsValue 호출
        CurrentJoinIndex = 0;      //합칠 인덱스
        AddonObject = null; //애드온 오브젝트
        TargetGoods = null; //타겟 물품 
        NoNeedCheckCurrentIndex = false;   //인덱스 확인 필요 여부
        CanProceed = false;     //진행가능 여부
        isInitialized = false;  //진행 시작 가능 여부

        DetectedObject = new GameObject[InputNumber];   //들어오는 벨트에 확인된 오브젝트들
        DetectSensor = new GameObject[InputNumber];        //들어오는 벨트 확인하는 센서들
        NextBelt = new GameObject[InputNumber];
        PrevBelt = null;
        PrevBelt2 = new GameObject[InputNumber];            //들어오는 벨트들
        NextBelt2 = null;
        

        for (int i = 0; i < InputNumber; i++)      //센서 오브젝트 받아오기
        {
            // 0 ; Left
            // 1 : Right
            DetectSensor[i] = transform.GetChild(2).GetChild(0).GetChild(i).gameObject;        //센서 호출
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjectActCall.isInstall)            //오브젝트 설치되었는지 여부
        {
            GetBelt();                          //벨트 초기화

            if (isInitialized)                  //초기화 가능
            {
                ObjectActCall.IsWorking = true; //작업중 여부 = true

                if (CanProceed)                 //진행 가능
                {
                    //foreach(Prev)

                    if (MainBeltActCall.GoodsOnBelt != null)                //메인 이전 벨트에 물품 존재
                    {
                        if (MainBeltActCall.GoodsOnBelt == TargetGoods)     // 그 물품이 타겟 물품일때
                        {
                            if (MainBeltActCall.isCenter(-1))               //메인 벨트가 방향 일치일때
                            {
                                Joining();                             //합치기 실행
                            }
                        }
                    }
                }

                else                                                    //진행 불가(?)
                {
                    if (NextBeltActCall.GoodsOnBelt != null)            //출구벨트에 물건 있음
                    {
                        WhereToGo();
                    }
                }
            }
        }
        else
        {
            ObjectActCall.CanInstall = CheckInstallCondition();
        }
    }

    void CheckDirection()                       //오브젝트 방향 확인 함수
    {
        if (gameObject.transform.eulerAngles.y == 0)
        {
            JoinerDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            JoinerDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            JoinerDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            JoinerDirection = 3;
        }
    }

    //올바르게 설치되었는지 여부 출력 함수
    bool CheckInstallCondition()
    {
        bool result = true;
        int NextBeltCount = 0;
        int NextBeltIndex = -1;     //들어오는 2개 벨트중 이후벨트가 연결되어있는 벨트 인덱스

        CheckDirection();

        for (int i = 0; i < InputNumber; i++)  //들어오는 벨트마다
        {
            GameObject DetectedBelt = DetectSensor[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;        //부착된 오브젝트 불러오기(벨트)
            if (DetectedBelt != null)      //연결된 벨트 오브젝트 있음
            {
                BeltAct BeltActCall = DetectedBelt.GetComponent<BeltAct>();// 벨트 오브젝트 스크립트 불러오기

                if (BeltActCall.BeltDirection != JoinerDirection)   //1. 방향 다르면 실패
                {
                    result = false;
                    break;
                }

                if (BeltActCall.NextBelt != null)                   //2. 연결된 벨트의 이후 벨트 존재 => Joiner 이전에 연결되어있던 벨트 존재
                {
                    if (NextBeltCount > 0)                          //2-1.이후 벨트가 이미 세팅되어있으면 => 이후 벨트가 2개 이상시 실패
                    {
                        result = false;                             
                        NextBeltIndex = -1;
                        break;
                    }

                                                                    //연결된 벨트 이후 벨트 1개
                    BeltAct NextBeltAct = BeltActCall.NextBelt.GetComponent<BeltAct>(); //이후 벨트 스크립트

                    if (NextBeltAct.BeltDirection != JoinerDirection)       //이후 벨트 방향 확인
                    {
                        result = false;
                        break;
                    }

                    if (NextBeltAct.NextBelt != null)                       //이후 벨트의 이후 벨트 존재
                    {
                        if (NextBeltAct.PrevBelt.GetComponent<BeltAct>().BeltDirection != JoinerDirection)  //방향 확인
                        {
                            result = false;
                            break;
                        }
                    }

                    NextBeltIndex = i;
                    NextBeltCount++;
                }
            }                
            else        //연결된 벨트 없음
            {
                result = false;
                break;
            }
        }

        if (NextBeltIndex != -1)        //이후벨트 연결된게 있음
        {
            Transform StructCarrier = ObjectActCall.StructObject.transform.parent;
            string StructName = "Joiner" + InputNumber.ToString() + "-" + NextBeltIndex.ToString();
            if (ObjectActCall.StructObject.name != StructName)
            {
                GameObject newStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/InstallableObject/Struct/" + StructName), StructCarrier);
                newStruct.name = StructName;
                Destroy(ObjectActCall.StructObject);
                ObjectActCall.StructObject = newStruct;
            }
        }
        else                            //
        {
            result = false;
        }

        return result;
    }

    void GetBelt()
    {
        bool CanInitialize = true;              //첫 실행 가능 여부 = true

        for (int i = 0; i < InputNumber; i++)   //들어오는 벨트별로 반복
        {
            DetectedObject[i] = DetectSensor[i].GetComponent<ObjectAttachmentDetector>().DetectedObject;       //센서에 감지된 오브젝트 인식
            BeltAct TargetBeltAct = DetectedObject[i].GetComponent<BeltAct>();  //감지된 오브젝트 target으로 호출

            TargetBeltAct.ModuleObject = gameObject;    //벨트의 module에 joiner 입력

            if (TargetBeltAct.NextBelt != null)         //출력벨트 이후벨트 존재( = 메인벨트)
            {
                NextBelt2 = TargetBeltAct.NextBelt;      //이후 벨트 호출
                NextBeltActCall = NextBelt2.GetComponent<BeltAct>();
                NextBeltActCall.ModuleObject = gameObject;      //이전 벨트의 모듈에 this입력

                MainBeltIndex = i;                       //이후 벨트가 연결되어 있는 벨트를 메인벨트로 선언
                MainBeltActCall = TargetBeltAct;         //메인벨트스크립트 호출
            }

            PrevBelt2[i] = TargetBeltAct.PrevBelt;       //벨트에 추가
            if (TargetBeltAct.PrevBelt == null) CanInitialize = false;  //이전 벨트 없으면 첫 실행 불가
        }

        for (int i = 0; i < InputNumber; i++)           //들어오는 벨트별로 반복
        {
            DetectedObject[i].GetComponent<BeltAct>().NextBelt = NextBelt2;      //이후벨트 하나로 입력
        }

        isInitialized = CanInitialize;
    }

    void SetCurrentJoinIndex()
    {
        if (AddonObject == null)
        {
            for (int i = 0; i < DetectedObject.Length; i++)     //출력 벨트 내에서 반복
            {
                int Nextindex = CurrentJoinIndex + i;       //개수 내에서 반복
                if (Nextindex >= DetectedObject.Length)          //
                {
                    Nextindex -= DetectedObject.Length;
                }


                BeltAct indexBeltActCall = DetectedObject[Nextindex].GetComponent<BeltAct>();        //출력벨트

                if (Nextindex == MainBeltIndex)                                             //바뀔 벨트가 메인벨트
                {
                    BeltAct nextIndexBeltActCall = NextBelt[Nextindex].GetComponent<BeltAct>();             //*+ : 출력벨트 배열과 센서에 탐지된 벨트 배열 목록이 동일한가?
                    if (indexBeltActCall.GoodsOnBelt == null)       //바뀔벨트에 물품 없음
                    {
                        if (nextIndexBeltActCall.GoodsOnBelt == null)   
                        {
                            CurrentJoinIndex = Nextindex;           //벨트 변경
                            CanProceed = true;                      //진행 가능
                            return;
                        }
                        else                                        //다음벨트에 물품 있음
                        {
                            if (!nextIndexBeltActCall.isStop)       //다음 벨트 진행중
                            {
                                CurrentJoinIndex = Nextindex;       
                                CanProceed = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!indexBeltActCall.isStop)
                        {
                            if (!nextIndexBeltActCall.isStop)
                            {
                                CurrentJoinIndex = Nextindex;
                                CanProceed = true;
                                return;
                            }
                        }
                    }
                }
                else                                                                    //메인벨트 아님
                {
                    if (indexBeltActCall.GoodsOnBelt == null)                           //물품 없음
                    {
                        CurrentJoinIndex = Nextindex;                                              
                        CanProceed = true;
                        return;
                    }
                    else                                                                //물품 있음
                    {
                        if (!indexBeltActCall.isStop)                                   //출력 벨트 멈추지 않았음
                        {
                            CurrentJoinIndex = Nextindex;                                      
                            CanProceed = true;
                            return;
                        }
                    }
                }
            }
            CanProceed = false;
            return;
        }
        else        //애드온 존재
        {
            if (DetectedObject[CurrentJoinIndex].GetComponent<BeltAct>().GoodsOnBelt == null)   //현재 벨트에 물품 없으면 진행 가능
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
        NextBeltActCall.ChangeNeedStop(true, gameObject);       //이전 벨트 정지
        if (!NoNeedCheckCurrentIndex)                                          //
        {
            if (NextBeltActCall.GoodsOnBelt != null)            //이전벨트에 물품 존재
            {
                if (NextBeltActCall.GoodsOnBelt != TargetGoods) //타겟 물품이 아님
                {
                    TargetGoods = NextBeltActCall.GoodsOnBelt;  //타겟 변경
                    if (AddonObject != null)                    //애드온 세팅
                    {
                        AddonObject.GetComponent<AddonObjectAct>().Tic = true;
                    }
                    else
                    {
                        NoNeedCheckCurrentIndex = true;                        
                    }
                }
            }

            CanProceed = false;                                 //진행 불가
            return;
        }
        else
        {
            SetCurrentJoinIndex();       //??
        }
    }

    void Joining()
    {
        if (MainBeltIndex != CurrentJoinIndex)     //현재 진행되는 벨트가 메인벨트가 아님
        {
            MainBeltActCall.GoodsOnBelt.transform.position =
                new Vector3(NextBelt2.transform.position.x, DetectedObject[CurrentJoinIndex].transform.position.y + 0.325f, NextBelt2.transform.position.z);      //메인벨트의 물품 이동
            DetectedObject[CurrentJoinIndex].GetComponent<BeltAct>().GoodsOnBelt = MainBeltActCall.GoodsOnBelt;     //이전의 벨트의 물품 = 
            MainBeltActCall.GoodsOnBelt = null;

            TargetGoods = null;
            NoNeedCheckCurrentIndex = false;
            CanProceed = false;
        }

        else
        {
            TargetGoods = null;
            NoNeedCheckCurrentIndex = false;
            CanProceed = false;
        }

        if (AddonObject == null)
        {
            CurrentJoinIndex += 1;
            if (CurrentJoinIndex >= DetectedObject.Length)
            {
                CurrentJoinIndex = 0;
            }
        }
    }

    public bool DeleteObject()
    {
        if (AddonObject != null)
        {
            Destroy(AddonObject);
        }

        for (int i = 0; i < InputNumber; i++)
        {
            if (i != MainBeltIndex) DetectedObject[i].GetComponent<BeltAct>().PrevBelt = null;

            DetectedObject[i].GetComponent<BeltAct>().ChangeNeedStop(false, gameObject);
            DetectedObject[i].GetComponent<BeltAct>().ModuleObject = null;
        }

        if (PrevBelt != null)
        {
            NextBeltActCall.ChangeNeedStop(false, gameObject);
            NextBeltActCall.ModuleObject = null;
        }

        return true;
    }
}
