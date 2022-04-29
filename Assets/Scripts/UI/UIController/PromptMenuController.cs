using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UIController;


namespace UIController {
    public class PromptMenuController: BaseSubMenuController
    {  
        private TextMeshProUGUI promptField;

        // Start is called before the first frame update
        void Awake()
        {
            promptField = GetComponentInChildren<TextMeshProUGUI>();   
        }

        public void SetPrompt(string prompt) {
            if (promptField == null)
                return;

            promptField.SetText(prompt);
        } // end SetPrompt

        public void SetButtonActions(UnityAction yesAction, UnityAction noAction) {
            Button[] buttons = PromptExtractButtons(this.gameObject);
            // yes button
            if (buttons[0] != null) {
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(yesAction);
            }
                
            // no button
            if (buttons[1] != null) {
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(noAction);
            }
        } // end SetButtonActions

        /*
            return an array of 2 elements that contain the buttons Yes/No of the prompt menu. 
            array[0] is the yes button
            array[1] is the no button
            precodnition: menu is not null
        */
        private Button[] PromptExtractButtons(GameObject menu) {
            Button[] result = new Button[2];

            // extract the yes button
            Button[] temp = menu.GetComponentsInChildren<Button>();
            if (temp.Length > 2 || temp.Length < 2) {
                return result;
            } // end if
            if (temp[0].name.ToLower() == "no") {
                Button t = temp[0];
                temp[0] = temp[1];
                temp[1] = t;
            } // end if

            result = temp;

            return result;
        } // end PromptExtractbutton
    } // end ProgressMenuController
} // end UIController

