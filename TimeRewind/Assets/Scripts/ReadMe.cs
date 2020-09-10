/*
The Timeline script is used for AI path following. The Timeline object
takes a list of transforms and chooses to follow one based on whether the time state
in the game manager is forwards or backwards.

Using the time controls is optional, though the Game Manager has options for setting the time state.
The UI provided allows for time to be paused, reversed, or played forward. If you don't want to use the
time controls, make sure the time state is set to forward.

To give the AI a path to follow, add empty gameObjects to the list via the inspector in the order 
that the path should be followed.
*/
