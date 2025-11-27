using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private AudioClip clickClip;

    public void OnClickGameStart()
    {
        SoundManager.Instance.PlaySoundEffect(clickClip);
        SceneManager.LoadScene(nextSceneName);
    }
}
