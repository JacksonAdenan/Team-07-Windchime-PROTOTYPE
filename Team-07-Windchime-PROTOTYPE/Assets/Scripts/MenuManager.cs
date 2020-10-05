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
        Transform soupOrganiser = orderUI.transform.Find("SoupStuff");
        Transform orderOrganiser = orderUI.transform.Find("OrderCreationStuff");

        Transform soup1Parent = soupOrganiser.transform.Find("soup1Name");
        Transform soup2Parent = soupOrganiser.transform.Find("soup2Name");
        Transform soup3Parent = soupOrganiser.transform.Find("soup3Name");

        TMP_Dropdown soupDropdown = orderOrganiser.Find("soupDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI soupDropdownLabel = soupDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        TMP_Dropdown colourDropdown = orderOrganiser.Find("colourDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI colourDropdownLabel = colourDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        if (!soupOrganiser.transform.Find("soup1Name"))
        {
            Debug.Log("BROKEN");
        }
        if (!orderOrganiser.Find("soupDropdown").GetComponent<TMP_Dropdown>())
        {
            Debug.Log("BROKEN");
        }
        if (!soupDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>())
        {
            Debug.Log("BROKEN");
        }
        if (!orderOrganiser.Find("colourDropdown").GetComponent<TMP_Dropdown>())
        {
            Debug.Log("BROKEN");
        }
        if (!colourDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>())
        {
            Debug.Log("BROKEN");
        }
        if (CookingManager.allSoups.Count > 0)
        {
            Debug.Log("hello");
        }


        //soupOrganiser.transform.Find("soup1Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].soupName;
        //soup1Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core1.name.ToString();
        //soup1Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core2.name.ToString();
        //soup1Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core3.name.ToString();
        //
        //soupOrganiser.transform.Find("soup2Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].soupName;
        //soup2Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core1.name.ToString();
        //soup2Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core2.name.ToString();
        //soup2Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core3.name.ToString();
        //
        //soupOrganiser.transform.Find("soup3Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].soupName;
        //soup3Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core1.name.ToString();
        //soup3Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core2.name.ToString();
        //soup3Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core3.name.ToString();

        //soupDropdown.options.Clear();
        //soupDropdownLabel.text = "Soup Recipe";
        //
        //colourDropdown.options.Clear();
        //colourDropdownLabel.text = "Colour Preference";
        //
        //PopulateSoupDropdownOptions(soupDropdown);




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

    void PopulateSoupDropdownOptions(TMP_Dropdown dropDownBox)
    {
        for (int i = 0; i < CookingManager.allSoups.Count; i++)
        {
            
            dropDownBox.options.Add(new TMP_Dropdown.OptionData(CookingManager.allSoups[i].soupName));
        }
    }
    void PopulateColourDropdownOptions(TMP_Dropdown dropDownBox)
    {
        //for (int i = 0; i < CookingManager.allSoups.Count; i++)
        //{
        //
        //    dropDownBox.options.Add(new TMP_Dropdown.OptionData(CookingManager.allSoups[i].soupName));
        //}
    }
}


