﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardPSO
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfRuns = 10;
            double optimumFitness;
            const int functionNumber = 1;
            int numberOfDimensions;
            double maxX;
            double minX;
            switch (functionNumber)
            {
                //sphere
                case 1:
                    numberOfDimensions = 30;
                    maxX = 5.12;
                    minX = -5.12;
                    optimumFitness = 0;
                    break;
                //Rosenbrock
                case 2:
                    numberOfDimensions = 30;
                    maxX = 2.048;
                    minX = -2.048;
                    optimumFitness = 0;
                    break;
                //Ackley
                case 3:
                    numberOfDimensions = 30;
                    maxX = 32;
                    minX = -32;
                    optimumFitness = 0;
                    break;
                //Griewank
                case 4:
                    numberOfDimensions = 30;
                    maxX = 600;
                    minX = -600;
                    optimumFitness = 0;
                    break;
                //Rastrigin
                case 5:
                    numberOfDimensions = 30;
                    maxX = 5.12;
                    minX = -5.12;
                    optimumFitness = 0;
                    break;
                //Schaffers
                case 6:
                    numberOfDimensions = 2;
                    maxX = 100;
                    minX = -100;
                    optimumFitness = 0;
                    break;
            }

            for (int run = 0; run < numberOfRuns; run++)
            {
                Console.WriteLine("Beggining run {0}", run + 1);
                int numberOfParticles = 49;
                int squareSize = (int) Math.Sqrt(numberOfParticles);
                bool includeSelf = true;
                int numberOfIterations = 10000;

                double[] bestGlobalPosition = new double[numberOfDimensions];
                double bestGlobalFitness = Double.MaxValue;
                Random rand = new Random();
                Particle[] swarm = new Particle[numberOfParticles];

                outputStartInfo(numberOfParticles, numberOfIterations, maxX, minX, functionNumber);

                Console.WriteLine("Beginning Initialisation");
                //initialise swarm
                for (int j = 0; j < swarm.Length; j++)
                {
                    //get random initial position
                    double[] initialPos = new double[numberOfDimensions];
                    for (int i = 0; i < numberOfDimensions; i++)
                    {
                        initialPos[i] = (maxX - minX) * rand.NextDouble() + minX;
                    }

                    //get random initial velocity
                    double[] initialVelocity = new double[numberOfDimensions];
                    for (int i = 0; i < numberOfDimensions; i++)
                    {
                        double hi = Math.Abs(maxX - minX);
                        double lo = -1.0 * Math.Abs(maxX - minX);
                        initialVelocity[i] = (hi - lo) * rand.NextDouble() + lo;
                    }
                    //create and initialse particle
                    swarm[j] = new Particle(initialPos, initialVelocity, numberOfDimensions, maxX, functionNumber);
                    //check if new particle has the best position/fitness
                    if (swarm[j].Fitness < bestGlobalFitness)
                    {
                        bestGlobalFitness = swarm[j].Fitness;
                        swarm[j].Position.CopyTo(bestGlobalPosition, 0);
                    }

                }

                //create vonNeumann structure
                for (int i = 0; i < swarm.Length; i++)
                {
                    if (includeSelf)
                        swarm[i].addNeighbour(swarm[i]);
                    //left edge of square
                    if (i % squareSize == 0)
                    {
                        swarm[i].addNeighbour(swarm[i + 1]);
                        swarm[i].addNeighbour(swarm[i + squareSize - 1]);
                        //top corner
                        if (i == 0)
                        {
                            swarm[i].addNeighbour(swarm[i + (squareSize * (squareSize - 1))]);
                            swarm[i].addNeighbour(swarm[i + squareSize]);
                        }
                        //bottom corner
                        else if(i == (squareSize * (squareSize - 1)))
                        {
                            swarm[i].addNeighbour(swarm[0]);
                            swarm[i].addNeighbour(swarm[i - squareSize]);
                        }
                        //rest of edge
                        else
                        {
                            swarm[i].addNeighbour(swarm[i - squareSize]);
                            swarm[i].addNeighbour(swarm[i + squareSize]);
                        }
                    }
                    //top edge
                    else if(i < squareSize)
                    {
                        swarm[i].addNeighbour(swarm[i - 1]);
                        swarm[i].addNeighbour(swarm[i + squareSize]);
                        swarm[i].addNeighbour(swarm[i + (squareSize * (squareSize - 1))]);
                        if (i != squareSize - 1)
                            swarm[i].addNeighbour(swarm[i + 1]);

                        else
                            swarm[i].addNeighbour(swarm[i - squareSize + 1]);
                    }

                    //right edge
                    else if ((i + 1) % squareSize == 0)
                    {
                        swarm[i].addNeighbour(swarm[i - 1]);
                        swarm[i].addNeighbour(swarm[i - squareSize + 1]);
                        swarm[i].addNeighbour(swarm[i - squareSize]);
                        if (i < swarm.Length - 1)
                            swarm[i].addNeighbour(swarm[i + squareSize]);
                        else
                            swarm[i].addNeighbour(swarm[i - (squareSize * (squareSize - 1))]);
                    }

                    //bottom edge
                    else if (i > (squareSize * (squareSize - 1)))
                    {
                        swarm[i].addNeighbour(swarm[i + 1]);
                        swarm[i].addNeighbour(swarm[i - 1]);
                        swarm[i].addNeighbour(swarm[i - squareSize]);
                        swarm[i].addNeighbour(swarm[i - (squareSize * (squareSize - 1))]);
                    }

                    //particles in the middle of grid
                    else
                    {
                        swarm[i].addNeighbour(swarm[i + 1]);
                        swarm[i].addNeighbour(swarm[i - 1]);
                        swarm[i].addNeighbour(swarm[i + squareSize]);
                        swarm[i].addNeighbour(swarm[i - squareSize]);
                    }
                }
                Console.WriteLine("Initialisation Complete");

                Console.WriteLine("Beginning Main Loop");
                //main loop
                for (int iteration = 0; iteration < numberOfIterations; iteration++)
                {
                    bool stop = false;
                    foreach (Particle particle in swarm)
                    {
                        particle.update();
                        if (particle.Fitness < bestGlobalFitness)
                        {
                            bestGlobalFitness = particle.Fitness;
                            particle.Position.CopyTo(bestGlobalPosition, 0);
                            Console.WriteLine("Best Fitness: {0} Iteration: {1}", bestGlobalFitness, iteration);
                        }

                        //if (bestGlobalFitness < (optimumFitness + optimumFitness * 0.01) && bestGlobalFitness > (optimumFitness - optimumFitness * 0.01))
                        //{
                        //    Console.WriteLine("Current global best within 1% of optimum");
                        //    Console.WriteLine("Stopping at iteration: {0}", iteration);
                        //    stop = true;
                        //    break;
                        //}
                    }
                    if (stop)
                        break;
                }
                Console.WriteLine("End of main loop");
                outputSummary(bestGlobalPosition, bestGlobalFitness);
            }
        }

        public static void outputStartInfo(int noParticles, int noIterations, double max, double min, int funcNumber)
        {
            Console.WriteLine("Beginning vonNeumann PSO");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Number of particles: {0}", noParticles);
            Console.WriteLine("Maximum number of iterations: {0}", noIterations);
            Console.WriteLine("Function Number: {0}", funcNumber);
            switch (funcNumber)
            {
                case 1:
                    Console.WriteLine("Problem being optimised: f(x) = ∑ (xi^2)");
                    break;
                case 2:
                    Console.WriteLine("Problem being optimised: f(x) = ∑ (100(xi+1 - xi^2)^2 + (xi - 1)^2)");
                    break;
                case 3:
                    Console.WriteLine("Problem being optimised: f(x) = -20*exp(-0.2*sqrt(1/n * ∑(xi^2)) - exp(1/n*∑(cos(2*pi*xi)) + 20 + exp(1)");
                    break;
                case 4:
                    Console.WriteLine("Problem being optimised: f(x) = 1 + (1/4000) * ∑(xi^2) - N(cos(xi/pi))");
                    break;
                case 5:
                    Console.WriteLine("Problem being optimised: f(x) = ∑ (xi^2 - 10cos(2*pi*xi) + 10)");
                    break;
                case 6:
                    Console.WriteLine("Problem being optimised: f(x) = 0.5 - ((sin(sqrt(x1^2+x2^2)))^2 - 0.5))/(1 + 0.001(x1^2+x2^2))^2");
                    break;
            }
            Console.WriteLine("Range of values: {0} < x < {1}", min, max);
            Console.WriteLine("--------------------------------------\n\n");
        }

        public static void outputSummary(double[] bestPos, double bestFitness)
        {
            Console.WriteLine("Summary");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Best particle position: ");
            foreach (Double pos in bestPos)
            {
                Console.Write(pos + " ");
            }
            Console.WriteLine("\nBest fitness: {0}", bestFitness);
            Console.WriteLine("--------------------------------------\n\n");
            Console.ReadLine();

        }


    }
}