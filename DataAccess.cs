using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using OwinSelfHosted.Model;

namespace OwinSelfHosted
{
    public class DataAccess
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Return all records from database Todo table
        /// </summary>
        /// <returns></returns>
        public List<Todo> GetAll()
        {
            List<Todo> list = new List<Todo>();

            string getTaskListQuery = "Select * from Todo";

            using (var conn = new SqlConnection(Helper.GetConnection("TodoConnection")))
            {
                var command = new SqlCommand(getTaskListQuery, conn);
                conn.Open();

                log.Info("Oppening connection");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var todo = new Todo
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            IsDone = (bool)reader["IsDone"],
                            Created = (DateTime)reader["Created"]
                        };

                        list.Add(todo);
                    }
                }
            }
            log.Info("Connection closed");

            return list;
        }

        /// <summary>
        /// Get Todo item by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Todo Get(int Id)
        {
            var todo = new Todo();

            string query = "Select * FROM Todo WHERE Id = @Id";

            using (var conn = new SqlConnection(Helper.GetConnection("TodoConnection")))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        todo = new Todo()
                        {
                            Id = (int) reader["Id"],
                            Name = (string) reader["Name"],
                            IsDone = (bool) reader["IsDone"],
                            Created = (DateTime) reader["Created"]
                        };

                        log.Info($"Get by Id successfully [{todo}]");
                    }
                }
                catch (Exception e)
                {
                    log.Error("Problem with getting item by Id", e);
                    throw;
                }
            }

            return todo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Insert(Todo item)
        {
            string query = "INSERT INTO Todo(Name, IsDone, Created) VALUES(@Name, @IsDone, @Created)";

            using (var conn = new SqlConnection(Helper.GetConnection("TodoConnection")))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@IsDone", item.IsDone);
                    cmd.Parameters.AddWithValue("@Created", item.Created);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            string query = "Delete FROM Todo WHERE Id = @Id";

            using (var conn = new SqlConnection(Helper.GetConnection("TodoConnection")))
            {
                log.Info($"Deleting Todo item by Id = {Id}");
                try
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();

                    if (count >= 1)
                    {
                        log.Info($"Deleted Todo item by Id = {Id} successfully");
                        return true;
                    }
                    else
                    {
                        log.Info($"Nothing to delete");
                        return false;
                    }

                }
                catch (Exception e)
                {
                    log.Error($"Problem with deleting item by Id = {Id}", e);
                    return false;
                }
                
            }
            return false;
        }
    }
}
