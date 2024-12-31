using Auradent.core;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Auradent.Data
{
    public class LabTestDataHelper : IdataHelper<LabTest>
    {
        private readonly string _connectionString;

        public LabTestDataHelper()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        public void Add(LabTest item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO LabTests (PatientID, ImagePath, OcrText, UploadDate, TestType) VALUES (@PatientID, @ImagePath, @OcrText, @UploadDate, @TestType)", connection))
                {
                    command.Parameters.AddWithValue("@PatientID", item.PatientID);
                    command.Parameters.AddWithValue("@ImagePath", item.ImagePath);
                    command.Parameters.AddWithValue("@OcrText", item.OcrText);
                    command.Parameters.AddWithValue("@UploadDate", item.UploadDate);
                    command.Parameters.AddWithValue("@TestType", item.TestType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM LabTests WHERE LabTestID = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<LabTest> GetAllData()
        {
            var labTests = new List<LabTest>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM LabTests", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labTests.Add(new LabTest
                            {
                                LabTestID = reader.GetInt32("LabTestID"),
                                PatientID = reader.GetInt32("PatientID"),
                                ImagePath = reader.GetString("ImagePath"),
                                OcrText = reader.GetString("OcrText"),
                                UploadDate = reader.GetDateTime("UploadDate"),
                                TestType = reader.GetString("TestType")
                            });
                        }
                    }
                }
            }
            return labTests;
        }

        public LabTest GetDatabyid(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM LabTests WHERE LabTestID = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LabTest
                            {
                                LabTestID = reader.GetInt32("LabTestID"),
                                PatientID = reader.GetInt32("PatientID"),
                                ImagePath = reader.GetString("ImagePath"),
                                OcrText = reader.GetString("OcrText"),
                                UploadDate = reader.GetDateTime("UploadDate"),
                                TestType = reader.GetString("TestType")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Update(LabTest item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE LabTests SET PatientID = @PatientID, ImagePath = @ImagePath, OcrText = @OcrText, UploadDate = @UploadDate, TestType = @TestType WHERE LabTestID = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", item.LabTestID);
                    command.Parameters.AddWithValue("@PatientID", item.PatientID);
                    command.Parameters.AddWithValue("@ImagePath", item.ImagePath);
                    command.Parameters.AddWithValue("@OcrText", item.OcrText);
                    command.Parameters.AddWithValue("@UploadDate", item.UploadDate);
                    command.Parameters.AddWithValue("@TestType", item.TestType);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}