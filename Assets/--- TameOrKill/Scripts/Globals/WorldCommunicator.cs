namespace dieow.krakjam2022
{
    using DG.Tweening;
    using DG.Tweening.Core;
    using DG.Tweening.Plugins.Options;
    using TMPro;
    using UnityEngine;

    public class WorldCommunicator : MonoBehaviour
    {
        #region Public Methods
        public static void Communicate(string msg)
        {
            _Instance.ShowMessage(msg);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private CanvasGroup _MessageWrap;
        [SerializeField] private TextMeshProUGUI _Message;
        [SerializeField] private float _FadeOutDuration = 0.66f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Instance = this;
            _MessageWrap.gameObject.SetActive(false);
        }
        #endregion Unity Methods

        #region Private Variables
        private static WorldCommunicator _Instance;
        private TweenerCore<float, float, FloatOptions> _FadeOutTween;
        #endregion Private Variables

        #region Private Methods
        private void ShowMessage(string msg, float duration = 3f)
        {
            _Message.text = msg;

            _MessageWrap.gameObject.SetActive(true);
            _MessageWrap.alpha = 1.0f;

            _FadeOutTween.Kill();
            _FadeOutTween = _MessageWrap.DOFade(0f, _FadeOutDuration).SetDelay(duration).OnComplete(HideMessage);
        }

        private void HideMessage()
        {
            _MessageWrap.gameObject.SetActive(false);
        }
        #endregion Private Methods
    }
}