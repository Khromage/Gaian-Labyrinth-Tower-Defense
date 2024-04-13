using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TowerSelectionWheel : MonoBehaviour
{

    public delegate void TowerWheelSelection(int hoveredSlotIndex);
    public static event TowerWheelSelection OnTowerSelected;

    [SerializeField]
    private TowerList towerList;
    private int[] EquippedTowerIDs;
    [SerializeField]
    private GameObject[] TowerSelectionSlots;

    [SerializeField]
    private int hoveredSlot;

    [SerializeField]
    private GameObject BackToGameLayout;
    private GameObject backToGameTile;
    [SerializeField]
    private GameObject HoveredTowerLayout;

    public TMP_Text hoveredName;
    public Image hoveredIcon;
    public TMP_Text hoveredDescription;
    public TMP_Text hoveredCost;

    
    // Start is called before the first frame update
    void Start()
    {
        EquippedTowerIDs = LoadoutManager.Instance.GetTowerLoadout();

        for(int i=0; i < TowerSelectionSlots.Length; i++)
        {
            // if there is a tower in the slot, fill in icon image
            if(EquippedTowerIDs[i] != -1)
            {
                // enable icon
                TowerSelectionSlots[i].transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                TowerSelectionSlots[i].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = towerList.GetTowerIcon(EquippedTowerIDs[i]);
            } else {
                // disable icon
                TowerSelectionSlots[i].transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighlightCenterTile(GameObject centerTile)
    {
        HoveredTowerLayout.SetActive(false);
        BackToGameLayout.SetActive(true);

        centerTile.transform.localScale *= 1.05f;
        hoveredSlot = -1;
        backToGameTile = centerTile;
    }

    public void UnHighlightCenterTile(GameObject centerTile)
    {
        centerTile.transform.localScale /= 1.05f;
        // Debug.Log("Resetting scale of CENTER");

    }
    
    
    // called when player cursor hovers a section of the screen corresponding to a slot
    public void HighlightSlot(GameObject Slot)
    {
        BackToGameLayout.SetActive(false);
        HoveredTowerLayout.SetActive(true);
        
        Slot.transform.localScale *= 1.05f;
        hoveredSlot = Array.IndexOf(TowerSelectionSlots, Slot);
        // Debug.Log("hoveredSlot: " + hoveredSlot);
        hoveredName.text = towerList.GetTowerName(EquippedTowerIDs[hoveredSlot]);
        hoveredIcon.sprite = towerList.GetTowerIcon(EquippedTowerIDs[hoveredSlot]);
        hoveredDescription.text = towerList.GetTowerDescription(EquippedTowerIDs[hoveredSlot]);
        hoveredCost.text = "Cost: " + towerList.GetTowerCost(EquippedTowerIDs[hoveredSlot]);
    }

    // called when the cursor leaves the slot area to reset hovered slot
    public void UnHighlightSlot(GameObject Slot)
    {
        Slot.transform.localScale /= 1.05f;
        // Debug.Log("Resetting scale of SLOT");
    }

    // called when player releases Q
    public void SlotSelected()
    {
        OnTowerSelected?.Invoke(hoveredSlot);
        if(hoveredSlot == -1)
        {
            UnHighlightCenterTile(backToGameTile);
        } else {
            UnHighlightSlot(transform.GetChild(hoveredSlot+1).gameObject);
        }

    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
    }

}
