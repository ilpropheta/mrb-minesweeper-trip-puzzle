using System;
using Assets.GameFramework.GameObjects;
using Assets.MrB_Code.GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameFramework
{
    public class DialogManager : Singleton<DialogManager>
    {
        public GameObject DialogPrefab;
        public GameObject InputBoxPrefab;
        
        public void AskOkOrCancel(Action<DialogInstance> okCallback)
        {
            Debug.Log("DialogManager.AskOkOrCancel");
            var dialog = CreateDialogInstance(DialogPrefab);
            dialog.WireEvents(okCallback, dialogInstance => {});
            dialog.FadeIn();
        }

        public void ShowNumericInputBox(Action<int> okCallback, Action cancelCallback)
        {
            Debug.Log("DialogManager.ShowInputBox");
            var dialog = CreateDialogInstance(InputBoxPrefab);
            dialog.WireEvents(dialogInstance =>
                {
                    var inputBox = dialogInstance.InnerGameObject;
                    var insertedText = inputBox.GetChildComponentByName<Text>("ResultText").text;
                    int result;
                    if (int.TryParse(insertedText, out result))
                        okCallback(result);
                    else
                        cancelCallback();
                },
                cancelCallback: dialogInstance =>
                {
                    cancelCallback();
                });
            dialog.FadeIn();
        }

        private DialogInstance CreateDialogInstance(GameObject dialogPrefab)
        {
            var dialogPrefabInstance = Instantiate(dialogPrefab);
            dialogPrefabInstance.transform.SetParent(transform);
            dialogPrefabInstance.transform.localPosition = Vector3.zero;
            return new DialogInstance(dialogPrefabInstance);
        }
        
    }
}
