using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingGroup : MonoBehaviour
{
    public GameObject TileGroupPrefab;
    public GameObject TileGroupContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initializing()
    {
        //Get map scale
        float Scale = TopValue.TopValueSingleton.MapSize / 2;
        //Basic Map Size is 2x2 TileGroup
        float BasicPos = 2.5f;

        //Make tile group fits map scale
        for (int i = 0; i < Scale * 2; i++)
        {
            for (int j = 0; j < Scale * 2; j++)
            {
                //Instatiate a new tile group
                GameObject newTileGroup = Instantiate(TileGroupPrefab);

                //Make tile group child of map
                //Because of object scale, setParent should be implemented at this position
                newTileGroup.transform.parent = TileGroupContainer.transform;
                GameObject TileStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Initiating/Struct/GrassLand" + Random.Range(1,6).ToString()), newTileGroup.transform);
                TileStruct.transform.position = new Vector3(0, -5f, 0);

                //Chang tile group name like TileGroup(0, 0)
                string TileGroupName = "TileGroup(" + i + ", " + j + ")";
                newTileGroup.transform.name = TileGroupName;
                
                Vector3 BasicPostion = new Vector3(BasicPos / Scale * (Scale * 2 - 1), 0, BasicPos / Scale * (Scale * 2 - 1));
                newTileGroup.transform.localPosition = BasicPostion + new Vector3(-1 * (i * 5 / Scale), 0, -1 * (j * 5 / Scale));
            }
        }

        transform.parent.gameObject.GetComponent<GroupActivation>().GroupInitialize();
    }
}