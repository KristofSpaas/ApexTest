﻿using System;
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
    [Authorize(Roles = "Doctor")]
    public class PatientsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Patients
        public IQueryable<Patient> GetPatients()
        {
            return db.Patients;
        }

        // GET: api/Patients/Doctor/5
        [Route("api/Patients/Doctor/{doctorId}")]
        public IHttpActionResult GetPatientsByDoctorId(int doctorId)
        {
            Doctor doctor = db.Doctors.Find(doctorId);
            if (doctor == null)
            {
                return BadRequest("Doctor with id " + doctorId + " does not exist.");
            }

            IQueryable<Patient> patientsByDoctorId = db.Patients.Where(r => r.DoctorId == doctorId);

            return Ok(patientsByDoctorId);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof (Patient))]
        [Route("api/Patients/{id}")]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return BadRequest("Patient with id " + id + " does not exist.");
            }

            var user = db.Users.Find(patient.UserId);
            if (user == null)
            {
                return BadRequest("User with id " + patient.UserId + " does not exist.");
            }

            db.Patients.Remove(patient);
            db.Users.Remove(user);

            db.SaveChanges();

            return Ok(patient);
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