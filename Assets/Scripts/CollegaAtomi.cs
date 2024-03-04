using UnityEngine;
using System.Collections.Generic;
using System.Linq;
/*Questo script gestisce il collegamento tra gli atomi
Quando viene rilevata una collisione tra due atomi collegabili (controllando i tag) viene creato un FixedJoint e viene aggiunto un oggetto link
Se viene raggiunto il numero massimo di collegamenti gli oggetti vengono tutti distrutti e al loro posto viene istanziato il modello della molecola.
In questo modo si può gestire la molecola in modo più semplice essendo un oggetto unico, invece di un insieme di 3+ oggetti diversi

N.B: Allo stato attuale va assegnato ad uno solo degli atomi collegabili, in quanto assegnandolo ad entrambi si creerebbero dei doppi legami.
ESEMPIO: Se assegno lo script all'ossigeno perchè voglio creare l'acqua, in atomoCollegabile metto il prefab dell'idrogeno e in maxCollegamenti metto 2. Sull'atomo di idrogeno non aggiungo lo script.
In questo modo, appena collego 2 atomi di idrogeno a uno di ossigeno formo la molecola.*/
public class CollegaAtomi : MonoBehaviour
{
    public GameObject atomoCollegabile; //Atomo che voglio collegare
    public GameObject link; //Modello del collegamento tra gli atomi
    public GameObject molecolaPrefab; //Prefab della molecola da creare
    public int maxCollegamenti=0; //Quanti atomi posso collegare prima di creare la molecola
    private string atomoTag;
    private List<GameObject> atomiCollegati = new List<GameObject>();
    private bool destructionScheduled=false; //Boolean che mi assicura che una molecola venga istanziata una sola volta e gli oggetti che la compongono vengano distrutti solo in quel caso
    void Start(){
        atomoTag=atomoCollegabile.gameObject.tag;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!string.IsNullOrEmpty(atomoTag) && collision.gameObject.CompareTag(atomoTag) && atomiCollegati.Count < maxCollegamenti)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody;
            atomiCollegati.Add(collision.gameObject);
            CreaLink(gameObject, collision.gameObject);

            if (atomiCollegati.Count==maxCollegamenti && !destructionScheduled){
                GameObject molecolaCorretta = Instantiate(molecolaPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
                foreach (GameObject atom in atomiCollegati)
                {
                    Destroy(atom);
                }
                destructionScheduled = true;
            }
        }       
    }
    void CreaLink(GameObject atom1, GameObject atom2) //La scelta della posizione e le rotazioni applicate al link servono a farlo effettivamente "puntare" gli atomi come fosse un legame chimico
    {
        if (link != null)
        {
            Vector3 linkPosition = (atom1.transform.position + atom2.transform.position) / 2f;
            Vector3 direction = (atom2.transform.position - atom1.transform.position).normalized;
            Quaternion linkRotation = Quaternion.LookRotation(direction);
            linkRotation *= Quaternion.Euler(90, 0, 0);
            GameObject linkInstance = Instantiate(link, linkPosition, linkRotation);
            linkInstance.transform.parent = atom1.transform;
        }
    }
    // Per pulizia rimuovo gli oggetti disattivati dalla lista degli atomi collegati
    void Update(){
        atomiCollegati = atomiCollegati.Where(atom => atom != null).ToList();
    }
}