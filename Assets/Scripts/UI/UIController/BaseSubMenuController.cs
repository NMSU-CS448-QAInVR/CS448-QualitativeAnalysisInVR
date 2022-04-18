using UnityEngine;

namespace UIController {
    public class BaseSubMenuController : MonoBehaviour {
        public void Show() {
            this.gameObject.SetActive(true);
        } // end Show

        public void Hide() {
            this.gameObject.SetActive(false);
        } // end End
    } // end BaseSubMenuController
} // end UIBase



