# Unity-Platformer-Demo
> Inspired by Pandemonium // Built to learn the fundamentals of game development

![Status](https://img.shields.io/badge/Status-In%20Progress-yellow) ![Engine](https://img.shields.io/badge/Engine-Unity-black) ![Language](https://img.shields.io/badge/Language-C%23-blue) ![Input](https://img.shields.io/badge/Input-New%20Input%20System-green)

---

## About

A 2D platformer developed in Unity as a hands-on introduction to game programming with C#. Inspired by the classic platformer *Pandemonium*, this project explores core game mechanics including player movement, combat, projectiles, and camera behaviour.

> ⚠️ **Project is not yet complete.** Scripts and features are actively being developed.

---

## Scripts

### `PlayerMovement.cs`
Handles all player movement including walking, jumping, wall sliding and wall jumping.

- Reads **A/D keyboard input** and applies horizontal velocity via `Rigidbody2D`
- **Sprite flipping** — mirrors the player sprite on the X axis based on movement direction
- **Ground detection** — uses a downward `BoxCast` against the ground layer to check if the player is standing on a surface
- **Wall detection** — uses a sideways `BoxCast` in the direction the player faces to detect wall contact
- **Wall sliding** — disables gravity and zeroes velocity when the player touches a wall mid-air, creating a brief "stick" effect
- **Wall jumping** — supports two wall jump types:
  - No directional input: pops the player straight off the wall horizontally
  - Directional input held: smaller horizontal push combined with an upward arc
- **Post-wall-jump suppression** — a 0.2 second cooldown timer prevents the player from immediately re-grabbing the wall after jumping off
- Syncs **Animator parameters**: `Running` (bool), `Grounded` (bool), `Jump` (trigger)
- Exposes `CanAttack()` — returns true only when the player is grounded, idle, and not on a wall

---

### `PlayerAttack.cs`
Handles the player's fireball attack, cooldown management and projectile pooling.

- Listens for **left mouse button** input using `Mouse.current.leftButton.wasPressedThisFrame` (New Input System) — fires once per click, not every frame the button is held
- Enforces a **minimum attack cooldown** (default 0.5s) between consecutive attacks
- Calls `CanAttack()` from `PlayerMovement` — attack only fires when the player is grounded, still and not on a wall
- Uses an **object pool** (`GameObject[] fireballs`) — reuses pre-created fireball GameObjects by toggling `SetActive` rather than instantiating and destroying each time
- `FindFireball()` — scans the pool for the first inactive fireball to reuse; falls back to index `0` if all fireballs are currently active
- Positions the fireball at the **FirePoint** transform before launching
- Passes the player's facing direction (`+1` or `-1`) to `Projectile.SetDirection()` so the fireball travels the correct way
- Triggers the **Attack animation** via `animator.SetTrigger("Attack")`

---

### `Projectile.cs`
Controls fireball behaviour from launch through to impact and cleanup.

- Moves the fireball along the **X axis** every frame using `transform.Translate`, scaled by `speed`, `Time.deltaTime` and direction
- **Lifetime limit** — automatically deactivates the fireball after 5 seconds if it hasn't hit anything, preventing it from travelling forever off-screen
- `OnTriggerEnter2D` — detects collision with any object:
  - Sets `hit = true` to stop movement
  - Disables the `BoxCollider2D` to prevent further collision detection
  - Triggers the **Explode animation**
- `SetDirection(float _direction)` — called by `PlayerAttack` when firing:
  - Stores the travel direction (`+1` right, `-1` left)
  - Reactivates the GameObject and resets hit state and collider
  - Flips the fireball sprite to match the travel direction if needed
  - Resets the lifetime counter
- `Deactivate()` — called by an **Animation Event** at the end of the Explode animation to turn the fireball GameObject off, returning it to the pool

---

### `CameraMovementRoom.cs`
Moves the camera smoothly between rooms when the player transitions to a new area.

- Uses `Vector3.SmoothDamp` to gradually move the camera toward a target X position, avoiding instant snapping
- `MoveToNewRoom(Transform _newRoom)` — called externally when the player enters a new room, updating the target X position the camera moves toward
- Camera only moves on the **X axis** — Y and Z remain fixed

---

### `CameraMovementFollowPlayer.cs`
Makes the camera directly follow the player's horizontal position in real time.

- Each frame, sets the camera's X position to match the **tracked player's X position** exactly
- Y and Z axes remain fixed — no vertical or depth following
- `MoveToNewRoom(Transform _newRoom)` — stores a new room target position for potential use alongside room-based transitions

---

## Controls

| Action | Input |
|---|---|
| Move Left | `A` |
| Move Right | `D` |
| Jump | `Space` |
| Wall Jump | `Space` (while on wall) |
| Attack | `Left Mouse Button` |

---

## Inspector Setup

| Script | Fields to assign in Inspector |
|---|---|
| `PlayerMovement` | `speed`, `jumpPower`, wall jump forces, `groundLayer`, `wallLayer` |
| `PlayerAttack` | `attackCooldown`, `firePoint` Transform, `fireballs` array |
| `Projectile` | `speed` |
| `CameraMovementRoom` | `speed` |
| `CameraMovementFollowPlayer` | `speed`, `trackingPlayer` Transform |

---

## Layer Setup

| Layer | Used By |
|---|---|
| `Ground` | `PlayerMovement` — ground detection BoxCast |
| `Wall` | `PlayerMovement` — wall detection BoxCast |

---

## Project Status

| Feature | Status |
|---|---|
| Player Movement | ✅ Complete |
| Wall Jump | ✅ Complete |
| Fireball Attack | ✅ Complete |
| Camera Follow | ✅ Complete |
| Room Camera | ✅ Complete |
| Enemy AI | ⏳ Not started |
| Health System | ✅ Complete |
| Collectibles | ⏳ Not started |
| Level Design | ✅ Complete |

---

## Built With

- [Unity](https://unity.com/)
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Unity New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html)
