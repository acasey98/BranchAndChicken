using System;
using System.Collections.Generic;
using System.Linq;
using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BranchAndChicken.Api.DataAccess
{
    public class TrainerRepository
    {
        //static List<Trainer> _trainers = new List<Trainer>
        //{
        //    new Trainer
        //    {
        //        Name = "Nathan",
        //        Specialty = Specialty.TaeCluckDoe,
        //        YearsOfExperience = 0
        //    },
        //    new Trainer
        //    {
        //        Name = "Martin",
        //        Specialty = Specialty.Chudo,
        //        YearsOfExperience = 12
        //    },
        //    new Trainer
        //    {
        //        Name = "Adam",
        //        Specialty = Specialty.ChravBacaw,
        //        YearsOfExperience = 3
        //    }
        //};



        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True;";

        public List<Trainer> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"Select * 
                                From Trainer";

                var dataReader = cmd.ExecuteReader();

                var trainers = new List<Trainer>();

                while (dataReader.Read())
                {
                    //explicit cast
                    var id = (int)dataReader["Id"];
                    //implicit cast
                    var returnedName = dataReader["name"] as string;
                    //convert to
                    var yearsOfExperience = Convert.ToInt32(dataReader["YearsOfExperience"]);
                    //try parse
                    Enum.TryParse<Specialty>(dataReader["specialty"].ToString(), out var specialty);

                    var trainer = new Trainer
                    {
                        Specialty = specialty,
                        Id = id,
                        Name = returnedName,
                        YearsOfExperience = yearsOfExperience
                    };
                    trainers.Add(trainer);
                }
                return trainers;
            }
        }

        public Trainer Get(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $@"select *
                                    from Trainer
                                    where Trainer.Name = @trainerName";

                cmd.Parameters.AddWithValue("trainerName", name);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //explicit cast
                    var id = (int)reader["Id"];
                    //implicit cast
                    var returnedName = reader["name"] as string;
                    //convert to
                    var yearsOfExperience = Convert.ToInt32(reader["YearsOfExperience"]);
                    //try parse
                    Enum.TryParse<Specialty>(reader["specialty"].ToString(), out var specialty);

                    var trainer = new Trainer
                    {
                        Specialty = specialty,
                        Id = id,
                        Name = returnedName,
                        YearsOfExperience = yearsOfExperience
                    };
                    return trainer;
                }
                return null;
            }
        }

        public bool Remove(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"delete
                                    from Trainer
                                    where Trainer.Name = @name";
                cmd.Parameters.AddWithValue("name", name);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public ActionResult<Trainer> GetSpecialty(string specialty)
        {
            throw new NotImplementedException();
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE [Trainer]
                                    output inserted.*
                                     SET [Name] = @name
                                        ,[YearsOfExperience] = @yearsOfExperience
                                        ,[Specialty] = @specialty
                                   WHERE id = @id";

                cmd.Parameters.AddWithValue("name", updatedTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", updatedTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("Specialty", updatedTrainer.Specialty);
                cmd.Parameters.AddWithValue("id", id);

                var reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    return GetTrainerFromDataReader(reader);
                }
            }
            return null;
        }

        public Trainer Add(Trainer newTrainer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO [Trainer]
                                   ([Name]
                                   ,[YearsOfExperience]
                                   ,[Specialty])
                                   VALUES
                                   (@name
                                   ,@yearsOfExperience
                                   ,@specialty)";
                cmd.Parameters.AddWithValue("name", newTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", newTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("Specialty", newTrainer.Specialty);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetTrainerFromDataReader(reader);
                }
            }
                return null;
        }
        
        Trainer GetTrainerFromDataReader(SqlDataReader reader)
            {
             //explicit cast
                    var id = (int)reader["Id"];
        //implicit cast
        var returnedName = reader["name"] as string;
        //convert to
        var yearsOfExperience = Convert.ToInt32(reader["YearsOfExperience"]);
        //try parse
        Enum.TryParse<Specialty>(reader["specialty"].ToString(), out var specialty);

        var trainer = new Trainer
        {
            Specialty = specialty,
            Id = id,
            Name = returnedName,
            YearsOfExperience = yearsOfExperience
        };
                    return trainer;
}
    }
}
