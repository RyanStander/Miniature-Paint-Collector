using System;
using Paints;
using UI.Paints;
using UnityEngine;

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
    private bool paintDataLoaded;

    #endregion

    private void OnValidate()
    {
        SetupAutoAssignedFields();
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
        paintInventory = new PaintInventory();
        OpenLoadingPanel();
        paintDataLoaded = false;
    }

    private void Update()
    {
        if (paintInventory.IsInitialized && !paintDataLoaded)
        {
            paintSpawner.SpawnPlayerCollection(paintInventory.GetPaintQuantities());
            paintDataLoaded = true;
            CloseLoadingPanel();
        }
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
        paintInventory.AddPaintQuantity(1, 1);
        paintInventory.AddPaintQuantity(0,2);
    }
}
