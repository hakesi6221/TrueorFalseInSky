using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField, Header("スタートボタン")]
    private Button _startButton;

    [SerializeField, Header("遊び方ボタン")]
    private Button _howtoButton;

    [SerializeField, Header("遊び方閉じるボタン")]
    private Button _closeButton;

    [SerializeField, Header("遊び方UI")]
    private RectTransform _howtoUI;

    [SerializeField, Header("タイトル画面のパーツの親")]
    private RectTransform _titleParts;

    [SerializeField, Header("タイトル画面スライドインの時間：秒")]
    private float _titlePartsInTimesec = 0.8f;

    [SerializeField, Header("遊び方UIのスライドの時間：秒")]
    private float _howtoUISlideTimeSec = 0.8f;

    [SerializeField, Header("雲のスライドの時間")]
    private float _cloudsSlideTimeSec = 1f;

    [SerializeField, Header("落ちてるパーティクル")]
    private ParticleSystem _fallingParticle;

    private CancellationToken _destroyToken;

    // Start is called before the first frame update
    async void Start()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();
        _startButton.interactable = false;
        _howtoButton.interactable = false;
        _closeButton.interactable = false;
        _howtoUI.gameObject.SetActive(false);
        _fallingParticle.Play();

        await UniTask.WaitUntil(() => !SlideCloudsManager.Instance.IsSlide, cancellationToken: _destroyToken);

        await SlideUI.Instance.OnSlideUI(_titleParts, new Vector2(0, -1500), Vector2.zero, _titlePartsInTimesec, _destroyToken);

        _startButton.interactable = true;
        _howtoButton.interactable = true;
    }

    public void OnStartGame(string sceneName)
    {
        _startButton.interactable = false;
        _howtoButton.interactable = false;
        _closeButton.interactable = false;
        _howtoUI.gameObject.SetActive(false);

        SlideCloudsManager.Instance.OnCallSceneWithSlideUI(sceneName, _cloudsSlideTimeSec, _cloudsSlideTimeSec, 0f);
    }

    public async void OnOpenHowToPlay()
    {
        _startButton.interactable = false;
        _howtoButton.interactable = false;
        _closeButton.interactable = false;
        _howtoUI.gameObject.SetActive(true);

        await SlideUI.Instance.OnSlideUI(_howtoUI, new Vector2(0, -1500), Vector2.zero, _howtoUISlideTimeSec, _destroyToken);

        _closeButton.interactable = true;
    }

    public async void OnCloseHowToPlay()
    {
        _closeButton.interactable = false;

        await SlideUI.Instance.OnSlideUI(_howtoUI, Vector2.zero, new Vector2(0, -1500), _howtoUISlideTimeSec, _destroyToken);

        _howtoUI.gameObject.SetActive(false);
        _startButton.interactable = true;
        _howtoButton.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
