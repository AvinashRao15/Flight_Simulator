<h1 align="left">My Flight Simulator</h1>

###

<h4 align="left">Controls</h4>

###

<p align="left">*If a controller is connected, keyboard input will not be registered*<br><br>Keyboard<br><br>W - Throttle Up<br>S - Throttle Down<br>Q - Rudder Left<br>E - Rudder Right<br><br>Down Arrow - Pull Up<br>Up Arrow -Pull Down<br>Left Arrow - Roll Left<br>Right Arrow - Roll Right<br><br>Left or Right Click and Move Mouse - Move camera<br>Middle Mouse - Reset The Camera<br>Scroll Up - Zoom In<br>Scroll Down - Zoom Out<br><br>Controller<br><br>Right Trigger - Throttle<br>Left Trigger - Brake<br>Left Bumper - Rudder Left<br>Right Bumper - Rudder Right<br><br>Left Stick -Roll, and Pitch<br>Right Stick - Camera Rotate<br>Right Stick In - Reset Camera<br><br>North - Zoom out<br>West - Zoom in<br>East - Respawn</p>

###

<h4 align="left">World Generation</h4>

###

<p align="left">This project started when I began using Procedural Generation (a method of creating data algorithmically as opposed to manually) to create landmasses. My first attempt at this was simply two nested for loops that went through a grid and either made the next point one point higher or one point lower. This solution worked fine but I wanted something with some more variation.<br>That lead me to learn about Perlin Noise, (a type of gradient noise with a naturally ordered (“smooth”) sequence of pseudo-random numbers) which laid the foundation for the project.<br><br>First I generated the noise and then used techniques to alter it such as octaves, persistence, and lacunarity.<br><br>After that, I generated the mesh and UVs for the noise. <br><br>Challenge #1: a mesh in unity can only have up to 60k vertices so I could only make each mesh 240 x 240.<br><br>After that, I created the illusion of an infinite world by implementing a level of detail and a chunk system. Each chunk was made up of a 240 x 240 grid they were generated around the viewer's position. The chunks that were farther away were generated with fewer vertices based on the LOD values. I also implemented threading to generate these chunks.<br><br>Challenge #2:   Because Unity only allows you to work on meshes and textures from inside the main thread, I had to create a queue of callbacks that contained all the actions that needed to be done on the main thread and every time something from a different thread on the main thread I went through the queue and executed the callbacks.<br><br>Next, I fixed the seems between chunks and adjusted the normals of the borders of chunks using cross products.<br><br>Next, I added collisions to meshes with the mesh collider component.<br><br>Challenge #3 After implementing this component I was getting massive lag spikes, so using Unity's profiler tool I figured out that Generating so many mesh colliders every time a new chunk was generated was creating massive lag spikes. My solution to this problem was to implement the level of detail system to the mesh colliders.<br><br>After that, I brought the world alive by adding colors and textures to mesh which changed based on the height at a particular point. I created a custom material using a custom-made shader that took in different inputs relating to the different layers of the world at each height like water, sand, grass, and so on. Then the shader changed the albedo of the material based on that and applied the material on the mesh along all three axes to make sure the texture looked real.</p>

###

<h4 align="left">After all, that was done, it was time to work on the actual flight simulator.</h4>

###

<p align="left">First I created a flight controller script which I attached to the land generator's viewer object and then added the rigid body component and enabled gravity.<br><br>There are 4 main forces acting on a plane, and I had already added 1 of them so all that was remaining 3: thrust, lift, and drag.<br><br>Thrust was easy, I just had to add a force pushing the plane in the direction the nose was facing based on the maximum thrust value and the throttle input of the player, the rest on the other hand would not be so easy.<br><br>Challenge #4: While there are formulas for the remaining forces, they all had air density as a factor, but adding particle physics into the game would be way overkill, so it took a while to hand-tune the controller's values to get a realistic flight experience.<br><br>The lift force was calculated using variables such as the plane's speed and its angle of attack. Then added to the induced drag of the plane.<br><br>Lastly, drag is calculated by getting the plane's velocity in each direction as well as how the plane is the orientation of the plane. Drag is also calculated with different drag coefficients based on which way the plane is facing and the drag coefficient is higher if the plane is not facing the way that it is moving.<br><br>Finally, the plane is controlled using a vector 3 which contains all the player's input values relating to how the plane should be oriented. Then a force is added to the plane in a direction and magnitude based on the x, y, and z values of the vector.<br><br>Finishing touches include adding controller support with the new input manager, a HUD, a respawn system, and much more.</p>

###
