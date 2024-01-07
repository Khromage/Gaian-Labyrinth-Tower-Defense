using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;
    public string dropSlotTag = "ActiveTowerSlot";

    private GameObject dragIcon;

    /*
    void Awake()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    */

    public void OnDrag(PointerEventData eventData)
    {
        dragIcon.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //maybe unnecessary? Or could be done better than parent.parent.parent...
        var canvas = transform.parent.parent.parent;
        if (canvas == null)
            return;


        dragIcon = new GameObject("icon");

        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        var image = dragIcon.AddComponent<Image>();
        image.sprite = GetComponent<Image>().sprite;
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
        if (raycastResult.gameObject?.tag == dropSlotTag)
        {
            //maybe instead, Destroy(dragIcon) and then replace the raycastResult.gameObject.GetComponent<Image>().sprite with a filled version

            Debug.Log($"Dropping on {dropSlotTag}");
            
            //place icon in slot and change transparency
            dragIcon.transform.position = raycastResult.gameObject.transform.position;

            GameObject temp = Instantiate(new GameObject("tempActiveTowerIcon"), raycastResult.gameObject.transform.position, raycastResult.gameObject.transform.rotation, raycastResult.gameObject.transform.GetChild(0));
            temp.transform.SetSiblingIndex(1);
            temp.AddComponent<Image>().sprite = dragIcon.GetComponent<Image>().sprite;
            

            //var image = dragIcon.GetComponent<Image>();
            //image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            canvasGroup.alpha = 1f;

            //ACTIVE TOWER SLOT CHANGED. maybe make an event
        }
        Destroy(dragIcon);
    }
}
