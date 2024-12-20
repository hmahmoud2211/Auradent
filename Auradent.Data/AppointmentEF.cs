using Auradent.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class AppointmentEF : IdataHelper<Appointment>
    {
        private db_context db;
        private Appointment table;

        public AppointmentEF()
        {
            db = new db_context();
            table = new Appointment();
        }

        public int Add(Appointment table)
        {
            try
            {
                db.Appointment.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Appointment.Any(p => p.AppointmentID == id);
        }

        public int Delete(Appointment item)
        {
            try
            {
                table = Find(item);
                db.Appointment.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Appointment Find(Appointment item)
        {
            try
            {
                return db.Appointment.Where(x => x.AppointmentID == item.AppointmentID).FirstOrDefault() ?? new Appointment();
            }
            catch { return new Appointment(); }
        }

        public List<Appointment> GetAllData()
        {
            try
            {
                return db.Appointment.ToListAsync().Result;
            }
            catch { return new List<Appointment>(); }
        }

        public List<Appointment> Search(string searchItem)
        {
            try
            {
                return db.Appointment.Where(x => x.AppointmentID.ToString().Contains(searchItem) || x.AppointmentStatus.Contains(searchItem) ||
                x.AppointmentTime.ToString().Contains(searchItem) || x.AppointmentDate.ToString().Contains(searchItem)).ToList();
            }
            catch { return new List<Appointment>(); }
        }

        public int Update(Appointment table)
        {
            try
            {
                db = new db_context();
                db.Appointment.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
        public List<Appointment> Add_list(Appointment table)
        {
            try
            {
                db.Appointment.Add(table);
                db.SaveChanges();
                return db.Appointment.ToList();
            }
            catch
            {
                return new List<Appointment>();
            }
        }
    }
}
