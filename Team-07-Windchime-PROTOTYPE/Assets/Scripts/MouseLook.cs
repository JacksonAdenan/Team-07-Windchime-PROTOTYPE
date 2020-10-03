﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraMode
{ 
    lookMode,
    handMode,
    pauseMode
}
public class MouseLook : MonoBehaviour
{
    // Adjustable Values //
    // ------------------------------------------ //
    public float mouseSensitivity = 100f;
    public float handControlSensitivity = 100f;
    public float handZDistance = 0.7f;
    public float PickUpUIPos = 0.0f;
    // ------------------------------------------ //

    // Inspector Variables //
    // ------------------------------------------ //
    public Transform playerBody;
    public Material selectedMat;
    public Transform testCube;
    public Transform hand;
    public Canvas PickUpUI;
    // ------------------------------------------ //


    Vector3 handPos;
    private CameraMode cameraMode = CameraMode.lookMode;
    private Transform selectedObj;
    private Material defaultMat;
    float xRotation = 0.0f;
    RaycastHit raycastHit;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraMode();
        Debug.DrawRay(hand.position, hand.forward * 10, Color.green);


        if (cameraMode == CameraMode.lookMode)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        else if (cameraMode == CameraMode.handMode)
        {
            float mouseX = Input.GetAxis("Mouse X") * handControlSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * handControlSensitivity * Time.deltaTime;

            Vector3 handMovement = transform.right * mouseX + transform.up * mouseY;


            hand.transform.position += handMovement * Time.deltaTime;

            handPos = hand.transform.localPosition;
            handPos.z = Mathf.Clamp(handPos.z, handZDistance, handZDistance);
            handPos.y = Mathf.Clamp(handPos.y, -0.3f, 0.3f);
            handPos.x = Mathf.Clamp(handPos.x, -0.5f, 0.5f);
            hand.transform.localPosition = handPos;
        }
        else if (cameraMode == CameraMode.pauseMode)
        { 
            
        }

        SelectObj();

        if (selectedObj)
        {
            PickUpUI.gameObject.SetActive(true);
            Vector3 UIPos = selectedObj.position;
            UIPos.y += PickUpUIPos;
            PickUpUI.transform.position = UIPos;
            PickUpUI.transform.LookAt(gameObject.transform);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem(selectedObj);
            }
        }
        else
        {
            PickUpUI.gameObject.SetActive(false);
        }

    }

    void SetCameraMode()
    {
        if (Input.GetMouseButton(1))
        {
            cameraMode = CameraMode.handMode;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraMode = CameraMode.pauseMode;
        }
        else if(Input.GetMouseButtonUp(1) && cameraMode != CameraMode.pauseMode)
        {
            cameraMode = CameraMode.lookMode;
        }
    }

    void SelectObj()
    {
        Physics.Raycast(hand.position, hand.forward * 10, out raycastHit);
        if (raycastHit.transform != null && raycastHit.transform.CompareTag("Item") && selectedObj != raycastHit.transform)
        {
            selectedObj = raycastHit.transform;
            defaultMat = selectedObj.GetComponent<Renderer>().material;
            selectedObj.GetComponent<Renderer>().material = selectedMat;
            Debug.Log("Looking at obj...");

            testCube.gameObject.GetComponent<Renderer>().material = defaultMat;
        }
        else if (raycastHit.transform != selectedObj)
        {
            Debug.Log("Not looking at obj...");
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Renderer>().material = defaultMat;
            }
            selectedObj = null;
        }
    }

    void PickUpItem(Transform itemToPickUp)
    {
        itemToPickUp.SetParent(hand);
        itemToPickUp.position = Vector3.zero;
    }
}
