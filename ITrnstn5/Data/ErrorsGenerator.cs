using Bogus;
using Bogus.DataSets;
using ITrnstn5.Models;
using System;

namespace ITrnstn5.Data
{
    public static class ErrorsGenerator
    {
        public static void ApplyErrors(ref FakeUser fakeuser, double errorRate, Random random, Faker faker)
        {
            int errorCount = (int)Math.Floor(errorRate);
            double relativeError = errorRate - errorCount;

            for (int i = 0; i < errorCount; i++)
            {
                ApplyRandomError(ref fakeuser, random, faker);
            }

            if (random.NextDouble() < relativeError)
            {
                ApplyRandomError(ref fakeuser, random, faker);
            }
        }

        private static void ApplyRandomError(ref FakeUser fakeuser, Random random, Faker faker)
        {
            int field = random.Next(3);
            int errorType = random.Next(3);

            switch (field)
            {
                case 0:
                    fakeuser.Name = ApplyErrorToString(fakeuser.Name, errorType, random, faker);
                    break;
                case 1:
                    fakeuser.Address = ApplyErrorToString(fakeuser.Address, errorType, random, faker);
                    break;
                case 2:
                    fakeuser.Phone = ApplyErrorToString(fakeuser.Phone, errorType, random, faker);
                    break;
            }
        }

        private static string ApplyErrorToString(string input, int errorType, Random random, Faker faker)
        {
            if (string.IsNullOrEmpty(input)) return input;

            switch (errorType)
            {
                case 0:
                    return DeleteSymbol(random, input);
                case 1:
                    return AddSymbol(random, input, faker);
                case 2:
                    return TransposeSymbols(random, input);
            }

            return input;
        }

        private static string DeleteSymbol(Random random, string input)
        {
            int deleteIndex = random.Next(input.Length);

            return input.Remove(deleteIndex, 1);
        }

        private static string AddSymbol(Random random, string input, Faker faker)
        {
            int insertIndex = random.Next(input.Length);
            char randomChar = faker.Random.Char('a', 'z');

            return input.Insert(insertIndex, randomChar.ToString());
        }

        private static string TransposeSymbols(Random random, string input)
        {
            if (input.Length < 2) return input;

            int swapIndex = random.Next(input.Length - 1);
            char[] chars = input.ToCharArray();
            (chars[swapIndex + 1], chars[swapIndex]) = (chars[swapIndex], chars[swapIndex + 1]);

            return new string(chars);
        }
    }
}
