using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject door;


    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o que deve ativar a porta
        if (other.CompareTag("Player")) // Substitua "Player" pela tag apropriada
        {
            // Ativa a porta
            door.SetActive(true);

            // Desativa a colisão do objeto atual
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
