using UnityEngine;

public class RespawnTrigger : MonoBehaviour {
    public GameObject respawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = respawn.transform.position;
        }
    }
}
