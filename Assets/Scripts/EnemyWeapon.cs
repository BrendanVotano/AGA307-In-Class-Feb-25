using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private int damage = 10;
    private Collider col;

    private void Start()
    {
        if (GetComponent<Collider>() != null)
        {
            col = GetComponent<Collider>();
            SetCollider(false);
        }
    }
    public void SetDamage(int _damage) => damage = _damage;

    public void SetCollider(bool _enabled)
    {
        if (col == null)
            return;
            
        col.enabled = _enabled;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
