# Block Puzzle Game

A simple 2D block puzzle game built using **Unity**.

---

## 🔧 Unity Version & Tools Used

- Unity Version: **2022.3.49f1 (LTS)**
- Language: **C#**
- 2D Project Template
- Tools Used:
  - Unity SpriteRenderer
  - TMPro (TextMeshPro for UI)
  - Unity UI System
  - AudioManager for sound effects
  - ScriptableObject for block data

---

## 🚀 How to Run the Project

1️⃣ **Clone or Download the Project**

- Clone this repository or download the ZIP and extract it.

2️⃣ **Open in Unity**

- Open Unity Hub.
- Click `Open` and select the project folder.

3️⃣ **Load the Main Scene**

- Go to `Assets/Scenes/GameScene.unity` and open it.

4️⃣ **Play**

- Press `Play` inside Unity Editor.

✅ The game should run immediately without extra configuration.

---

## ✅ Features Implemented

- Drag & Drop Block Placement
- Random Block Generation
- Line Clear System (rows & columns)
- Animated Block Clearing (shrink effect)
- Score System with:
  - Bonus multiplier for multiple lines cleared
  - High Score tracking via PlayerPrefs
- Game Over Handling:
  - Game Over panel with animated score counting
  - Filling remaining cells visually before game over
- Audio Feedback:
  - Block placement sound
  - Line clear sound
  - Game over sound

---

## 📂 Project Structure

- **/Scripts**
  - BlockSpawner.cs
  - DraggableBlock.cs
  - BoardPlacement.cs
  - ScoreSystem.cs
  - GameOverManager.cs
  - AudioManager.cs

- **/Prefabs**
  - Block prefab
  - Score popup prefab

- **/ScriptableObjects**
  - BlockDataSO (defines each block's shape and sprite)

- **/Scenes**
  - GameScene.unity

---

## 🔥 Future Improvements

- More visual & particle effects
- Theme variations
- Sound and music improvements

---

## ✅ Requirements

- Unity 2022.3.49f1 LTS or later  
- TextMeshPro package (included by default in Unity)

---

Enjoy and happy coding! 🎮
