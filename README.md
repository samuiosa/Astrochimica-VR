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
