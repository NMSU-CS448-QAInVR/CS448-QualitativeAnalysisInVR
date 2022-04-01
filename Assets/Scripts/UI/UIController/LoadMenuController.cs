using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;
using UIController;


namespace UIController {
    public class LoadMenuController: BaseSubMenuController
    {  
        [SerializeField]
        private GameObject ListViewContentObject;
        [SerializeField]
        private GameObject ListViewButtonTemplate;

        // Start is called before the first frame update
        void Awake()
        { 

        }
        public void PopulateSessionList(List<string> sessions, UnityAction<string> loadAction) {
            Transform[] children = ListViewContentObject.GetComponentsInChildren<Transform>();
            // remove existing buttons
            for (int i = 0; i < children.Length; ++i) {
               Transform child = children[i];
               if (child.parent.gameObject != ListViewContentObject || child.gameObject == ListViewButtonTemplate) {
                  continue;
               } // end if
                  
               GameObject.Destroy(child.gameObject);
            } // end foreach

            // add new buttons
            foreach (string session in sessions) {
               GameObject button = GameObject.Instantiate(ListViewButtonTemplate);
               button.transform.SetParent(ListViewButtonTemplate.transform.parent, false);
               ListViewButton lvb = button.GetComponentInChildren<ListViewButton>();
               lvb.UpdateText(session.Substring(0, session.Length - 4));
               lvb.SetOnClick(delegate {loadAction(session);});
               button.SetActive(true);
            } // end foreach

            ListViewButtonTemplate.SetActive(false);
        } // end PopulateSessionList
           
        
    } // end LoadMenuController
} // end UIController

