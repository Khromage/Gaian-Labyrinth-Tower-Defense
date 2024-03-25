using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void ActiveTowerChange(int newTypeID, int slotIndex);
    public static event ActiveTowerChange OnActiveTowerChange;

    [SerializeField]
    private TowerList towerList;

    private int towerID;

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
        var canvas = transform.parent.parent;
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
            towerID = raycastResult.gameObject.transform.parent.GetSiblingIndex(); //ID = sibling index of the button itself
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

            Debug.Log(raycastResult.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite);
            //changes the towerIcon child's sprite in the active tower slot button to the sprite of the tower type dragged in.
            raycastResult.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = towerList.GetTowerIcon(towerID);

            //canvasGroup.alpha = 1f;


            //ACTIVE TOWER SLOT CHANGED
            OnActiveTowerChange?.Invoke(towerID, raycastResult.gameObject.transform.GetSiblingIndex());
        }
    }
}
