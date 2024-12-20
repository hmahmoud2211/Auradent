using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auradent.core;

namespace Auradent.Data
{
    public class RadiologyEF : IdataHelper<RadiologyORtest>
    {
        private db_context db;
        private RadiologyORtest table;
        public RadiologyEF()
        {
            db = new db_context();
            table = new RadiologyORtest();
        }
        public int Add(RadiologyORtest table)
        {
            try
            {
                db.radiologyORtests.Add(table);
                db.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }
        public bool CheckIfIdExists(int id)
        {
            return db.radiologyORtests.Any(p => p.RadiologyORtestID == id);
        }
        public int Delete(RadiologyORtest item)
        {
            try
            {
                table = Find(item);
                db.radiologyORtests.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
        public RadiologyORtest Find(RadiologyORtest item)
        {
            try
            {
                return db.radiologyORtests.Where(x => x.RadiologyORtestID == item.RadiologyORtestID).FirstOrDefault() ?? new RadiologyORtest();
            }
            catch { return new RadiologyORtest(); }
        }
        public List<RadiologyORtest> GetAllData()
        {
            try
            {
                return db.radiologyORtests.ToListAsync().Result ?? new List<RadiologyORtest>();
            }
            catch { return new List<RadiologyORtest>(); }
        }

        public List<RadiologyORtest> Add_list(RadiologyORtest table)
        {
            try
            {
                db.radiologyORtests.Add(table);
                db.SaveChanges();
                return db.radiologyORtests.ToList();
            }
            catch
            {
                return new List<RadiologyORtest>();
            }
        }

        public List<RadiologyORtest> Search(string searchItem)
        {
            try
            {
                return db.radiologyORtests
                    .Where(x => x.RadiologyORtestID.ToString().Contains(searchItem) || (x.Report != null && x.Report.Contains(searchItem)))
                    .ToList();
            }
            catch { return new List<RadiologyORtest>(); }
        }

        public int Update(RadiologyORtest table)
        {
            try
            {
                db = new db_context();
                db.radiologyORtests.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}
