# UrNarrativer - NodeGraph Narrative System for Unity
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/30903560f37d4fa8991300bab7236e89)](https://www.codacy.com/gh/DanielGDS/UrNarrativer/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=DanielGDS/UrNarrativer&amp;utm_campaign=Badge_Grade)
- This is a node-based - visual narrative flow creation tool that uses Unity's GraphView API.
- Is this a much improved and rewritten version of my [Node Dialogue System for Unity](https://github.com/DanielGDS/NodeDialogueSystem)

![alt_text](https://github.com/DanielGDS/UrNarrativer-Node-Graph-Narrative-System/blob/main/sample1.png?raw=true)

## Features
- Infinite Braching and Merging dialogue capability.
- Support multi-languages for text.
- Dialogue&Graph save/load system.
- Search window to node creation.
- Backed by Unity's embedded GraphView api.
- Support switch background image from Dialogue Node.
- Support two side (Left/Right) in current time talking character image.
- Support a characters name and Dialogue ID.
- Support for unique ID for Dialogue search.
- Infinity multiple line dialogue text support.
- Narrtive files Save to CSV Table and Load from CSV.

## Additional
- The included script includes full support for all the available features of this Node Narrative System.
- And as a bonus to everything else, a built-in effect for writing text for your visual novels or any texts.
- And like the cherry on the cake, it also includes skipping the letter-by-letter effect when the mouse clicks on the text.

## Language
- Supports adding any language in the world to your game and files automatically.

## Usage
- Graph generates narrative saves text, character's name and link background images and link to character images into Resources folder as a scriptable objects.

## EndNode:
#### Supports: 
- End --- Disables the dialog interface when this option is selected. 
- Repeat ---  Repeats the text over and over again with this selection. 
- Go Back --- Returns to one text back with this selection. 
- Return To Start --- Returns to the starting position of this narrative file with this selection. 

## NodeLinks
- Node Links is a serialized class that holds node connection and branching data.

## NodeData
- Nodes Datas is holding Narrative, Script, Event, Start, End node's position data for graph.

## ScriptNode 
- Developing In Process
#### Includes logic:
- If; 
- Else; 
- = ; 
- != ; 
- bool = (false, true)

#### !Currently does not work, is saved, but does not have the logic and saving of operators and Boolean parameters, is under development!

##### The working concept described is demonstrated in the image below:
![alt_text](https://github.com/DanielGDS/UrNarrativer-Node-Graph-Narrative-System/blob/8207ebdb21515fdfcc0109c9cda0fc79ce9a8548/sample2.png?raw=true)

#### 📫 If you have any questions, please contact me by email, and also help us further develop this repository with your suggestions.
