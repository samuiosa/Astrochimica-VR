using UnityEngine;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

public class AtomSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // Il prefab dell'oggetto da creare
    private AtomDestroyer destroyer; // Importo lo script di distruzione atomi per gestire il timer
    public float lifeTime=10; // Il tempo di vita di un atomo se non viene spostato
    public TextAsset coordinatesFile; // Il percorso del file CSV contenente le coordinate
    public float spawnInterval = 3.0f; // Intervallo tra la creazione di ciascun oggetto
    public float scala = 0.2f; // Fattore di scala per le coordinate lette dal file
    public Transform parentObject; // Oggetto genitore
    public Timer timer; // Riferimento al timer
    private List<string> lines; // Lista delle righe del file
    private int currentIndex = 0; // Indice corrente nella lista
    void Start()
    {
        destroyer=objectPrefab.GetComponent<AtomDestroyer>();
        destroyer.timeLimit=lifeTime;
        // Legge il file CSV
        if(coordinatesFile!=null){
            lines = new List<string>(coordinatesFile.text.Split('\n')); // Rimuove eventuali spazi vuoti o righe vuote
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            Shuffle(lines); // Mescola le righe
                
            // Avvia la creazione degli oggetti
            InvokeRepeating("SpawnNextObject", 0.0f, spawnInterval);
        }
    }
    void SpawnNextObject()
        {
            if ((currentIndex < lines.Count) && timer.timeUp==false) // Crea un nuovo oggetto se ci sono ancora coordinate nel file e il timer non è scaduto
            {
                string line = lines[currentIndex];
                string[] values = line.Split(',');

                if (values.Length >= 3)
                {
                    float x = float.Parse(values[0], CultureInfo.InvariantCulture)*scala;
                    float y = float.Parse(values[1], CultureInfo.InvariantCulture)*scala;
                    float z = float.Parse(values[2], CultureInfo.InvariantCulture)*scala;
                    
                    GameObject newObj = Instantiate(objectPrefab, new Vector3(x, y, z), Quaternion.identity); // Crea l'oggetto e posizionalo alle coordinate specificate
                    newObj.transform.parent=parentObject; // Assegna il genitore
                    currentIndex++;
                }
            }
            else
            {
                // Interrompe l'invocazione se sono stati creati tutti gli oggetti o se il timer è scaduto
                CancelInvoke("SpawnNextObject");
            }
        }

    // Funzione per mescolare una lista
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            T temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }
}