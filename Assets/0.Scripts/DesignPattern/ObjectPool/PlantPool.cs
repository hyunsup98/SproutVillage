using UnityEngine;

public class PlantPool : ObjectPool<Plant>
{
    protected override void Awake()
    {
        base.Awake();
    }
}
