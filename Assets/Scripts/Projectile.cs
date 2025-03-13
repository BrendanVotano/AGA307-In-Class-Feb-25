using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticles;
    public int damage;
    void Start()
    {
        Invoke("DestroyProjectile", 3);
    }

    public void DestroyProjectile()
    {
        GameObject particles = Instantiate(hitParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<Enemy>() != null)
                collision.gameObject.GetComponent<Enemy>().Hit(damage);
        }
        DestroyProjectile();
    }

}
