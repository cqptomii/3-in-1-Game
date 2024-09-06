
## Description succinte

Ce projet a été réalisé dans le cadre de l'UE LP2B à l'Université de Belfort Montbeliard au cours du semestre de printemps 2024. Cette UE avait pour but de nous apprendre les bases de Unity 2D.

Le but de ce projet était donc de mettre en oeuvre les connaissances que nous avons apprises au cours de ce semestre en récreant et en ameliorant le jeu Mini UFO (Un Jeux dans le style du jeux connus R-Type). De plus ce projet voulais regrouper 3 mini-jeux dans un application
- Apple Catcher
- Brick Breaker
- Mini UFO Game

Il a donc fallut dans un premier temps recréer ces trois mini-jeux afin de pouvoir par la suite ajouter quelques méchaniques / design / animations permettant d'améliorer le gameplay de chaque jeu.
Il est important de noter que les jeux Apple Catcher et Brick Breaker ont été implementés pendant les séances de TP de cette UE, afin d'apprendre le fonctionnement d'Unity. 
Ainsi la partie la plus `importante` de ces deux jeux sont les `améliorations` porté à ces deux jeux.

Enfin la partie la plus `importante` de ce projet a été l'implementation du jeu `Mini-UFO` que nous devions créer entiérement avec les différentes assets fournis. En proposant et implémentant au minimum `quatres` améliorations de notre choix dans celui-ci.

## Menu 

![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/Menu.png)

Comme ce projet nécéssité de regrouper trois jeux différents, nous avons créer un menu afin dans naviguer facilement et efficacement entre les différents jeux.

  - Animations / Boutons
  - Sound Management
  - Video Management
  - Scenes Management
  
## Apple Catcher

![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/ScreenAC.png)

Apple Catcher est le premier jeu sur lequel nous avons travaillés. Il consiste simplement à récuperer le plus de pomme possible dans un intervalle de temps donné.
  - implémentation d'un personnage 2D deplacable.
  - Animations du personnage
  - Coroutines
  - Spawner d'objet / prefab
  - Bases moteur de physique

# Ameliorations:

Plusieurs améliorations ont été implémenter sur ce jeu, principalement sur la notion de difficulté de celui-ci.
- trois nouveaux type de pommes ont été introduites avec des effets particuliers

     ![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/Gold.png) - Pomme doré : Donne 5 points au lieu d'un seul 

   ![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/Rotten.png) - Pomme zombie : Fais perdre un point au joueur

    ![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/Bomb.png) - Bombe : Finis la partie si le joueur l'attrape
- Modulation du niveau de pomme instanciés en fonction du temps écoulé

## Brick Breaker

![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/ScreenBB.png)

Brick Breaker est le second jeu sur lequel nous avons travaillés, qui reproduit le jeu éponyme Brick Breaker.
  - instanciation aléatoire de préfab
  - Sound Management
  - Moteur de physique

  # Améliorations

Comme pour Apple catcher l'essentiels des améliorations ont été apportées sur la difficulté de celui-ci afin d'essayer de le rendre moins monotones.

- Ajout de bonus
  - Ball Bonus : permet de récuperer une vie
- Modulation du niveau de difficulté : Augmente la vitesse de la balle en fonction du temps écoulé
 
## Mini-UFO

![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/ScreenMU.png)

Mini UFO est le corps de notre projet, car il réprésente l'application de ce qu'on a pu apprendre durant ce semestre et plus encore. Ce jeu s'inspire du jeu R-Type et met en scene un personnage dans l'espace combattant des vaisseau ennemies.
Le but du jeu est essentiellement d'obtenir le plus de points possible, pouvant être obtenu en détruisant des vaisseaux ennemies.

La première tache de ce jeu a été la mise en place de l'aspect visuel du jeu. Cettetache a permis de rendre le jeux plus vivant, notamment en implementant des asteroids en mouvements en arrière plan. De plus l'ajout d'une ambiance sonore était nécéssaire.
![alt text]()

Par la suite l'implementation d'un joueur opérationel avec un aspect visuel correct et de belles animations a été primordial, ce qui m'a permis de découvrir le système de génération de particules de Unity et d'approfondir la production d'animations.
Cette tache a aussi nécéssité de gerer le mouvement de notre joueur dans l'espace de jeux et la gestion des projectiles de celui-ci. Mais aussi son niveau de vie à chaque instant du jeu.

![alt text]()

Enfin la mise en place d'ennemies était essentiel pour ce jeu. Cette tache était similaire à la mise en place de notre joueur, seulement le deplacement et l'envoie de projectiles de nos ennemies sont prédefinis en fonctions de la difficulté de ce jeu. De plus il était nécessaire de mettre en place des bonus obtenable lors de la mort des ennemies.
En plus de l'implémentation des ennemies, nous avons du mettre en place un système afin d'instancier ces objets dans notre scène, mais aussi d'en gérer le nombre / la vitesse d'apparition etc dans une `GameManager`.

![alt text]()

  - Particle System
  - Sound Management
  - Coroutines
  - Gestion du temps
  - Animations

# Améliorations

Ce jeu a été celui dans lequel nous avons ajouté le plus de nouvelle mécaniques, tant au niveau de la difficultés, mais aussi graphiquement et dans le gameplay.


- Mécanique

  - Mécaniques des ennemies afin de diversifiés les attaques
  - Gestion du temps lors de l'obtention d'un bonus : (chaque bonus à un effets pendant un certain temps
  - Bonus doublement de points
  - Bonus d'invicibilité
  - Menu de pause
    
- Graphique
  
  - Animations de la barre de vie
  - Effets visuels des différents bonus ( par exemple un bouclier apparait devant le joeur)
    ![alt text]()
  - Gestion des particules sur les Ennemies
    ![alt text]()
  - Animations et ajout de sons lors de la destruction des vaisseaux ennmies
  - Design du Menu de pause
    ![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/PauseScreen.png)
  - Wiki afin de connaitre les différentes spécificité du jeux (accesible en jeux)
    ![alt text](https://github.com/cqptomii/3-in-1-Game/blob/master/Pic/Wiki.png)
  - Différentes animations sur l'envoie de projectiles
    
- Difficulté

  - évolution du nombre d'ennemies maximum en même temps en fonction du temps
  - Fluctuation de l'intervalles entre deux instanciation de vaisseaux en fonction du temps écoulé

Enfin il est important de noté que de nombreuses améliorations peuvent encore être ajoutées dans ce jeu, notamment à travers l'ajout de Boss mais encore dans l'ajout de compétence au joueur.

  
