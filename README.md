# Unity-Fighting-Game-Framework
Work in Progress. 

Dynamic framework for developing fighting games in Unity. 

Features: 
- Base Character Controller that can be implemented across all characters.
  
- Dynamic Camera with smooth tracking and dynamic zoom.
  
- Keybind System that saves settings on disk in a player settings file.
  
- Combo Input System -> Generates tree data structure from Move List data to efficiently detect inputs and execute moves in real-time.
  
- Code-free Character Development System -> Developed a character design framework for fast iteration and fast content creation using Scriptable Objects to   instantiate new characters with unique animations and Move-Sets. This way, designers and easily implement new characters with unique art, animations, and ability stats and create and edit character Move-Sets with no coding required. I feel that this is an essential feature to truly be able to develop and iterate balanced content for a competitive game.

- Custom Animation System -> Created a custom animation system to avoid the shortcomings of Unity's Mechanim. This is a work in progress and is built on the Unity Playables API to manage the animations of the player characters. This allows me to have the level of control over animation states that is necessary for a competitive game.
  
- Code-Free Hit-Box Editor (WIP) -> Created using Unity's Editor Extension APIs. Allows designers to match hitbox data frame-by-frame to traditional imported character animation files and export them as a combined-format unity Animation that includes both bone key frames and Hit-Box key frames. This way, character moves can be fully implemented by designers without code and Hit-Box data can be easily iterated and balanced throughout the course of development.

TO-DO:
The next upcoming feature in the development pipeline is hit-interaction between players using the custom Hit-Boxes. This will be implemented once the Hit-Box Editor is completed.
