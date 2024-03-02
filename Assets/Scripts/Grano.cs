using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Grano : MonoBehaviour
{
    public Material granoMaterial;
    public Score score;
    public float tranparency=1f; //  Valore di trasparenza iniziale
    public GameObject prefabWanDerWaals;
    private List<GameObject> molecoleInside = new List<GameObject>(); // Lista delle molecole attualmente sulla superficie del grano

    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();

        Color granoColor = granoMaterial.color;
        granoColor.a = 0.03921569f*tranparency;
        granoMaterial.color = granoColor;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Verifica se l'oggetto con cui si è verificata una collisione ha il tag "Molecola"
        // E se la molecola non è già presente nella lista delle molecole all'interno del grano
        if (other.gameObject.CompareTag("Molecola") && !molecoleInside.Contains(other.gameObject) )
        {
            IncrementaPunteggio();
            ChangeAlpha(); // Cambia l'alpha del grano

            // Memorizza la posizione e la rotazione della molecola corrente
            Vector3 molecolaPosition = other.transform.position;
            Quaternion molecolaRotation = other.transform.rotation;
            Rigidbody molecolaRigidbody = other.gameObject.GetComponent<Rigidbody>(); // Ferma il movimento della molecola
            molecolaRigidbody.isKinematic = true;

            // Distruggi l'oggetto molecola corrente
            Destroy(other.gameObject);

            // Crea una nuova istanza del prefab sostitutivo con la molecola di Wan Der Waals
            GameObject nuovaMolecola = Instantiate(prefabWanDerWaals, molecolaPosition, molecolaRotation);
            nuovaMolecola.transform.parent = transform; // Rendi la nuova molecola figlia del grano

            molecoleInside.Add(nuovaMolecola); // Aggiungi la nuova molecola alla lista delle molecole attaccate al grano

        }
    }

    private void IncrementaPunteggio()
    {
        int currentScore = score.GetScore();
        score.SetScore(currentScore + 1);
    }

    private void ChangeAlpha() // Modifica l'alpha del grano gradualmente ad ogni chiamata
    {
        if (granoMaterial != null)
        {
            Color granoColor = granoMaterial.color;
            granoColor.a = granoColor.a + 0.02f;
            granoMaterial.color = granoColor;
        }
    }
    public void ResetGrano()
    {
        // Resetta l'alpha del grano
        Color granoColor = granoMaterial.color;
        granoColor.a = 0.03921569f * tranparency;
        granoMaterial.color = granoColor;

        // Distruggi tutti gli oggetti figli
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Pulisci la lista delle molecole attaccate
        molecoleInside.Clear();
    }
}
