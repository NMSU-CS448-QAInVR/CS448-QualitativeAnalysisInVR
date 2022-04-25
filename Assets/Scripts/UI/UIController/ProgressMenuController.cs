using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UIController {
    public class ProgressMenuController : BaseSubMenuController
    {  
        private TextMeshProUGUI promptField;
        
        public Button cancelButton;
        public Button doneButton;

        private bool success = true;

        // Start is called before the first frame update
        void Awake()
        {
            promptField = GetComponentInChildren<TextMeshProUGUI>();   
            cancelButton.gameObject.SetActive(false);
            doneButton.gameObject.SetActive(false);
        }

        public void SetPrompt(string prompt) {
            if (promptField == null)
                return;

            promptField.SetText(prompt);
        } // end SetPrompt

        public void SetDoneAction() {

        } // end SetDoneAction

        public async Task<bool> ShowOnProgress(Func<Task<bool>> actionToDo) {
            // show cancel button
            cancelButton.gameObject.SetActive(true);

            // hide done button
            doneButton.gameObject.SetActive(false);

            // create the cancellation token
            var cancelTokenSrc = new CancellationTokenSource();
            CancellationToken ct = cancelTokenSrc.Token;

            bool task = await actionToDo();

            // set cancel action
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(delegate {cancelTokenSrc.Cancel();});
            await Task.Delay(1, ct);
            return task;
        } // end ShowOnProgress

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

