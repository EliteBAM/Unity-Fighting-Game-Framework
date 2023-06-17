using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

//Read in INI file
//Create Key/Value Pair HashSet of Key Names and KeyCodes based on INI information
//if Input.GetKeyDown(controls.GetValue("West"))
public enum GenericControls
{
    None,

    North,
    East,
    South,
    West,
    Forward,
    Backward,
    Jump,
    Duck,
    Block,
    Grab,

    //derivations of forward and backward
    Left,
    Right
}

public struct ControlSet
{
    //Generic Keybind Variables

    //Attack
    public KeyCode North;
    public KeyCode East;
    public KeyCode South;
    public KeyCode West;

    //Movement
    public KeyCode Right;
    public KeyCode Left;
    public KeyCode Jump;
    public KeyCode Duck;

    //Utility
    public KeyCode Block;
    public KeyCode Grab;

    //public List<KeyCode> controlsList;
}

public class ControlSettings : MonoBehaviour
{
    //Controller Keybinds
    public static ControlSet Keyboard_Player_1 = new ControlSet();
    public static ControlSet Keyboard_Player_2 = new ControlSet();
    //ControlSet consoleController = new ControlSet();

    //Config File Data
    string configFile;
    string fileDefaults =   "///								///\n" +
                            "///			Fighting Game Config			///\n" +
                            "///								///\n" +
                            "\n" +
                            "-- Lines that do not conform to file format will be ignored\n" +
                            "-- Misspelled key/value pairs will not be detected\n" +
                            "-- Missing key/value pairs will reset the file to default values\n" +
                            "\n" +
                            "[Keyboard_Player_1]\n" +
                            "\n" +
                            "\tNorth = I\n" +
                            "\tEast = H\n" +
                            "\tSouth = N\n" +
                            "\tWest = K\n" +
                            "\n" +
                            "\tRight = D\n" +
                            "\tLeft = A\n" +
                            "\tJump = W\n" +
                            "\n" +
                            "\tDuck = S\n" +
                            "\tBlock = Shift\n" +
                            "\tGrab = CapsLock\n" +
                            "\n" +
                            "[Keyboard_Player_2]\n" +
                            "\n" +
                            "\tNorth = I\n" +
                            "\tEast = H\n" +
                            "\tSouth = N\n" +
                            "\tWest = K\n" +
                            "\n" +
                            "\tRight = L\n" +
                            "\tLeft = J\n" +
                            "\tJump = I\n" +
                            "\tDuck = S\n" +
                            "\n" +
                            "\tBlock = Shift\n" +
                            "\tGrab = CapsLock\n";

    void Awake()
    {
        configFile = Application.persistentDataPath + "/PlayerConfig.ini";

        CheckConfig();

        Keyboard_Player_1 = InitializeControlSet(ConfigParser.GetSectionData(configFile, nameof(Keyboard_Player_1)));
        Keyboard_Player_2 = InitializeControlSet(ConfigParser.GetSectionData(configFile, nameof(Keyboard_Player_2)));

        Destroy(gameObject);
    }

    public void CheckConfig()
    {

        //Opens FileStream and Creates Specified file in file path location
        if(!File.Exists(configFile))
        {
            FileStream stream = new FileStream(configFile, FileMode.Create, FileAccess.ReadWrite);

            //write the defaults to file
            byte[] buffer = Encoding.ASCII.GetBytes(fileDefaults);
            stream.Write(buffer, 0, buffer.Length);

            //ensure all modifications are pushed to the file
            stream.Flush();
            stream.Close();

        }

    }

    public ControlSet InitializeControlSet(HashSet<KeyValuePair<string, string>> keybindings)
    {

        ControlSet set = new ControlSet();
        //set.controlsList = new List<KeyCode>();

        foreach (KeyValuePair<string, string> keybinding in keybindings)
        {
            KeyCode parsedValue;
            if (Enum.TryParse(keybinding.Value, out parsedValue))
            {
                switch (keybinding.Key)
                {
                    case nameof(GenericControls.North):
                        set.North = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.East):
                        set.East = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.South):
                        set.South = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.West):
                        set.West = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Right):
                        set.Right = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Left):
                        set.Left = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Jump):
                        set.Jump = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Duck):
                        set.Duck = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Block):
                        set.Block = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                    case nameof(GenericControls.Grab):
                        set.Grab = parsedValue;
                        //set.controlsList.Add(parsedValue);
                        break;
                }
            }
        }

        return set;
    }

}