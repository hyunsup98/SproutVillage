using System;
using UnityEngine;

public enum GameState
{
    Title,              //타이틀 화면일 때
    Playing,            //인게임에서 플레이중일 때
    InteractionLock,    //Playing 상태에서 UI가 켜지는 등의 이유로 몇몇 상호작용이 꺼질 때, 게임 시간이 멈추지는 않음
}

public class GameManager : Singleton<GameManager>
{
    #region 게임 상태 관리 델리게이트 및 프로퍼티
    public event Action onGameStateTitle;       //GameState가 Title이 되었을 때 실행할 액션 이벤트
    public event Action onGameStatePlaying;     //GameState가 Playing이 되었을 때 실행할 액션 이벤트
    public event Action onGameStatePause;       //GameState가 Pause가 되었을 때 실행할 액션 이벤트

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set
        {
            if (value == currentGameState) return;

            if(value == GameState.Title)
            {
                onGameStateTitle?.Invoke();
            }
            else if(value == GameState.Playing)
            {
                onGameStatePlaying?.Invoke();
            }
            else if(value == GameState.InteractionLock)
            {
                onGameStatePause?.Invoke();
            }

            currentGameState = value;
        }
    }
    #endregion

    public PlayerController Player { get; private set; }

    //외부에서 플레이어 받아오기
    public void SetPlayer(PlayerController player) => Player = player;

    private void Start()
    {
        CurrentGameState = GameState.Playing;
    }
}
