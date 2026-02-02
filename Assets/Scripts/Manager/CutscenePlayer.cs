using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Localization.Editor;

public class CutscenePlayer : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup root;
    public Image background;
    public Image speakerIcon;
    public TextMeshProUGUI dialogueText;

    [Header("Typing")]
    public float charDelay = 0.03f;

    private CutsceneData data;
    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingRoutine;

    public UnityEvent onCutsceneFinished;

    void Awake()
    {
        root.alpha = 0;
        root.blocksRaycasts = false;
        root.interactable = false;
    }

    public void Play(CutsceneData cutscene)
    {
        data = cutscene;
        index = 0;

        root.alpha = 1;
        root.blocksRaycasts = true;
        root.interactable = true;

        ShowLine();
    }

    void ShowLine()
    {
        if (index >= data.lines.Count)
        {
            EndCutscene();
            return;
        }

        var line = data.lines[index];
        speakerIcon.sprite = line.speakerIcon;

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(PlayerPrefs.GetInt("LocaleIndex", 0) == 0 ? line.text : line.vnText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
    }

    public void OnClick()
    {
        if (isTyping)
        {
            StopCoroutine(typingRoutine);
            dialogueText.text = PlayerPrefs.GetInt("LocaleIndex", 0) == 0 ? data.lines[index].text : data.lines[index].vnText;
            isTyping = false;
        }
        else
        {
            index++;
            ShowLine();
        }
    }

    void EndCutscene()
    {
        root.alpha = 0;
        root.blocksRaycasts = false;
        root.interactable = false;

        onCutsceneFinished?.Invoke();
    }
}
