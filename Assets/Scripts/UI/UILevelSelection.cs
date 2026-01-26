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
        SoundManager.Instance.PlayMusic(true);
    }

    void Start()
    {
        // 1. Đọc dữ liệu level đã mở khóa từ PlayerPrefs
        // Nếu chưa có dữ liệu thì mặc định là 0 (Level 1)
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 0);
        
        // Cập nhật biến currentLevelIndex của script này bằng dữ liệu đã lưu
        currentLevelIndex = unlockedLevel;

        // 2. Hiển thị các level đã mở khóa
        ShowUnlockedLevelsImmediate();
        StartCoroutine(AnimateNewLevelUnlock(0.5f));
    }

    private void ShowUnlockedLevelsImmediate()
    {
        for (int i = 0; i < currentLevelIndex; i++)
        {
            if (i <= currentLevelIndex)
            {
                // Hiển thị Level Button
                levelPaths[i].levelButton.gameObject.SetActive(true);

                // Hiển thị đường nối (Path Line) nếu có
                if (levelPaths[i].pathLine != null)
                {
                    levelPaths[i].pathLine.gameObject.SetActive(true);
                    levelPaths[i].pathLine.fillAmount = 1f;
                }
            }
            else
            {
                // Ẩn các Level chưa mở
                levelPaths[i].levelButton.gameObject.SetActive(false);
                if (levelPaths[i].pathLine != null)
                {
                    levelPaths[i].pathLine.gameObject.SetActive(false);
                }
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
