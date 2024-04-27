# CineQuebec

![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?logo=mongodb&logoColor=white)
![.Net](https://img.shields.io/badge/C%23%20%2B%20WPF-5C2D91?logo=.net&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?logo=windows&logoColor=white)
[![.NET Core Desktop](https://github.com/heartbeatcountry/tp3-conception/actions/workflows/dotnet.yml/badge.svg)](https://github.com/heartbeatcountry/tp3-conception/actions/workflows/dotnet.yml)

<br>
Projet de Pascal & Cynthia.

<br>

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

<br>

## Exportation de la Bd

Vous trouverez dans le dossier [exportation-bd-mongo](./xportation-bd-mongo) une
exportation de la base de données MongoDb.

> Cette exportation n'est pas nécessaire et le projet fonctionnera même sans
> données dans la Bd.

<br>

## Instructions pour le premier lancement

Avant le premier lancement, **veuillez faire une copie du fichier**
`CineQuebec.Windows/appsettings.json` nommée `appsettings.local.json` et
placez-là au même endroit: `CineQuebec.Windows/appsettings.local.json`.

**Modifiez le fichier** pour y ajouter vos informations de connexion à la base
MongoDB.