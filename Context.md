_(draft)_

**Contexte**
*   2 axes vont être abordés pour exposer les choix :
    *   au niveau architecture globale
    *   au niveau des design patterns et des moyens à utiliser

**Terminologie**
*   Distinguo entre Architecture et Design patterns ou méthodes de développement.
*   Utilisation du mot stratégie pour Architecture et tactique pour Design patterns ou méthodes.

**Principe global**
*   La stratégie (~Architecture) :
    *   découper l’application par domaines étanches et indépendants.
    *   Découpage par Api (esprit « microservices ») et par vue dédiée (esprit « microfrontend »).
    *   Le mot esprit ne veut pas dire « on suit exactement » ou « on fait comme ».
*   La tactique (~Méthode, Design patterns) :
    *   Définir un découpage : vertical slice par couche.
    *   Appliquer des méthodes d’aide au développement
        *   Générateur de solution qualité clé-en-main
        *   Générateur d’API à partir d’un contrat OpenApi (option non traitée ici)

**Liste des besoins avec en face l’argumentaire et la solution technique**
*   « Avoir une solution moins lourde pour le développement »
    *   Cas concret qui nous a obligé à prendre en compte ce besoin.
    *   Possible solution :
*   « Appliquer les bonnes pratiques de développement »
    *   Solution :
*   « Appliquer les bonnes pratiques .Net, Blazor, Asp.Net Core »
    *   Solution :
*   « Eviter que toute évolution d’un domaine ne casse l’application »
    *   Solution :
*   « Eviter que toute évolution sur une couche du domaine casse la rétrocompatibilité »
    *   Solution :
*   « Pouvoir livrer une évolution ou une correction de manière modulaire »
    *   Solution :
*   « Augmenter la testabilité et la validation de l’application »
    *   Solution :
*   « Automatiser les tests »
    *   Solution :
*   « Introduire la notion d’observabilité et de supervision pour maitriser son application »
    *   Solution :
- "Internationalisation"
  - Culture dont langue + date UTC + regional settings.  
*   …

- Organisation du travail :
  - Mob Programming et Pair programming ont des limites
  - Il faut pouvoir laisser les devs travailler seuls avec du contrôle.
  - Tout en sachant qu'ils ne pourront pas s'éloigner de l'objectif initial ou à défaut, toute erreur restera encaspulée au maximum à leur domaine.

**Gains**
*   Gains pédagogiques :
    *   rails,
    *   meilleurs approches de notre métier à date pour tout le monde,
    *   picorer dans les éléments générés.
*   Gains organisationnelles :
    *   découpage d’équipe : 1 dev UI, 1 dev back ou 1 dev fonctionnalité read-only type débutant 1 dev fonctionnalité avec intégrité plus expert.
    *   montée en compétence des juniors : socle propre pour s’en extraire ensuite s’il le souhaite.
*   Gains en Multiversioning / étanchéité :
    *   Solutions découplées et pouvant évoluer indépendamment sans frein à l’innovation
    *   Changer le moteur SGBD ou le type de moteur d’API ou la version de .Net, on peut le faire par baby step.
*   Gains en Sécurité :
    *   des accès sécurisés et dédiés,
    *   sanitisation by design des entrées (découpage par couche),
    *   patterns de typage.
*   Gains en Montée en charge / coûts en ressources
    *   Ciblage et monitoring de l’utilisation des services pour augmenter les ressources de manière économique et non globale.
    *   Ajout de quota et de limites spécifiques.
    *   Mécanisme de cache ciblé.
*   Amélioration sur l’environnement de développement :
    *   Limités et indépendants,
    *   Environnement « light »,
    *   Validation des couches de manière indépendantes avant livraison : ce qu’on livre est ciblé et testé.
*   Gains en Revue de code et de compréhension de qui fait quoi :
    *   une PR ciblera exclusivement une et une seule responsabilité (va éviter le phénomène des 100 modifications parallèles pour une PR réparties sur plusieurs modules.)

**Inconvénients**
*   A ce jour, on a 4 groupes d’entités indépendantes pour toute une application : VO, DTO, BO, MongoEntity. Il faut faire des mappings à chaque fois (d’où le Getting Started du générateur).
*   Importance de réfléchir la structure des entités en fonction des contextes :
    *   VO va être en face d’un composant graphique, il peut agréger plusieurs données d’entités BDD différentes sous formes de propriétés dans un simple objet.
    *   DTO doit limiter les données exclusivement à ce que le client souhaite avoir.
    *   BO représente le métier et la vision pérenne du métier.
    *   MongoEntity : réprésentation spécifique et optimisée serialization Mongo.
*   Illusion qu’à date il y a des recouvrements entre entités alors que par expérience ces entités se doivent d’évoluer indépendamment.
