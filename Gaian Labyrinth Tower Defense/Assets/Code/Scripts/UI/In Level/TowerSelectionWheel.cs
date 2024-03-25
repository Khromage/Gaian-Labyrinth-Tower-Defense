using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionWheel : MonoBehaviour
{

    public delegate void TowerWheelSelection(int towerID);
    public static event TowerWheelSelection OnTowerSelected;

    [SerializeField]
    private TowerList towerList;
    private int[] EquippedTowerIDs;
    [SerializeField]
    private GameObject[] TowerSelectionSlots;
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

    public void HighlightSlot(GameObject Slot)
    {
        Slot.transform.localScale *= 1.1f;
        Debug.Log("Mouse entered Tower Selection Slot. The tower in this slot is: " + towerList.GetTower(Array.IndexOf(TowerSelectionSlots, Slot)).name);
    }
    public void UnHighlightSlot(GameObject Slot)
    {
        Slot.transform.localScale /= 1.1f;
        Debug.Log("Mouse exited Tower Selection Slot. Resetting scale");
    }

    public void SlotSelected(int slotIndex)
    {
        // int towerID = EquippedTowerIDs[slotIndex];
        OnTowerSelected?.Invoke(slotIndex);
    }

}
