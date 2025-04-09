using UnityEngine;

public class Doors : MonoBehaviour
{
    public Animator anim;
    public GameObject leftDoor;
    public GameObject rightDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            anim.SetTrigger("DoorOpen");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("DoorClose");
        }
    }
}
