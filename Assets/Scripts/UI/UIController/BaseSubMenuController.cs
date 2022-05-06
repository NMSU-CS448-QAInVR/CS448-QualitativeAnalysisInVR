using UnityEngine;

namespace UIController {
    /*
        A Base Controller for submenus.
        It contains the function to show and hide the submenu.
    */
    public class BaseSubMenuController : MonoBehaviour {
        public void Show() {
            this.gameObject.SetActive(true);
        } // end Show

        public void Hide() {
            this.gameObject.SetActive(false);
        } // end End
    } // end BaseSubMenuController
} // end UIBase



