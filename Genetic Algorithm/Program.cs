using System;

namespace Genetic_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var ga = new GA();
            var temp = ga.Run();

            Console.WriteLine("To be, or not to be, that is the question.");

            while (temp.BestFitness < 1)
            {
                temp.NewGeneration();
                
                Console.WriteLine($"Generation: {temp.Generation}");
                Console.WriteLine($"Best fitness: {temp.BestFitness}");
                Console.WriteLine(String.Join("",temp.BestGenes));
            }
        }
        

    }

    public class GA
    {
        GeneticAlgorithm<char> ga;
        string targetString = "To be, or not to be, that is the question.";
        string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";

        public GeneticAlgorithm<char> Run()
        {

            var random = new Random();

            ga = new GeneticAlgorithm<char>(
                populationSize: 200,
                dnaSize: targetString.Length,
                random: random,
                GetRandomGene: () => validCharacters[random.Next(validCharacters.Length)],
                FitnessFunction: FitnessFunction
                
            );

            return ga;
        }

        private float FitnessFunction(int index)
        {
            float score = 0;
            DNA<char> dna = ga.Population[index];

            for (int i = 0; i < dna.Genes.Length; i++)
            {
                if (dna.Genes[i] == targetString[i])
                {
                    score += 1;
                }
            }

            score /= targetString.Length;

            score = (MathF.Pow(5, score) - 1) / (5 - 1 );

            return score;
        }
    }
}
