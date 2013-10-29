using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardPSO
{
    class Particle
    {
        public double[] Position { get; private set; }
        private int functionNumber;
        private double[] velocity;
        public double Fitness { get; private set; }
        private double bestFitness;
        private double[] bestPosition;
        private Particle[] neighbourhood;
        private double boundary;
        private const double C1 = 2.05;
        private const double C2 = 2.05;
        private double w;
        private double X;
        //TODO Set VMax based on region max values of x for each function
        //this might be the cause of poor performance at high dimensions, maybe due to swarm explosion...
        private double VMAX;
        private Random random;
        private int dimension;

        public Particle(double[] initialPosition, double[] initialVelocity, int dims, double maxX, int number)
        {
            Position = new double[dims];
            velocity = new double[dims];
            bestPosition = new double[dims];
            boundary = maxX;
            VMAX = maxX;
            w = C1 + C2;
            functionNumber = number;
            X = 2 / Math.Abs(2 - w - Math.Sqrt(w * w - 4 * w));
            //X = 0.729;
            initialPosition.CopyTo(Position, 0);
            initialPosition.CopyTo(bestPosition, 0);
            bestFitness = Double.MaxValue;
            initialVelocity.CopyTo(velocity, 0 );
            dimension = dims;
            random = new Random();
            getFitness();
        }

        //TODO fitness update and velocity update shoul be seperate steps
        public void update(double[] globalbest)
        {
            bool calcFitness = true;
            for (int i = 0; i < dimension; i++)
            {
<<<<<<< HEAD
                double newVelocity = X *( velocity[i] + C1 * random.NextDouble() * (bestPosition[i] - Position[i]) + C2 * random.NextDouble() * (globalbest[i] - Position[i]));
=======
                double newVelocity = X * (velocity[i] + C1 * random.NextDouble() * (bestPosition[i] - Position[i]) + C2 * random.NextDouble() * (globalbest[i] - Position[i]));
>>>>>>> 890ffa328a578ab368c07258ebe9e36033e14604
                if (newVelocity > VMAX)
                    velocity[i] = VMAX;
                else if (newVelocity < -1 * VMAX)
                    velocity[i] = -1 * VMAX;
                else
                    velocity[i] = newVelocity;

                Position[i] = Position[i] + velocity[i];
                if (Math.Abs(Position[i]) > boundary)
                    calcFitness = false;
            }
            if (calcFitness)
                getFitness();
        }


        private void getFitness()
        {
            switch (functionNumber)
            {
                case 1:
                    Sphere();
                    break;
                case 2:
                    Rosenbrock();
                    break;
                case 3:
                    Ackley();
                    break;
                case 4:
                    Griewank();
                    break;
                case 5:
                    Rastrigin();
                    break;
                case 6:
                    Schaffers();
                    break;
            }
        }

        #region TestFunctions
        //f1
        private void Sphere()
        {
            Fitness = 0; // f(x) = ∑ (xi^2 -10cos(2*pi*xi) + 10)
            for (int i = 0; i < dimension; i++)
            {
                Fitness += Position[i] * Position[i] - 10 * Math.Cos(2 * Math.PI * Position[i]) + 10;
            }
            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }
        //f2
        private void Rosenbrock()
        {
            Fitness = 0; // f(x) = ∑ (100(xi+1 - xi^2)^2 + (xi - 1)^2)
            for (int i = 0; i < dimension - 1; i++)
            {
                Fitness += 100 * (Position[i + 1] - Position[i] * Position[i]) * (Position[i + 1] - Position[i] * Position[i]) + (Position[i] - 1) * (Position[i] - 1);
            }
            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }
        //f3
        private void Ackley()
        {
            Fitness = 0; // f(x) = -20*exp(-0.2*sqrt(1/n * ∑(xi^2)) - exp(1/n*∑(cos(2*pi*xi)) + 20 + exp(1)
            double sum1 = 0;
            double sum2 = 0;
            for (int i = 0; i < dimension - 1; i++)
            {
                sum1 += Position[i] * Position[i];
                sum2 += Math.Cos(2 * Math.PI * Position[i]);
            }
            Fitness = -20 * Math.Exp(-0.2 * Math.Sqrt((1 / dimension) * sum1)) - Math.Exp((1 / dimension) * sum2) + 20 + Math.Exp(1);

            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }
        //f4
        private void Griewank()
        {
            Fitness = 0; // f(x) = 1 + (1/4000) * ∑(xi^2) - N(cos(xi/pi))
            double sum = 0;
            double prod = 0;
            for (int i = 0; i < dimension; i++)
            {
                sum += Position[i] * Position[i];
                prod = prod * Math.Cos(Position[i] / Math.PI);
            }
            Fitness = 1 + (1 / 4000) * sum - prod;
            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }
        //f5
        private void Rastrigin()
        {
            Fitness = 0; // f(x) = ∑ (xi^2 - 10cos(2*pi*xi) + 10)
            for (int i = 0; i < dimension; i++)
            {
                Fitness += Position[i] * Position[i] - 10 * Math.Cos(2 * Math.PI * Position[i]) + 10;
            }
            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }
        //f6
        private void Schaffers()
        {
            Fitness = 0; // f(x) = 0.5 - ((sin(sqrt(x1^2+x2^2)))^2 - 0.5))/(1 + 0.001(x1^2+x2^2))^2

            Fitness = 0.5 - ((Math.Sin(Position[0] * Position[0] + Position[1] * Position[1])) * (Math.Sin(Position[0] * Position[0] + Position[1] * Position[1])) - 0.5) / ((1 + 0.001 * (Position[0] * Position[0] + Position[1] * Position[1])) * (1 + 0.001 * (Position[0] * Position[0] + Position[1] * Position[1])));

            if (Fitness < bestFitness)
            {
                bestFitness = Fitness;
                Position.CopyTo(bestPosition, 0);
            }
        }

        #endregion
    }
}
