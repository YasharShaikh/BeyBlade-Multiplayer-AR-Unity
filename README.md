**BeyBlade Multiplayer AR 🔥**
=============================

A Unity-based mobile game that brings classic Beyblade battles into the real world using **Augmented Reality** and **Multiplayer Networking**.

Built using **Unity C#**, **AR Foundation (ARKit & ARCore)**, and **Photon PUN2**, this prototype demonstrates real-time AR battles between two players using network-synced Beyblade tops and realistic physics interactions.

---

**Project Highlights ✨**
------------------------

* **AR Arena Placement**
  - Detect flat surfaces like tables or floors using AR Foundation.
  - Spawn a virtual Beyblade arena directly onto real-world space.

* **Multiplayer Battles**
  - Two players join the same room via Photon PUN2.
  - Each player's Beyblade is synced in real-time: launch, spin, collide.

* **Physics-Based Gameplay**
  - Swipe or flick to launch your Beyblade.
  - Unity physics simulate spin, force, friction, and impacts.

* **Real-Time Sync**
  - Uses `PhotonNetwork.Instantiate()` and PhotonViews to sync objects across devices.
  - Consistent arena placement, rotation, and state on both devices.

---

**Objective 💡**
---------------
Build a cross-platform AR multiplayer experience to:
* Learn how to use **AR Foundation** to anchor 3D objects in the real world.
* Integrate **Photon PUN2** for matchmaking and real-time object synchronization.
* Simulate realistic Beyblade physics using **Rigidbody**, **Colliders**, and collision detection.
* Demonstrate full-stack multiplayer AR game development using industry-standard tools.

---

**Gameplay 🎮**
--------------
* Scan a flat surface to spawn the arena.
* Join or create a Photon room.
* Launch your Beyblade and watch the battle unfold in AR.
* Tops collide, spin, and bounce using Unity's physics.
* All interactions are mirrored across players in real-time.

---

**Getting Started 📃**
----------------------

* **Unity Version**: 2020.3 LTS or later
* **Requirements**:
  - AR Foundation
  - ARKit XR Plugin (iOS)
  - ARCore XR Plugin (Android)
  - Photon PUN2
  - ARKit/ARCore supported mobile device

---

**Setup Instructions ⚙️**
--------------------------

1. Install AR Foundation & plugins via *Unity Package Manager*.
2. Enable **XR Plug-in Management** and select ARKit or ARCore for your target platform.
3. Import **Photon PUN2** and add your **App ID** in `PhotonServerSettings`.
4. Open the project in Unity Hub and load the **MainScene**.
5. Connect to Photon, join/create a room, and place the arena in AR.
6. Build and deploy to a real mobile device for full functionality.

---

**Core Components 🛠️**
-----------------------

**Photon Networking**
* `Launcher.cs`: Connects to Photon and manages room flow.
* `PhotonNetwork.Instantiate()`: Spawns networked Beyblade prefabs.
* `PhotonView`: Ensures transforms and logic are synced.
* `OnJoinedRoom`, `OnPlayerLeft`: Manage multiplayer events.

**AR Integration**
* `ARPlaneManager`: Detects horizontal surfaces.
* `ARAnchorManager`: Anchors arena/Beyblade prefabs to planes.
* Keeps virtual content fixed to real-world positions.

**Game Logic**
* Beyblade prefabs include `Rigidbody`, `Collider`, and custom spin logic.
* Launch force is applied via touch or motion input.
* Collision events (via `OnCollisionEnter`) trigger effects or knockouts.

---

**Skills Demonstrated 💻**
--------------------------

* **Unity C# Scripting** – Touch input, physics, prefab control, and multiplayer.
* **AR Development** – Used AR Foundation to deploy to both iOS and Android with one codebase.
* **Multiplayer Sync** – Photon PUN2 for networked object control and scene management.
* **Physics & Effects** – Realistic movement, collision, and feedback using Unity’s physics engine.

---

**Preview 👀**
-------------
> Two players face off with phones aimed at the same table.  
> Each sees the same arena and spinning tops clashing in real time.  
> Battles play out with realistic motion, sound, and synced interaction.

*_(GIF or Screenshot Placeholder Here)_*

---

**Contributing 💪**
-------------------
This is a personal prototype project, but contributions are welcome!

* Found a bug? Open an **Issue**.
* Want to improve it? Fork and submit a **Pull Request**.
* All helpful feedback is appreciated — let’s build cool AR stuff together!

---

**Reach Out 💬**
----------------
* **Yashar Shaikh** – yashrsk6@gmail.com  
* [GitHub](https://github.com/YasharShaikh)  
* [LinkedIn](https://www.linkedin.com/in/yashar-shaikh/)  
