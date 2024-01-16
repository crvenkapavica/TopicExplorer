using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10f; 


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal"); 
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        CheckBounds();
    }

    private void CheckBounds()
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        Vector3 position = transform.position;

        if (position.x < -screenWidth)
        {
            position.x = -screenWidth;
        }
        if (position.x > screenWidth)
        {
            position.x = screenWidth;
        }
        
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "obstacle")
        {
            GameObject.FindObjectOfType<Game>().EndGame();
        }
    }
}