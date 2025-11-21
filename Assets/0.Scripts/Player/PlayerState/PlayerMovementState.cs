using UnityEngine;

public abstract class PlayerMovementState : IState
{
    protected PlayerController player;

    public PlayerMovementState(PlayerController player)
    {
        this.player = player;
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();
}
