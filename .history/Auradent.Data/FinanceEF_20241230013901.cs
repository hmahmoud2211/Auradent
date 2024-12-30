using Auradent.core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class FinanceEF : IdataHelper<Finance>
    {
        private db_context db;
        private Finance table;

        public FinanceEF()
        {
            db = new db_context();
            table = new Finance();
        }

        public int Add(Finance table)
        {
            try
            {
                db.Finance.Add(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public List<Finance> Add_list(Finance table)
        {
            try
            {
                db.Finance.Add(table);
                db.SaveChanges();
                return db.Finance.ToList();
            }
            catch
            {
                return new List<Finance>();
            }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Finance.Any(p => p.FinanceId == id);
        }

        public int Delete(Finance item)
        {
            try
            {
                table = Find(item);
                db.Finance.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Finance Find(Finance item)
        {
            try
            {
                return db.Finance.Where(x => x.FinanceId == item.FinanceId).FirstOrDefault() ?? new Finance();
            }
            catch { return new Finance(); }
        }

        public List<Finance> GetAllData()
        {
            try
            {
                using (var freshDb = new db_context())
                {
                    return freshDb.Finance.ToList();
                }
            }
            catch
            {
                return new List<Finance>();
            }
        }

        public List<Finance> Search(string searchItem)
        {
            try
            {
                return db.Finance.Where(x => x.FinanceId.ToString().Contains(searchItem)
                    || (x.TypeOfPayment != null && x.TypeOfPayment.Contains(searchItem))
                    || x.TotalAmount.ToString().Contains(searchItem)
                    || (x.PaymentMethod != null && x.PaymentMethod.Contains(searchItem))).ToList();
            }
            catch
            {
                return new List<Finance>();
            }
        }

        public int Update(Finance table)
        {
            try
            {
                db = new db_context();
                db.Finance.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}
