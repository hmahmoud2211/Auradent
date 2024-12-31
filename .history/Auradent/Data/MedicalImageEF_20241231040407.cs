using Auradent.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auradent.Data
{
    public class MedicalImageEF : IdataHelper<MedicalImage>
    {
        private readonly db_context _context;

        public MedicalImageEF(db_context context)
        {
            _context = context;
        }

        public void Add_list(MedicalImage item)
        {
            _context.MedicalImages.Add(item);
            _context.SaveChanges();
        }

        public void Delete_list(int id)
        {
            var image = _context.MedicalImages.Find(id);
            if (image != null)
            {
                _context.MedicalImages.Remove(image);
                _context.SaveChanges();
            }
        }

        public List<MedicalImage> GetAllData()
        {
            return _context.MedicalImages.ToList();
        }

        public MedicalImage GetById(int id)
        {
            return _context.MedicalImages.Find(id);
        }

        public void Update_list(MedicalImage item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
} 