using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// based on https://www.youtube.com/watch?v=jOn0YWoNFVY
public class MenuToggle : MonoBehaviour
{   
    public InputActionReference toggleReferences;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject MenuMointPoint;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogError("Awake Start");
        toggleReferences.action.started +=  this.Toggle;
    }

    private void OnDestroy() {
        Debug.LogError("destroyed");
        toggleReferences.action.started -=  this.Toggle;
    } // end OnDestroy

    private void Toggle(InputAction.CallbackContext context) {
        Debug.LogError("Toggled");
        menu.SetActive(!menu.activeSelf);
        this.transform.position = MenuMointPoint.transform.position;
        this.transform.rotation = MenuMointPoint.transform.rotation;
    } // end Toggle

}
