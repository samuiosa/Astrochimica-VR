# 🇬🇧 Astrochemistry in Virtual Reality with Unity

## Development Setup

### Preliminary Requirements
To import the project, you need a version of Unity equal to or higher than 2022.3.19.f1, as this is the version used for development. The project can be exported for any VR device that supports OpenXR.

### Importing Packages
To replicate the project, some packages are necessary. To import them, simply:
1. Open Unity
2. In 'Window > Package Manager', select 'Packages: Unity Registry'
3. Select and install the following packages one by one:
  - XR Interaction Toolkit
  - XR Plugin Management
  - Oculus XR Plugin
  - OpenXR Plugin
  - Universal Render Pipeline (URP)
  - TextMeshPro
  - Visual Effect Graph

Other packages will be imported automatically as needed.

### XR Device Simulator (only needed if you do not have a VR device)
This tool allows development even for those without a VR headset at home, simulating VR headset and controller inputs with mouse and keyboard. Although this option is **very** inconvenient, it can be useful for testing simple tasks like looking around or grabbing objects. However, I encourage you to mainly test with a real device.
To activate it, go to 'Edit > Project Settings > XR Plug-In Management', expand the option, select 'XR Interaction Toolkit', and enable 'Use XR Device Simulator in scenes'. Once done, when you enter Play Mode, you'll have a menu at the bottom left explaining the simulator's commands.

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/8b032d4b-79f6-42aa-934d-fed02f0ceed7)

### Creating a Scene
To create a functional VR scene, some fundamental elements are necessary:
- Origin: create an 'XR Origin (VR)' asset from the 'XR' menu
  - Camera and controllers: automatically created with the XR Origin, their configuration will be explained later
  - Locomotion system: create a 'Locomotion System (Action based)' asset from the 'XR' menu
- Input and interaction managers: three GameObjects where you import the corresponding script from the previously imported VR package
  - Input Action Manager: once the script is imported, insert an Action Asset. The 'XRI Default Input Actions (Input Action Asset)' available in the repo is an extension of the one provided by Unity in the default VR sample scene, found in the 'VRTemplateAssets' folder.
  - XR Interaction Manager: simply import the script without further configuration
  - Event System: besides the 'Event System' script, insert the 'XR UI Input Module' script with the following parameters. The UI Actions are directly taken from the previously inserted Input Action. ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/c106d861-2a9b-44c7-9ee1-8a319dfcb4dd)

### Locomotion System
Once the asset is created, you can add elements for user movement and view rotation. Create two 'Move' and 'Turn' objects as children of the locomotion system. Insert a movement management script in the 'Move' object. In this case, I used 'DynamicMoveProvider', provided by Unity in the VR example scene, found in the 'VRTemplateAssets' folder. Assign the corresponding move actions to both controllers in the Input Action. Do the same for the 'Turn' object, where you will insert the 'Snap Turn Provider' and 'Continuous Turn Provider' scripts.

### Managing Controllers
If the controllers inserted by Unity are 'Device Based', delete them and add 'Action Based' controllers, configuring them by adding the necessary interactor scripts (which must always be action based). In my case, I used the [Direct Interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-direct-interactor.html) and the [Ray Interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-ray-interactor.html). Insert these interactors into the controller within the "Action Based Controller Manager" script, available in the Scripts folder of the repo. This script is also provided by Unity in the 'VRTemplateAssets' package. Then, add the interactors to the 'XR Interaction Group' script, which must be added manually. Once this step is completed, configure the various actions in the 'Action Based Controller Manager' and 'XR Controller' (Action-Based) scripts, adding commands from the previously imported Input Action. In the end, you should have a similar configuration for both controllers: ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/1ee88aa0-1d4a-46da-8c0a-122a271e0e4d)

In my case, I increased the sphere collider radius of the direct interactor, effectively increasing the range with which objects can be touched. You can experiment with this value based on your needs.

In the 'Models' section, you can add a model to the controllers. In my case, I used the default model provided by Unity, found in the Models folder. I then created a prefab for the left controller by simply mirroring it (setting the X-axis scale to -1).

After completing this step, you should have a hierarchy structure similar to this:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/79b000cb-dcd7-4d41-bc24-3770ccba727e)

This scene (which I set as the default scene in the repo) can be used as a base to build a VR environment similar to those used in the project. It is also important to set the spatial gravity by setting the Y-axis to 0 in 'Edit > Project Settings > Physics > Gravity'.

### Managing Layers and Tags
To better manage the functionality of scripts and objects in the scene, I created some layers and tags:
- Layers make some objects invisible to the player or visible only to certain cameras: the UI Camera and the Post camera, overlays that respectively display the 'VFX' and 'UI' layers, allowing these elements to be managed separately from the main camera. The 'Hidden' layer is for objects that must be completely invisible to the player, such as spawners.
- Tags simplify script management, particularly I created the tags 'AtomoO', 'AtomoH', 'Molecola', and 'Destroyer'.

## Script Configuration
### Collega Atomi
This script is responsible for creating links between atoms. Assign the script to only one linkable atom to avoid forming double bonds.
In the inspector, insert the following objects:
  - **atomoCollegabile**: prefab of the atom that can be linked with the current atom.
  - **link**: prefab of the link between atoms.
  - **molecolaPrefab**: prefab of the molecule to be created.
  - **maxCollegamenti**: maximum number of atoms that can be linked before creating the molecule.

When a collision occurs between two linkable atoms with corresponding tags, a FixedJoint is created to link them. Then, a link object between the atoms is created to represent the connection. When the maximum number of links is reached, the molecule prefab is instantiated, destroying the linked atoms and the current atom.

### Atom Destroyer
This script manages the destruction of atoms that are not used or go beyond the map boundaries. It is assigned to atom prefabs so that it can be configured to manage the maximum lifetime of unused atoms. A reference to this script is inserted in each minigame to allow customized management of the "timeLimit" based on the specific needs of the game.
  - **timeLimit**: maximum lifetime of an unused atom. If an atom remains at its spawn position for a period longer than this limit, it will be destroyed.
Another case in which the atom is destroyed is if it comes into contact with a collider tagged "Destroyer".

### Atom Spawner
This script manages atom spawning in the first minigame. It should be assigned to a different spawner object for each atom type. Coordinates are taken from a CSV file, whose rows are shuffled. These coordinates are then multiplied by a scale factor so that the environment dimensions can be adjusted as desired. The script periodically spawns an atom based on the provided coordinates as long as there are still rows to read and the timer has not expired.
To use the script, configure these objects in the inspector:
  - **objectPrefab**: Atom prefab to spawn.
  - **lifeTime**: Maximum lifetime of an unused atom
  - **coordinatesFile**: CSV file with coordinates
  - **spawnInterval**: Time interval between spawns
  - **scale**: Scale factor
  - **parentObject**: Parent for atoms to keep the scene hierarchy organized.
  - **timer**: Timer reference

### Atom Random
This script handles the random spawning of atoms in the second game. It should be assigned to a spawner object with a collider. Atoms will then be generated at random positions within the collider. The spawn frequency is controlled by an interval, and spawning only occurs if the timer is still active.
The objects to configure in the inspector are:
  - **atomPrefabO** and **atomPrefabH**: corresponding prefabs for oxygen and hydrogen atoms
  - **lifeTime**: Maximum lifetime of an unused atom
  - **spawnInterval**: Time interval between atom spawns
  - **timer**: Timer reference to stop spawning when it reaches the end

### Dimensione Atomo
This script should be assigned to the prefab of each atom. It uses the Van Der Waals radius as input to determine the size, scaling it to 1/10 of the Unity base unit.

### Molecola
This script handles events associated with the appearance of a molecule on screen. In particular, it triggers a particle effect and a sound clip. It also manages the situation where a molecule touches a "Destroyer" object to prevent unused molecules from lingering in the scene. Unlike atoms, a molecule is not deleted due to inactivity.
In the inspector, configure:
  - **AudioSource** with the chosen audio clip
  - **ParticleSystem** representing the success indicator to the user. In my case, I used the following parameters:
![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/08876b0b-4a7a-4240-af57-19ec65a355dd)

### Grano
This script should be assigned to the dust grain and manages its behavior in both games. Upon colliding with a correct molecule, it increases the score, increases the material's alpha of the grain, and replaces the molecule with the corresponding Van Der Waals model. The ResetGrano function is called (in the second game) by MovGrain when the difficulty is changed.
In the inspector, configure:
  - **granoMaterial**: Material of the dust grain
  - **score**: Score reference
  - **tranparency**: Initial transparency of the grain (which otherwise would not be reset)
  - **prefabVanDerWaals**: Prefab of the Van Der Waals molecule

### Mov Grano
This script should be assigned to the grain and manages its movements in the second game, adjusting behavior based on the difficulty chosen by the player from the dropdown menu in the pause menu. Whenever the value changes, the ImpostaDifficoltà function is called to update the current option. The function corresponding to the chosen difficulty is called every frame to maintain movement consistent with the selected option.
In the inspector, configure:
  - **velocitaFacile**: Movement speed of the grain when the difficulty is set to "Easy"
  - **velocitaDifficile**: Movement speed of the grain when the difficulty is set to "Hard"
  - **offsetY**: Y-axis offset for sinusoidal grain movement in "Hard" mode
  - **offsetZ**: Same as above, but for the Z-axis
  - **posIniziale**: Initial position of the grain
  - **posFinale**: Final position of the grain
  - **timer**: Timer reference
  - **score**: Score reference
  - **dropdownDifficolta**: Dropdown reference in the pause menu

The OnValueChanged section of the dropdown is then configured as follows, calling the corresponding function within this script:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/f736706a-2413-48a2-bfa5-888ce1356e9a)

### Level Manager
Manages the transition between different game scenes. Provides methods to change scenes, restart the level, and exit the game. In my case, I assigned it to an empty GameObject. The methods of this script will then be linked to the onClick methods of the main menu and the pause menu, in this example the script is called when a button is pressed to switch to the scene with id 2:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/df4889da-939a-429e-a556-f3b6f968274e)


### Pause Menu
Manages the pause menu in the game. When the player presses the pause button on the controller (via InputActionReference), the TogglePauseMenu function is called. This function checks whether the game is currently paused or not, and based on that, activates or deactivates the pause menu panel. Additionally, it adjusts the game time, activates or deactivates the Direct and Ray Interactors to allow interaction with the pause menu. The pause button needs to be configured in the Input Action Asset by adding a button to the action as shown in the image. Note: avoid using the Menu button, as for headsets like the Meta Quest, it is used to return to the headset menu, ignoring in-game input. In my case, I used the "Primary Button" of both controllers.

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/0c860b9a-2344-4d6e-90e6-be91d2a543db)

In the inspector, configure:

- **pauseButton**: Pause button reference on the controller
- **Pannello**: Reference to the pause menu panel
- **directInteractor**: Reference to the Direct Interactor
- **rayInteractor**: Reference to the Ray Interactor

### Score
This script should be assigned to a TextMeshPro object, which will display the score. It is updated every frame based on the player's actions. The grain will update the score every time it detects the connection of a correct molecule. In the inspector, configure:

- **score**: Player's score, updatable by SetScore
- **timer**: Reference to the timer to update the score only when the timer is active

### Time Display
This script should also be assigned to a TextMeshPro object, which will display the remaining time, ignoring the decimal part. The update occurs every frame by decrementing the time until 0, when the text "Fine"(End) is displayed. The "ResetTimer" function is called to reset the timer when the player changes the difficulty. In the inspector, configure:

- **timeRemaining**: Current time
- **timeUp**: Boolean indicating if the time is up

### Shooting Star
Should be assigned to an empty object to handle the creation and movement of particles representing a shooting star. The star falls in a random direction with parameters such as spawn interval, speed, lifetime, and distance from the spawner. In the inspector, configure:

- **particleSystemPrefab**: Prefab of the particle system representing the shooting star
- **spawnInterval**: Time interval between one star and the next
- **destroyTime**: Lifetime of each shooting star
- **spawnDistance**: Maximum distance to generate a star from the spawner's position
- **speed**: Movement speed of the shooting stars

### Stars Manager
Manages a particle system representing stars in the space environment. It ensures that the particle system follows the main camera (creating the illusion that the stars are in space) and dynamically updates the color and brightness of the stars. In the inspector, configure:

- **brightness**: Desired brightness of the stars
- **Star Parameters**:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/b4eec15c-dac2-46e7-be23-5214d65c98bd)


### Project Export
#### Export for Meta Quest
To export the project for a Meta device, select 'Android' in build settings, change the texture compression to ASTC, and set Max Texture Size to 2048. In the project settings, in the Player section, set the Minimum API level to Android 6.0 or any later version, as it is the minimum API supported by Quest devices. Also, select 'Automatic' as the Install location. Then, set the following entries as shown in the image:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/88d168e0-b4ab-455d-ad82-48f55d5d7868)

In the 'Quality' section, create a new level to set as default on Android, with the parameters in the image.

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/043b6a8c-601d-40f5-936e-afa05028a93f)


In the 'Time' section, enter the value 1/72 (0.01388889) as the Fixed Timestep, which is the rate at which the Meta Quest processes time.

In the 'XR Plugin Management' section, select 'Android' and check 'Oculus'. In the Oculus section, select the following entries:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/4f785d31-7a6b-437b-b6e0-64208577d0f2)


Save these settings, and you can proceed with building the project, which in this case will produce an .apk file that you can install on the Meta Quest.

#### Export for HTC Vive / Any headset supporting SteamVR
Starting from the previous settings, in build settings select 'Windows' as the platform and remove the texture limit. In the 'XR Plugin Management' settings, select OpenXR and then insert the desired devices in the 'Interaction Profiles' section. In my case, it was sufficient to insert the profile for HTC Vive. Once done, you can proceed with building the project, which in this case will be a folder with various files, where the one to start the game is the .exe file.

For more in-depth information, refer to: [YouTube Video Link](https://youtu.be/pNYY1JsS7tY?si=62n99udqdYlcChZC)


# :it: Astrochimica in realtà virtuale con Unity

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

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/8b032d4b-79f6-42aa-934d-fed02f0ceed7)


### Creazione di una scena
Per creare una scena funzionante in realtà virtuale sono necessari alcuni elementi fondamentali:
- Origine: per crearla è sufficiente creare un asset di tipo 'XR Origin (VR)' dal menu 'XR'
  - Camera e controller: creati in automatico con l'XR Origin, la loro configurazione verrà spiegata dopo
  - Sistema di locomozione: creare un asset di tipo 'Locomotion Systerm (Action based)' dal menu 'XR'
- Gestori di input e interazioni: tre gameobject in cui va importato lo script omonimo dal package VR importato prima
  - Input Action Manager: una volta importato lo script, inserire un Action Asset. L'asset 'XRI Default Input Actions (Input Action Asset)' disponibile nella repo è un estensione di quello fornito di default da Unity nella sample scene VR, nella cartella 'VRTemplateAssets'.
  - XR Interaction Manager: in questo caso va semplicemente importato lo script omonimo, senza configurare altro
  - Event System: oltre allo script 'Event System' inserire lo script 'XR UI Input Module', con i seguenti parametri. Le UI Action inserite sono prese direttamente dall'Input Action inserito prima. ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/c106d861-2a9b-44c7-9ee1-8a319dfcb4dd)
 
### Sistema di locomozione
Una volta creato l'asset, potete aggiungere gli elementi per il movimento e la rotazione della visuale dell'utente. Creare come oggetti figli del locomotion system due oggetti 'Move' e 'Turn'. Nel primo andrà inserito uno script per la gestione del movimento. In questo caso ho utilizzato 'DynamicMoveProvider', fornito da Unity nella scena di esempio per il VR, nella cartella 'VRTemplateAssets'. Andremo poi ad assegnare alle move action le azioni corrispondenti nell'Input Action di entrambi i controller. Stessa cosa andrà fatta per l'oggetto 'Turn', dove inseriremo gli script 'Snap Turn Provider' e 'Continuous Turn Provider'.

### Gestione dei controller
Se i controller inseriti da unity sono di tipo 'Device Based', cancellarli ed aggiungere dei controller 'Action Based' e configurarli aggiungendo gli interactor a voi necessari (che devono sempre essere action based). Nel mio caso sono serviti il [Direct Interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-direct-interactor.html) e il [Ray interactor](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-ray-interactor.html). Questi interactor vanno poi inseriti nel controller all'interno dello script "Action Based Controller Manager", presente nella cartella Scripts nella repo. Anche questo script è fornito da Unity nel package 'VRTemplateAssets' di cui ho parlato prima. Gli interactor vanno poi inseriti anche nello script 'XR Interaction Group', che va aggiunto manualmente. Una volta fatto questo passaggio, vanno configurate le varie action negli script 'Action Based Controller Manager' e 'XR Controller' (Action-Based), aggiungendo i comandi dall'Input Action importato prima. Alla fine, dovreste avere una configurazione simile per entrambi i controller: ![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/1ee88aa0-1d4a-46da-8c0a-122a271e0e4d)

Nel mio caso ho aumentato il raggio dello Sphere collider del direct interactor, aumentando di fatto il range con cui possono essere toccati gli oggetti. Potete sperimentare con questo valore in base alle vostre necessità.

Nella sezione 'Models' potete aggiungere un modello ai controller, nel mio caso ho inserito il modello di default sempre fornito da Unity nel package di default, che trovate nella cartella Models. Ho poi creato un prefab per il controller sinistro semplicemente specchiandolo (scale dell'asse X  a -1).

Completato questo passaggio, dovreste avere nella gerarchia una struttura simile

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/79b000cb-dcd7-4d41-bc24-3770ccba727e)

Questa scena (che ho inserito come scena di default nella repo) può essere utilizzata come base da cui partire per costruire un ambiente VR simile a quelli utilizzati nel progetto.
Importante è anche impostare la gravità spaziale settando a 0 l'asse Y in 'Edit > Project Settings > Physics > Gravity'.

 ### Gestione di Layer e tag
 Per gestire al meglio il funzionamento degli script e degli oggetti sulla scena ho creato alcuni layer e tag:
   - I layer servono a rendere alcuni oggetti invisibili al giocatore o visibili solo ad alcune camere: la UI Camera e la Post camera, overlay che visualizzano rispettivamente i layer 'VFX' e 'UI', in modo da poter gestire queste cose separatamente dalla camera principale. Il layer 'Nascosto' invece è per gli oggetti che devono essere completamente invisibili al giocatore, come gli spawner.
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
Questo script gestisce la distruzione degli atomi che non vengono utilizzati o escono dai limiti della mappa. È assegnato ai prefab degli atomi in modo che possa essere configurato per gestire il tempo di vita massimo di quelli inutilizzati.
Un riferimento a questo script viene inserito in ogni minigioco per consentire la gestione personalizzata del "timeLimit" in base alle esigenze specifiche del gioco.
  - **timeLimit** è il tempo massimo di vita di un atomo inutilizzato. Se un atomo resta alla sua posizione di spawn per un periodo di tempo superiore a questo limite, verrà distrutto.
L'altro caso in cui l'atomo viene distrutto è se entra in contatto con un collider con il tag "Destroyer"

### Atom Spawner
Questo script gestisce lo spawn degli atomi nel primo minigioco. Deve essere assegnato a un oggetto spawner diverso per ogni tipo di atomo. Le coordinate vengono prese da un file CSV, le cui righe sono rimescolate. Tali coordinate vengono moltiplicate per un fattore di scala in modo da poter regolare a piacimento le dimensioni dell'ambiente. Lo script spawna periodicamente un atomo basato sulle coordinate fornite finchè ci sono ancora righe da leggere e il timer non è scaduto.
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

Parametri delle stelle:

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/b23d42c1-d9c6-4eb9-881a-72d7b9b592db)

## Esportare il progetto
### Export per Meta Quest
Per esportare il progetto per un dispositivo Meta è necessario selezionare 'Android' in build settings, cambiare la compressione delle texture in ASTC e Max Texture Size a 2048.
Nelle impostazioni del progetto, alla sezione Player è necessario impostare come **Minimum API level** Android 6.0 o una qualsiasi versione successiva, visto che è l'API minima supportata dai dispositivi Quest. Inoltre selezionare 'Automatic' come **Install location**.
Successivamente, impostare le voci successive come nell'immagine

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/657a0848-ed86-4b62-9fbf-14d775d6940a)

Nella sezione 'Quality' creare un nuovo livello da impostare come default su Android, con i parametri nell'immagine.

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/09461de4-4b32-4bb3-90a1-51104530c2b0)

Nella sezione 'Time' inserire come Fixed Timestep il valore 1/72 (0.01388889), che è il rateo a cui il Meta Quest processa il tempo.

Nella sezione 'XR Plugin Management' selezionare 'Android' e spuntare la voce 'Oculus'. Nella sezione Oculus selezionare le seguenti voci

![image](https://github.com/samuiosa/Astrochimica-VR/assets/57435078/a6a900ab-52b6-432f-ab0a-22910c2e7b1b)

Salvate queste impostazioni, potete procedere con la build del progetto, che in questo caso produrrà un file .apk che potete installare sul Meta Quest.

### Export per HTC Vive / Qualsiasi headset che supporta SteamVR
Partendo dalle impostazioni precedenti, in build settings selezionare 'Windows' come piattaforma e rimuovere il limite alle texture.
Nelle impostazioni 'XR Plugin Management', selezionare **OpenXR** per poi inserire i device desiderati nella sezione 'Interaction Profiles', quindi nel mio caso è stato sufficiente inserire il profilo per HTC Vive.
Una volta fatto, potete procedere con la build del progetto, che in questo caso sarà una cartella con vari file, in cui quello per avviare il gioco è l'exe.

Maggiori approfondimenti a riguardo: https://youtu.be/pNYY1JsS7tY?si=62n99udqdYlcChZC 
