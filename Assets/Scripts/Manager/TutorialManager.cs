using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum TutorialSteps
{
    EnterLevel,
    SelectTurret,
    ChooseTurret,
    PlaceTurret
}

[System.Serializable]
public struct TutorialStepData
{
    public TutorialSteps step;
    public GameObject stepObject;
    public bool forceButtonClick;
}
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    [Header("Tutorial")]
    public GameObject tutorialPanel;

    public bool IsTutorialDone;

    public List<TutorialStepData> tutorialStepDatas = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        tutorialPanel.GetComponent<Image>().raycastTarget = false;

        IsTutorialDone = PlayerPrefs.GetInt("IsTutorialDone", 0) == 1;

        /*ShowSecondTutorial();*/
        /*ShowThirdTutorial();*/
    }

    #region Tutorial
    public void ShowTutorial()
    {
        if (IsTutorialDone)
        {
            return;
        }

        tutorialPanel.SetActive(true);
        NavigateToStep(TutorialSteps.EnterLevel);
    }
    private void NavigateToStep(TutorialSteps steps)
    {
        Debug.Log("NavigateToStep: " + steps.ToString());
        TutorialStepData stepData = GetStepData(steps);
        GameObject go = stepData.stepObject;
        go.SetActive(true);
        Transform stepPopup = go.transform.Find("Popup").transform;
        stepPopup.DOKill();
        stepPopup.localScale = Vector3.zero;
        foreach (var item in go.GetComponentsInChildren<Image>(true))
        {
            item.DOKill();
            item.DOFade(item.color.a, 0.3f).From(0f).SetUpdate(true);
        }
        stepPopup.DOScale(Vector3.one, 0.3f).SetUpdate(true).SetDelay(0.7f).OnComplete(() =>
        {
            stepPopup.DOScale(Vector3.one * 1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        });
        if (!stepData.forceButtonClick)
        {
            tutorialPanel.GetComponent<Image>().raycastTarget = true;
            tutorialPanel.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick_TutorialStep((int)stepData.step);
            });
        }
        else
        {
            tutorialPanel.GetComponent<Image>().raycastTarget = false;
            tutorialPanel.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
    public void OnClick_DisableTutorial()
    {
        foreach (var data in tutorialStepDatas)
        {
            data.stepObject.SetActive(false);
        }
    }
    public void OnClick_TutorialStep(int stepIndex)
    {
        TutorialSteps steps = (TutorialSteps)stepIndex;
        if (IsTutorialDone)
        {
            return;
        }
        foreach (var data in tutorialStepDatas)
        {
            data.stepObject.SetActive(false);
        }
        switch (steps)
        {
            case TutorialSteps.EnterLevel:
                NavigateToStep(TutorialSteps.SelectTurret);
                break;
            case TutorialSteps.SelectTurret:
                NavigateToStep(TutorialSteps.ChooseTurret);
                break;
            case TutorialSteps.ChooseTurret:
                NavigateToStep(TutorialSteps.PlaceTurret);
                break;
            case TutorialSteps.PlaceTurret:
                IsTutorialDone = true;
                PlayerPrefs.SetInt("IsTutorialDone", 1);
                tutorialPanel.GetComponent<Button>().onClick.RemoveAllListeners();
                tutorialPanel.SetActive(false);
                break;
        }
    }
    private TutorialStepData GetStepData(TutorialSteps step)
    {
        return tutorialStepDatas.Find(data => data.step == step);
    }
    #endregion
}
