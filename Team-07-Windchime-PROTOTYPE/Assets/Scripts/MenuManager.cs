using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Canvas debugUI;
    
    TMP_Dropdown soupDropdown;
    TMP_Dropdown colourDropdown;
    TMP_Dropdown meatVegDropdown;

    Toggle spicyToggle;
    Toggle chunkyToggle;

    TextMeshProUGUI orderCreatedText;
    float orderCreatedTextTimer;

    // Current order text stuff //
    TextMeshProUGUI soup;
    TextMeshProUGUI colour;
    TextMeshProUGUI meatVeg;
    TextMeshProUGUI spicy;
    TextMeshProUGUI chunky;

    // Player UI stuff //
    TextMeshProUGUI heldItemText;
    TextMeshProUGUI selectedItemText;
    TextMeshProUGUI selectedApplianceText;

    // Start is called before the first frame update
    void Start()
    {
        heldItemText = debugUI.transform.Find("heldItem").GetComponent<TextMeshProUGUI>();
        selectedItemText = debugUI.transform.Find("selectedItem").GetComponent<TextMeshProUGUI>();
        selectedApplianceText = debugUI.transform.Find("selectedAppliance").GetComponent<TextMeshProUGUI>();


        Transform soupOrganiser = orderUI.transform.Find("SoupStuff");
        Transform orderOrganiser = orderUI.transform.Find("OrderCreationStuff");
        Transform currentOrderOrganiser = orderUI.transform.Find("OrderList");

        soup = currentOrderOrganiser.Find("soupName").GetComponent<TextMeshProUGUI>();
        colour = currentOrderOrganiser.Find("colourPreference").GetComponent<TextMeshProUGUI>();
        meatVeg = currentOrderOrganiser.Find("meatVegPref").GetComponent<TextMeshProUGUI>();
        spicy = currentOrderOrganiser.Find("spicy").GetComponent<TextMeshProUGUI>();
        chunky = currentOrderOrganiser.Find("chunky").GetComponent<TextMeshProUGUI>();

        orderCreatedText = orderOrganiser.Find("orderCreatedText").GetComponent<TextMeshProUGUI>();
        orderCreatedText.gameObject.SetActive(false);


        Transform soup1Parent = soupOrganiser.transform.Find("soup1Name");
        Transform soup2Parent = soupOrganiser.transform.Find("soup2Name");
        Transform soup3Parent = soupOrganiser.transform.Find("soup3Name");

        soupDropdown = orderOrganiser.Find("soupDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI soupDropdownLabel = soupDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        colourDropdown = orderOrganiser.Find("colourDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI colourDropdownLabel = colourDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        meatVegDropdown = orderOrganiser.Find("meatVegDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI meatVegDropdownLabel = meatVegDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        spicyToggle = orderOrganiser.Find("spicyToggle").GetComponent<Toggle>();
        chunkyToggle = orderOrganiser.Find("chunkyToggle").GetComponent<Toggle>();

        soupOrganiser.transform.Find("soup1Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].soupName;
        soup1Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core1.name.ToString();
        soup1Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core2.name.ToString();
        soup1Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].core3.name.ToString();
        
        soupOrganiser.transform.Find("soup2Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].soupName;
        soup2Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core1.name.ToString();
        soup2Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core2.name.ToString();
        soup2Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].core3.name.ToString();
        
        soupOrganiser.transform.Find("soup3Name").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].soupName;
        soup3Parent.transform.Find("core1").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core1.name.ToString();
        soup3Parent.transform.Find("core2").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core2.name.ToString();
        soup3Parent.transform.Find("core3").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].core3.name.ToString();

        soupDropdown.options.Clear();
        soupDropdownLabel.text = "Soup Recipe";
        
        colourDropdown.options.Clear();
        colourDropdownLabel.text = "None";

        meatVegDropdown.options.Clear();
        meatVegDropdownLabel.text = "None";

        PopulateSoupDropdownOptions(soupDropdown);
        PopulateColourDropdownOptions(colourDropdown);
        PopulateMeatVegDropdownOptions(meatVegDropdown);


    }
   
    // Update is called once per frame
    void Update()
    {
        MenuState();


        // Making the "Order Created" text disappear after a while.
        orderCreatedTextTimer += Time.deltaTime;

        if (orderCreatedTextTimer >= 2)
        {
            orderCreatedText.gameObject.SetActive(false);
        }

        // Display player UI stuff // 
        DisplayHeldItem();
        DisplaySelectedAppliance();
        DisplaySelectedItem();
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
                    Cursor.lockState = CursorLockMode.Locked;
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
                    Cursor.lockState = CursorLockMode.Locked;
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
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("None"));
    }

    void PopulateMeatVegDropdownOptions(TMP_Dropdown dropDownBox)
    {
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("None"));
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("No Meat"));
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("No Greens"));
    }


    public void CreateOrder()
    {
        OrderManager.AddOrder(Order.CreateOrder(soupDropdown, colourDropdown, meatVegDropdown, spicyToggle, chunkyToggle));
        orderCreatedText.gameObject.SetActive(true);
        orderCreatedTextTimer = 0;

        DisplayCurrentOrder(soup, colour, meatVeg, spicy, chunky);
          
    }

    void DisplayCurrentOrder(TextMeshProUGUI soup, TextMeshProUGUI colour, TextMeshProUGUI meatVeg, TextMeshProUGUI spicy, TextMeshProUGUI chunky)
    {
        soup.text = OrderManager.currentOrders[0].mainSoup.soupName;
        colour.text = OrderManager.currentOrders[0].colourPreference.name;

        if (!OrderManager.currentOrders[0].noMeat && !OrderManager.currentOrders[0].noVeg)
        {
            meatVeg.text = "Meat and Veg allowed";
        }
        else if (OrderManager.currentOrders[0].noMeat && !OrderManager.currentOrders[0].noVeg)
        {
            meatVeg.text = "Meat not allowed";
        }
        else if (OrderManager.currentOrders[0].noVeg && !OrderManager.currentOrders[0].noMeat)
        {
            meatVeg.text = "Veg not allowed";
        }

        switch (OrderManager.currentOrders[0].isSpicy)
        {
            case true:
                spicy.text = "Spicy";
                break;
            case false:
                spicy.text = "No spice";
                break;
        }

        switch (OrderManager.currentOrders[0].isChunky)
        {
            case true:
                chunky.text = "Chunky";
                break;
            case false:
                chunky.text = "No chunky";
                break;
        }
    }

    void DisplayHeldItem()
    {
        if (MouseLook.heldItem)
        {
            heldItemText.text = MouseLook.heldItem.name;
        }
        else
        {
            heldItemText.text = "None";
        }
    }

    void DisplaySelectedItem()
    {
        if (MouseLook.selectedItem)
        {
            selectedItemText.text = MouseLook.selectedItem.name;
        }
        else
        {
            selectedItemText.text = "None";
        }
    }

    void DisplaySelectedAppliance()
    {
        if (MouseLook.selectedAppliance)
        {
            selectedApplianceText.text = MouseLook.selectedAppliance.name;
        }
        else
        {
            selectedApplianceText.text = "None";
        }
    }

    




}


