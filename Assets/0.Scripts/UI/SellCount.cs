using TMPro;
using UnityEngine;

public class SellCount : MonoBehaviour
{
    [SerializeField] private TMP_InputField splitItemCountField;
    private Slot slot;
    private int count;

    public void Init(Slot slot)
    {
        this.slot = slot;
    }

    public void OnClickCancel()
    {
        gameObject.SetActive(false);
    }

    public void OnClickAccept()
    {
        if(count != 0 && slot != null)
        {
            GameManager.Instance.Player.Gold += slot.SlotItem.itemData.sellPrice * count;
            slot.SlotItem.Count -= count;
            slot.UpdateSlotData();
            gameObject.SetActive(false);
        }
    }

    public void OnValueChangedCount()
    {
        var isNum = int.TryParse(splitItemCountField.text, out count);

        if (slot.SlotItem.Count < count)
            count = slot.SlotItem.Count;
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        splitItemCountField.text = string.Empty;
        slot = null;
        count = 0;
    }
}
