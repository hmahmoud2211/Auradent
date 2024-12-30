using Auradent.core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Auradent.Data
{
    public class VisitRecordEF : IdataHelper<Visit_Record>
    {
        private db_context db;
        private Visit_Record table;

        public VisitRecordEF()
        {
            db = new db_context();
            table = new Visit_Record();
        }

        public int Add(Visit_Record table)
        {
            try
            {
                db.Visit_Records.Add(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public List<Visit_Record> Add_list(Visit_Record table)
        {
            try
            {
                db.Visit_Records.Add(table);
                db.SaveChanges();
                return db.Visit_Records.ToList();
            }
            catch
            {
                return new List<Visit_Record>();
            }
        }

        public bool CheckIfIdExists(int id)
        {
            return db.Visit_Records.Any(p => p.VisitId == id);
        }

        public int Delete(Visit_Record item)
        {
            try
            {
                table = Find(item);
                db.Visit_Records.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public Visit_Record Find(Visit_Record item)
        {
            try
            {
                return db.Visit_Records.Where(x => x.VisitId == item.VisitId).FirstOrDefault() ?? new Visit_Record();
            }
            catch { return new Visit_Record(); }
        }

        public List<Visit_Record> GetAllData()
        {
            try
            {
                return db.Visit_Records.ToListAsync().Result;
            }
            catch
            {
                return new List<Visit_Record>();
            }
        }

        public List<Visit_Record> Search(string searchItem)
        {
            try
            {
                return db.Visit_Records.Where(x => x.VisitId.ToString().Contains(searchItem)
                    || x.Status.Contains(searchItem)
                    || x.Notes.Contains(searchItem)).ToList();
            }
            catch
            {
                return new List<Visit_Record>();
            }
        }

        public int Update(Visit_Record table)
        {
            try
            {
                db = new db_context();
                db.Visit_Records.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
    }
}