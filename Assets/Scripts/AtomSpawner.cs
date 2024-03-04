using UnityEngine;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

/* Questo script gestisce lo spawn degli atomi nel primo minigioco,  esso va assegnato ad un oggetto spawner per ogni atomo:
Prende delle coordinate da un file .csv, rimescolandolo e moltiplicando le coordinate per un fattore di scala in modo da gestire quanto è grande l'ambiente. Nella mia versione è 0.25, ma potete sperimentare
Dopodichè SpawnNextObject spawna un atomo in base alle coordinate fornite nel file CSV. Viene chiamata periodicamente ogni spawnInterval secondi fintanto che ci sono ancora righe da leggere e il timer non è scaduto*/
public class AtomSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab dell'atomo
    public float lifeTime=10; // Tempo di vita di un atomo inutilizzato
    public TextAsset coordinatesFile; // File CSV con le coordinate
    public float spawnInterval = 3.0f; // Intervallo di tempo tra gli spawn
    public float scala = 0.2f; // Fattore di scala
    public Transform parentObject; // Parent per gli atomi per mantenere ordinata la gerarchia della scena
    public Timer timer; // Riferimento al timer per bloccare lo spawn quando arriva alla fine
    private AtomDestroyer destroyer;
    private List<string> lines;
    private int currentIndex = 0;
    void Start()
    {
        destroyer=objectPrefab.GetComponent<AtomDestroyer>();
        destroyer.timeLimit=lifeTime;
        if(coordinatesFile!=null){
            lines = new List<string>(coordinatesFile.text.Split('\n'));
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            Shuffle(lines);
            InvokeRepeating("SpawnNextObject", 0.0f, spawnInterval);
        }
    }
    void SpawnNextObject()
        {
            if ((currentIndex < lines.Count) && timer.timeUp==false)
            {
                string line = lines[currentIndex];
                string[] values = line.Split(',');

                if (values.Length >= 3)
                {
                    //CultureInfo.InvariantCulture serve ad assicurarsi che il separatore decimale è sempre il punto (.) indipendentemente dalle impostazioni culturali dell'utente
                    float x = float.Parse(values[0], CultureInfo.InvariantCulture)*scala;
                    float y = float.Parse(values[1], CultureInfo.InvariantCulture)*scala;
                    float z = float.Parse(values[2], CultureInfo.InvariantCulture)*scala;
                    
                    GameObject newObj = Instantiate(objectPrefab, new Vector3(x, y, z), Quaternion.identity); 
                    newObj.transform.parent=parentObject;
                    currentIndex++;
                }
            }
            else
            {
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