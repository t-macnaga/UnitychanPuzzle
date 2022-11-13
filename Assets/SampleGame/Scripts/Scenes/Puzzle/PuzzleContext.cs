using System;
[Serializable]
public class PuzzleContext
{
    public PuzzleDirector Director { get; set; }
    public PuzzleStateMachine StateMachine { get; set; }
    public PuzzleBoardView View { get; set; }
    public PuzzleBoardModel Model { get; set; }
    public PuzzleLogic Logic { get; set; }
    public PuzzleCommands Commands { get; set; }
}