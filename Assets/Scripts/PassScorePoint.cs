using UnityEngine;

public class PassScorePoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameManager.singleton.AddScore(1);
        //FindObjectOfType<BallController>().perfectPass++;
        FindAnyObjectByType<BallController>().perfectPass++;
    }
}
