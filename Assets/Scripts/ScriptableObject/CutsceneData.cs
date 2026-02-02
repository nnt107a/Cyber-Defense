using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Cutscene/Cutscene Data")]
public class CutsceneData : ScriptableObject
{
    public List<CutsceneLine> lines;
}

[System.Serializable]
public class CutsceneLine
{
    public Sprite speakerIcon;
    [TextArea(3, 6)]
    public string text;
    [TextArea(3, 6)]
    public string vnText;
}