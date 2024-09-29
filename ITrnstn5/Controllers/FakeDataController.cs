using Bogus;
using Bogus.DataSets;
using ITrnstn5.Data;
using ITrnstn5.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace ITrnstn5.Controllers
{
    public class FakeDataController : Controller
    {
        [HttpGet]
        public IActionResult GenerateData(string region, double errorrate, int seed, int pagenumber = 1)
        {
            var fakedata = FakeDataGenerator.GenerateFakeUserData(region, seed, pagenumber);

            return View("index", fakedata);
        }

        [HttpGet]
        public IActionResult LoadData(string region, double errorRate, int seed, int pageNumber)
        {
            var fakeData = FakeDataGenerator.GenerateFakeUserData(region, seed, pageNumber);

            fakeData = ChangeDataWithErrors(fakeData, errorRate);

            return PartialView("_FakeDataPartial", fakeData);
        }

        [HttpPost]
        public IActionResult ApplyErrors([FromBody] ErrorRequest request)
        {
            var tableData = request.TableData;
            var errorRate = request.ErrorRate;

            tableData = ChangeDataWithErrors(tableData, errorRate);

            return PartialView("_FakeDataPartial", tableData);
        }

        [HttpGet]
        public IActionResult ExportToCsv([FromBody] ErrorRequest request)
        {
            var tableData = request.TableData;

            var csv = new StringBuilder();
            csv.AppendLine("Number,Id,Name,Address,Phone");

            foreach (var user in tableData)
            {
                csv.AppendLine($"{user.Number},{user.Id},{user.Name},{user.Address},{user.Phone}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "FakeData.csv");
        }


        public List<FakeUser> ChangeDataWithErrors(List<FakeUser> fakedata, double errorRate)
        {
            for (int i = 0; i < fakedata.Count; i++)
            {
                var fakeuser = fakedata[i];
                ErrorsGenerator.ApplyErrors(ref fakeuser, errorRate, FakeDataGenerator.Random, FakeDataGenerator.Faker);
                fakedata[i] = fakeuser;
            }

            return fakedata;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
