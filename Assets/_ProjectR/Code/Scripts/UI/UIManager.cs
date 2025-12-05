using DG.Tweening;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI params")]
    [SerializeField] private float _buttonSelectTime = 0.1f;
    [SerializeField] private float _buttonSelectScale = 1.05f;

    [Header("Sounds")]
    [SerializeField] private AudioClip _selectSFX;
    [SerializeField] private AudioClip _confirmSFX;

    private AudioSource _audioSource;

    [Header("Containers")]
    [SerializeField] private CanvasGroup _startMenuContainer;
    [SerializeField] private CanvasGroup _networkMenuContainer;
    private CanvasGroup _currentContainer;

    [Header("Menus")]
    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private CanvasGroup _settingsMenu;
    [SerializeField] private CanvasGroup _controlsMenu;
    private CanvasGroup _currentMenu;

    [Header("Menu cover")]
    [SerializeField] private CanvasGroup _menuCover;

    private void Start()
    {
        InitializeContainers();
        InitializeMenus();

        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    #region Navigation
    public void NavigateToContainer(CanvasGroup targetContainer)
    {
        float fadeTime = 0.2f;

        _currentContainer.alpha = 1f;
        _currentContainer.DOFade(0f, fadeTime).OnComplete(() =>
        {
            _currentContainer.gameObject.SetActive(false);

            _currentContainer = targetContainer;

            _currentContainer.alpha = 0f;
            _currentContainer.gameObject.SetActive(true);
            _currentContainer.DOFade(1f, fadeTime);
        });
    }

    public void NavigateToMenu(CanvasGroup targetMenu)
    {
        float fadeTime = 0.2f;

        _currentMenu.alpha = 1f;
        _currentMenu.DOFade(0f, fadeTime).OnComplete(() =>
        {
            _currentMenu.gameObject.SetActive(false);

            _currentMenu = targetMenu;

            _currentMenu.alpha = 0f;
            _currentMenu.gameObject.SetActive(true);
            _currentMenu.DOFade(1f, fadeTime);
        });
    }

    public void CloseCurrentMenu()
    {
        float fadeTime = 0.2f;

        _currentMenu.alpha = 1f;
        _currentMenu.DOFade(0f, fadeTime).OnComplete(() =>
        {
            _currentMenu.gameObject.SetActive(false);
        });
    }

    public void ToggleMenuCover(bool toggle)
    {
        float fadeTime = 0.2f;

        if (toggle)
        {
            _currentMenu.gameObject.SetActive(true);
            _menuCover.alpha = 0f;
            _menuCover.DOFade(1f, fadeTime);
        }
        else
        {
            _menuCover.alpha = 1f;
            _menuCover.DOFade(0f, fadeTime).OnComplete(() =>
            {
                _menuCover.gameObject.SetActive(false);
            });
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Initialization
    public void InitializeContainers()
    {
        _startMenuContainer.gameObject.SetActive(true);
        _currentContainer = _startMenuContainer;

        _networkMenuContainer.gameObject.SetActive(false);
    }

    public void InitializeMenus()
    {
        _menuCover.alpha = 1f;
        _menuCover.gameObject.SetActive(true);

        _mainMenu.gameObject.SetActive(true);
        _currentMenu = _mainMenu;

        _settingsMenu.gameObject.SetActive(false);
        _controlsMenu.gameObject.SetActive(false);
    }
    #endregion

    #region Mouse events
    public void OnMouseEnterEvent(Button button)
    {
        if (!_audioSource.isPlaying) PlayAudio(_selectSFX);

        button.transform.localScale = Vector3.one;
        button.transform.DOScale(_buttonSelectScale, _buttonSelectTime)
            .OnComplete(() => { button.transform.localScale = Vector3.one * _buttonSelectScale; });
    }

    public void OnMouseExitEvent(Button button)
    {
        button.transform.localScale = Vector3.one * _buttonSelectScale;
        button.transform.DOScale(1f, _buttonSelectTime)
            .OnComplete(() => { button.transform.localScale = Vector3.one; });
    }

    public void OnMouseClickEvent(Button button)
    {
        PlayAudio(_confirmSFX);
    }
    #endregion

    #region Helpers
    private void PlayAudio(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }
    #endregion
}
