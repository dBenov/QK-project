﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace God
{
    // This is the game engine responsible for showing the game menu and simulating world actions.
    public class Simulator
    {
        private static readonly int inRangeOfAttackDistance = 500;
        private static God mPlayer;

        public Simulator()
        {
            this.MPlayer = new God("User", 2000, 2000, 0, 0, new Point2D(1, 1), State.Analyzing);
            Planet a = new Planet("Earth");
            Scene.AllPlanets.Add(a);       
        }
       
        public God MPlayer
        {
            get
            {
                return mPlayer;
            }
            set
            {
                if(value==null)
                {
                    throw new ArgumentOutOfRangeException("You should initialize God");
                }
                mPlayer = value;       
            }
        }

        public void Run()
        {
            Thread thread = new Thread(Update);
            thread.Start();
            this.Menu();
            Update();          
        }

        public void Menu()
        {
            Console.WriteLine("The commands are:");
            Console.WriteLine("For adding a planet: add<name_planet>");
            Console.WriteLine("For adding creatures to a planet:add<name_planet><entity|animal|human|god><numbers_creatures>");
            Console.WriteLine("For killing creatures on a planet: kill<name_planet>");
            Console.WriteLine("For destroying a planet: destroy<name_planet>");
            Console.WriteLine("For receiving a statistic: statistic");

            while (true)
            {          
                CommandManager.Read();
            }       
        }

        // Moving every entity to a new random location and then doing a random action.
        public static void Update()
        {
            while (true)
            {
               for (int i = 0; i < Scene.AllPlanets.Count(); i++)
                {                    
                    for (int k = 0; k < Scene.AllPlanets[i].citizens.Count(); k++)
                    {
                        if (Scene.AllPlanets[i].citizens[k]!=null)
                        {
                            Point2D point = new Point2D(RandomG.RandomNumbers(-100, 100), RandomG.RandomNumbers(-100, 100));
                            Scene.AllPlanets[i].citizens[k].Move(point);
                            Console.WriteLine("Creature {0} is with coordinates {1}", Scene.AllPlanets[i].citizens[k], point);
                        }
                       
                    }
                }
               simulateActions();
               Thread.Sleep(8000);
            }
        }

        public static double Distance(Point2D a, Point2D b)
        {
            double distance = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
            distance = Math.Sqrt(distance);
            return distance;
        }
        public static void simulateActions()
        {
            
            for (int i = 0; i < Scene.AllPlanets.Count(); i++)
            {
                for (int m = 0; m < Scene.AllPlanets[i].citizens.Count() - 1; m++)
                {

                    if (Scene.AllPlanets[i].citizens[m] != null && Scene.AllPlanets[i].citizens[m+1]!=null)
                    {

                        if (Distance(Scene.AllPlanets[i].citizens[m].Location, Scene.AllPlanets[i].citizens[m+1].Location) <= inRangeOfAttackDistance)
                        {

                            if (RandomG.RandomNumbers(-100, 100) < 0)
                            {
                                Console.WriteLine("{0} attacked {1}", Scene.AllPlanets[i].citizens[m], Scene.AllPlanets[i].citizens[m+1]);
                                Scene.AllPlanets[i].citizens[m].Attack(Scene.AllPlanets[i].citizens[m+1]);
                            }
                            else
                            {
                                if (Scene.AllPlanets[i].citizens[m].GetType() == Scene.AllPlanets[i].citizens[m+1].GetType())
                                {
                                    Entity q = new Entity(RandomG.RandomName());
                                    mPlayer.AddCitizen(Scene.AllPlanets[i], q);
                                    Console.WriteLine("{0} and {1} have baby-{2}", Scene.AllPlanets[i].citizens[m], Scene.AllPlanets[i].citizens[m + 1], q.Name);
                                }
                                else
                                {
                                    Console.WriteLine("{0} and {1} tried to have baby but are diferent types", Scene.AllPlanets[i].citizens[m], Scene.AllPlanets[i].citizens[m + 1]);
                                }
                            }
                            Thread.Sleep(5000);
                        }
                    }
                }              
            }
        }
    }
}
