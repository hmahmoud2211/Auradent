using Auradent.core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class PatientEF : IdataHelper<Patient>
    {
        private db_context db;
        private Patient table;

        public PatientEF()
        {
            db = new db_context();
            table = new Patient();
        }
        public int Add(Patient table)
        {
            try
            {
                db.Patient.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }

        public List<Patient> Add_list(Patient table)
        {
            try
            {
                db.Patient.Add(table);
                db.SaveChanges();
                return db.Patient.ToList();
            }
            catch
            {
                return new List<Patient>();
            }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Patient.Any(p => p.PatientID == id);
        }

        public int Delete(Patient item)
        {
            try
            {
                table = Find(item);
                db.Patient.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Patient Find(Patient item)
        {
            try
            {
                return db.Patient.Where(x => x.PatientID == item.PatientID).FirstOrDefault() ?? new Patient();
            }
            catch { return new Patient(); }
        }

        public List<Patient> GetAllData()
        {
            try
            {
                return db.Patient.ToListAsync().Result;
            }
            catch { return new List<Patient>(); }
        }

        public List<Patient> Search(string searchItem)
        {
            try
            {
                return db.Patient.Where(x => x.PatientID.ToString().Contains(searchItem) ||
                                             (x.PatientName != null && x.PatientName.Contains(searchItem)) ||
                                             (x.PatientPhone != null && x.PatientPhone.Contains(searchItem)) ||
                                             (x.chronic_diseases != null && x.chronic_diseases.Contains(searchItem))).ToList();
            }
            catch { return new List<Patient>(); }
        }

        public int Update(Patient table)
        {
            try
            {
                db = new db_context();
                db.Patient.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}
