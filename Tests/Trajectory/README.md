# Vérification de la trajectoire

`TrajectorySmokeChecks.cs` exécute 15 tests avec le moteur Physics2D de Unity,
sans package de tests supplémentaire. Il reste hors de `Assets` afin de ne pas
ajouter de code de test au jeu.

## Exécution isolée

1. Créer un projet temporaire avec la même version Unity que FlightQuest.
2. Activer le module intégré `com.unity.modules.physics2d` dans ce projet.
3. Copier `Assets/Scripts/Plane/PlaneTrajectory.cs` dans ses `Assets` et
   `TrajectorySmokeChecks.cs` dans ses `Assets/Editor`.
4. Lancer Unity avec `-batchmode -nographics -projectPath <projet-temporaire>
   -executeMethod TrajectorySmokeChecks.Run -logFile <journal>`.

Résultat attendu : `TRAJECTORY_CHECKS_COMPLETE failures=0` et sortie avec code 0.
Ne pas exécuter cette méthode dans l'éditeur de travail : elle quitte l'éditeur.

## Couverture et limites

Les tests comparent le vol libre à une simulation réelle, puis vérifient la
distance cumulée maximale, les collisions fines, les formes polygonales, les
atterrissages, le décollage avec contact initial, les surfaces temporairement
en trigger, les objets ignorés et le retournement de l'avion.

La distance affichée se règle via `Max Trajectory Distance` sur `PlaneActions`
(6 unités sur le prefab PlaneGame). La durée de prévision reste aussi plafonnée
par `Trajectory Duration`. Aucun danger situé après ces limites n'est annoncé.

La prévision s'arrête au premier contact physique ou à une zone qui modifie le
vol (boost, téléportation, arrivée, etc.). Elle ne prédit pas l'activation future
d'un pouvoir par le joueur ni le déplacement futur des obstacles. Les tests
isolés ne remplacent pas une vérification visuelle des niveaux dans le jeu.
