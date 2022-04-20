using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UIController;

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
    GameObject ContextualMenuMountPoint;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogError("Awake Start");
        toggleReferences.action.started +=  this.Toggle;
        cmc = ContextualMenu.GetComponentInChildren<ContextualMenuController>();
        ContextualMenuMountPoint = MenuMointPoint;
    }

    private void OnDestroy() {
        Debug.LogError("destroyed");
        toggleReferences.action.started -=  this.Toggle;
    } // end OnDestroy

    private void Toggle(InputAction.CallbackContext context) {
        Debug.LogError("Toggled");
        menu.SetActive(!menu.activeSelf);
        MoveThisToPosition(menu, MenuMointPoint);
    } // end Toggle

    private void MoveThisToPosition(GameObject target, GameObject mountPoint) {
        target.transform.position = mountPoint.transform.position;
        target.transform.rotation = mountPoint.transform.rotation;
    } // end MoveToPosition

    public void ShowContextualMenuNotecard(GameObject obj) {
        cmc.SetTargetObject(obj, FormatType.NOTECARD);
        ContextualMenu.SetActive(true);
        MoveThisToPosition(ContextualMenu, ContextualMenuMountPoint);
    } // end OpenMenuMountPoint

    public void HideContextualMenu() {
        ContextualMenu.SetActive(false);
    } // end OpenMenuMountPoint

}
