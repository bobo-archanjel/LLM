using UnityEngine;

public class CameraController : MonoBehaviour
{
    //room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhaed;

    private void Update()
    {
        //room camera
        // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.localPosition.y, transform.localPosition.z), ref velocity, speed);

        //player camera
        transform.position = new Vector3(player.position.x + lookAhaed, transform.position.y, transform.position.z);
        lookAhaed = Mathf.Lerp(lookAhaed, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }

}
