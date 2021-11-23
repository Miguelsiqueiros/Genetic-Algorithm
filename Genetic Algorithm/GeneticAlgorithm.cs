using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic_Algorithm
{
    public class GeneticAlgorithm<T>
    {
        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }

        public float MutationRate;
        private readonly Random Random;

        private float FitnessSum;
        public float BestFitness { get; set; }
        public T[] BestGenes { get; set; }

        public GeneticAlgorithm(
            int populationSize,
            int dnaSize,
            Random random,
            Func<T> GetRandomGene,
            Func<int, float> FitnessFunction,
            float mutationRate = 0.01f
            )
        {
            Generation = 1;
            MutationRate = mutationRate;
            Random = random;
            Population = new List<DNA<T>>(populationSize);
            BestGenes = new T[dnaSize];

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, GetRandomGene, FitnessFunction, shouldInitGenes: true));
            }
        }

        public void NewGeneration()
        {
            if(Population.Count <= 0)
            {
                return;
            }

            CalculateFitness();

            Population.OrderByDescending(x => x.Fitness);

            var newPopulation = new List<DNA<T>>();

            for (int i = 0; i < Population.Count; i++)
            {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);
                
                child.Mutate(MutationRate);
                
                newPopulation.Add(child);
            }

            Population = newPopulation;
            Generation++;
        }

        public void CalculateFitness()
        {
            FitnessSum = 0;
            DNA<T> bestIndividual = Population[0];

            for (int i = 0; i < Population.Count; i++)
            {
                FitnessSum += Population[i].CalculateFitness(i);

                if(Population[i].Fitness > bestIndividual.Fitness)
                {
                    bestIndividual = Population[i];
                }
            }

            BestFitness = bestIndividual.Fitness;
            bestIndividual.Genes.CopyTo(BestGenes, 0);
        }

        private DNA<T> ChooseParent()
        {
            double randomNumber = Random.NextDouble() * FitnessSum;

            for (int i = 0; i < Population.Count; i++)
            {
                // review this again
                if(randomNumber < Population[i].Fitness)
                {
                    return Population[i];
                }
                randomNumber -= Population[i].Fitness;
            }

            return null;
        }
    }
}
