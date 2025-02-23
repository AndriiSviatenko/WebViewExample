using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class WebViewController : MonoBehaviour
{
    private const string DEFAULT_URL = "https://google.com";
    private const int TOP_MARGIN = 200;
    private const int HEIGHT_BOTTOM = 400;

    [SerializeField] private CanvasGroup webViewCanvas;
    [SerializeField] private Button showWebViewBtn;
    [SerializeField] private Button hideWebViewBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button forwardBtn;
    [SerializeField] private Button reloadBtn;
    [SerializeField] private UniWebView webView;
    private bool _isWebViewInitialized;

    private async void Start()
    {
        RequestPermissions();
        AssignButtonListeners();
        await InitializeWebViewAsync();
    }

    private void OnDestroy()
    {
        RemoveButtonListeners();
    }

    private void RequestPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    private async Task InitializeWebViewAsync()
    {
        if (_isWebViewInitialized)
            return;

        UpdatePosition();
        webView.OnPageFinished += OnPageLoaded;
        webView.OnLoadingErrorReceived += OnPageErrorV2;
        webView.OnShouldClose += OnShouldClose;
        _isWebViewInitialized = true;
        Debug.Log("WebView initialized.");
    }

    private void UpdatePosition()
    {
        float width = Screen.width;
        float height = Screen.height - TOP_MARGIN - HEIGHT_BOTTOM;

        float xPos = 0;
        float yPos = TOP_MARGIN;

        webView.Frame = new Rect(xPos, yPos, width, height);
    }

    private void OnPageErrorV2(UniWebView webView, int errorCode, string errorMessage, UniWebViewNativeResultPayload payload)
    {
        Debug.LogError($"WebView Error ({errorCode}): {errorMessage}");
    }

    public async void ShowWebView()
    {
        await InitializeWebViewAsync();
        webView.Load(DEFAULT_URL);
        webView.Show();
        ToggleCanvas(true);
    }

    public void HideWebView()
    {
        ToggleCanvas(false);
        webView.Hide();
    }

    private void ToggleCanvas(bool isVisible)
    {
        webViewCanvas.alpha = isVisible ? 1 : 0;
        webViewCanvas.interactable = isVisible;
        webViewCanvas.blocksRaycasts = isVisible;
    }

    private void OnPageLoaded(UniWebView view, int statusCode, string url)
    {
        Debug.Log($"Page Loaded: {url}");
        UpdateButtons();
    }


    private bool OnShouldClose(UniWebView view) => false;

    public void GoBack()
    {
        if (webView.CanGoBack) 
            webView.GoBack();

        UpdateButtons();
    }

    public void GoForward()
    {
        if (webView.CanGoForward) 
            webView.GoForward();

        UpdateButtons();
    }

    public void Reload()
    {
        webView.Reload();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        backBtn.interactable = webView.CanGoBack;
        forwardBtn.interactable = webView.CanGoForward;
        reloadBtn.interactable = true;
    }

    private void AssignButtonListeners()
    {
        showWebViewBtn.onClick.AddListener(ShowWebView);
        hideWebViewBtn.onClick.AddListener(HideWebView);
        backBtn.onClick.AddListener(GoBack);
        forwardBtn.onClick.AddListener(GoForward);
        reloadBtn.onClick.AddListener(Reload);
    }

    private void RemoveButtonListeners()
    {
        showWebViewBtn.onClick.RemoveListener(ShowWebView);
        hideWebViewBtn.onClick.RemoveListener(HideWebView);
        backBtn.onClick.RemoveListener(GoBack);
        forwardBtn.onClick.RemoveListener(GoForward);
        reloadBtn.onClick.RemoveListener(Reload);
    }
}
