using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UIController {
    /*
        The controller for the progress menu.
    */
    public class ProgressMenuController : BaseSubMenuController
    {  
        private TextMeshProUGUI promptField;
        
        public Button cancelButton;
        public Button doneButton;

        // Start is called before the first frame update
        void Awake()
        {
            promptField = GetComponentInChildren<TextMeshProUGUI>();   
            cancelButton.gameObject.SetActive(false);
            doneButton.gameObject.SetActive(false);
        }

        /*
            Set the prompt of the menu.
        */
        public void SetPrompt(string prompt) {
            if (promptField == null)
                return;

            promptField.SetText(prompt);
        } // end SetPrompt

        public void SetDoneAction() {

        } // end SetDoneAction

        /*
            Action when the task in in progress
            Output: return the output of actionToDo().
        */
        public async Task<bool> ShowOnProgress(Func<Task<bool>> actionToDo) {
            // show cancel button
            cancelButton.gameObject.SetActive(true);

            // hide done button
            doneButton.gameObject.SetActive(false);

            // create the cancellation token
            var cancelTokenSrc = new CancellationTokenSource();
            CancellationToken ct = cancelTokenSrc.Token;
            await Task.Delay(10, ct);
            // set cancel action
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(delegate {cancelTokenSrc.Cancel();});

            bool task = await actionToDo();

            return task;
        } // end ShowOnProgress

        /*
            Action when the task is done.
        */
        public void ShowDone(UnityAction onDoneAction) {
            // show done button
            doneButton.gameObject.SetActive(true);

            // hide cancel button
            cancelButton.gameObject.SetActive(false);

            // set done action
            doneButton.onClick.RemoveAllListeners();
            doneButton.onClick.AddListener(onDoneAction);
        } // end ShowDone
    } // end ProgressMenuController
} // end UIController

