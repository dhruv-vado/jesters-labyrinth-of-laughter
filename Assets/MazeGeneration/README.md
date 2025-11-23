# Maze Generation

## All the scripts and their uses are mentioned below

- MazeCell.cs is attached to every cell and is used to delete walls and keep track on them
- MazeExit.cs gets the walls of the exit cell which is my way to work around for making the exit door.
- MazeGenerator.cs is the main generating script which uses an algorithm by visiting randomly chosen adjacent unvisited cells and clearing the walls.