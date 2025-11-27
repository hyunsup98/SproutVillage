using UnityEngine;
using UnityEngine.InputSystem;

//작업대에서 씨앗을 사고 아이템을 팔 수 있음
public class WorkTable : MonoBehaviour
{
    [SerializeField] private GameObject workTable;
    private bool canInteract;

    private void Update()
    {
        if(canInteract)
        {
            if(Keyboard.current.fKey.wasPressedThisFrame)
            {
                GameManager.Instance.CurrentGameState = GameState.InteractionLock;
                workTable.SetActive(true);
                UIManager.Instance.EnabledInteractableText(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canInteract = true;
            UIManager.Instance.EnabledInteractableText(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.CurrentGameState = GameState.Playing;
        canInteract = false;
        UIManager.Instance.EnabledInteractableText(false);
        workTable.SetActive(false);
    }
}
