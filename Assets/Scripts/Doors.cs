using UnityEngine;

public class Doors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            leftDoor.SetActive(true);
            rightDoor.SetActive(true);
        }
    }
}
