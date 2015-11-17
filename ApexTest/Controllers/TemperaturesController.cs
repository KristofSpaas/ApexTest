using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApexTest.Models;

namespace ApexTest.Controllers
{
    [Authorize(Roles = "Doctor, Patient")]
    public class TemperaturesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Temperatures/Patient/5
        [Authorize(Roles = "Doctor, Patient")]
        [Route("api/Temperatures/Patient/{patientId}")]
        public IHttpActionResult GetTemperaturesByPatientId(int patientId)
        {
            Patient patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return BadRequest("Patient with id " + patientId + " does not exist.");
            }

            IQueryable<Temperature> temperaturesByPatientId = db.Temperatures.Where(r => r.PatientId == patientId);

            return Ok(temperaturesByPatientId);
        }

        // POST: api/Temperatures
        [Authorize(Roles = "Patient")]
        [Route("api/Temperatures")]
        [ResponseType(typeof (Temperature))]
        public IHttpActionResult PostTemperature(Temperature temperature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient patient = db.Patients.Find(temperature.PatientId);
            if (patient == null)
            {
                return BadRequest("Patient with id " + temperature.PatientId + " does not exist.");
            }

            db.Temperatures.Add(temperature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {controller = "temperatures", id = temperature.TemperatureId},
                temperature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}