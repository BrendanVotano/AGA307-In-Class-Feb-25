using UnityEngine;

public class Playground : MonoBehaviour
{
    public int health = 100;
    public GameObject lightObject;
    public Light lightComponent;

    void Start()
    {
        //lightObject.SetActive(false);
        lightComponent.color = Color.green;
        lightComponent.intensity = 5;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //health = ChangeHealth(10);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AdjustHealth(9);
        }
    }

    private int ChangeHealth(int _value)
    {
        return health += _value;
    }

    private void TakeDamage(int _damage)
    {
        //This takes damage 
        health -= _damage;
        //print("Health is " + health);
    }

    private void AddHealth(int _bonus)
    {
        health += _bonus;
        //print("Health is " + health);
    }

    private void AdjustHealth(int _value)
    {
        health += _value;
        //print("Health is " + health);
    }
}
