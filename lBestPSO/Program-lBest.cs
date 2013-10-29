using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardPSO
{
    class Program
    {
        static void Main(string[] args)
        {
            double optimumFitness;
            const int functionNumber = 1;
            int numberOfDimensions;
            double maxX;
            double minX;
            switch (functionNumber)
            {
                case 1:
                    numberOfDimensions = 10;
                    maxX = 5.12;
                    minX = -5.12;
                    optimumFitness = 0;
                    break;
                case 2:
                    numberOfDimensions = 10;
                    maxX = 2.048;
                    minX = -2.048;
                    optimumFitness = 0;
                    break;
                    //TODO investigate issues with functions 3 & 4
                case 3:
                    numberOfDimensions = 10;
                    maxX = 30;
                    minX = -30;
                    optimumFitness = 0;
                    break;
                case 4:
                    numberOfDimensions = 2;
                    maxX = 600;
                    minX = -600;
                    optimumFitness = 0;
                    break;
                case 5:
                    numberOfDimensions = 10;
                    maxX = 5.12;
                    minX = -5.12;
                    optimumFitness = 0;
                    break;
                case 6:
                    numberOfDimensions = 2;
                    maxX = 100;
                    minX = -100;
                    optimumFitness = 0;
                    break;
            }
            
            int numberOfParticles = 100;
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

            //create neighbourhoods
            for (int i = 0; i < swarm.Length; i++)
            {
                //first particle
                if (i == 0)
                {
                    swarm[i].addNeighbour(swarm[i]);
                    swarm[i].addNeighbour(swarm[i + 1]);
                    swarm[i].addNeighbour(swarm[swarm.Length - 1]);
                }
                //last particle
                else if (i == (swarm.Length - 1))
                {
                    swarm[i].addNeighbour(swarm[i]);
                    swarm[i].addNeighbour(swarm[0]);
                    swarm[i].addNeighbour(swarm[i - 1]);
                }
                //other particles
                else
                {
                    swarm[i].addNeighbour(swarm[i]);
                    swarm[i].addNeighbour(swarm[i + 1]);
                    swarm[i].addNeighbour(swarm[i - 1]);
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

                    if (bestGlobalFitness < (optimumFitness + optimumFitness * 0.01) && bestGlobalFitness > (optimumFitness - optimumFitness * 0.01))
                    {
                        Console.WriteLine("Current global best within 1% of optimum");
                        Console.WriteLine("Stopping at iteration: {0}", iteration);
                        stop = true;
                        break;
                    }
                }
                if (stop)
                    break;
            }
            Console.WriteLine("End of main loop");
            outputSummary(bestGlobalPosition, bestGlobalFitness);
        }

        public static void outputStartInfo(int noParticles, int noIterations, double max, double min, int funcNumber)
        {
            Console.WriteLine("Beginning gBest PSO");
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
