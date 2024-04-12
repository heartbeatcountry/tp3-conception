# CineQuebec

Projet de Pascal & Cynthia.

## **Attention!** Sous-module git

Ce projet contient un sous-module git! Pour cloner le projet avec le
sous-module, utilisez la commande suivante:

```sh
git clone --recurse-submodules https://github.com/heartbeatcountry/tp2-conception
```

Si le projet a été clôné sans le sous-module, vous pouvez initialiser le
sous-module avec la commande suivante:

```sh
git submodule update --init --recursive
```

## Exportation de la Bd

Vous trouverez dans le dossier [exportation-bd-mongo](./xportation-bd-mongo) une
exportation de la base de données MongoDb.

> Cette exportation n'est pas nécessaire et le projet fonctionnera même sans
> données dans la Bd.

## Instructions pour le premier lancement

Avant le premier lancement, **veuillez faire une copie du fichier**
`CineQuebec.Windows/appsettings.json` nommée `appsettings.local.json` et
placez-là au même endroit: `CineQuebec.Windows/appsettings.local.json`.

**Modifiez le fichier** pour y ajouter vos informations de connexion à la base
MongoDB.
