
namespace TOI2D
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class UITransition : MonoBehaviour
    {
        float baseAnimationDuration = .25f;
        public RectTransform transitionImage;
        public RectTransform loadingImage;
        // Start is called before the first frame update
        void Start()
        {
            FadeIn();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                FadeOutFadeIn();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                FadeOut(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                FadeIn();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                FadeOut();
            }
        }
        void QuitGame()
        {
            FadeOut(true);
        }


        public void FadeIn()
        {
            float totalAnimationDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, baseAnimationDuration).From(1).SetEase(Ease.InBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.zero, baseAnimationDuration).From(1).SetEase(Ease.InBack));
        }

        public void FadeOutFadeIn()
        {
            float totalAnimationDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack));
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, 0).From(1).SetEase(Ease.OutBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(1, baseAnimationDuration).From(0).SetEase(Ease.OutBack));
            totalAnimationDuration += baseAnimationDuration;
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, baseAnimationDuration).From(1).SetEase(Ease.InBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.zero, baseAnimationDuration).From(Vector3.one).SetEase(Ease.InBack));
        }
        public void FadeOut()
        {
            float totalAnimationDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack));
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, 0).From(1).SetEase(Ease.OutBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, loadingImage.DOScale(1, baseAnimationDuration).From(0).SetEase(Ease.OutBack));
        }

        public void FadeOut(string sceneName)
        {
            Debug.Log(sceneName);
            float totalAnimationDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack));
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, 0).From(1).SetEase(Ease.OutBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(1, baseAnimationDuration).From(0).SetEase(Ease.OutBack)).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
        public void FadeOut(bool quit)
        {
            float totalAnimationDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(totalAnimationDuration, transitionImage.DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack));
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(0, 0).From(1).SetEase(Ease.OutBack));
            totalAnimationDuration += baseAnimationDuration;
            sequence.Insert(totalAnimationDuration, loadingImage.DOScaleY(1, baseAnimationDuration).From(0).SetEase(Ease.OutBack)).OnComplete(() =>
            {
                Application.Quit();
            });

        }

        public float GetBaseAnimationDuration()
        {
            return baseAnimationDuration;
        }
    }

}
