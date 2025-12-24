using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelPathData
{
    public string levelName;
    public UILoadLevelButton levelButton;
    public Image pathLine;
}

public class UILevelSelection : MonoBehaviour
{
    public static UILevelSelection Instance;
    [SerializeField] public int currentLevelIndex = 0;
    public List<LevelPathData> levelPaths;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowUnlockedLevelsImmediate();
        StartCoroutine(AnimateNewLevelUnlock(0.5f));
    }

    private void ShowUnlockedLevelsImmediate()
    {
        for (int i = 0; i < currentLevelIndex; i++)
        {
            levelPaths[i].levelButton.gameObject.SetActive(true);

            if (levelPaths[i].pathLine != null)
            {
                levelPaths[i].pathLine.gameObject.SetActive(true);
                levelPaths[i].pathLine.fillAmount = 1f;
            }
        }
    }

    IEnumerator AnimateNewLevelUnlock(float delay)
    {
        var targetLevel = levelPaths[currentLevelIndex];

        yield return new WaitForSeconds(delay);

        if (targetLevel.pathLine != null)
        {
            targetLevel.pathLine.gameObject.SetActive(true);
            targetLevel.pathLine.fillAmount = 0f;

            yield return targetLevel
                .pathLine.DOFillAmount(1f, 1.0f)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
        }

        targetLevel.levelButton.gameObject.SetActive(true);
        targetLevel.levelButton.Show();
    }
}
