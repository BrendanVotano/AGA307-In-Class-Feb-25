using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static PlayerController _PLAYER { get { return PlayerController.instance; } }

    public GameState _CurrentGameState => GameManager.instance.GameState;

    /// <summary>
    /// Shuffles a list using Unity's Random
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    /// <param name="_list">The list to shuffle</param>
    /// <returns></returns>
    public static List<T> ShuffleList<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = UnityEngine.Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }

    /// <summary>
    /// Gets the closest object from a list of GameObjects
    /// </summary>
    public Transform GetClosestObject(Transform _origin, List<GameObject> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = go.transform;
            }
        }
        return closest;
    }

    public Transform GetClosestObject(Transform _origin, List<Enemy> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (Enemy go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = go.transform;
            }
        }
        return closest;
    }
}
