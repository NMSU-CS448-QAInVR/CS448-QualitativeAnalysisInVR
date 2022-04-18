using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
        public void PopulateSessionList(List<string> sessions, UnityAction<string> loadAction, UnityAction<string, UnityAction> deleteAction) {
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
                lvb.UpdateText(SaveLoadSystem.GetSessionName(session));
                lvb.SetOnClick(delegate {loadAction(session);});

                ListViewDeleteButton deleteButton = button.GetComponentInChildren<ListViewDeleteButton>();
                deleteButton.SetOnClick(delegate { 
                   deleteAction(session, delegate {GameObject.Destroy(button);});
                });
               button.SetActive(true);
            } // end foreach

            ListViewButtonTemplate.SetActive(false);
        } // end PopulateSessionList
           
        
    } // end LoadMenuController
} // end UIController

