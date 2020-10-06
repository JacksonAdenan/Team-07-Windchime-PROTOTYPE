using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
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
    public Material itemSelectedMat;
    public Transform testCube;
    public Transform hand;
    public Canvas PickUpUI;
    // ------------------------------------------ //



    bool isHoldingItem = false;
    Vector3 handPos;
    public CameraMode currentCameraMode = CameraMode.lookMode;
    public static Transform selectedItem;
    public static Transform heldItem;
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

        Physics.Raycast(hand.position, hand.forward * 10, out raycastHit);
        Debug.DrawRay(hand.position, hand.forward * 10, Color.green);

        CameraState();


        SelectObj();
        
        DisplayPickupUI();
        

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
                if (Input.GetMouseButtonDown(1))
                {
                    currentCameraMode = CameraMode.handMode;
                }
                break;
            case CameraMode.handMode:
                CameraHandControl();
                if (Input.GetMouseButtonDown(1))
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
        handPos.y = Mathf.Clamp(handPos.y, -0.3f, 0.3f);
        handPos.x = Mathf.Clamp(handPos.x, -0.5f, 0.5f);
        hand.transform.localPosition = handPos;
    }

    void CameraPause()
    { 
        
    }

    void SelectObj()
    {
        
        
 
        if (IsLookingAtObj())
        {
            selectedItem = raycastHit.transform;
            defaultMat = selectedItem.GetComponent<Renderer>().material;
            selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
           
        }

        // If the player looks away from the item //

        else if (raycastHit.transform != selectedItem || raycastHit.transform == heldItem)
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
            }
            selectedItem = null;
        }
        
        
    }

    bool IsLookingAtObj()
    {
        if (raycastHit.transform != null && raycastHit.transform.CompareTag("Item") && selectedItem != raycastHit.transform && !isHoldingItem)
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
        itemToPickUp.localPosition = new Vector3(0, 0, -5);

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
            UIPos.y += PickUpUIPos;
            PickUpUI.transform.position = UIPos;
            PickUpUI.transform.LookAt(gameObject.transform);

        }
        else
        {
            PickUpUI.gameObject.SetActive(false);
        }
    }
}
