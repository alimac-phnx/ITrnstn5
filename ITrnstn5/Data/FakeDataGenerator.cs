﻿using Bogus;
using ITrnstn5.Models;
using System;
using System.Collections.Generic;

namespace ITrnstn5.Data
{
    public class FakeDataGenerator
    {
        public static Random Random { get; set; }

        public static Faker Faker { get; set; }

        public static List<FakeUser> GenerateFakeUserData(string region, int seed, int pageNumber)
        {
            int combinedSeed = (seed + pageNumber.ToString()).GetHashCode();

            Random = new Random(combinedSeed);
            Faker = CreateFaker(region);

            return GenerateTableSet(pageNumber);
        }

        private static Faker CreateFaker(string region)
        {
            if (region != null) return new Faker(region);

            return new Faker();
        }

        private static List<FakeUser> GenerateTableSet(int pageNumber)
        {
            List<FakeUser> fakeusers = new List<FakeUser>();

            for (int i = 0; i < 20; i++)
            {
                fakeusers.Add(GenerateUser(pageNumber, i));
            }

            return fakeusers;
        }

        private static FakeUser GenerateUser(int pageNumber, int i)
        {
            return new FakeUser
            {
                Number = (pageNumber - 1) * 20 + i + 1,
                Id = Guid.NewGuid().ToString(),
                Name = Faker.Name.FullName(),
                Address = Faker.Address.FullAddress(),
                Phone = Faker.Phone.PhoneNumber()
            };
        }
    }
}
