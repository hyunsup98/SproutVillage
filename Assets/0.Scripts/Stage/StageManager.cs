using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private GameState gamestate;

    private void Start()
    {
        GameManager.Instance.CurrentGameState = gamestate;
        SoundManager.Instance.PlayBGM(bgmClip);
    }
}
