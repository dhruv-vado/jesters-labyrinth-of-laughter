# Enemy Behavior 

## All the scripts and their uses are mentioned below

- States folder contains scripts for what enemy is supposed to do in a particular state
- Enemy.cs handles the enemy's states and calls necessary functions
- EnemyDetection.cs handles how enemy detects the player (this was the most time consuming part)
- EnemySounds.cs plays random sounds when called by Enemy.cs
- EnemyStatesBase.cs is the parent class of all the states 
- EnemyStatesFactory.cs contains all the states