using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UIController;
using UnityEngine.XR.Interaction.Toolkit;

// based on https://www.youtube.com/watch?v=jOn0YWoNFVY
public class MenuToggle : MonoBehaviour
{   
    public InputActionReference toggleReferences;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject ContextualMenu;
    private ContextualMenuController cmc;
    [SerializeField]
    GameObject MenuMointPoint;
    [SerializeField]
    GameObject ContextualMenuMountPoint;

    private GameObject hoveredNotecard;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogError("Awake Start");
        toggleReferences.action.started +=  this.Toggle;
        cmc = ContextualMenu.GetComponentInChildren<ContextualMenuController>();
    }

    private void OnDestroy() {
        Debug.LogError("destroyed");
        toggleReferences.action.started -=  this.Toggle;
    } // end OnDestroy

    private void Toggle(InputAction.CallbackContext context) {
        Debug.LogError("Toggled");
        if (hoveredNotecard == null) {
            menu.SetActive(!menu.activeSelf);
            MoveThisToPosition(menu, MenuMointPoint);
        } else {
            ShowContextualMenuNotecard(hoveredNotecard);
        } // end else
       
    } // end Toggle

    private void MoveThisToPosition(GameObject target, GameObject mountPoint) {
        target.transform.position = mountPoint.transform.position;
        target.transform.rotation = mountPoint.transform.rotation;
    } // end MoveToPosition

    public void ShowContextualMenuNotecard(GameObject obj) {
        Debug.Log("Show contextual");
        cmc.SetTargetObject(obj, FormatType.NOTECARD);
        ContextualMenu.SetActive(true);
        MoveThisToPosition(ContextualMenu, ContextualMenuMountPoint);
    } // end OpenMenuMountPoint

    public void HideContextualMenu() {
        ContextualMenu.SetActive(false);
    } // end OpenMenuMountPoint

    public void SetHoveredNotecard(HoverEnterEventArgs ev) {
        string name = ev.interactorObject.transform.name.ToLower();
        if (name.Contains("lefthand")) {
            hoveredNotecard = ev.interactableObject.transform.gameObject;
        } // end if
    } // end SetHoverNotecard

    public void UnhoverNotecard(HoverExitEventArgs ev) {
        string name = ev.interactorObject.transform.name.ToLower();
        if (name.Contains("lefthand")) {
             hoveredNotecard = null;
        } // end if
    } // end UnhoverNotecard


}
