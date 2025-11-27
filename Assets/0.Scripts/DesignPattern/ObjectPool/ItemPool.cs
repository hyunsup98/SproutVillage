using UnityEngine;

public class ItemPool : ObjectPool<Item>
{
    protected override void Awake()
    {
        base.Awake();
    }
}
