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
        // 1. Đọc dữ liệu level đã mở khóa từ GameManager (BinarySaveSystem) thay vì PlayerPrefs
        if (GameManager.Instance != null && GameManager.Instance.currentData != null)
        {
            // Lấy dữ liệu từ file save đã load sẵn trong GameManager
            currentLevelIndex = GameManager.Instance.currentData.currentLevelIndex;
        }
        else
        {
            // Fallback nếu chưa có data (mặc định level 0)
            currentLevelIndex = 0; 
        }

        // 2. Hiển thị các level đã mở khóa
        ShowUnlockedLevelsImmediate();
        StartCoroutine(AnimateNewLevelUnlock(0.5f));
    }

    private void ShowUnlockedLevelsImmediate()
    {
        // SỬA LỖI Ở ĐÂY: Thêm "levelPaths.Count" vào giữa
        for (int i = 0; i <= currentLevelIndex; i++) 
        {
            // 1. Gán số thứ tự cho nút (để khi bấm vào nút này, Game Manager biết là level mấy)
            var btnScript = levelPaths[i].levelButton.GetComponent<UILoadLevelButton>();
            if (btnScript != null)
            {
                btnScript.levelIndex = i; // Gán index: Level 1 là 0, Level 2 là 1...
            }

            // 2. Logic Hiển thị / Ẩn
            // Nếu i nhỏ hơn hoặc bằng level hiện tại đang mở -> Hiển thị
            if (i < currentLevelIndex) 
            {
                levelPaths[i].levelButton.gameObject.SetActive(true);

                if (levelPaths[i].pathLine != null)
                {
                    levelPaths[i].pathLine.gameObject.SetActive(true);
                    levelPaths[i].pathLine.fillAmount = 1f;
                }
            }
            // Nếu i lớn hơn -> Ẩn (Level chưa mở)
            else
            {
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
