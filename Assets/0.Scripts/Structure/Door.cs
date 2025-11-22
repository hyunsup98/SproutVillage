using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnim;

    private void Awake()
    {
        TryGetComponent(out doorAnim);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(doorAnim != null)
            {
                doorAnim.SetBool("isOpen", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (doorAnim != null)
            {
                doorAnim.SetBool("isOpen", false);
            }
        }
    }
}
