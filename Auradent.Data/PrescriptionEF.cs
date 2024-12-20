using Auradent.core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class PrescriptionEF : IdataHelper<Prescription>
    {
        private db_context db;
        private Prescription table;

        public PrescriptionEF()
        {
            db = new db_context();
            table = new Prescription();
        }
        public int Add(Prescription table)
        {
            try
            {
                db.Prescription.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }

        public List<Prescription> Add_list(Prescription table)
        {
            try
            {
                db.Prescription.Add(table);
                db.SaveChanges();
                return db.Prescription.ToList();
            }
            catch
            {
                return new List<Prescription>();
            }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Prescription.Any(p => p.PrescriptionID == id);
        }

        public int Delete(Prescription item)
        {
            try
            {
                table = Find(item);
                db.Prescription.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Prescription Find(Prescription item)
        {
            try
            {
                return db.Prescription.Where(x => x.PrescriptionID == item.PrescriptionID).FirstOrDefault() ?? new Prescription();
            }
            catch { return new Prescription(); }
        }

        public List<Prescription> GetAllData()
        {
            try
            {
                return db.Prescription.ToListAsync().Result;
            }
            catch { return new List<Prescription>(); }
        }

        public List<Prescription> Search(string searchItem)
        {
            try
            {
                return db.Prescription.Where(x =>
                    (x.Doses != null && x.Doses.Contains(searchItem)) ||
                    (x.Medicines != null && x.Medicines.Any(u => u.MedicineName != null && u.MedicineName.Contains(searchItem)))).ToList();
            }
            catch
            {
                return new List<Prescription>();
            }
        }

        public int Update(Prescription table)
        {
            try
            {
                db = new db_context();
                db.Prescription.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}

