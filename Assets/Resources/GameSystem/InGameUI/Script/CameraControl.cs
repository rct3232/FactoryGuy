using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    ClickChecker ClickCheckerCall;
    float ZoomSpeed;
    float CamSize;
    float PanSpeed;
    float BorderThicknees;
    Vector3 DefaultCameraPosition;
    Vector3[] RotateArray;
    int CurrentRotate;

    // Start is called before the first frame update
    void Start()
    {
        ClickCheckerCall = GameObject.Find("BaseSystem").GetComponent<ClickChecker>();

        CamSize = Camera.main.orthographicSize;
        ZoomSpeed = 0;

        PanSpeed = 20f;
        BorderThicknees = Screen.height * 0.01f;

        DefaultCameraPosition = new Vector3(-30, 25, -30);
        RotateArray = new Vector3[4];
        CurrentRotate = 0;

        RotateArray[0] = new Vector3(30, 45, 0);
        RotateArray[1] = new Vector3(30, 135, 0);
        RotateArray[2] = new Vector3(30, 225, 0);
        RotateArray[3] = new Vector3(30, 315, 0);

        CameraReset();
    }

    // Update is called once per frame
    void Update()
    {
        CameraZoom();
        MoveCamera();
        CameraRotate();
        //Debug.Log(Input.mousePosition);
    }

    public void CameraReset()
    {
        Camera.main.orthographicSize = 20f;
        transform.rotation = Quaternion.Euler(RotateArray[0]);
        transform.position = DefaultCameraPosition;
    }

    void CameraZoom()
    {
        if(!ClickCheckerCall.MouseOnUI)
        {
            // Wheel Forward
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.Z))
            {
                ZoomSpeed = 5f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey(KeyCode.X))
            {
                ZoomSpeed = -5f;
            }

            if(ZoomSpeed != 0)
            {
                //if (Camera.main.orthographicSize < 6 & ZoomSpeed > 0)
                //    ZoomSpeed = ZoomSpeed * 0.1f;
                //else if(Camera.main.orthographicSize < 5 & ZoomSpeed < 0)
                //    ZoomSpeed = ZoomSpeed * 0.1f;
                //else
                //    ZoomSpeed = ZoomSpeed * 0.5f;

                //// Mathf.Abs means Absolute Value
                //if(Mathf.Abs(ZoomSpeed) < 0.001)
                //{
                //    ZoomSpeed = 0;
                //}

                ZoomSpeed = ZoomSpeed * 0.3f;

                // Mathf.Abs means Absolute Value
                if (Mathf.Abs(ZoomSpeed) < 0.001)
                {
                    ZoomSpeed = 0;
                }
            }

            CamSize = CamSize - ZoomSpeed;
            CamSize = Mathf.Clamp(CamSize, 1f, 50f);

            Camera.main.orthographicSize = CamSize;
            PanSpeed = CamSize;
        }
    }

    void MoveCamera()
    {
        Vector3 Pos = transform.position;

        if (/*Input.mousePosition.y >= Screen.height - BorderThicknees || */Input.GetKey(KeyCode.W))
        {
            if (CurrentRotate == 0)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 1)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 2)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 3)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
        }
        else if (/*Input.mousePosition.y <= BorderThicknees || */Input.GetKey(KeyCode.S))
        {
            if (CurrentRotate == 0)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 1)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 2)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 3)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
        }
        else if (/*Input.mousePosition.x >= Screen.width - BorderThicknees || */Input.GetKey(KeyCode.D))
        {
            if (CurrentRotate == 0)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 1)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 2)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 3)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
        }
        else if (/*Input.mousePosition.x <= BorderThicknees || */Input.GetKey(KeyCode.A))
        {
            if (CurrentRotate == 0)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 1)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z += PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 2)
            {
                Pos.x += PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
            else if (CurrentRotate == 3)
            {
                Pos.x -= PanSpeed * Time.deltaTime;
                Pos.z -= PanSpeed * Time.deltaTime;
            }
        }

        transform.position = Pos;
    }

    void CameraRotate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 Pos = transform.position;
            if (CurrentRotate == 3)
            {
                CurrentRotate = 0;
                transform.rotation = Quaternion.Euler(RotateArray[CurrentRotate]);

                Vector3 AddtionalPos = Pos - new Vector3(30, 25, -30);
                transform.position = new Vector3(-30, 25, -30) + AddtionalPos;
            }
            else
            {
                CurrentRotate++;
                transform.rotation = Quaternion.Euler(RotateArray[CurrentRotate]);

                if (CurrentRotate == 1)
                {
                    Vector3 AddtionalPos = Pos - new Vector3(-30, 25, -30);
                    transform.position = new Vector3(-30, 25, 30) + AddtionalPos;
                }
                else if (CurrentRotate == 2)
                {
                    Vector3 AddtionalPos = Pos - new Vector3(-30, 25, 30);
                    transform.position = new Vector3(30, 25, 30) + AddtionalPos;
                }
                else if (CurrentRotate == 3)
                {
                    Vector3 AddtionalPos = Pos - new Vector3(30, 25, 30);
                    transform.position = new Vector3(30, 25, -30) + AddtionalPos;
                }
            }
        }
    }
}
