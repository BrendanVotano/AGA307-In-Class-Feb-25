using UnityEngine;

public enum GameState{ Start, Playing, Paused, GameOver}
public enum Difficulty { Easy, Medium, Hard, Nightmare}

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public Difficulty difficulty;
}
