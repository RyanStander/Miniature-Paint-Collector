using System;
using System.Collections.Generic;
using Events;
using Paints;
using Paints.PaintItems;
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

    [Header("Manual Assignment")] 
    [SerializeField] private GameObject dataLoadingScreen;
    [SerializeField] private GameObject sidePopupPanel;
    [SerializeField] private GameObject homeMenu;

    [Header("Auto Assigned")] [SerializeField]
    private PaintSpawner paintSpawner;
    [SerializeField] private CanvasGroup dataLoadingScreenCanvasGroup;
    [SerializeField] private CanvasGroup sidePopupPanelCanvasGroup;
    [SerializeField] private CanvasGroup homeMenuCanvasGroup;
    [SerializeField] private Animator sidePanelAnimator;
    
    [Header("AnimationNames")]
    [SerializeField] private string sidePanelOpenAnimationName = "SidePanelOpen";
    [SerializeField] private string sidePanelCloseAnimationName = "SidePanelClose";

    #endregion

    #region Private Fields

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
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventIdentifiers.SetPlayerPaintQuantity, OnSetPlayerPaintQuantity);
        EventManager.currentManager.Unsubscribe(EventIdentifiers.RequestPaintData, OnRequestPaintData);
    }

    private void SetupAutoAssignedFields()
    {
        if(paintSpawner==null)
            paintSpawner = FindObjectOfType<PaintSpawner>();
        
        if(dataLoadingScreen!=null && dataLoadingScreenCanvasGroup==null)
            dataLoadingScreenCanvasGroup = dataLoadingScreen.GetComponent<CanvasGroup>();
        
        if(homeMenu!=null && homeMenuCanvasGroup==null)
            homeMenuCanvasGroup = homeMenu.GetComponent<CanvasGroup>();

        if (sidePopupPanel != null)
        {
            if(sidePanelAnimator==null)
                sidePanelAnimator = sidePopupPanel.GetComponent<Animator>();
            if(sidePopupPanelCanvasGroup==null)
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
        if (paintInventory.IsInitialized && !playerPaintDataLoaded)
            LoadPlayerCollection();
    }
    
    public void OpenHomeMenu()
    {
        paintInventory = new PaintInventory();
        OpenLoadingPanel();
        playerPaintDataLoaded = false;
    }


    private void LoadPlayerCollection()
    {
        paintSpawner.SpawnPlayerCollection(paintInventory.GetPaintQuantities(), paintDatas.Values);
        playerPaintDataLoaded = true;
        CloseLoadingPanel();
    }
    

    public void OpenCatalogue()
    {
        LoadCatalogue();
    }
    
    private void LoadCatalogue()
    {
        paintSpawner.SpawnAllPaints(paintDatas.Values);
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
    
    public void TemporaryAddPaint()
    {
        paintInventory.SetPaintQuantity(1, 1);
        paintInventory.SetPaintQuantity(0,2);
    }

    public void ClearData()
    {
        paintInventory.DeletePaintQuantityData();
        SceneManager.LoadScene(0);
    }


    #region On Events

    private void OnSetPlayerPaintQuantity(EventData eventData)
    {
        if (!eventData.IsEventOfType<SetPlayerPaintQuantity>(out var setPlayerPaintQuantity))
            return;
        
        paintInventory.SetPaintQuantity(setPlayerPaintQuantity.Id, setPlayerPaintQuantity.Quantity);
    }
    
    private void OnRequestPaintData(EventData eventData)
    {
        if (!eventData.IsEventOfType<RequestPaintData>(out var requestPaintData))
            return;
        
        EventManager.currentManager.AddEvent(new OpenPaintContextMenu(paintDatas[requestPaintData.Id], paintInventory.GetPaintQuantity(requestPaintData.Id)));
    }

    #endregion
}
