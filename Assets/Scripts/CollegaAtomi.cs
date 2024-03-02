using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CollegaAtomi : MonoBehaviour
{
    public GameObject atomoCollegabile;
    public GameObject link;
    public GameObject molecolaPrefab;
    private string atomoTag;
    public int maxCollegamenti=0;
    private Score score;
    private List<GameObject> atomiCollegati = new List<GameObject>();
    private bool destructionScheduled=false;
    void Start(){
        score = GameObject.Find("Score").GetComponent<Score>();
        atomoTag=atomoCollegabile.gameObject.tag;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!string.IsNullOrEmpty(atomoTag) && collision.gameObject.CompareTag(atomoTag) && atomiCollegati.Count < maxCollegamenti)
        {
            // Aggiunge un componente FixedJoint all'oggetto corrente
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody; // Collega l'altro oggetto
            atomiCollegati.Add(collision.gameObject);
            CreaLink(gameObject, collision.gameObject); //crea il collegamento tra atomi

            Debug.Log("Questo atomo ha "+atomiCollegati.Count+" collegamenti");
            Debug.Log("Oggetti attaccati!");
            if (atomiCollegati.Count==maxCollegamenti && !destructionScheduled){
                
                // Distruggi l'oggetto corrente insieme agli oggetti collegati
                // E istanzia il prefab della molecola al suo posto
                GameObject molecolaCorretta = Instantiate(molecolaPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
                foreach (GameObject atom in atomiCollegati)
                {
                    Destroy(atom);
                }
                // Imposta il flag di distruzione
                destructionScheduled = true;
            }
        }       
    }
    void CreaLink(GameObject atom1, GameObject atom2)
    {
        if (link != null)
        {
            // Calcola la posizione media tra gli atomi collegati
            Vector3 linkPosition = (atom1.transform.position + atom2.transform.position) / 2f;

            // Calcola la rotazione per far puntare il link tra gli atomi
            Vector3 direction = (atom2.transform.position - atom1.transform.position).normalized;
            Quaternion linkRotation = Quaternion.LookRotation(direction);
            
            // Aggiungi una rotazione di 90 gradi sull'asse X
            linkRotation *= Quaternion.Euler(90, 0, 0);

            // Crea un'istanza del link tra gli atomi
            GameObject linkInstance = Instantiate(link, linkPosition, linkRotation);
            // Rendi il link un figlio degli atomi collegati
            linkInstance.transform.parent = atom1.transform;
        }
    }
    void Update(){
        // Rimuovi gli oggetti disattivati dalla lista
        atomiCollegati = atomiCollegati.Where(atom => atom != null).ToList();
    }
}