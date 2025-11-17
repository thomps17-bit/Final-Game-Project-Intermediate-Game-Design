using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver, restart, heart1, heart2, heart3, background;
    public static int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 3;
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (health)
        {

          case 3:
          heart1.gameObject.SetActive(true);
          heart2.gameObject.SetActive(true);
          heart3.gameObject.SetActive(true);
          break;

          case 2:
          heart1.gameObject.SetActive(true);
          heart2.gameObject.SetActive(true);
          heart3.gameObject.SetActive(false);
          break;

           case 1:
          heart1.gameObject.SetActive(true);
          heart2.gameObject.SetActive(false);
          heart3.gameObject.SetActive(false);
          break;

          default:
          heart1.gameObject.SetActive(false);
          heart2.gameObject.SetActive(false);
          heart3.gameObject.SetActive(false);
          gameOver.gameObject.SetActive(true);
          restart.gameObject.SetActive(true);
          background.gameObject.SetActive(true);
          Time.timeScale = 0;
          break;
        }
    }

   
    
}
