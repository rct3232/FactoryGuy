using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupActivation : MonoBehaviour
{
    InGameValue ValueCall;
    ClickChecker ClickCheckerCall;
    TimeManager TimeManagerCall;
    NotificationManager NotificationManagerCall;
    CompanyValue PlayerCompanyValueCall;
    LandValue LandValueCall;
    public GameObject TileGroupContainer;
    GameObject[,] GroupArray;
    GameObject Camera;
    int[] TargetIndex;
    bool isSelectMode;
    Vector3 OriginalCameraPosition;
    Material GroundColorSelected;
    // Start is called before the first frame update
    void Start()
    {
        ValueCall = gameObject.GetComponent<InGameValue>();
        ClickCheckerCall = gameObject.GetComponent<ClickChecker>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        NotificationManagerCall = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        PlayerCompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        LandValueCall = PlayerCompanyValueCall.GetLandValue().GetComponent<LandValue>();
        isSelectMode = false;
        TargetIndex = new int[2] {0, 0};
        Camera = GameObject.Find("Main Camera");
        OriginalCameraPosition = new Vector3(-30, 25, -30);
        GroundColorSelected = Resources.Load<Material>("GameSystem/Initiating/Material/GroundColorSelected");
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelectMode)
        {
            if(!ValueCall.ModeBit[2])
            {
                ExitSelectMode();
            }

            if(ClickCheckerCall.target != GroupArray[TargetIndex[0], TargetIndex[1]])
            {
                GetTarget();
            }

            if(TargetIndex[0] != 0)
            {
                NotificationManagerCall.SetNote("$ " + LandValueCall.GetLandValue(((TargetIndex[0] - 1) * TopValue.TopValueSingleton.MapSize) + (TargetIndex[1] - 1)), new Color(1f,0.2f,0.2f));
                if(Input.GetMouseButtonDown(0))
                {
                    SetGroupActive();
                }
            }
        }
        else
        {
            if(ValueCall.ModeBit[2])
            {
                StartSelectMode();
            }
        }
    }

    public void GroupInitialize()
    {
        GroupArray = new GameObject[TopValue.TopValueSingleton.MapSize + 1, TopValue.TopValueSingleton.MapSize + 1];

        GroupArray[0, 0] = null;
        for(int i = 1; i < GroupArray.GetLength(0); i++)
        {
            for(int j = 1; j < GroupArray.GetLength(1); j++)
            {
                GroupArray[i, j] = TileGroupContainer.transform.GetChild((i - 1) * TopValue.TopValueSingleton.MapSize + (j - 1)).gameObject;
            }
        }
    }

    public void StartSelectMode()
    {
        // OriginalCameraPosition = Camera.transform.position;
        // Camera.transform.position = new Vector3(-30, 25, -30);
        isSelectMode = true;
    }

    public void ExitSelectMode()
    {
        // Camera.transform.position = OriginalCameraPosition;
        isSelectMode = false;
    }

    void GetTarget()
    {
        if(ClickCheckerCall.target != null)
        {
            if(TargetIndex[0] != 0)
            {
                if(!ValueCall.ActivatedGroup[TargetIndex[0] - 1, TargetIndex[1] - 1])
                {
                    Renderer SelectedGroupRenderer = GroupArray[TargetIndex[0], TargetIndex[1]].transform.GetChild(1).GetComponent<MeshRenderer>();
                    for(int i = 0; i < SelectedGroupRenderer.materials.Length; i++)
                    {
                        SelectedGroupRenderer.materials[i].color = new Color(1f,1f,1f,1f);
                    }
                }
            }

            for(int i = 0; i < GroupArray.GetLength(0); i++)
            {
                for(int j = 0; j < GroupArray.GetLength(1); j++)
                {
                    if(GroupArray[i, j] == ClickCheckerCall.target)
                    {
                        TargetIndex[0] = i;
                        TargetIndex[1] = j;
                        break;
                    }
                }
            }

            if(TargetIndex[0] == 0)
            {
                Debug.Log("Something is going wrong in GetTarget(). Cannot find Group in Array");
            }
            else
            {
                if(!ValueCall.ActivatedGroup[TargetIndex[0] - 1, TargetIndex[1] - 1])
                {
                    Renderer SelectedGroupRenderer = GroupArray[TargetIndex[0], TargetIndex[1]].transform.GetChild(1).GetComponent<MeshRenderer>();

                    for(int i = 0; i < SelectedGroupRenderer.materials.Length; i++)
                    {
                        SelectedGroupRenderer.materials[i].color = new Color(0.7f,0.7f,0.7f,1f);
                    }
                }
            }            
        }
        else
        {
            if(TargetIndex[0] != 0)
            {
                if(!ValueCall.ActivatedGroup[TargetIndex[0] - 1, TargetIndex[1] - 1])
                {
                    Renderer SelectedGroupRenderer = GroupArray[TargetIndex[0], TargetIndex[1]].transform.GetChild(1).GetComponent<MeshRenderer>();
                    for(int i = 0; i < SelectedGroupRenderer.materials.Length; i++)
                    {
                        SelectedGroupRenderer.materials[i].color = new Color(1f,1f,1f,1f);
                    }
                }
            }

            TargetIndex[0] = 0;
        }
    }

    void SetGroupActive()
    {
        if(!ValueCall.ActivatedGroup[TargetIndex[0] - 1, TargetIndex[1] - 1])
        {
            if(LandValueCall.BuyLand(TargetIndex[0] - 1, TargetIndex[1] - 1))
            {
                ValueCall.ActivatedGroup[TargetIndex[0] - 1, TargetIndex[1] - 1] = true;
                Destroy(GroupArray[TargetIndex[0], TargetIndex[1]].transform.GetChild(1).gameObject);
                GameObject NewLandStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Initiating/Struct/LandBase"), GroupArray[TargetIndex[0], TargetIndex[1]].transform);
            }
        }
        else
        {
            Debug.Log("This Group is already activated");
        }
    }

    public void ChangeGroupStruct(int x, int y, string StructName)
    {
        Destroy(GroupArray[x + 1, y + 1].transform.GetChild(1).gameObject);
        GameObject NewLandStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Initiating/Struct/" + StructName), GroupArray[x + 1, y + 1].transform);
    }

    public bool CheckActivatedGroup(GameObject Tile)
    {
        GameObject SelectedGroup = Tile.transform.parent.parent.gameObject;

        for(int i = 0; i < TopValue.TopValueSingleton.MapSize; i++)
        {
            for(int j = 0; j < TopValue.TopValueSingleton.MapSize; j++)
            {
                if(SelectedGroup == SelectedGroup.transform.parent.GetChild((i * TopValue.TopValueSingleton.MapSize) + j).gameObject)
                {
                    if(ValueCall.ActivatedGroup[i, j])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        Debug.Log("Something is going wrong in CheckActivatedGroup(). There is no " + Tile.name + " in GroupArray");
        return false;
    }

    public int[] GetTilePos(GameObject Tile)
    {
        int[] Result = new int[2] {-1, -1};
        int GroupIndexX = 0, GroupIndexY = 0;
        int TileIndexX = 0, TileIndexY = 0;
        
        GameObject SelectedGroup = Tile.transform.parent.parent.gameObject;

        for(int i = 0; i < TopValue.TopValueSingleton.MapSize; i++)
        {
            for(int j = 0; j < TopValue.TopValueSingleton.MapSize; j++)
            {
                if(SelectedGroup == SelectedGroup.transform.parent.GetChild((i * TopValue.TopValueSingleton.MapSize) + j).gameObject)
                {
                    GroupIndexX = i;
                    GroupIndexY = j;
                }
            }
        }

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if(Tile == SelectedGroup.transform.GetChild(0).GetChild(i * 10 + j).gameObject)
                {
                    TileIndexX = i;
                    TileIndexY = j;
                }
            }
        }

        Result[0] = GroupIndexX * 10 + TileIndexX;
        Result[1] = GroupIndexY * 10 + TileIndexY;

        return Result;
    }

    public Vector3 GetTilePhysicsPos(int[] TilePos)
    {
        Vector3 Result = new Vector3(0,0,0);
        GameObject Tile = GetTile(TilePos);
        
        if(Tile != null) Result = Tile.transform.position;

        return Result;
    }

    public GameObject GetTile(int[] TilePos)
    {
        int[] GroupIndex = new int[2];
        int[] TileIndex = new int[2];

        GroupIndex[0] = Mathf.FloorToInt(TilePos[0] / 10);
        GroupIndex[1] = Mathf.FloorToInt(TilePos[1] / 10);
        TilePos[0] = TilePos[0] % 10;
        TilePos[1] = TilePos[1] % 10;

        return TileGroupContainer.transform.GetChild(GroupIndex[0] * TopValue.TopValueSingleton.MapSize + GroupIndex[1]).GetChild(0).GetChild(TilePos[0] * 10 + TilePos[1]).gameObject;
    }

    public bool CheckInGroup(GameObject Tile, Vector3 Size, Vector3 EulerAngel)
    {
        bool result = true;

        int xSize, zSize;
        int mX, pX, mZ, pZ;
        if(EulerAngel.y == 0 || EulerAngel.y == 180)
        {
            xSize = Mathf.CeilToInt(Size.x);
            zSize = Mathf.CeilToInt(Size.z);
        }
        else
        {
            xSize = Mathf.CeilToInt(Size.z);
            zSize = Mathf.CeilToInt(Size.x);
        }

        if(xSize > 1)
        {
            if(xSize % 2 == 1)
            {
                mX = (xSize - 1) / 2;
                pX = (xSize - 1) / 2;
            }
            else
            {
                if(EulerAngel.y == 0 || EulerAngel.y == 90)
                {
                    mX = (xSize / 2) - 1;
                    pX = xSize / 2;
                }
                else
                {
                    mX = xSize / 2;
                    pX = (xSize / 2) - 1;
                }
            }
        }
        else
        {
            mX = 0;
            pX = 0;
        }

        if(zSize > 1)
        {
            if(zSize % 2 == 1)
            {
                mZ = (zSize - 1) / 2;
                pZ = (zSize - 1) / 2;
            }
            else
            {
                if(EulerAngel.y == 0 || EulerAngel.y == 90)
                {
                    mZ = (xSize / 2) - 1;
                    pZ = xSize / 2;
                }
                else
                {
                    mZ = zSize / 2;
                    pZ = (zSize / 2) - 1;
                }
            }
        }
        else
        {
            mZ = 0;
            pZ = 0;
        }

        int GroupIndexX = 0, GroupIndexY = 0;
        
        GameObject SelectedGroup = Tile.transform.parent.parent.gameObject;

        for(int i = 0; i < TopValue.TopValueSingleton.MapSize; i++)
        {
            for(int j = 0; j < TopValue.TopValueSingleton.MapSize; j++)
            {
                if(SelectedGroup == SelectedGroup.transform.parent.GetChild((i * TopValue.TopValueSingleton.MapSize) + j).gameObject)
                {
                    GroupIndexX = i;
                    GroupIndexY = j;
                }
            }
        }

        for(int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(Tile == Tile.transform.parent.GetChild((i * 10) + j).gameObject)
                {
                    if(i - mX < 0)
                    {
                        if(GroupIndexX - 1 >= 0)
                        {
                            if(j - mZ < 0)
                            {
                                if(GroupIndexY - 1 >= 0)
                                    result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                        GetChild(((GroupIndexX - 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY - 1).GetChild(0).GetChild(0).gameObject);
                                else
                                    result = false;
                            }
                            else if(j + pZ > 9)
                            {
                                if(GroupIndexY + 1 < TopValue.TopValueSingleton.MapSize)
                                    result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                        GetChild(((GroupIndexX - 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY + 1).GetChild(0).GetChild(0).gameObject);
                                else
                                    result = false;
                            }
                            else
                            {
                                result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                    GetChild(((GroupIndexX - 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY).GetChild(0).GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else if(j - mZ < 0)
                    {
                        if(GroupIndexY - 1 >= 0)
                        {
                            if(i + pX > 9)
                            {
                                if(GroupIndexX  + 1 < TopValue.TopValueSingleton.MapSize)
                                    result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                            GetChild(((GroupIndexX + 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY - 1).GetChild(0).GetChild(0).gameObject);
                                else
                                    result = false;
                            }
                            else
                            {
                                result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                    GetChild((GroupIndexX * TopValue.TopValueSingleton.MapSize) + GroupIndexY - 1).GetChild(0).GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            result = false;   
                        }
                    }
                    else if(i + pX > 9)
                    {
                        if(GroupIndexX + 1 < TopValue.TopValueSingleton.MapSize)
                        {
                            if(j - mZ < 0)
                            {
                                if(GroupIndexX - 1 >= 0)
                                    result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                            GetChild(((GroupIndexX + 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY + 1).GetChild(0).GetChild(0).gameObject);
                                else
                                    result = false;
                            }
                            else
                            {
                                result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                    GetChild(((GroupIndexX + 1) * TopValue.TopValueSingleton.MapSize) + GroupIndexY).GetChild(0).GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            result = false;   
                        }
                    }
                    else if(j + pZ > 9)
                    {
                        if(GroupIndexY + 1 < TopValue.TopValueSingleton.MapSize)
                            result = CheckActivatedGroup(SelectedGroup.transform.parent.
                                        GetChild((GroupIndexX * TopValue.TopValueSingleton.MapSize) + GroupIndexY + 1).GetChild(0).GetChild(0).gameObject);
                        else
                            result = false;
                    }
                }
            }
        }

        return result;
    }
}
