# V-Lab SDK for Unity

**V-Lab SDK** is a framework for component-driven development of interactive virtual world applications in the Unity editor based on the V-Lab approach. It allows you to set-up interactive scenes without writing code by adding components to the scenegraph and configuring them visually in the inspector.

V-Lab SDK relies on an event-driven architecture in which:

- **controls** catch various types of events, including user actions and environment-oriented events,
- controls modify **variables** as a response to events caught,
- **effectors** monitor variables and modify the world as a response to changes to the values of variables monitored.
- optionally, **triggers** may force effectors to run in response to external events, on a schedule, etc.

In general, controls do not affect the world, effectors do no respond to events directly and, with the exception of trigger logic, all interfacing among controls and effectors is via variables. This is to ensure maximum separation of responsibilities and maximum component disentanglement and, as a consequence, maximum manageability and scalability on the application-design level.

The framework operates like this in principle, while a variety of base and utility classes provide the means for the implementation of additional, customized functionality.

V-Lab SDK only focuses on interactivity within a scene and not application logic and behaviour. As such, it is not meant to replace other development tools such as behaviour trees, visual programming frameworks, etc. However, thanks to its event-driven nature and the fact that variables are first-class entities with application-wide accessibility, integration with such tools is straightforward when needed.

## Installation

Import the V-Lab SDK package by dragging-and-dropping the package file on the editor or via Assets > Import Package.

## Downgrading

It is possible to modify the SDK for use in Unity versions earlier than 2023.2. In order to do that, import TextMeshPro and edit TextMutator.cs to use the TMP namespace and the TMP_Text type instead of the respective UnityEngine.UI and Text.
