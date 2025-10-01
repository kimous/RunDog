<div id="top">

<!-- HEADER STYLE: COMPACT -->


# RUNDOG
<em></em>
<img src="https://github.com/kimous/RunDog/blob/main/logo.png?raw=true" width="15%" align="left" style="margin-right: 15px">

<!-- BADGES -->
<img src="https://img.shields.io/github/license/kimous/RunDog?style=plastic&logo=opensourceinitiative&logoColor=white&color=E92063" alt="license">
<img src="https://img.shields.io/github/last-commit/kimous/RunDog?style=plastic&logo=git&logoColor=white&color=E92063" alt="last-commit">
<img src="https://img.shields.io/github/languages/top/kimous/RunDog?style=plastic&color=E92063" alt="repo-top-language">
<img src="https://img.shields.io/github/languages/count/kimous/RunDog?style=plastic&color=E92063" alt="repo-language-count">

<em>Construit avec les outils et langages suivants:</em>  
<img src="https://img.shields.io/badge/-.NET%208.0-blueviolet?logo=dotnet" alt="Dotnet8">

<br clear="left"/>

## 🌈 Sommaire

<details>
<summary>Sommaire</summary>

- [RUNDOG](#rundog)
  - [🌈 Sommaire](#-sommaire)
  - [🔴 Description](#-description)
  - [🟠 Features](#-features)
  - [🟡 Structure du projet](#-structure-du-projet)
  - [🔵 Commencer avec RunDog](#-commencer-avec-rundog)
    - [🟣 Prérequis](#-prérequis)
    - [⚫ Installation](#-installation)
    - [⚪ Utilisation](#-utilisation)
  - [🌟 Roadmap](#-roadmap)
  - [📜 License](#-license)

</details>

---

## 🔴 Description

Une application légère pour Windows qui affiche l'activité de votre système (CPU, RAM, Disques, Réseau) via une icône animée dans la barre des tâches, avec des statistiques détaillées.

L'application est inspirée d'une application existante nommée [RunCat](https://kyome.io/runcat/index.html) qui est disponible uniquement sur MacOs.

---

## 🟠 Features

RunDog à les features suivantes:

- Quand on survole l'icône de RunDog le pourcentage d'utilisation du CPU s'affiche
- Quand on clique-gauche sur l'icône de RunDog, un menu s'ouvre et affiches les différentes statistique en temps réél (CPU, RAM, Disques et Réseau)  
<img src="https://github.com/kimous/RunDog/blob/main/screenshots/ScreenShot_menu_stats.png?raw=true" alt="menu_statistiques">  
- Quand on clique-droit sur l'application, un menu s'ouvre et on peut :
  - Mettre en pause l'animation
  - Changer l'animal de l'animation
  - Quitter RunDog

---

## 🟡 Structure du projet

```sh
└── RunDog/
    ├── LICENSE
    ├── Program.cs
    ├── README.md
    ├── RunDog.csproj
    ├── RunDogApplicationContext.cs
    ├── Sparkline.cs
    ├── SpriteSheet.cs
    ├── StatsForm.cs
    ├── animaux
    │   ├── rundog_animation_cat.png
    │   └── rundog_animation_chien.png
    ├── icons
    │   ├── batterie_battery.png
    │   ├── connexion_network.png
    │   ├── cpu.png
    │   ├── disque_storage.png
    │   └── ram_memory.png
    ├── logo.ico
    └── logo.png
```

---

## 🔵 Commencer avec RunDog

### 🟣 Prérequis

Ce projet necessite les dépendances suivantes:

- **Langage:** CSharp/C#
- **Framework:** Donet/.net 8

### ⚫ Installation

Build RunDog à partir des sources:

1. **Cloner le repository:**

    ```sh
    ❯ git clone https://github.com/kimous/RunDog
    ```

2. **Se déplacer dans le dossier du projet:**

    ```sh
    ❯ cd RunDog
    ```


### ⚪ Utilisation

Lancer le projet avec :

**Depuis une invite de commande [Powershell]():**
```sh
dotnet run
```

---

## 🌟 Roadmap

- [ ] **`UX`**: Améliorer l'UX / l'esthétique
- [ ] **`Logos`**: Refaire des meilleurs logos (disque, cpu, ram, ...)
- [ ] **`Animations`**: Refaire des meilleurs animations
- [ ] **`Optimisation`**: Améliorer la réactivité au clique après quelques minutes sans y toucher.

---



## 📜 License

Rundog is protected under the [LICENSE](https://choosealicense.com/licenses) License. For more details, refer to the [LICENSE](https://choosealicense.com/licenses/) file.

---

<div align="right">

[![][back-to-top]](#top)

</div>


[back-to-top]: https://img.shields.io/badge/-BACK_TO_TOP-151515?style=flat-square

---
