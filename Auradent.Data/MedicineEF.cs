using Auradent.core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class MedicineEF : IdataHelper<Medicine>
    {
        private db_context db;
        private Medicine table;

        public MedicineEF()
        {
            db = new db_context();
            table = new Medicine();
        }
        public int Add(Medicine table)
        {
            try
            {
                db.Medicine.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }

        public List<Medicine> Add_list(Medicine table)
        {
            try
            {
                db.Medicine.Add(table);
                db.SaveChanges();
                return db.Medicine.ToList();
            }
            catch
            {
                return new List<Medicine>();
            }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Medicine.Any(p => p.ID == id);
        }

        public int Delete(Medicine item)
        {
            try
            {
                table = Find(item);
                db.Medicine.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Medicine Find(Medicine item)
        {
            try
            {
                return db.Medicine.Where(x => x.ID == item.ID).FirstOrDefault() ?? new Medicine();
            }
            catch { return new Medicine(); }
        }

        public List<Medicine> GetAllData()
        {
            try
            {
                return db.Medicine.ToListAsync().Result;
            }
            catch { return new List<Medicine>(); }
        }

        public List<Medicine> Search(string searchItem)
        {
            try
            {
                return db.Medicine.Where(x => x.ID.ToString().Contains(searchItem) || (x.MedicineName != null && x.MedicineName.Contains(searchItem))).ToList();
            }
            catch { return new List<Medicine>(); }
        }

        public int Update(Medicine table)
        {
            try
            {
                db = new db_context();
                db.Medicine.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}
