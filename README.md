# Unity Dialogue Editor (Visual Scripting)

Branching dialogue for Unity authored as [Unity Visual Scripting](https://docs.unity3d.com/Packages/com.unity.visualscripting@latest) graphs, with a small runtime layer that plays those graphs through a UI Toolkit dialogue box.

## About

This is an experiment in building a dialogue system on top of Unity Visual Scripting. It started after an attempt to use Unity's Behavior graph for dialogue: branching conversations were awkward to express, and the Behavior package's codebase was hard to extend for the pieces that were missing. Visual Scripting turned out to be a better fit — custom nodes (units) plug straight into the existing Script Graph editor, fuzzy finder, and coroutine flow.

Instead of a bespoke editor window, a dialogue is a normal `ScriptGraphAsset`: you drop in `Dialogue Start`, `Actor Speech`, `Player Choice`, and `Dialogue End` nodes, wire the control flow, and connect graph variables or other nodes into line text and choice conditions. A `DialogueAdapter` / `DialogueController` / `DialogueView` trio runs the graph at play time and renders it.

**Status: early work in progress.** The core loop works end to end (see the included sample), but many things are still stubbed or missing — see [Limitations](#limitations).

## Features

- **Dialogue as Visual Scripting graphs** — no custom file format; author in the standard Script Graph window.
- **Custom dialogue nodes**:
  - `Dialogue Start` — entry point, fires when the dialogue's `ScriptMachine` is enabled.
  - `Actor Speech` — a list of lines with an optional gate `Condition`; each line has its own value input, so a line can be overridden by a graph variable or another node's output.
  - `Player Choice` — a list of options, each with a `Condition` and a `ChoiceType` (`Normal` / `Exit` / `Fight` / `Quest`), plus a per-option control output and a `Default` output for when nothing is available.
  - `Dialogue End` — ends the dialogue.
- **Node port previews** — custom unit descriptors label each line/choice port with a truncated preview of its text, and show `<SKIPPED>` for empty entries.
- **Custom inspector** for choice options (type + multi-line text).
- **Runtime playback** — typewriter text effect, skip-to-full-line, continue button vs. auto-advance, and choice-list population, all driven through events.
- **UI Toolkit view** — dialogue box built at runtime, styled from a `StyleSheet`, with keyboard input (space to skip/continue, number keys to pick an option).
- **Multiple named dialogue graphs** per adapter (a "main" runner plus a name-keyed dictionary of others).
- **Sample scene** with an NPC that starts a dialogue on an Input System action, and a `TestDialogue` graph that exercises conditions, loops, graph variables, and a multi-option choice.

## Tech Stack

- **C#**, Unity **6000.1.5f1** (Unity 6.1)
- **Unity Visual Scripting** (`com.unity.visualscripting`)
- **UI Toolkit** (UIElements) for the dialogue view
- **Unity Input System** (`com.unity.inputsystem`)
- **Universal Render Pipeline** (2D) — used by the sample scene only
- A small vendored utility namespace, `Hassa.Essentials` (serializable dictionary, keyed collections, an `EnsureThat` guard-clause helper, a few extension methods)

## Repository Layout

```
VSDialogueEditor/                     Unity project (open this folder in Unity)
  Assets/
    Editor/Hassa.VSDialogue/          Unit descriptors + inspectors (Editor-only)
    Runtime/Hassa.VSDialogue/
      Units/                          Dialogue nodes (DialogueStart, ActorSpeech, PlayerChoice, DialogueEnd)
      Components/                     DialogueAdapter, DialogueController, DialogueView
      UI/                             Dialogue.uss / Dialogue.uxml
    Runtime/Hassa.Essential/          Small general-purpose helpers
    Runtime/Resources/TestDialogue.asset   Sample dialogue graph
    Runtime/Scripts/Npc.cs            Sample trigger
    Scenes/SampleScene.unity          Sample scene
```

There are no assembly definitions; everything compiles into the default `Assembly-CSharp` / `Assembly-CSharp-Editor`.

## Getting Started

Requires Unity **6000.1.5f1** (or a close 6.1 release). The project uses Visual Scripting, Input System, and URP packages.

1. Clone the repo and open the **`VSDialogueEditor`** folder in Unity Hub. Let the first import finish.
2. Open **Edit → Project Settings → Visual Scripting** and click **Regenerate Nodes** so the custom `Dialogue` units show up in the fuzzy finder.
3. Open `Assets/Scenes/SampleScene.unity` and press **Play**.
4. Trigger the NPC's dialogue with the **Interact** action (see `Assets/InputSystem_Actions.inputactions`), advance lines with **Space**, and pick options by clicking or pressing a number key.

### Using it in your own project

1. Copy `Assets/Editor/Hassa.VSDialogue`, `Assets/Runtime/Hassa.VSDialogue`, and `Assets/Runtime/Hassa.Essential` into your project.
2. Install Unity Visual Scripting and regenerate nodes.
3. On a GameObject, add a `ScriptMachine` (the dialogue graph), plus `DialogueAdapter`, `DialogueController`, and `DialogueView`, and wire their serialized references (the view needs a `UIDocument` and the `Dialogue.uss` style sheet).
4. Author a graph that begins with `Dialogue Start` and ends with `Dialogue End`.
5. Call `DialogueAdapter.StartDialogue()` (or `StartDialogue("name")` for a named runner) to begin.

## Node Reference

| Node | Kind | Inputs | Outputs | Notes |
|------|------|--------|---------|-------|
| **Dialogue Start** | Event | — | trigger | Coroutine event unit, hooked to `OnEnable` of the dialogue's `ScriptMachine`. Put one per graph. |
| **Actor Speech** | Coroutine | `Enter`, `Condition` (bool, default `true`), one value input per line (`line_1…`, nullable) | `After` | Header field `Actor` (GameObject). If `Condition` is false the whole node is skipped. Each line is shown via the adapter, then it waits for the view to report the line complete before moving on. An `AudioClips` list exists but is not used yet. |
| **Player Choice** | Coroutine | `Enter`, one `condition_N` (bool, default `true`) per option | `Default`, one `option_N` control output per option | Field `Options`: list of `{ Text, ChoiceType }` where `ChoiceType` is `Normal` / `Exit` / `Fight` / `Quest`. Options with empty text are ignored; options whose condition is false are shown disabled. Routes control to the chosen option, or to `Default` if nothing is selectable. |
| **Dialogue End** | Control | `Enter` | — | Calls `HandleDialogueEnd` on the controller and stops the runner. |

### Runtime components

- **`DialogueAdapter`** — the bridge. Holds the `DialogueController` and the dialogue runner(s): a main `ScriptMachine` plus a `SerializableDictionary<string, ScriptMachine>` of named ones. Disables all runners on `Awake`; `StartDialogue(name)` enables the selected one through the controller. Nodes reach it with `GetComponentInParent<DialogueAdapter>()`.
- **`DialogueController`** — playback orchestration: typewriter effect (`~0.05s`/char), skip / continue / auto-advance logic, choice-list population, and event wiring to the view.
- **`DialogueView`** — UI Toolkit view. Builds the dialogue box in code, applies a `StyleSheet`, handles keyboard input, and raises `OnSkipLineTyping` / `OnContinue` / `OnOptionSelect`.

### Example graph

`Assets/Runtime/Resources/TestDialogue.asset` is a working sample:

```
Dialogue Start
  └─ Sequence
       ├─ Actor Speech  "Line #1.1" / "#1.2" / (line_2 ← graph var testStr) / (line_4 ← string literal)
       ├─ Actor Speech  "Conditional line #2.x"   [Condition ← testVar < 2025]
       ├─ Actor Speech  "Conditional line #3.x"   [Condition ← testVar > 2025]   (skipped)
       ├─ Actor Speech  "Line #4.1"
       └─ Player Choice  7 options (Normal / Quest / Fight / Exit, some condition-gated)
            ├─ option_1 → Actor Speech "Reaction line #1.1" → Dialogue End
            ├─ …
            └─ Default  → Actor Speech "Reaction line Default" → Dialogue End
```

## Limitations

Early WIP — known gaps include actor name/portrait wiring (currently placeholder), audio-clip playback, localization/formatting, dialogue history, and building the view from UXML rather than code. These are planned but not done.

## License

[MIT](LICENSE) © 2026 Vojtěch Krupička
