using System;
using UnityEngine;

public enum GameState
{
    Title,      //타이틀 화면일 때
    Playing,    //인게임에서 플레이중일 때
    Pause,      //인게임이지만 일시중지 상태일 때
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
            else if(value == GameState.Pause)
            {
                onGameStatePause?.Invoke();
            }

            currentGameState = value;
        }
    }
    #endregion

    public PlayerController Player { get; private set; }

    //외부에서 플레이어 받아오기
    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }
}
