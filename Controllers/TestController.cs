using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EFCorePerformanceTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCorePerformanceTest.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly IOptions<SettingOptions> _options;
        private readonly Repository _repository;
        public TestController(IOptions<SettingOptions> options, Repository repository)
        {
            _options = options;
            _repository = repository;
        }
        [HttpGet]
        public IEnumerable<ExampleTable> Get([FromQuery] bool? useEFCore)
        {
            var stopwatch = Stopwatch.StartNew();
            List<ExampleTable> results = null;
            if (useEFCore.GetValueOrDefault())
            {
                results = _repository.ExampleTable.ToList();
            }
            else
            {
                using (var db = new SqlConnection(_options.Value.ConnectionString))
                {
                    results = db.Query<ExampleTable>(@"SELECT * FROM dbo.ExampleTable").ToList();
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Finished GET in {stopwatch.ElapsedMilliseconds}ms.");
            return results;            
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExampleTable row, [FromQuery] bool? useEFCore)
        {
            var stopwatch = Stopwatch.StartNew();
            if (useEFCore.GetValueOrDefault())
            {
                _repository.ExampleTable.Add(row);
                _repository.SaveChanges();
            }
            else
            {
                using (var db = new SqlConnection(_options.Value.ConnectionString))
                {
                    row.Id = db.Query<int>(@"INSERT INTO dbo.ExampleTable (Created, Message, IsEfCore) VALUES (@Created, @Message, @IsEfCore); SELECT SCOPE_IDENTITY()", row).Single();
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Finished POST in {stopwatch.ElapsedMilliseconds}ms.");
            return Ok(row);
        }      
    }
}
