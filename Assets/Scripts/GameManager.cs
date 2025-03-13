using UnityEngine;

public enum GameState{ Start, Playing, Paused, GameOver}
public enum Difficulty { Easy, Medium, Hard, Nightmare}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState gameState;
    public GameState GameState => gameState;
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private int score;

    public void AddScore(int _score)
    {
        score += _score;
        print("Score: " + score);
    }
}
