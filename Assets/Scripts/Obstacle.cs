using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 9f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector2.zero).y - 1)
        {
            GameObject.FindObjectOfType<Game>().IncrementScore();
            Destroy(gameObject);
        }
    }
}