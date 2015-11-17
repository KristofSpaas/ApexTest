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
    [Authorize(Roles = "Doctor, Patient")]
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages/5
        [Route("api/Messages/{id}")]
        [ResponseType(typeof (Message))]
        public IHttpActionResult GetMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return BadRequest("Message with id " + id + " does not exist.");
            }

            return Ok(message);
        }

        // GET: api/Messages/Patient/5
        [Route("api/Messages/Patient/{patientId}")]
        public IHttpActionResult GetMessagesByPatientId(int patientId)
        {
            Patient patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return BadRequest("Patient with id " + patientId + " does not exist.");
            }

            IQueryable<Message> messagesByPatientId = db.Messages.Where(r => r.PatientId == patientId);

            return Ok(messagesByPatientId);
        }

        // PUT: api/Messages/5
        [Route("api/Messages/{id}")]
        [ResponseType(typeof (void))]
        public IHttpActionResult PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.MessageId)
            {
                return BadRequest("The MessageId in the URL and the MessageId in the data do not match.");
            }

            Patient patient = db.Patients.Find(message.PatientId);
            if (patient == null)
            {
                return BadRequest("Patient with id " + message.PatientId + " does not exist.");
            }

            Doctor doctor = db.Doctors.Find(message.DoctorId);
            if (doctor == null)
            {
                return BadRequest("Doctor with id " + message.DoctorId + " does not exist.");
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return BadRequest("Message with id " + id + " does not exist.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new {controller = "messages", id = message.MessageId}, message);
        }

        // POST: api/Messages
        [Route("api/Messages")]
        [ResponseType(typeof (Message))]
        public IHttpActionResult PostMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient patient = db.Patients.Find(message.PatientId);
            if (patient == null)
            {
                return BadRequest("Patient with id " + message.PatientId + " does not exist.");
            }

            Doctor doctor = db.Doctors.Find(message.DoctorId);
            if (doctor == null)
            {
                return BadRequest("Doctor with id " + message.DoctorId + " does not exist.");
            }

            db.Messages.Add(message);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {controller = "messages", id = message.MessageId}, message);
        }

        // DELETE: api/Messages/5
        [Route("api/Messages/{id}")]
        [ResponseType(typeof (Message))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return BadRequest("Message with id " + id + " does not exist.");
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return Ok(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.MessageId == id) > 0;
        }
    }
}