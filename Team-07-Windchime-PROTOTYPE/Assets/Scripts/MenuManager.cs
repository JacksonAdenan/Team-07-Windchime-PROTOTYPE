using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum MenuState
{ 
    pauseMenu,
    orderMenu,
    none
}

public class MenuManager : MonoBehaviour
{
    private MenuState currentState = global::MenuState.none;

    public Transform playerCamera;

    public Canvas orderUI;
    public Canvas pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        Transform soup1Parent = orderUI.transform.Find("soup1Name");
        Transform soup2Parent = orderUI.transform.Find("soup2Name");
        Transform soup3Parent = orderUI.transform.Find("soup3Name");

        orderUI.transform.Find("soup1Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].soupName;
        soup1Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core1.name.ToString();
        soup1Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core2.name.ToString();
        soup1Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core3.name.ToString();

        orderUI.transform.Find("soup2Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].soupName;
        soup2Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core1.name.ToString();
        soup2Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core2.name.ToString();
        soup2Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core3.name.ToString();
        
        orderUI.transform.Find("soup3Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].soupName;
        soup3Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core1.name.ToString();
        soup3Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core2.name.ToString();
        soup3Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core3.name.ToString();


    }
   
    // Update is called once per frame
    void Update()
    {
        MenuState();
    }

    void MenuState()
    {
        switch (currentState)
        {
            case global::MenuState.pauseMenu:
                orderUI.gameObject.SetActive(false);
                pauseUI.gameObject.SetActive(true);
                playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.pauseMode;
                if (Input.GetKeyDown(KeyCode.P))
                {
                    currentState = global::MenuState.none;
                    playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.lookMode;
                }
                break;
            case global::MenuState.orderMenu:
                orderUI.gameObject.SetActive(true);
                pauseUI.gameObject.SetActive(false);
                playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.pauseMode;
                if (Input.GetKeyDown(KeyCode.O))
                {
                    currentState = global::MenuState.none;
                    playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.lookMode;
                }
                break;
            case global::MenuState.none:        
                orderUI.gameObject.SetActive(false);
                pauseUI.gameObject.SetActive(false);

                if (Input.GetKeyDown(KeyCode.O))
                {
                    currentState = global::MenuState.orderMenu;
                }
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    currentState = global::MenuState.pauseMenu;
                }
                break;
            
        }
    }
}


