# Astrochimica in realtà virtuale con Unity

## Setup di sviluppo

### Requisiti Preliminari
Per importare il progetto è necessaria una versione di Unity uguale o superiore alla 2022.3.19.f1, in quanto è quella utilizzata per lo sviluppo.
Il progetto può essere esportato per qualsiasi device VR che supporta OpenXR.

### Importare i package
Per replicare il progetto sono necessari alcuni package, per importarli è sufficiente:
1. Aprire Unity
2. Su 'Window > Package Manager' selezionare 'Packages: Unity Registry'
3. Selezionare i seguenti package e installarli uno alla volta:
  - XR Interaction Toolkit
  - XR Plugin Management
  - Oculus XR Plugin
  - OpenXR Plugin
  - Universal Render Pipeline (URP)
  - TextMeshPro
  - Visual Effect Graph

Altri package verranno importati automaticamente in base alle necessità.

### XR Device Simulator (necessario solo se non si dispone di un dispositivo per la realtà virtuale)
Questo strumento permette di sviluppare anche a chi non ha un headset VR in casa, simulando con mouse e tastiera gli input di un visore VR e dei suoi controller. Premesso che questa opzione è **veramente** scomoda, può essere utile se si devono testare delle cose semplici come guardarsi intorno o afferrare oggetti. In ogni caso vi invito a testare principalmente con un vero device.
Per attivarlo è necessario andare in 'Edit > Project Settings > XR Plug-In Management', estendendo l'opzione selezionare 'XR Interaction Toolkit' ed abilitare 'Use XR Device Simulator in scenes'.
Una volta fatto, quando entrerete in Play Mode avrete in basso a sinistra un menu con cui saranno spiegati i comandi del simulatore.

### Creazione di una scena
Per creare una scena funzionante in realtà virtuale sono necessari alcuni elementi fondamentali:
- Origine: per crearla è sufficiente creare un asset di tipo 'XR Origin (VR)' dal menu 'XR'
  - Camera e controller: creati in automatico con l'XR Origin, la loro configurazione verrà spiegata dopo
  - Sistema di locomozione: creare un asset di tipo 'Locomotion Systerm (Action based)' dal menu 'XR'
- Gestori di input e interazioni: tre gameobject in cui va importato lo script omonimo dal package VR importato prima
  - Input Action Manager: una volta importato lo script, inserire un Action Asset. L'asset 'XRI Default Input Actions (Input Action Asset)' disponibile nella repo è un estensione di quello fornito di default da Unity nella sample scene VR fornita dal software, nella cartella 'VRTemplateAssets'.
  - XR Interaction Manager: in questo caso va semplicemente importato lo script omonimo, senza configurare altro
  - Event System: oltre allo script 'Event System' inserire lo script 'XR UI Input Module', con i seguenti parametri. Le UI Action inserite sono prese direttamente dall'Input Action inserito prima. ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/c106d861-2a9b-44c7-9ee1-8a319dfcb4dd)
 
### Sistema di locomozione
Una volta creato l'asset, potete aggiungere gli elementi per il movimento e la rotazione della visuale dell'utente. Creare come oggetti figli del locomotion system due oggetti 'Move' e 'Turn'. Nel primo andrà inserito uno script per la gestione del movimento. In questo caso ho utilizzato 'DynamicMoveProvider', fornito da Unity nella scena di esempio per il VR, nella cartella 'VRTemplateAssets'. Andremo poi ad assegnare alle move action le azioni corrispondenti nell'Input Action di entrambi i controller. Stessa cosa andrà fatta per l'oggetto 'Turn', dove inseriremo gli script 'Snap Turn Provider' e 'Continuous Turn Provider'.

### Gestione dei controller
Se i controller inseriti da unity sono di tipo 'Device Based', cancellarli ed aggiungere dei controller 'Action Based' e configurarli aggiungendo gli interactor a voi necessari (che devono sempre essere action based). Nel mio caso sono serviti il [Direct Interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-direct-interactor.html) e il [Ray interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-ray-interactor.html). Questi interactor vanno poi inseriti nel controller all'interno dello script "Action Based Controller Manager", presente nella cartella Scripts nella repo. Anche questo script è fornito da Unity nel package 'VRTemplateAssets' di cui ho parlato prima. Gli interactor vanno poi inseriti anche nello script 'XR Interaction Group', che va aggiunto manualmente. Una volta fatto questo passaggio, vanno configurate le varie action negli script 'Action Based Controller Manager' e 'XR Controller' (Action-Based), aggiungendo i comandi dall'Input Action importato prima. Alla fine, dovreste avere una configurazione simile per entrambi i controller: ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/1ee88aa0-1d4a-46da-8c0a-122a271e0e4d)

Nella sezione 'Models' potete aggiungere un modello ai controller, nel mio caso ho inserito il modello di default sempre fornito da Unity nel package di default, che trovate nella cartella Models. Ho poi creato un prefab per il controller destro e sinistro semplicemente specchiandone uno.

Completato questo passaggio, dovreste avere nella gerarchia una struttura simile

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/79b000cb-dcd7-4d41-bc24-3770ccba727e)

Questa scena (che ho inserito come scena di default nella repo) può essere utilizzata come base da cui partire per costruire un ambiente VR simile a quelli utilizzati nel progetto.

 ### Gestione di Layer e tag
 Per gestire al meglio il funzionamento degli script e degli oggetti sulla scena ho creato alcuni layer e tag:
   - I layer servono a rendere alcuni oggetti invisibili al giocatore 'Nascosto' o visibili solo ad alcune camere: la UI Camera e la Post camera, camere overlay che visualizzano rispettivamente i layer 'VFX' e 'UI', in modo da poter gestire queste cose separatamente dalla camera principale in fase di test.
   - I tag servono a gestire il funzionamento degli script in modo più semplice, in particolare ho creato i tag 'AtomoO', 'AtomoH', 'Molecola' e 'Destroyer'

## Configurazione degli script
### Collega atomi
Questo script è responsabile della creazione dei collegamenti tra gli atomi. Lo script va assegnato a un solo atomo collegabile per evitare la formazione di doppi legami.
Nell'inspector vanno inseriti i seguenti oggetti:
  - **atomoCollegabile**: prefab dell'atomo con cui l'atomo corrente può essere collegato.
  - **link**: prefab del collegamento tra atomi
  - **molecolaPrefab**: prefab della molecola da creare
  - **maxCollegamenti**: numero massimo di atomi a cui può essere collegato prima di creare la molecola
Quando avviene una collisione tra due atomi collegabili con tag corrispondenti, un FixedJoint viene creato per collegarli. Viene quindi creato un oggetto link tra gli atomi per rappresentare il collegamento. Quando il numero massimo di collegamenti viene raggiunto, viene istanziato il prefab della molecola, distruggendo gli atomi collegati e l'atomo corrente.

### Atom Destroyer
Questo script distruzione degli atomi che non vengono utilizzati o escono dai limiti della mappa. È assegnato ai prefab degli atomi in modo che possa essere configurato per gestire il tempo di vita massimo degli atomi inutilizzati.
Un riferimento a questo script viene inserito in ogni minigioco per consentire la gestione personalizzata del "timeLimit" in base alle esigenze specifiche del gioco.
  -**timeLimit** è il tempo massimo di vita di un atomo inutilizzato. Se un atomo resta alla sua posizione di spawn per un periodo di tempo superiore a questo limite, verrà distrutto.
L'altro caso in cui l'atomo viene distrutto è se entra in contatto con un collider con il tag "Destroyer"

### Atom Spawner
Questo script gestisce lo spawn degli atomi nel primo minigioco. Deve essere assegnato a un oggetto spawner **per ogni atomo**. Le coordinate vengono prese da un file CSV, le cui righe sono rimescolate. Poi moltiplica le coordinate per un fattore di scala in modo da poter regolare a piacimento le dimensioni dell'ambiente. Lo script spawna periodicamente un atomo basato sulle coordinate fornite finchè ci sono ancora righe da leggere e il timer non è scaduto.
Per utilizzare lo script è necessario configurare questi oggetti nell'inspector:
  - **objectPrefab**: Prefab dell'atomo da spawnare.
  - **lifeTime** Tempo di vita massimo di un atomo inutilizzato 
  - **coordinatesFile**: File CSV con le coordinate
  - **spawnInterval**: Intervallo di tempo tra gli spawn
  - **scala**: Fattore di scala
  - **parentObject**: Parent per gli atomi in modo da mantenere ordinata la gerarchia della scena.
  - **timer**: Riferimento al timer

### Atom Random
Questo script gestisce lo spawn casuale degli atomi nel secondo gioco. Esso deve essere assegnato a un oggetto spawner che abbia un collider. Gli atomi verranno quindi generati in posizioni casuali all'interno del collider. La frequenza di spawn è controllata da un intervallo, e lo spawn avviene solo se il timer è ancora attivo.
Gli oggetti da configurare nell'inspector sono:
  - in **atomPrefabO** e **atomPrefabH** vanno inseriti i prefab corrispondenti
  - **lifeTime**: Tempo massimo di vita di un atomo inutilizzato
  - **spawnInterval**: Intervallo di tempo tra gli spawn degli atomi
  - **timer**: Riferimento al timer per bloccare lo spawn quando arriva alla fine

### Dimensione Atomo
Questo script va assegnato al prefab di ogni atomo. Esso utilizza il raggio di Van Der Waals come input per determinare le dimensioni, scalandolo a 1/10 dell'unità base di Unity.

### Molecola
Questo script gestisce gli eventi associati all'apparizione di una molecola a schermo. In particolare vengono avviati un effetto particellare e una clip sonora. Inoltre, gestisce la situazione in cui una molecola tocca un oggetto "Destroyer" per evitare che molecole non utilizzate rimangano vaganti per la scena. A differenza degli atomi, una molecola non viene eliminata per inattività.
Nell'inspector vanno configurati
  - **AudioSource** con la clip audio scelta
  - **ParticleSystem** il particle system che indica all'utente che ha avuto successo. Nel mio caso ho utilizzato i seguenti parametri:
![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/08876b0b-4a7a-4240-af57-19ec65a355dd)

### Grano
Questo script va assegnato al grano di polvere e gestisce il suo comportamento in entrambi i giochi. Ad ogni collisione con una molecola corretta esso incrementa il punteggio, aumenta l'alpha del materiale del grano e sostituisce la molecola con il modello di Van Der Waals corrispondente. La funzione ResetGrano viene richiamata (nel secondo gioco) da movGrano quando viene modificata la difficoltà.
Nell'inspector vanno configurati
  - **granoMaterial**: Materiale del grano di polvere
  - **score**: Riferimento al punteggio
  - **tranparency**: Trasparenza iniziale del grano (che altrimenti non verrebbe resettata)
  - **prefabVanDerWaals**: Prefab della molecola di Van Der Waals

### Mov Grano
Questo script va assegnato al grano e gestisce i suoi movimenti nel secondo gioco, regolando il comportamento in base alla difficoltà scelta dal giocatore dal dropdown nel menu di pausa. Ogni volta che il valore cambia, la funzione ImpostaDifficoltà viene chiamata per aggiornare l'opzione corrente. La funzione corrispondente alla difficoltà scelta viene chiamata ad ogni frame per mantenere il movimento coerente con l'opzione selezionata.
Nell'inspector vanno configurati
  - **velocitaFacile**: velocità di movimento del grano quando la difficoltà è impostata su "Facile"
  - **velocitaDifficile**: velocità di movimento del grano quando la difficoltà è impostata su "Difficile"
  - **offsetY**: offset sull'asse Y per il movimento sinusoidale del grano nella modalità "Difficile"
  - **offsetZ**: come sopra, ma per l'asse Z
  - **posIniziale**:posizione iniziale del grano
  - **posFinale**:posizione finale del grano.
  - **Timer**: riferimento al timer
  - **Score**: riferimento al punteggio
  - **dropdownDifficolta**: riferimento al dropdown nel menu di pausa

La sezione OnValueChanged del dropdown viene quindi configurata come segue, chiamando la funzione corrispondente all'interno di questo script:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/f736706a-2413-48a2-bfa5-888ce1356e9a)

### Level Manager
Gestisce la transizione tra le diverse scene del gioco. Fornisce metodi per cambiare scena, riavviare il livello e uscire dal gioco. Nel mio caso l'ho assegnato ad un GameObject vuoto.
I metodi di questo script saranno quindi collegati ai metodi onClick del menu principale e del menu di pausa, in questo esempio lo script viene chiamato alla pressione di un bottone per passare alla scena con id 2:
![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/8a99f9ae-a8fc-4fae-a56d-f9ace63b57c3)

### Pause Menu
Gestisce il menu di pausa nel gioco. Quando il giocatore preme il tasto di pausa sul controller (tramite InputActionReference), viene chiamata la funzione TogglePauseMenu. Questa funzione verifica se il gioco è attualmente in pausa o meno, e in base a ciò attiva o disattiva il pannello del menu di pausa. Inoltre, regola il tempo di gioco, attiva o disattiva i Direct e Ray interactor per consentire l'interazione con il menu di pausa. Il tasto di pausa va configurato nell'Input Action Asset, aggiungendo un tasto all'azione come nell'immagine. N.B: evitate di utilizzare il Menu button, in quanto per visori come il Meta Quest è usato per tornare al menu del visore, ignorando l'input in-game. Nel mio caso ho utilizzato il "Primary Button" di entrambi i controller.

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/9048b795-3bb6-44e1-8e99-11d8f2346e7f)

Nell'inspector vanno configurati
  - **pausaButton**: riferimento al tasto di pausa sul controller
  - **Pannello**: Riferimento al pannello del menu di pausa
  - **directInteractor**: Riferimento al Direct Interactor
  - **rayInteractor**: Riferimento al Ray Interactor

### Score
Questo script va assegnato ad un oggetto TextMeshPro, che mostrerà il punteggio. Esso viene aggiornato ad ogni frame in base alle azioni del giocatore. Sarà il grano ad aggiornare il punteggio ogni volta che rileva il collegamento di una molecola corretta.
Nell'inspector vanno configurati
  - **score**: Punteggio del giocatore, aggiornabile da SetScore
  - **timer**: Riferimento al timer in modo da aggiornare il punteggio solo quando il timer è attivo

### Time Display
Anche questo script va assegnato ad un oggetto TextMeshPro, che mostrerà il tempo rimanente, ignorando la parte decimale. L'aggiornamento avviene ad ogni frame decrementando il tempo fino allo 0, quando viene visualizzato il testo "Fine". La funzione "ResetTimer" viene chiamata per resettare il timer quando il giocatore cambia la difficoltà.
Nell'inspector vanno configurati
  - **timeRemaining** tempo attuale
  - **timeUp** boolean che indica se il tempo è scaduto

### Shooting Star
Va assegnato ad un oggetto vuoto in modo da gestire la creazione e il movimento di particelle che rappresentano una stella cadente. La stella cade in una direzione casuale con parametri come l'intervallo di spawn, la velocità, il tempo di vita e la distanza dallo spawner.
Nell'inspector vanno configurati
  - **particleSystemPrefab**:Prefab del sistema di particelle che rappresenta la stella cadente
  - **spawnInterval**:Intervallo di tempo tra una stella e la successiva
  - **destroyTime**:Tempo di vita di ciascuna stella cadente
  - **spawnDistance**:Massima distanza in cui generare una stella a partire dalla posizione dello spawner
  - **speed**:Velocità di movimento delle stelle cadenti

### Stars Manager
Gestisce un sistema di particelle che rappresenta le stelle nell'ambiente spaziale. Esso assicura che il sistema di particelle segua la camera principale (in modo da creare l'illusione che le stelle siano nello spazio) e aggiorna dinamicamente il colore e la luminosità delle stelle.
Nell'inspector va configurata
  - **Brightness**: Luminosità desiderata delle stelle
