﻿/* Program name:            Haunted House (Work in title)
 * Project file name:       HauntedHouse
 * Author:                  Steve Parker, 
 * Date:                    17/10/2020
 * Language:                C#
 * Platform:                Microsoft Visual Studio 2019
 * Purpose:                 To work in a team environment by making a text based adventure game.
 * Description:             Explore a haunted house using text commands ...
 *
 * known bugs:              
 * Additional features:     
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HauntedHouse
{
    class Program
    {
        //devMode constant
        private const bool DEVMODE = false;

        //Constants
        private const string FILELOCATION = @"save.txt";
        private const int SCREENSAVECOUNT = 20;

        //Fields
        private static List<bool> conditions;    //keeps a track of all the work the player has done. //maybe have a list of list of bool       
        private static List<string> screenSave;  //saves what is currently on the screen
        private static string text;              //use this to show a message for the player


        //Main method
        static void Main(string[] args)
        {
            conditions = new List<bool>();
            screenSave = new List<string>();
            MainMenu(MethodBase.GetCurrentMethod());
        }

        //Main menu
        static public void MainMenu(MethodBase method)
        {
            string selection;
            bool exitLoop = false;
            List<string> title = new List<string>();

            do
            {
                //Main menu text
                Console.Clear();

                //main menu reload
                title.Clear();
                if ((method.ToString() == "Void Main(System.String[])") || (method.ToString() == "Void End()"))
                {
                    title.Add("New Game");
                }
                else
                {
                    title.Add("Resume");
                    title.Add("Save Game");
                }
                if (File.Exists(FILELOCATION))
                {
                    title.Add("Load Game");
                }
                title.Add("Exit");

                if (DEVMODE) //tells the location of the previous method
                {
                    Console.WriteLine(method.ToString());
                }

                Console.SetCursorPosition(0, 8);
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("Welcome to Haunted House".Length / 2)) + "}", "Welcome to Haunted House"));
                Console.SetCursorPosition(0, 10);
                for (int i = 0; i < title.Count; i++)
                {
                    Console.WriteLine("{0," + ((Console.WindowWidth / 2) + (title[i].Length / 2)) + "}", title[i]);
                }
                // selection to call out task or exit the program
                Console.SetCursorPosition(40, 20);
                Console.WriteLine("#");
                Console.SetCursorPosition(41, 20);
                selection = Console.ReadLine().ToLower();
                switch (selection)
                {
                    case "new game":
                        {
                            if (title.Contains("New Game"))
                            {
                                NewGame();
                            }
                        }
                        break;

                    case "save game":
                        {
                            if (title.Contains("Save Game"))
                            {
                                SaveGame(method.ToString());
                                Console.WriteLine("Press any key to continue");
                                Console.ReadLine();
                            }
                        }
                        break;

                    case "load game":
                        {
                            if (title.Contains("Load Game"))
                            {
                                LoadGame();
                            }
                        }
                        break;

                    case "exit":
                        {
                            exitLoop = true;
                        }
                        break;

                    case "resume":
                        {
                            if (title.Contains("Resume"))
                            {
                                exitLoop = true;
                            }
                        }
                        break;

                    default:
                        break;
                }
            } while (!exitLoop);
            if (selection == "resume")
            {
                Console.Clear();
                foreach (string line in screenSave)
                {
                    Console.WriteLine(line);
                }
                method.Invoke(method, null);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        //Save game method
        static public void SaveGame(string method)
        {
            //What needs to be saved?
            //method location, list of conditions, items, screensave text.
            StreamWriter sw = new StreamWriter(FILELOCATION);
            string methodName = method.Replace("Void", "");
            methodName = methodName.Replace("()", "");
            methodName = methodName.Replace(" ", "");
            sw.WriteLine(methodName);

            sw.WriteLine(conditions.Count);
            foreach (bool item in conditions)
            {
                sw.WriteLine(item);
            }

            //removes strings if above certain count and saves each line.
            if (screenSave.Count > SCREENSAVECOUNT)
            {
                screenSave.RemoveRange(0, screenSave.Count - SCREENSAVECOUNT);
            }
            sw.WriteLine(screenSave.Count);
            foreach (string item in screenSave)
            {
                sw.WriteLine(item);
            }
            sw.Close();
            Console.WriteLine("Game is saved!");
        }

        //Load Game method
        static public void LoadGame()
        {
            string methodName = "";
            screenSave.Clear();

            StreamReader sr = new StreamReader(FILELOCATION);

            //clears what ever is in the conditions list or creates it 
            if (conditions != null)
            {
                conditions.Clear();
            }

            while (!sr.EndOfStream)
            {
                methodName = sr.ReadLine();
                int count = Convert.ToInt16(sr.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    conditions.Add(Convert.ToBoolean(sr.ReadLine()));
                }
                
                count = Convert.ToInt16(sr.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    screenSave.Add(sr.ReadLine());
                }
            }
            sr.Close();
            Console.Clear();
            Type type = typeof(Program);
            MethodBase method = type.GetMethod(methodName);
            foreach (string line in screenSave)
            {
                Console.WriteLine(line);
            }
            method.Invoke(method, null);
        }

        //New game method
        static public void NewGame()
        {
            Console.Clear();
            LoadConditions();
            Room1();
        }

        //Load all the conditions for the game
        public static void LoadConditions()
        {
            conditions = new List<bool>();
            conditions.Add(false); //chestopen
            conditions.Add(false); //picked up key
            conditions.Add(true); //door is locked
        }

        //shows the player the message and saves it
        public static void ShowMessage()
        {
            Console.WriteLine(text);
            screenSave.Add(text);
        }
        //First room of the game
        static public void Room1()
        {
            bool leave = false;
           
            text = "It's a room with a chest. There's a door to the left";
            if (screenSave.Count > 0)
            {
                if (text != screenSave[screenSave.Count - 1])
                {
                    ShowMessage();
                }
            }
            else
            {
                ShowMessage();
            }

            do
            {
                text = Console.ReadLine().ToLower();
                screenSave.Add(text);
                switch (text)
                {
                    case "open chest":
                        {
                            if (conditions[0])
                            {
                                text = "The chest is already open.";
                                ShowMessage();
                            }
                            else
                            {
                                text = "You open the chest. There is a key inside";
                                ShowMessage();
                                conditions[0] = true;
                            }
                        }
                        break;

                    case "take key":
                        {
                            if ((!conditions[1])&&(conditions[0]))
                            {
                                text = "you take the key";
                                ShowMessage();
                                conditions[1] = true;
                            }
                            else
                            {
                                if (conditions[1])
                                {
                                    text = "You have already picked up the key.";
                                    ShowMessage();
                                }
                            }
                        }
                        break;

                    case "use key on door":
                        {
                            if ((conditions[1]) && (conditions[2]))
                            {
                                text = "You unlock the door";
                                ShowMessage();
                                conditions[2] = false;
                            }
                            else
                            {
                                if (conditions[2])
                                {
                                    text = "what key? you don't have a key";
                                    ShowMessage();
                                    Console.WriteLine();
                                }
                                else
                                {
                                    text = "The door is already unlocked. did you want to lock it?";
                                    ShowMessage();
                                    text = Console.ReadLine().ToLower();
                                    screenSave.Add(text);
                                    if (text == "yes")
                                    {
                                        text = "You locked the door... for some reason.";
                                        ShowMessage();
                                        conditions[2] = true;
                                    }
                                }
                            }
                        }
                        break;

                    case "go left":
                        {
                            if (!conditions[2])
                            {
                                leave = true;
                            }
                            else
                            {
                                text = "The door is locked. Maybe there is a key somewhere.";
                                ShowMessage();
                            }
                        }
                        break;

                    case "main menu":
                        {
                            screenSave.Remove(text);
                            MainMenu(MethodBase.GetCurrentMethod());
                        }
                        break;
                }

            } while (!leave);
            Room2();
        }


        //Second room of the game
        static public void Room2()
        {
            bool leave = false;
            bool left = false;

            text = "You are in another room. Another door is to the left, the door you came through is to the right";
            if (screenSave.Count > 0)
            {
                if (text != screenSave[screenSave.Count - 1])
                {
                    ShowMessage();
                }
            }
            else
            {
                ShowMessage();
            }

            do
            {
                text = Console.ReadLine().ToLower();
                screenSave.Add(text);
                switch (text)
                {
                    case "go left":
                        {
                            leave = true;
                            left = true;
                        }
                        break;

                    case "go right":
                        {
                            leave = true;
                        }
                        break;

                    case "main menu":
                        {
                            screenSave.Remove(text);
                            MainMenu(MethodBase.GetCurrentMethod());
                        }
                        break;
                }

            } while (!leave);

            if (left)
            {
                End();
            }
            else
            {
                Room1();
            }
        }

        //the end sequence
        public static void End()
        {
            Console.Clear();
            Console.WriteLine("That is the end for now.");
            Console.WriteLine("Press any key to go back to main menu...");
            Console.ReadLine();
            MainMenu(MethodBase.GetCurrentMethod());
        }
    }
}
