using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingTile : MonoBehaviour
{

    public GameObject TilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Make 10 x 10 tiles
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j <= 9; j++)
            {
                //Instantiate a new tile
                GameObject newTile = Instantiate(TilePrefab);

                //Make tile child of TileGroup
                newTile.transform.parent = gameObject.transform.GetChild(0);

                //Change tile name like Tile(0, 0)
                string TileName = "Tile(" + i + ", " + j + ")";
                newTile.transform.name = TileName;

                //Position of First tile is (4.5, 0 , 4.5), Tile's width is 1
                Vector3 BasicPosition = new Vector3((float)4.5, 0, (float)4.5);
                newTile.transform.localPosition = BasicPosition + new Vector3(-i, -0.5f, -j);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}