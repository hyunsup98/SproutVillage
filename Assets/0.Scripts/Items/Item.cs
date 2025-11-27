using UnityEngine;

public class Item : MonoBehaviour
{
    [field : SerializeField] public ItemSO itemData { get; private set; }   //아이템 정보 관련 Scriptable Object

    [SerializeField] private int count = 1;
    public int Count
    {
        get { return count; }
        set
        {
            if (value <= 0)
                value = 0;
            else if (value > itemData.maxCount)
                value = itemData.maxCount;

            count = value;

            if(count <= 0)
            {
                ItemPool.Instance.TakeObjects(this);
            }
        }
    }

    private void OnEnable()
    {
        Count = 1;
    }
}
