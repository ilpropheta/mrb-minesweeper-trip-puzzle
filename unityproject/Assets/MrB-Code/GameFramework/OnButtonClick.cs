using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameFramework
{
    /// <summary>
    /// An abstract class for performing an action on a button click.
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class OnButtonClick : MonoBehaviour
    {
        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public abstract void OnClick();
    }
}
