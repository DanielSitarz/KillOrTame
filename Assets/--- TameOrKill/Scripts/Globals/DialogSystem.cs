namespace dieow.krakjam2022
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class DialogSystem : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Set msg to empty to destroy dialog box.
        /// </summary>
        public static void ShowMessage(string ownerId, Transform worldTarget, string msg)
        {
            if (_Instance._Dialogs.ContainsKey(ownerId))
            {
                _Instance._Dialogs[ownerId].Set(msg, worldTarget);
                return;
            }

            _Instance.CreateDialogBox(ownerId, worldTarget, msg);
        }

        public static void HideMessage(string ownerId)
        {
            _Instance.DestroyDialog(ownerId);
        }
        #endregion Public Variables

        #region Public Methods

        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private DialogBox _DialogBoxTemplate;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Instance = this;
            _DialogBoxTemplate.gameObject.SetActive(false);
        }
        #endregion Unity Methods

        #region Private Variables
        private static DialogSystem _Instance;
        private Dictionary<string, DialogBox> _Dialogs = new Dictionary<string, DialogBox>();
        #endregion Private Variables

        #region Private Methods
        private void CreateDialogBox(string ownerId, Transform worldTarget, string msg)
        {
            var dialogBox = Instantiate(_Instance._DialogBoxTemplate, _Instance._DialogBoxTemplate.transform.parent);

            dialogBox.Set(msg, worldTarget);
            dialogBox.gameObject.SetActive(true);

            _Instance._Dialogs.Add(ownerId, dialogBox);
        }

        private void DestroyDialog(string ownerId)
        {
            if (!_Dialogs.ContainsKey(ownerId)) { return; }

            Destroy(_Dialogs[ownerId].gameObject);
            _Dialogs.Remove(ownerId);
        }
        #endregion Private Methods
    }
}