using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UIController;
using UnityEngine.XR.Interaction.Toolkit;

// based on https://www.youtube.com/watch?v=jOn0YWoNFVY
/*
    A script to toggle the menu active/unactive.
    If a notecard is being hovered with the left hand, then we will show the contextual menu of that notecard, else toggle the main menu.
    For this to work, the setting and unsetting of variable hoveredNotecard should be set in the OnHoverEnter and OnHoverExit event of XR Interaction in the prefab notecard (in the scene). This is one of the reasons why we need to keep the prefab on the scene. 
    Set it with SetHoveredNotecard() and UnhoverNotecard().
*/
public class MenuToggle : MonoBehaviour
{   
    // the InputAction to listen on. Set in the editor. 
    public InputActionReference toggleReferences;
    
    // the main menu
    [SerializeField]
    GameObject menu;

    // the contextual menu
    [SerializeField]
    GameObject ContextualMenu;
    private ContextualMenuController cmc;

    // the mounting point of the main menu
    [SerializeField]
    GameObject MenuMointPoint;

    // the mounting point of the contextual menu
    [SerializeField]
    GameObject ContextualMenuMountPoint;

    // the notecard that is being hovered.
    private GameObject hoveredNotecard;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogError("Awake Start");
        toggleReferences.action.started +=  this.Toggle;
        cmc = ContextualMenu.GetComponentInChildren<ContextualMenuController>();
    }

    /*
        When Toggle is destroyed, remove the action of toggling from the InputAction.
    */
    private void OnDestroy() {
        Debug.LogError("destroyed");
        toggleReferences.action.started -=  this.Toggle;
    } // end OnDestroy

    /*
        Toggle the menu active/unactive.
        If a notecard is being hovered with the left hand, then we will show the contextual menu of that notecard, else toggle the main menu.
    */
    private void Toggle(InputAction.CallbackContext context) {
        Debug.LogError("Toggled");
        if (hoveredNotecard == null) {
            menu.SetActive(!menu.activeSelf);
            MoveThisToPosition(menu, MenuMointPoint);
        } else {
            ShowContextualMenuNotecard(hoveredNotecard);
        } // end else
       
    } // end Toggle

    /*
        Move the a GameObject to the position of another GameObject
        Input:
        + target: the target to move the position
        + mountPoint: the position to move to.
    */
    private void MoveThisToPosition(GameObject target, GameObject mountPoint) {
        target.transform.position = mountPoint.transform.position;
        target.transform.rotation = mountPoint.transform.rotation;
    } // end MoveToPosition

    /*
        Display the ContextualMenuNotecard for the notecard obj at the contextual menu mount point.
        + obj: the notecard to show contextual menu of.
    */
    public void ShowContextualMenuNotecard(GameObject obj) {
        Debug.Log("Show contextual");
        cmc.SetTargetObject(obj, FormatType.NOTECARD);
        ContextualMenu.SetActive(true);
        MoveThisToPosition(ContextualMenu, ContextualMenuMountPoint);
    } // end OpenMenuMountPoint

    /*
        Hide the ContextualMenu of the notecard.
    */
    public void HideContextualMenu() {
        ContextualMenu.SetActive(false);
    } // end OpenMenuMountPoint

    /*
        Get and store the GameObject of the notecard is currently being hovered with the left hand.

        Precondition: The left interactor gameobject has the world "lefthand" in it.
        Input: 
        + ev: The hover event of the notecard. This will contain the interactor and the interatable (the notecard).

        HoverEnterEventArgs is a class of XR Interaction Toolkit.
    */
    public void SetHoveredNotecard(HoverEnterEventArgs ev) {
        string name = ev.interactorObject.transform.name.ToLower();
        if (name.Contains("lefthand")) {
            hoveredNotecard = ev.interactableObject.transform.gameObject;
        } // end if
    } // end SetHoverNotecard

     /*
        Unset the hoveredNotecard if the left hand unhover a notecard.

        Precondition: The left interactor gameobject has the world "lefthand" in it.
        Input: 
        + ev: The unhover event of the notecard. This will contain the interactor and the interatable (the notecard).

        HoverExitEventArgs is a class of XR Interaction Toolkit.
    */
    public void UnhoverNotecard(HoverExitEventArgs ev) {
        string name = ev.interactorObject.transform.name.ToLower();
        if (name.Contains("lefthand")) {
             hoveredNotecard = null;
        } // end if
    } // end UnhoverNotecard


}
