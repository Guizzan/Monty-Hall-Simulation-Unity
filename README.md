# Monty Hall Problem — Unity Simulation

An interactive 3D simulation of the **Monty Hall Problem** built in Unity. Play manually or run thousands of automated simulations to see the probability difference between switching and staying.

## What is the Monty Hall Problem?

You're shown 3 doors. Behind one is a car, behind the other two are goats. You pick a door. The host — who knows what's behind each door — opens one of the other doors to reveal a goat. Now you're asked: **do you switch to the remaining door, or stay with your original choice?**

Statistically, **switching wins ~66% of the time**. This simulation lets you verify that empirically.

## Features

- **Manual mode** — click doors yourself and play through each round step by step
- **Auto simulation** — run any number of simulations automatically with configurable:
  - Always switch / always stay toggle
  - Simulation count
  - Simulation speed (time scale slider)
- **Live statistics** — tracked across all runs:
  - Total simulations
  - Cars found / missed
  - Success rate (%)
- **Simulation log** — each round shows: first selected door, host-opened door, second selection, car position, result
- **Narrator** — the host comments on every action
- Supports **mouse** (Editor/Desktop) and **touch** (Android/iOS)

## How to run

1. Open the project in Unity (developed with Unity 2020.x)
2. Open `Assets/Scenes/` and load the main scene
3. Press Play

### Manual play
- Click a door to make your first selection
- The host opens a goat door
- Click again to either switch or stay

### Auto simulation
1. Set simulation count in the input field
2. Toggle **Auto Pick** on
3. Toggle **Change Decision** on or off (switch vs stay strategy)
4. Click **Start Simulation**
5. Use the speed slider to run simulations faster

## Project structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs    # Core simulation logic, UI updates, input handling
│   └── FovFix.cs         # Camera field-of-view utility
└── Scenes/               # Main simulation scene
```

## Expected results

| Strategy | Win rate |
|----------|----------|
| Always switch | ~66.7% |
| Always stay | ~33.3% |

Run at least 1000 simulations for the results to converge toward the theoretical values.
