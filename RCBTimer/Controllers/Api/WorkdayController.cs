using DAL.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace RCBTimer.Controllers.Api
{
    public class WorkdayController : ApiController
    {
        private EmployeeRepository employeeRepository;
        private WorkdayRepository workdayRepository;
        private JsonSerializerSettings jsonConverterSettings;

        public WorkdayController()
        {
            employeeRepository = new EmployeeRepository();
            workdayRepository = new WorkdayRepository();
            var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm"
            };
            jsonConverterSettings = new JsonSerializerSettings();
            jsonConverterSettings.Converters.Add(dateConverter);
        }

        // GET api/<controller>
        public string GetBoard()
        {
            var model = employeeRepository.GetAll();
            var json = new JavaScriptSerializer().Serialize(model);
            return json;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}