using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float smoothSpeed = 2f;
    private float halfWidth, halfHeight;


    private void Start()
    {
        //aspect → 카메라 화면 너비를 높이로 나눈 값 반환, Width / Height
        //orthographicSize → 카메라의 세로 절반 사이즈
        halfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        halfHeight = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target == null) return;

        Vector3 pos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime);
    }
}
