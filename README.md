![Gemini_Generated_Image_jrynscjrynscjryn](https://github.com/user-attachments/assets/7e74ea13-d60f-49a9-829d-e3c807e12391)
# 📚 Tactical-Terrain

Welcome to Tactical-Terrain, a 3D tactics game developed as an assignment project. In this game, you'll experience the basics of grid-based movement and simple AI behavior within a tactical environment. Navigate your player across tiles while being pursued by an enemy in a strategic dance of positioning!

## 🎮 Game Overview
Tactical-Terrain is a single-player game where the objective is to move your player across a grid while avoiding the enemy that follows you. The challenge lies in continuous movement and evasion. All of the assets used in game are made by me.

## 🖥️ Main Features
- **Player Movement**: Move your player across the grid.
- **Enemy AI**: An enemy that follows your movements.
- **Grid System**: A dynamic grid where the gameplay takes place.
- **Pathfinding**: Both player and enemy use pathfinding algorithms for movement.

## 📋 How It Works
1. **Start the Game**: Load the game and begin your tactical evasion.
2. **Move the Player**: Use the mouse to select tiles and move your player.
3. **Enemy Follows**: The enemy will track and follow your movements.
4. **Keep Moving**: Stay ahead and strategize your movements to avoid the enemy.

## 📄 Code & Functionality

### GridManager

This script handles the creation and management of the grid system.

* **Grid Generation**: Creates a grid of tiles for the game area.
* **Tile Management**: Keeps track of tile states (e.g., occupied, empty).

### TileSelect

This script allows the player to select tiles and move their character.

* **Selection Input**: Detects player input for tile selection.
* **Movement Handling**: Moves the player character to the selected tile.

### PathFinder

This script manages the pathfinding logic for both player and enemy movements.

* **Path Calculation**: Uses A* algorithm to find the shortest path between tiles.
* **Path Execution**: Moves the characters along the calculated path.

### PlayerController

This script handles the player's movement and interactions.

* **Movement Control**: Manages player movement across the grid.
* **Interaction Handling**: Manages interactions with game elements.

### EnemyController

This script manages the enemy's movement and behavior.

* **Movement Execution**: Controls enemy movement based on pathfinding results.
* **Behavior Management**: Defines how the enemy behaves in different situations.

### EnemyPathFinder

This script calculates the path for the enemy to follow the player.

* **Path Calculation**: Finds the optimal path for the enemy to move towards the player.
* **Path Updating**: Updates the path when the player moves to a new tile.

## 🖼️ Screenshots & Video

![Screenshot 2024-07-14 005844](https://github.com/user-attachments/assets/6feafff2-318a-49b2-864d-c2ca9b5de9bd)
<p align="center">
  <strong>Grid (Terrain)</strong>
</p>

![Screenshot 2024-07-14 011304](https://github.com/user-attachments/assets/29861dfd-c330-4bb7-82d0-bf5b565e8d36)
<p align="center">
  <strong>Assets</strong>
</p>

![Screenshot 2024-07-14 011345](https://github.com/user-attachments/assets/e38d53a7-afee-4b7f-85a0-161d3f310bbf)
<p align="center">
  <strong>Grid Generation with categorization of Tiles</strong>
</p>

https://github.com/user-attachments/assets/cf01d123-cda8-42d1-8b7d-c06196d3462d
<p align="center">
  <strong>Game Video</strong>
</p>

https://github.com/user-attachments/assets/2f9004d6-340a-4473-89ce-512736ed042a
<p align="center">
  <strong>Game Video</strong>
</p>

## 💻 Development Details
- **3D Models**: Created using Blender.
- **Code**: Written in JetBrains Rider.
- **Platform**: Windows only.
- **Status**: Initial build, may contain bugs.
- **Unity Version**: Requires Unity version 2023.2.7f1 or newer.

## 🚀 Installation
1. **Download**: Clone or download the repository.
2. **Open in Unity**: Open the project in Unity version 2023.2.7f1 or newer.
3. **Play**: Run the game from the Unity editor or the built executable.

## 📝 Contact
For any inquiries, suggestions, or feedback, reach out to me at pranavdabhi360@gmail.com.

Thank you for exploring Tactical-Terrain! Your feedback is appreciated. Enjoy the game and sharpen your tactical skills!

Made with ❤️ by Pranav Dabhi
