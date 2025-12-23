using UnityEngine;

public class UILoadLevelButton : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName="SampleScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }
}
