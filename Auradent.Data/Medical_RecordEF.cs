using Auradent.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class Medical_RecordEF : IdataHelper<Medical_Record>
    {
        private db_context db;
        private Medical_Record table;
        public Medical_RecordEF()
        {
            db = new db_context();
            table = new Medical_Record();
        }
        public int Add(Medical_Record table)
        {
            try
            {
                db.medical_Records.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }
        public bool CheckIfIdExists(int id)
        {
            return db.medical_Records.Any(p => p.RecordId == id);
        }
        public int Delete(Medical_Record item)
        {
            try
            {
                table = Find(item);
                db.medical_Records.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
        public Medical_Record Find(Medical_Record item)
        {
            try
            {
                return db.medical_Records.Where(x => x.RecordId == item.RecordId).FirstOrDefault() ?? new Medical_Record();
            }
            catch { return new Medical_Record(); }
        }
        public List<Medical_Record> GetAllData()
        {
            try
            {
                return db.medical_Records.ToList();
            }
            catch { return new List<Medical_Record>(); }
        }

        List<Medical_Record> IdataHelper<Medical_Record>.Add_list(Medical_Record table)
        {
            try
            {
                db.medical_Records.Add(table);
                db.SaveChanges();
                return db.medical_Records.ToList();
            }
            catch
            {
                return new List<Medical_Record>();
            }
        }

        List<Medical_Record> IdataHelper<Medical_Record>.Search(string searchItem)
        {
            try
            {
                return db.medical_Records.Where(x => x.RecordId.ToString().Contains(searchItem) || (x.Report != null && x.Report.Contains(searchItem)) ||
               (x.TreatmentPlan != null && x.TreatmentPlan.Contains(searchItem)) || (x.Subjective != null && x.Subjective.Contains(searchItem)) ||
               (x.objective != null && x.objective.Contains(searchItem)) || (x.Assessment != null && x.Assessment.Contains(searchItem)) ||
               (x.Notes != null && x.Notes.Contains(searchItem))).ToList();
            }
            catch { return new List<Medical_Record>(); }
        }

        int IdataHelper<Medical_Record>.Update(Medical_Record table)
        {
            throw new NotImplementedException();
        }
    }   
    
    
}
