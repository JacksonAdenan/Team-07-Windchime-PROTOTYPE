using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraMode
{ 
    lookMode,
    handMode
}
public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public float handControlSensitivity = 100f;
    public float handZDistance = 0.7f;
    public Transform playerBody;
    public Material selectedMat;

    public Transform hand;
    Vector3 handPos;
    private Transform selectedObj;
    private Material defaultMat;
    float xRotation = 0.0f;
    RaycastHit raycastHit;
    private CameraMode cameraMode = CameraMode.lookMode;
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
            
            Physics.Raycast(hand.position, hand.forward * 10, out raycastHit);
            if (raycastHit.transform != null && raycastHit.transform.CompareTag("Item"))
            {
                selectedObj = raycastHit.transform;
                defaultMat = selectedObj.GetComponent<Renderer>().material;

            }
            else
            {
                selectedObj = null;
            }
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Renderer>().material = selectedMat;
            }
            else
            {
                selectedObj.GetComponent<Renderer>().material = defaultMat;
            }

            float mouseX = Input.GetAxis("Mouse X") * handControlSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * handControlSensitivity * Time.deltaTime;

            
            
            Vector3 handMovement = transform.right * mouseX + transform.up * mouseY;



            //handMovement.x = Mathf.Clamp(handMovement.x, -0.5f, 0.5f);
            //handMovement.y = Mathf.Clamp(handMovement.y, -0.5f, 0.5f);
            hand.transform.position += handMovement * Time.deltaTime;

            handPos = hand.transform.localPosition;
            handPos.z = Mathf.Clamp(handPos.z, handZDistance, handZDistance);
            handPos.y = Mathf.Clamp(handPos.y, -0.3f, 0.3f);
            handPos.x = Mathf.Clamp(handPos.x, -0.5f, 0.5f);
            hand.transform.localPosition = handPos;

            
            


        }

        

    }

    void SetCameraMode()
    {
        if (Input.GetMouseButton(1))
        {
            cameraMode = CameraMode.handMode;
        }
        else
        {
            cameraMode = CameraMode.lookMode;
        }
    }
}
