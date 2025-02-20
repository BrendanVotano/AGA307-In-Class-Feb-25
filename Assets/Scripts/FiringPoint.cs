using UnityEngine;

public class FiringPoint : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 1000f;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
            FireProjectile();
            
    }

    private void FireProjectile()
    {
        //Instantiate our projectile prefab
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        //Get the rigidbody of our projectile and add force to it
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed); 
    }
}
