using UnityEngine;

public enum GameState
{
    playing,
    paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

}
