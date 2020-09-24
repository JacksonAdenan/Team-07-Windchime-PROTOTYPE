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
    public Transform playerBody;

    public Transform hand;
    Vector3 handPos;

    float xRotation = 0.0f;

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
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            
            
            Vector3 handMovement = transform.right * mouseX + transform.up * mouseY;



            //handMovement.x = Mathf.Clamp(handMovement.x, -0.5f, 0.5f);
            //handMovement.y = Mathf.Clamp(handMovement.y, -0.5f, 0.5f);
            hand.transform.position += handMovement * Time.deltaTime;

            handPos = hand.transform.localPosition;
            handPos.z = Mathf.Clamp(handPos.z, 0.7f, 0.7f);
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
