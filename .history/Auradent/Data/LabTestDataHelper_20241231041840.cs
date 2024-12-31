using Auradent.core;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Auradent.Data
{
    public class LabTestDataHelper : IdataHelper<LabTest>
    {
        private readonly db_context _context;

        public LabTestDataHelper(db_context context)
        {
            _context = context;
        }

        public int Add(LabTest item)
        {
            var radiologyTest = new RadiologyORtest
            {
                MedicalRecordID = item.PatientID,
                Report = item.OcrText,
                Labtests = File.ReadAllBytes(item.ImagePath)
            };
            _context.RadiologyORtests.Add(radiologyTest);
            _context.SaveChanges();
            return radiologyTest.ID;
        }

        public void Add_list(LabTest item)
        {
            Add(item);
        }

        public bool CheckIfIdExists(int id)
        {
            return _context.RadiologyORtests.Any(r => r.ID == id);
        }

        public void Delete(int id)
        {
            var test = _context.RadiologyORtests.Find(id);
            if (test != null)
            {
                _context.RadiologyORtests.Remove(test);
                _context.SaveChanges();
            }
        }

        public void DeleteLabTest(int id)
        {
            Delete(id);
        }

        public LabTest Find(LabTest item)
        {
            var test = _context.RadiologyORtests.Find(item.LabTestID);
            if (test != null)
            {
                return new LabTest
                {
                    LabTestID = test.ID,
                    PatientID = test.MedicalRecordID,
                    OcrText = test.Report,
                    ImagePath = null, // Image is stored as bytes in database
                    UploadDate = DateTime.Now,
                    TestType = "Lab"
                };
            }
            return null;
        }

        public List<LabTest> GetAllData()
        {
            return _context.RadiologyORtests
                .Select(r => new LabTest
                {
                    LabTestID = r.ID,
                    PatientID = r.MedicalRecordID,
                    OcrText = r.Report,
                    ImagePath = null, // Image is stored as bytes in database
                    UploadDate = DateTime.Now,
                    TestType = "Lab"
                })
                .ToList();
        }

        public LabTest GetDatabyid(int id)
        {
            var test = _context.RadiologyORtests.Find(id);
            if (test != null)
            {
                return new LabTest
                {
                    LabTestID = test.ID,
                    PatientID = test.MedicalRecordID,
                    OcrText = test.Report,
                    ImagePath = null, // Image is stored as bytes in database
                    UploadDate = DateTime.Now,
                    TestType = "Lab"
                };
            }
            return null;
        }

        public string Search(string searchText)
        {
            var test = _context.RadiologyORtests
                .FirstOrDefault(r => r.Report.Contains(searchText));
            return test?.Report;
        }

        public int Update(LabTest item)
        {
            var test = _context.RadiologyORtests.Find(item.LabTestID);
            if (test != null)
            {
                test.MedicalRecordID = item.PatientID;
                test.Report = item.OcrText;
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    test.Labtests = File.ReadAllBytes(item.ImagePath);
                }
                _context.SaveChanges();
                return test.ID;
            }
            return 0;
        }
    }
}