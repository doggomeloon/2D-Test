using UnityEngine;

public static class GlobalVariables
{
    // PLAYER DATA
    public static double health = 5.0;
    public static double maxHealth = 5.0;
    public static bool focusLocked = false;


    // GAME DATA
    public static string currentScene = "MainMenu"; // MainMenu, Settings, Credits, SampleScene


    // KEYBINDS
    public static string currentlyEditing = "";
    public static string[] eligibleKeys = new string[] {
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    "LeftShift", "Space"
    };
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode rightKey = KeyCode.D;
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode sprintKey = KeyCode.LeftShift;
    public static KeyCode interactKey = KeyCode.F;

}