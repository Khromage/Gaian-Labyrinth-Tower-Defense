using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWeaponDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void ActiveWeaponChange(int newTypeID, int slotIndex);
    public static event ActiveWeaponChange OnActiveWeaponChange;

    [SerializeField]
    private WeaponList weaponList;

    private int weaponID;

    CanvasGroup canvasGroup;
    public string dropSlotTag = "ActiveWeaponSlot";

    private GameObject dragIcon;


    public void OnDrag(PointerEventData eventData)
    {
        dragIcon.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //maybe unnecessary? Or could be done better than parent.parent.parent...
        var canvas = transform.parent.parent.parent.parent;
        if (canvas == null)
            return;

        dragIcon = new GameObject("dragIcon");

        RaycastResult raycastResult = eventData.pointerCurrentRaycast;

        if (raycastResult.gameObject.tag == "DragElement")
        {
            dragIcon.transform.SetParent(canvas.transform, false);
            dragIcon.transform.SetAsLastSibling();

            var image = dragIcon.AddComponent<Image>();

            image.sprite = raycastResult.gameObject.GetComponent<Image>().sprite;
            weaponID = raycastResult.gameObject.transform.parent.GetSiblingIndex(); //ID = sibling index of the button itself
        }
        else
        {
            return;
        }


        Debug.Log(raycastResult.gameObject.name);
        //image.SetNativeSize();

        if (dragIcon.GetComponent<CanvasGroup>() == null)
        {
            dragIcon.AddComponent<CanvasGroup>();
        }
        canvasGroup = dragIcon.GetComponent<CanvasGroup>();

        //image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Ending drag");

        RaycastResult raycastResult = eventData.pointerCurrentRaycast;
        Debug.Log($"raycastResult: {raycastResult}");

        Destroy(dragIcon);

        if (raycastResult.gameObject.tag == dropSlotTag)
        {
            Debug.Log($"Dropping on {dropSlotTag}");

            //GameObject temp = Instantiate(new GameObject("tempActiveTowerIcon"), raycastResult.gameObject.transform.position, raycastResult.gameObject.transform.rotation, raycastResult.gameObject.transform.GetChild(0));
            //temp.transform.SetSiblingIndex(1);
            //temp.AddComponent<Image>().sprite = dragIcon.GetComponent<Image>().sprite;

            //Debug.Log(raycastResult.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite);
            //changes the towerIcon child's sprite in the active tower slot button to the sprite of the tower type dragged in.
            raycastResult.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = weaponList.GetWeaponIcon(weaponID);

            //canvasGroup.alpha = 1f;


            //ACTIVE WEAPON SLOT CHANGED
            OnActiveWeaponChange?.Invoke(weaponID, raycastResult.gameObject.transform.GetSiblingIndex());
        }
    }
}
