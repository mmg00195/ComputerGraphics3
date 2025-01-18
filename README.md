# Computer Graphics Application Documentation
#### Members
- Manuel Molina García
- Celia María Marquez Gracia
- Vasco Maria Braga Costa
## Project Overview
This project is a 3D graphics application built using OpenTK, a .NET binding for OpenGL. The application allows users to interact with a 3D environment, rendering various geometric shapes and furniture objects, while providing camera controls and user interactions.

## Objectives
- To create a 3D rendering engine using OpenGL.
- To implement a camera system for navigating the 3D space.
- To manage and render various 3D objects with textures and lighting.
- To provide user interaction through keyboard and mouse inputs.
- To demonstrate the use of shaders for rendering graphics.

## Key Components

### 1. Shaders
- **File**: `Shader.cs`
- **Description**: This class handles the creation, compilation, and management of vertex and fragment shaders.
- **Key Functions**:
  - `Shader(string vertPath, string fragPath)`: Constructor that loads and compiles shaders from specified file paths.
  - `Use()`: Activates the shader program for rendering.
  - `SetInt(string name, int data)`: Sets an integer uniform variable in the shader.
  - `SetFloat(string name, float data)`: Sets a float uniform variable in the shader.
  - `SetMatrix4(string name, Matrix4 data)`: Sets a 4x4 matrix uniform variable in the shader.
  - `SetVector3(string name, Vector3 data)`: Sets a 3D vector uniform variable in the shader.

### 2. Textures
- **File**: `Texture.cs`
- **Description**: This class simplifies the loading and management of textures for 3D objects.
- **Key Functions**:
  - `LoadFromFile(string path)`: Loads a texture from a file and sets its parameters.
  - `Use(TextureUnit unit)`: Binds the texture to a specified texture unit for rendering.

### 3. Camera
- **File**: `Camera.cs`
- **Description**: Implements a camera system that allows for movement and rotation in 3D space.
- **Key Functions**:
  - `GetViewMatrix()`: Returns the view matrix based on the camera's position and orientation.
  - `GetProjectionMatrix()`: Returns the projection matrix for perspective rendering.
  - `MoveTo(Vector3 newPosition)`: Modify the camera position.`
  - Properties for `Position`, `Pitch`, `Yaw`, and `Fov` to control camera settings.

### 4. Rendering Objects
- **File**: `Axes.cs`
  - **Description**: Renders the coordinate axes in the 3D space.
  - **Key Functions**:
    - `Render(Camera camera)`: Renders the axes using the provided camera.
    - `ToggleVisibility()`: Toggles the visibility of the axes.

- **File**: `furniture.cs`
  - **Description**: Base class for furniture objects, providing rendering and texture management.
  - **Key Functions**:
    - `Render(Camera camera, Room room)`: Renders the furniture object.
    - `ToggleTexture()`: Toggles the current texture of the furniture.

- **File**: `table.cs`
  - **Description**: Inherits from `furniture` and defines a table object with specific vertices and indices for rendering.
  - **Key Functions**: Inherits functions from `furniture`.

- **File**: `Object.cs`
  - **Description**: Class for rendering various geometric shapes (cubes, pyramids, spheres) based on a polygon type.
  - **Key Functions**:
    - `GenerateSphere(int sectorCount, int stackCount)`: Generates vertex data for a sphere.
    - `Render(Camera camera, Room room)`: Renders the object using the provided camera and room context.
    - `ToggleColor()`, `ToggleVisibility()`, `ToggleWireframeMode()`, `ToggleGravity()`: Methods to toggle various properties of the object.
    - `Update(float deltaTime)`: Updates the object's position based on gravity and checks for collisions with the room boundaries.

- **File**: `Room.cs`
  - **Description**: Represents the room environment, including the floor and walls, and manages lighting.
  - **Key Functions**:
    - `Render(Camera camera)`: Renders the room and its lighting, including the lamp.
    - `ConfigureLighting(Shader shader, Camera camera, int tamLights)`: Configures lighting properties for the room.

### 5. Randomization
- **File**: `Randomizer.cs`
- **Description**: Provides methods for generating random colors and 3D points.
- **Key Functions**:
  - `RandomColor()`: Generates a random color.
  - `Random3DPoint()`: Generates a random 3D point within specified bounds.

### 6. Main Application
- **File**: `Program.cs`
- **Description**: The entry point of the application, setting up the window and running the main loop.
- 
- **File**: `Window.cs`
- **Description**: Inherits from `GameWindow` and handles user input, camera movement, object rendering and updating the scene.
- **Key Functions**:
  - `OnLoad()`: Initializes shaders, textures, and the camera. Sets up the room and axes.
  - `OnResize(ResizeEventArgs e)`: Updates the viewport and aspect ratio when the window is resized.
  - `OnUpdateFrame(FrameEventArgs e)`: Handles user input for camera movement, object manipulation, and updates the scene.
  - `OnRenderFrame(FrameEventArgs e)`: Clears the screen and renders the room, axes, and objects.
  - `DisplayHelp()`: Displays a help menu in the console with controls and instructions.

## User Interaction
- **Camera Controls**:
  - Move the camera using W, A, S, D keys.
  - Adjust the view with the mouse.
  - Use Shift for downward movement and Space for upward movement.
  - Adjusts the camera's field of view (FOV) based on the mouse wheel input, allowing users to zoom in and out of the scene.
  - The FOV is decreased when the mouse wheel is scrolled up and increased when scrolled down, providing a dynamic zoom effect.

- **Object Controls**:
  - Add objects to the scene using the number keys (1, 2, 3).
  - Toggle object visibility with V.
  - Change object colors with C.
  - Toggle object surface between solid or lines with X.
  - Apply gravity effects with G and global gravity effects with arrow keys.
  - Use Left Alt to cycle through selected objects.

- **Help Menu**:
  - Press H to display the help menu in the console.

## Rendering Pipeline
1. Clear the screen.
2. Set up shaders and textures.
3. Render the room and its lighting.
4. Render the axes.
5. Render all objects in the scene.

## Additional Features
 The project includes surprise features, one for each one of the members, that can be triggered by specific key presses, enhancing user engagement and interaction.

## Conclusion
This 3D graphics application serves as a demonstration of using OpenGL with .NET to create interactive 3D environments. It showcases various techniques such as shader programming, texture management, and user input handling, providing a solid foundation for further development in graphics programming. 