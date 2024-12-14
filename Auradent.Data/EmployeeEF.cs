using Auradent.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auradent.Data
{
    public class EmployeeEF : IdataHelper<Employee>
    {
        private db_context db;

        public EmployeeEF()
        {
            db = new db_context();
        }

        public async Task DeleteData(int id)
        {
            try
            {
                var data = await db.Employees.FindAsync(id); 
                if (data != null)
                {
                    db.Employees.Remove(data);
                    await db.SaveChangesAsync(); 
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error while deleting data.", ex);
            }
        }

        public async Task<List<Employee>> GetAllData()
        {
            try
            {
                return await db.Employees.ToListAsync(); 
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while retrieving all data.", ex);
            }
        }

        public async Task<List<Employee>> GetDataById(int id)
        {
            try
            {
                return await db.Employees.Where(x => x.ID == id).ToListAsync(); 
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while retrieving data by ID.", ex);
            }
        }

        public async Task InsertData(Employee data)
        {
            try
            {
                await db.Employees.AddAsync(data); 
                await db.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error while inserting data.", ex);
            }
        }

        public async Task<List<Employee>> SearchData(string search)
        {
            try
            {
                // Implement search 
                return await db.Employees.Where(x => x.Nationa_ID.Contains(search) 
                || x.Username.Contains(search) 
                || x.ID.ToString().Contains(search)
                || x.Password.Contains(search)).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while searching data.", ex);
            }
        }

        public async Task UpdateData(Employee data)
        {
            try
            {
                db.Employees.Update(data); // Use Update
                await db.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while updating data.", ex);
            }
        }

        public async Task<bool> DataExists(int id)
        {
            try
            {
                return await db.Employees.AnyAsync(x => x.ID == id); 
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while checking if data exists.", ex);
            }
        }

        public async Task<Employee> FindData(int id)
        {
            try
            {
                return await db.Employees.FindAsync(id); 
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("Error while finding data.", ex);
            }
        }
    }
}
