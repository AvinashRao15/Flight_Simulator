<h1 align="left">Flight Simulator</h1>

###

<a href="https://youtu.be/HByUPdhhCwE">Video Demonstration</a>

<p align="left">World Generation:<br>
<ul>  
  <li>Started out recursively modifying a grid to create landmasses.</li>
  <li>Switched to Perlin Noise, a gradient noise algorithm, for generating more varied and natural-looking terrain.</li>
  <li>Added octaves, persistence, and lacunarity to adjust and fine-tune the Perlin Noise output.</li>
  <li>Generated a mesh representation of the terrain using the modified noise data.</li>
  <li>Calculated UVs to make the mesh visible.</li>
  <li>Curvumvented Unity's limit on the number of vertices in a mesh by splitting the mesh up into smaller sections.</li>
  <li>Implemented a level of detail (LOD) system to optimize performance by reducing the vertex count for distant chunks.</li>
  <li>Utilized a chunk system to generate terrain sections (chunks) dynamically around the viewer's position.</li>
  <li>Addressed threading restrictions by using a queue of callbacks to perform actions from auxiliary threads on the main thread.</li>
</ul>
<br>Flight Simulator:<br><br>
<ul>
  <li>Created a flight controller script attached to the viewer object in the land generator.</li>
  <li>Implemented forces acting on the plane, including thrust, lift, and drag.</li>
  <li>Handled the challenge of not having air density in the lift and drag formulas by manually fine-tuning animation curves for a realistic flight experience.</li>
</ul>

<br>Visual Enhancements and Additional Features:<br>
<ul>
  <li>Added colors and textures to the mesh to improve visual appeal.</li>
  <li>Created a custom material using a custom-made shader to apply different textures based on terrain height.</li>
  <li>Enhanced gameplay experience by incorporating controller support using Unity's input manager.</li>
  <li>Implemented a heads-up display (HUD) to provide essential information to the player during gameplay.</li>
  <li>Developed a respawn system for player convenience in case of crashes.</li>
  <li>Introduced other finishing touches and refinements.</li>
</ul>
</p>

###
