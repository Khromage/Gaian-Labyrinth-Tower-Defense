using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject BackToCombatLayout;
    [SerializeField]
    private GameObject HoveredTowerLayout;
    
    // Start is called before the first frame update
    void Start()
    {
        EquippedTowerIDs = LoadoutManager.Instance.GetTowerLoadout();

        for(int i=0; i < TowerSelectionSlots.Length; i++)
        {
            // if there is a tower in the slot, fill in icon image
            if(EquippedTowerIDs[i] != 1)
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

    // called when player cursor hovers a section of the screen corresponding to a slot
    public void HighlightSlot(GameObject Slot)
    {
        Slot.transform.localScale *= 1.1f;
        hoveredSlot = Array.IndexOf(TowerSelectionSlots, Slot.transform.parent.gameObject);
        Debug.Log("hovered slot index: " + hoveredSlot);
        Debug.Log("selected slot: " + Slot.name);
    }

    // called when the cursor leaves the slot area to reset hovered slot
    public void UnHighlightSlot(GameObject Slot)
    {
        Slot.transform.localScale /= 1.1f;
        hoveredSlot = -1;
        Debug.Log("Mouse exited Tower Selection Slot. Resetting scale");
    }

    // called when player releases Q
    public void SlotSelected()
    {
        OnTowerSelected?.Invoke(hoveredSlot);
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

}
