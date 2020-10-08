﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CameraMode
{ 
    lookMode,
    handMode,
    pauseMode
}

public enum InteractMode
{ 
    holdingItem,
    lookingAtItem,
    appliance
}
public class MouseLook : MonoBehaviour
{
    // Adjustable Values //
    // ------------------------------------------ //
    public float mouseSensitivity = 100f;
    public float handControlSensitivity = 100f;
    public float handZDistance = 0.7f;
    public float handYCeilingLimit = 0.0f;
    public float handYFloorLimit = 0.0f;
    public float handXLeftLimit = 0.0f;
    public float handXRightLimit = 0.0f;
    public float PickUpUIYPos = 0.0f;
    public float ApplianceUIZPos = 3.0f;
    public float ApplianceUIYPos = 5.0f;
    public float handCollisionRadius = 0;
    public float heldItemPosX = 0;
    public float heldItemPosY = 0;
    public float heldItemPosZ = 0;

    // ------------------------------------------ //

    // Inspector Variables //
    // ------------------------------------------ //
    public Transform playerBody;
    public Material itemSelectedMat;
    

    public Transform hand;
    public Transform collisionSphere;

    public Canvas PickUpUI;
    public Canvas ApplianceUI;
    // ------------------------------------------ //



    bool isHoldingItem = false;
    Vector3 handPos;
    public CameraMode currentCameraMode = CameraMode.lookMode;
    public static Transform selectedItem;
    public static Transform selectedAppliance = null;
    public static Transform heldItem = null;
    private Material defaultMat;
    float xRotation = 0.0f;

    Transform insertText = null;
    Transform notHoldingText = null;

    // Different raycasts //
    RaycastHit raycastFromHand;
    RaycastHit raycastFromScreen;
    Collider[] collisions;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        
    }

    // Update is called once per frame
    void Update()
    {
        // Doing raycast from hand //
        Physics.Raycast(hand.position, hand.forward * 10, out raycastFromHand);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, Color.blue);

        // Doing raycast from screen //
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * 5, out raycastFromScreen, 5);

        // Doing sphere check //
        collisions = Physics.OverlapSphere(collisionSphere.position, handCollisionRadius);

        CameraState();


        //SelectObj();
        
        DisplayPickupUI();
        DisplayApplianceIU();

        NewSelectObj();

        if (Input.GetKeyDown(KeyCode.E) && !isHoldingItem && selectedItem)
        {
            PickUpItem(selectedItem);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isHoldingItem)
        {
            DropItem();
        }

    }
    void CameraState()
    {
        switch (currentCameraMode)
        {
            case CameraMode.lookMode:
                CameraLook();
                if (Input.GetMouseButton(1))
                {
                    currentCameraMode = CameraMode.handMode;
                }
                break;
            case CameraMode.handMode:
                CameraHandControl();
                if (Input.GetMouseButtonUp(1))
                {
                    currentCameraMode = CameraMode.lookMode;
                }
                break;
            case CameraMode.pauseMode:
                CameraPause();
                Cursor.lockState = CursorLockMode.None;
                
                break;
        }
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void CameraHandControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * handControlSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * handControlSensitivity * Time.deltaTime;

        Vector3 handMovement = transform.right * mouseX + transform.up * mouseY;


        hand.transform.position += handMovement * Time.deltaTime;

        handPos = hand.transform.localPosition;
        
        handPos.z = Mathf.Clamp(handPos.z, handZDistance, handZDistance);
        handPos.y = Mathf.Clamp(handPos.y, handYFloorLimit, handYCeilingLimit);
        handPos.x = Mathf.Clamp(handPos.x, handXLeftLimit, handXRightLimit);


        hand.transform.localPosition = handPos;
    }

    void CameraPause()
    { 
        
    }

    void SetSelectedItem(Transform itemToSelect)
    {
        selectedItem = itemToSelect;
        defaultMat = selectedItem.GetComponent<Renderer>().material;
        selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
    }

    void RemoveSelectedItem()
    {
        selectedItem.GetComponent<Renderer>().material = defaultMat;
        selectedItem = null;
    }

    void SelectObj()
    {
        
        // Item selection stuff //
        // -------------------------------------------------------- //   
        if (IsLookingAtItem())
        {
            selectedItem = raycastFromHand.transform;
            defaultMat = selectedItem.GetComponent<Renderer>().material;
            selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
           
        }

        // If the player looks away from the item //

        else if (raycastFromHand.transform != selectedItem || raycastFromHand.transform == heldItem)
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
            }
            selectedItem = null;
        }
        // -------------------------------------------------------- //  

        // Appliance selection stuff //
        // -------------------------------------------------------- // 
        if (IsLookingAtAppliance())
        {
            selectedAppliance = raycastFromScreen.transform;
        }

        else if (raycastFromScreen.transform != selectedAppliance)
        {
            selectedAppliance = null;
        }


    }

    void NewSelectObj()
    {

        if (NewIsLookingAtItem())
        {
            selectedItem = NewIsLookingAtItem();

            // This if statement is a cheap fix for a badly written overall system... FIX IT ONE DAY //
            if (!defaultMat)
            { 
                defaultMat = selectedItem.GetComponent<Renderer>().material;
            }
            selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
        }
        

        else
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
            }
            selectedItem = null;
        }

    }

    Transform NewIsLookingAtItem()
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].gameObject.tag == "Item" && collisions[i].gameObject)
            {
                return collisions[i].transform;

            }
        }

        return null;
    }

    bool IsLookingAtItem()
    {
        if (raycastFromHand.transform != null && raycastFromHand.transform.CompareTag("Item") && selectedItem != raycastFromHand.transform && !isHoldingItem)
        {
            return true;
        }
        return false;
    }

    void PickUpItem(Transform itemToPickUp)
    {
        isHoldingItem = true;
        heldItem = itemToPickUp;
        itemToPickUp.SetParent(hand);
        itemToPickUp.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

        itemToPickUp.GetComponent<Rigidbody>().useGravity = false;
        itemToPickUp.GetComponent<Rigidbody>().isKinematic = true;
    }
    void DropItem()
    {
        isHoldingItem = false;

        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;

        heldItem.parent = null;
        heldItem = null;
    }

    void DisplayPickupUI()
    {
        if (selectedItem)
        {
            if (!isHoldingItem)
            {
                PickUpUI.gameObject.SetActive(true);
            }
            else
            {
                PickUpUI.gameObject.SetActive(false);
            }

            Vector3 UIPos = selectedItem.position;
            UIPos.y += PickUpUIYPos;
            PickUpUI.transform.position = UIPos;
            PickUpUI.transform.LookAt(gameObject.transform);

        }
        else
        {
            PickUpUI.gameObject.SetActive(false);
        }
    }

    bool IsLookingAtAppliance()
    {
        if (raycastFromScreen.transform != null && raycastFromScreen.transform.CompareTag("Appliance"))
        {
            return true;
        }
        return false;
    }

    void DisplayApplianceIU()
    {
        Vector3 applianceUIPos;
        if (insertText == null && notHoldingText == null)
        {
            notHoldingText = ApplianceUI.transform.Find("notHoldingText");
            insertText = ApplianceUI.transform.Find("insertText");
        }
        if (selectedAppliance)
        {
            
            if (selectedAppliance.parent != null)
            {
                applianceUIPos = selectedAppliance.parent.position;
            }
            else
            {
                applianceUIPos = selectedAppliance.position;
            }

            // Adjusting the position based on the inspector values //
            applianceUIPos.y += ApplianceUIYPos;
            applianceUIPos.z += ApplianceUIZPos;


            // Applying new position and constantly making the UI face the player. //
            ApplianceUI.transform.position = applianceUIPos;
            ApplianceUI.transform.LookAt(gameObject.transform);



            if (IsLookingAtAppliance() && !isHoldingItem)
            {
                notHoldingText.gameObject.SetActive(true);
                insertText.gameObject.SetActive(false);
            }
            else if (IsLookingAtAppliance() && isHoldingItem)
            {
                insertText.gameObject.SetActive(true);
                notHoldingText.gameObject.SetActive(false);
                insertText.GetComponent<TextMeshProUGUI>().text = "INSERT " + heldItem.name + " [E]";
            }
            else
            {
                notHoldingText.gameObject.SetActive(false);
                insertText.gameObject.SetActive(false);
            }
        }
        else 
        {
            notHoldingText.gameObject.SetActive(false);
            insertText.gameObject.SetActive(false);
        }
    }

    // Appliance interactions //

    void InsertItem()
    { }
    void RemoveItem()
    { }

    void ActivateAppliance()
    { }

}
