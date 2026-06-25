# ReverseAssembly

**ReverseAssembly** is a 3D Sci-Fi / Cyberpunk Action game developed as a diploma project. The primary focus of this project was to build a robust, scalable, and maintainable architecture from scratch, avoiding tightly coupled code and Monobehaviour spaghetti.

## 🛠 Technical Stack
* **Engine:** Unity 2022+ (C#)
* **Architecture:** SOLID, Dependency Injection (Reflex DI), Event-Driven Architecture, FSM.
* **Libraries:** DOTween, Newtonsoft JSON, QuickOutline.

## ⚙️ Architecture & Under the Hood

As a Gameplay/System Programmer, I focused on creating modular and decoupled systems:

* **Dependency Injection (Reflex DI):** Used to inject core systems (Input, Audio, SaveManager, UI WindowManager) across the project, getting rid of Singletons and strict dependencies.
* **Custom Finite State Machine (FSM):** 
  * **Player Brain:** Manages high-level states (Gameplay, Terminal, Pause, Death).
  * **Movement & Combat:** Separate FSMs handling sprint, jump, melee, and ranged abilities smoothly.
  * **Enemy AI:** Behavior states (Patrol, Chase, Attack, Glitch-Stun) adapting to player actions and distance.
* **Event Bus:** Custom lightweight EventBus for global communications (e.g., `BranchSwitchedEvent`, `PlayerDeathEvent`, `CodeBlockCollectedEvent`).
* **Repository Pattern (Save/Load System):** Abstracted data persistence (`IDataRepository`). Gameplay data (Player stats, positions, Enemy states, Puzzle states) is serialized into JSON using `Newtonsoft.Json` with timestamp-based save slots.
* **Scriptable Object Abilities:** Combat skills (Melee, Scanner, Ranged, AoE) are entirely data-driven via ScriptableObjects (`AbilityDefinition`), allowing easy creation of new skills without changing code.
* **UI Window Manager:** A stack-based UI routing system handling complex nested views (HUD, Terminals, Pause Menu) efficiently.

## 🎮 Gameplay Features
* **Reality Switching (Branching):** Player can seamlessly switch between dimensions (Main and Alpha) to solve puzzles and avoid enemies.
* **Terminal Puzzles:** Interactive physical and terminal-based mini-games.
* **Dynamic Combat:** Combination of melee strikes, projectiles, and an interactive scanner to reveal weak points.
* **Boss Fights:** Multi-phase boss logic orchestrated by a custom `BossDirector` and FSM.
