using System;

namespace Genetic_Algorithm
{
    public class DNA<T>
    {
        public T[] Genes { get; private set; }
        public float Fitness { get; private set; }
        private readonly Random random;
        private readonly Func<T> GetRandomGene;
        private readonly Func<int, float> FitnessFunction; 

        public DNA(int size, Random random, Func<T> GetRandomGene, Func<int, float> FitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new T[size];
            this.random = random;
            this.GetRandomGene = GetRandomGene;
            this.FitnessFunction = FitnessFunction;

            if (shouldInitGenes)
            {
                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = GetRandomGene();
                }
            }
        }

        public float CalculateFitness(int index)
        {
            Fitness = FitnessFunction(index);
            return Fitness;
        }

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            DNA<T> child = new DNA<T>(Genes.Length, random, GetRandomGene, FitnessFunction, shouldInitGenes: false);

            for (int i = 0; i < Genes.Length; i++)
            {
                child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] :  otherParent.Genes[i];
            }

            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    Genes[i] = GetRandomGene();
                }
            }
        }
    }
}
