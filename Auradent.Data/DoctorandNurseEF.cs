using Auradent.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.Data
{
    public class DoctorandNurseEF : IdataHelper<DoctorandNurse>
    {
        private db_context db;
        private DoctorandNurse? table; 

        public DoctorandNurseEF()
        {

            db = new db_context();
            table = null; 
        }
        public int Add(DoctorandNurse table)
        {
            try
            {
                db.DoctorandNurses.Add(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
        public bool CheckIfIdExists(int id)
        {
            return db.DoctorandNurses.Any(p => p.ID == id);
        }
        public int Delete(DoctorandNurse item)
        {
            try
            {
                table = Find(item);
                db.DoctorandNurses.Remove(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }
        public DoctorandNurse Find(DoctorandNurse item)
        {
            try
            {
                return db.DoctorandNurses.Where(x => x.ID == item.ID).Select(x => new DoctorandNurse
                {
                    ID = x.ID,
                    Username = x.Username,
                    Password = x.Password,
                    Role = x.Role,
                    Nationa_ID = x.Nationa_ID,
                    Appointments = x.Appointments,
                    Prescriptions = x.Prescriptions
                }).FirstOrDefault() ?? new DoctorandNurse
                {
                    ID = 0,
                    Username = string.Empty,
                    Password = string.Empty,
                    Role = string.Empty,
                    Nationa_ID = string.Empty,
                    Appointments = new List<Appointment>(),
                    Prescriptions = new List<Prescription>()
                };
            }
            catch
            {
                return new DoctorandNurse
                {
                    ID = 0,
                    Username = string.Empty,
                    Password = string.Empty,
                    Role = string.Empty,
                    Nationa_ID = string.Empty,
                    Appointments = new List<Appointment>(),
                    Prescriptions = new List<Prescription>()
                };
            }
        }
        public List<DoctorandNurse> GetAllData()
        {
            try
            {
                return db.DoctorandNurses.ToList();
            }
            catch { return new List<DoctorandNurse>(); }
        }
        public List<DoctorandNurse> Search(string searchItem)
        {
            try
            {
                return db.DoctorandNurses.Where(x => x.ID.ToString().Contains(searchItem) || x.Username.Contains(searchItem) ||
                x.Nationa_ID.Contains(searchItem) || x.Password.Contains(searchItem)).ToList();
            }
            catch { return new List<DoctorandNurse>(); }
        }
        public int Update(DoctorandNurse table)
        {
            try
            {
                db = new db_context();
                db.DoctorandNurses.Update(table);
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public List<DoctorandNurse> Add_list(DoctorandNurse table)
        {
            try
            {
                db.DoctorandNurses.Add(table);
                db.SaveChanges();
                return db.DoctorandNurses.ToList();
            }
            catch
            {
                return new List<DoctorandNurse>();
            }
        }
        }
    }

