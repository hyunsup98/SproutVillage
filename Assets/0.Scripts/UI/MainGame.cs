using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private AudioClip clickClip;

    private void Start()
    {
        DataManager.Instance.DataLoad();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                titlePanel.SetActive(!titlePanel.activeSelf);
            }
        }
    }

    public void OnClickGoTitle()
    {
        DataManager.Instance.DataSave();
        SoundManager.Instance.PlaySoundEffect(clickClip);
        SceneManager.LoadScene("Title");
    }
}
