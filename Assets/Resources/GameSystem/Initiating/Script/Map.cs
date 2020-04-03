using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject MapObject;

    // Start is called before the first frame update
    void Start()
    {
        SetMapSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMapSize()
    {
        MapObject.transform.localPosition = new Vector3(0, 0, 0);
        int size = TopValue.TopValueSingleton.MapSize;
        MapObject.transform.localScale = new Vector3(size, 1, size);

        MapObject.GetComponent<PlacingGroup>().Initializing();
    }
}
