using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Target") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit the object");
            Destroy(gameObject);
        }

        
    }
}
