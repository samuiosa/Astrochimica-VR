using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Collider))]
/*Lo script gestisce il comportamento del grano di polvere in entrambi i giochi.
Ad ogni molecola corretta che lo tocca, viene aumentato il punteggio e l'alpha del suo materiale in modo che il colore sia più acceso
Inoltre sostituisce la molecola con il modello di Van Der Waals
La funzione ResetGrano verrà richiamata (nel secondo gioco) da movGrano quando viene modificata la difficoltà*/
public class Grano : MonoBehaviour
{
    public Material granoMaterial;//Materiale del grano
    public Score score; //Riferimento al punteggio
    public float tranparency=1f; //Trasparenza iniziale
    public GameObject prefabVanDerWaals; //Prefab della molecola di Van der Waals
    private List<GameObject> molecoleInside = new List<GameObject>();

    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
        Color granoColor = granoMaterial.color;
        granoColor.a = 0.03921569f*tranparency;
        granoMaterial.color = granoColor;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Molecola") && !molecoleInside.Contains(other.gameObject) )
        {
            IncrementaPunteggio();
            ChangeAlpha();
            Vector3 molecolaPosition = other.transform.position;
            Quaternion molecolaRotation = other.transform.rotation;
            Rigidbody molecolaRigidbody = other.gameObject.GetComponent<Rigidbody>();
            molecolaRigidbody.isKinematic = true;
            Destroy(other.gameObject);

            GameObject nuovaMolecola = Instantiate(prefabVanDerWaals, molecolaPosition, molecolaRotation);
            nuovaMolecola.transform.parent = transform;
            molecoleInside.Add(nuovaMolecola);

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
    public void ResetGrano()// Viene chiamata se cambio difficoltà nel secondo gioco, in modo da resettare completamente il livello
    {
        Color granoColor = granoMaterial.color;
        granoColor.a = 0.03921569f * tranparency;
        granoMaterial.color = granoColor;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        molecoleInside.Clear();
    }
}
