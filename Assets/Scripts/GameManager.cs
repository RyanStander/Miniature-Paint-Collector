using System;
using System.Collections.Generic;
using Events;
using Paints;
using Paints.PaintItems;
using UI;
using UI.Paints;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the paint inventory loading and transitions between screens.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Public Fields

    #endregion

    #region Serialized Fields

    [Header("Manual Assignment")] [SerializeField]
    private GameObject dataLoadingScreen;

    [SerializeField] private GameObject sidePopupPanel;
    [SerializeField] private GameObject homeMenu;

    [Header("Auto Assigned")] [SerializeField]
    private PaintSpawner paintSpawner;

    [SerializeField] private CanvasGroup dataLoadingScreenCanvasGroup;
    [SerializeField] private CanvasGroup sidePopupPanelCanvasGroup;
    [SerializeField] private CanvasGroup homeMenuCanvasGroup;
    [SerializeField] private Animator sidePanelAnimator;

    [Header("AnimationNames")] [SerializeField]
    private string sidePanelOpenAnimationName = "SidePanelOpen";

    [SerializeField] private string sidePanelCloseAnimationName = "SidePanelClose";

    #endregion

    #region Private Fields

    private CurrentMenu currentMenu = CurrentMenu.Home;
    private PaintInventory paintInventory;
    private bool playerPaintDataLoaded;
    private Dictionary<int, PaintData> paintDatas;

    #endregion

    private void OnValidate()
    {
        SetupAutoAssignedFields();
    }

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventIdentifiers.SetPlayerPaintQuantity, OnSetPlayerPaintQuantity);
        EventManager.currentManager.Subscribe(EventIdentifiers.RequestPaintData, OnRequestPaintData);
        EventManager.currentManager.Subscribe(EventIdentifiers.WishlistPaint, OnWishlistPaint);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventIdentifiers.SetPlayerPaintQuantity, OnSetPlayerPaintQuantity);
        EventManager.currentManager.Unsubscribe(EventIdentifiers.RequestPaintData, OnRequestPaintData);
        EventManager.currentManager.Unsubscribe(EventIdentifiers.WishlistPaint, OnWishlistPaint);
    }

    private void SetupAutoAssignedFields()
    {
        if (paintSpawner == null)
            paintSpawner = FindObjectOfType<PaintSpawner>();

        if (dataLoadingScreen != null && dataLoadingScreenCanvasGroup == null)
            dataLoadingScreenCanvasGroup = dataLoadingScreen.GetComponent<CanvasGroup>();

        if (homeMenu != null && homeMenuCanvasGroup == null)
            homeMenuCanvasGroup = homeMenu.GetComponent<CanvasGroup>();

        if (sidePopupPanel != null)
        {
            if (sidePanelAnimator == null)
                sidePanelAnimator = sidePopupPanel.GetComponent<Animator>();
            if (sidePopupPanelCanvasGroup == null)
                sidePopupPanelCanvasGroup = sidePopupPanel.GetComponent<CanvasGroup>();
        }
    }

    private void Awake()
    {
        OpenHomeMenu();

        // Load all PaintData assets from Resources folder
        paintDatas = new Dictionary<int, PaintData>();
        var tempPaintDatas = Resources.LoadAll<PaintData>("Paints");
        foreach (var paintData in tempPaintDatas)
        {
            paintDatas.Add(paintData.PaintItem.ID, paintData);
        }
    }

    private void Update()
    {
        switch (currentMenu)
        {
            case CurrentMenu.Home:
                if (paintInventory.IsInitialized && !playerPaintDataLoaded)
                    LoadPlayerCollection();
                break;
            case CurrentMenu.Catalogue:
                break;
            case CurrentMenu.Wishlist:
                if (paintInventory.IsInitialized && !playerPaintDataLoaded)
                    LoadWishlist();
                break;
            case CurrentMenu.Settings:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OpenHomeMenu()
    {
        currentMenu = CurrentMenu.Home;
        LoadSaveData();
    }

    private void LoadPlayerCollection()
    {
        paintSpawner.SpawnPlayerCollection(paintInventory.GetPaintQuantities(), paintDatas.Values);
        playerPaintDataLoaded = true;
        CloseLoadingPanel();
    }

    public void OpenCatalogue()
    {
        currentMenu = CurrentMenu.Catalogue;
        LoadCatalogue();
        StartCoroutine(paintSpawner.ToggleContentActive(false));
    }

    private void LoadCatalogue()
    {
        paintSpawner.SpawnAllPaints(paintDatas.Values);
    }

    public void OpenWishlist()
    {
        currentMenu = CurrentMenu.Wishlist;
        LoadSaveData();
    }

    private void LoadWishlist()
    {
        paintSpawner.SpawnPlayerWishlist(paintInventory.GetWishlistedPaints(), paintDatas.Values);
        playerPaintDataLoaded = true;
        CloseLoadingPanel();
    }

    private void LoadSaveData()
    {
        paintInventory = new PaintInventory();
        OpenLoadingPanel();
        playerPaintDataLoaded = false;

        StartCoroutine(paintSpawner.ToggleContentActive(true));
    }

    public void OpenSidePanel()
    {
        sidePanelAnimator.Play(sidePanelOpenAnimationName);
        sidePopupPanelCanvasGroup.interactable = true;
        sidePopupPanelCanvasGroup.blocksRaycasts = true;
        sidePopupPanelCanvasGroup.alpha = 1;
    }

    public void CloseSidePanel()
    {
        sidePanelAnimator.Play(sidePanelCloseAnimationName);
        sidePopupPanelCanvasGroup.interactable = false;
        sidePopupPanelCanvasGroup.blocksRaycasts = false;
    }

    public void OpenLoadingPanel()
    {
        dataLoadingScreenCanvasGroup.interactable = true;
        dataLoadingScreenCanvasGroup.blocksRaycasts = true;
        dataLoadingScreenCanvasGroup.alpha = 1;
    }

    public void CloseLoadingPanel()
    {
        dataLoadingScreenCanvasGroup.interactable = false;
        dataLoadingScreenCanvasGroup.blocksRaycasts = false;
        dataLoadingScreenCanvasGroup.alpha = 0;
    }

    public void ClearData()
    {
        paintInventory.ClearData();
        SceneManager.LoadScene(0);
    }


    #region On Events

    private void OnSetPlayerPaintQuantity(EventData eventData)
    {
        if (!eventData.IsEventOfType<SetPlayerPaintQuantity>(out var setPlayerPaintQuantity))
            return;

        paintInventory.SetPaintQuantity(setPlayerPaintQuantity.Id, setPlayerPaintQuantity.Quantity);

        // If the player is in the home menu and the paint quantity is 0 or 1 reload the ui
        if (currentMenu == CurrentMenu.Home && setPlayerPaintQuantity.Quantity < 2)
            LoadPlayerCollection();
    }

    private void OnWishlistPaint(EventData eventData)
    {
        if (!eventData.IsEventOfType<WishlistPaint>(out var wishlistPaint))
            return;

        paintInventory.WishlistPaint(wishlistPaint.Id);
    }

    private void OnRequestPaintData(EventData eventData)
    {
        if (!eventData.IsEventOfType<RequestPaintData>(out var requestPaintData))
            return;

        EventManager.currentManager.AddEvent(new OpenPaintContextMenu(paintDatas[requestPaintData.Id],
            paintInventory.GetPaintQuantity(requestPaintData.Id)));
    }

    #endregion
}
