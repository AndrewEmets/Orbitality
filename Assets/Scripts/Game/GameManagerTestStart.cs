using UnityEngine;

public class GameManagerTestStart : MonoBehaviour
{
    [SerializeField] private int playersCount;
    
    [ContextMenu("Start game")]
    public void TestStart()
    {
        GetComponent<GameManager>().StartGame(new StartGameParameters()
        {
            playersCount = playersCount
        });
    }
}