using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    public float threshold;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < threshold) {
            transform.position = new Vector3(0f, 2.81f, 0f);
            GameManager.health -= 1;
        }
    }
}
