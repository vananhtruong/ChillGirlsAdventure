using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}
