using EsercitazioneFinale_Week4.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercitazioneFinale_Week4
{
    internal class ExpensesManagerAdo
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExpensesManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void InsertExpense()
        {
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();


                #region InsertExpenses
                #region Date
                int year, month, day;

                do
                {
                    do
                    {
                        Console.WriteLine("Inserisci l'anno in cui hai effettuato la spesa");
                    } while (!int.TryParse(Console.ReadLine(), out year));
                    do
                    {
                        Console.WriteLine("Inserisci il mese in cui hai effettuato la spesa");
                    } while (!int.TryParse(Console.ReadLine(), out month));
                    do
                    {
                        Console.WriteLine("Inserisci il giorno in cui hai effettuato la spesa");
                    } while (!int.TryParse(Console.ReadLine(), out day));
                } while (!(new DateTime(year, month, day) <= DateTime.Now));
                string date = $"{year}-{month}-{day}";

                #endregion
                #region CategoryID
                int idCat;
                do
                {
                    Console.WriteLine("Inserisci l'id della categoria a cui appartiene la spesa");
                } while (!int.TryParse(Console.ReadLine(), out idCat));

                string query = "select * from Categories";

                SqlCommand command = new SqlCommand(query, connection);


                SqlDataReader reader = command.ExecuteReader();
                bool check = false;
                while (reader.Read())
                {
                    var id = (int)reader["Id"];
                    if (id == idCat)
                        check = true;
                }
                connection.Close();

                connection.Open();
                if (!check)
                {
                    if (UtilityClass.Confirm("La categoria che hai scelto non esiste, aggiungerla?"))
                    {
                        Console.WriteLine("Inserisci il nome della categoria a cui appartiene la spesa");
                        var cat = Console.ReadLine();
                        string insertCatSQL = $"insert into Categories values (@nomeCategoria)";
                        SqlCommand insertCatCommand = connection.CreateCommand();
                        insertCatCommand.Parameters.AddWithValue("@nomeCategoria", cat);
                        insertCatCommand.CommandText = insertCatSQL;
                        insertCatCommand.ExecuteNonQuery();
                        connection.Close();
                        connection.Open();
                    }
                    else
                        idCat = 0;
                }
                #endregion
                #region Description
                string desc;
                do
                {
                    Console.WriteLine("Inserisci una descrizione della spesa");
                    desc = Console.ReadLine();
                } while (!UtilityClass.Confirm("Confermi?"));
                #endregion
                #region Username
                string usr;
                do
                {
                    Console.WriteLine("Inserisci il tuo username");
                    usr = Console.ReadLine();
                } while (!UtilityClass.Confirm("Confermi?"));
                #endregion
                #region Amount
                decimal amount;
                do
                {
                    Console.WriteLine("Inserisci l'importo totale");
                } while (!decimal.TryParse(Console.ReadLine(), out amount));
                #endregion
                string insertSQL = $"insert into Expenses values ('{date}', '{idCat}', '{desc}', '{usr}', '{amount}', 'false')";



                SqlCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = insertSQL;

                insertCommand.ExecuteNonQuery();
                #endregion


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        public static void ApproveExpense()
        {
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();



                List<string> expenses = ShowExpenses(false);
                foreach (string record in expenses)
                {
                    Console.WriteLine(record);
                }

                int inputId;
                bool check = false;
                do
                {
                    do
                    {
                        Console.WriteLine("\nInserisci l'ID della spesa che vuoi approvare");
                    } while (!int.TryParse(Console.ReadLine(), out inputId));

                    foreach (var record in expenses)
                    {
                        var line = record.Split('-');
                        if (int.TryParse(line[0], out int id) && id == inputId)
                            check = true;
                    }
                    if (!check)
                        Console.WriteLine("Effettua una scelta valida");
                } while (!check);

                string update = $"Update Expenses set approved = 'true' where id = {inputId}";
                SqlCommand commandUpdate = new SqlCommand(update, connection);
                commandUpdate.ExecuteNonQuery();

                Console.WriteLine(ShowExpenseById(inputId));

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

        }

        #region ShowExpenses
        public static List<string> ShowExpenses(bool approved)
        {
            List<string> expenses = new List<string>();
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();


                string query = $"select * from Expenses where approved = '{approved}'";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();

                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var date = (DateTime)reader["date"];
                    var categoryId = (int)reader["categoryId"];
                    var desc = (string)reader["description"];
                    var usr = (string)reader["username"];
                    var amount = (decimal)reader["amount"];
                    var appr = (bool)reader["approved"];
                    string approvedString = appr ? "Approvato" : "Non Approvato";

                    var expenseString = $"{id} - {date} - {categoryId} - {desc} - {usr} - {amount} - {approvedString}";
                    expenses.Add(expenseString);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return expenses;
        }
        public static List<string> ShowExpenses()
        {
            List<string> expenses = new List<string>();
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();


                string query = $"select * from Expenses";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();

                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var date = (DateTime)reader["date"];
                    var categoryId = (int)reader["categoryId"];
                    var desc = (string)reader["description"];
                    var usr = (string)reader["username"];
                    var amount = (decimal)reader["amount"];
                    var appr = (bool)reader["approved"];
                    string approvedString = appr ? "Approvato" : "Non Approvato";

                    var expenseString = $"{id} - {date} - {categoryId} - {desc} - {usr} - {amount} - {approvedString}";
                    expenses.Add(expenseString);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return expenses;
        }
        public static string ShowExpenseById(int cercaId)
        {
            string expenseString = "";
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();

                string query = $"select * from Expenses where id = {cercaId}";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();
                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var date = (DateTime)reader["date"];
                    var categoryId = (int)reader["categoryId"];
                    var desc = (string)reader["description"];
                    var usr = (string)reader["username"];
                    var amount = (decimal)reader["amount"];
                    var appr = (bool)reader["approved"];
                    string approvedString = appr ? "Approvato" : "Non Approvato";

                    expenseString = $"{id} - {date} - {categoryId} - {desc} - {usr} - {amount} - {approvedString}";
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return expenseString;
        }
        public static List<string> ShowExpensesByUser(string user)
        {
            List<string> expenses = new List<string>();
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();

                string query = $"select * from Expenses where username = '{user}'";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();
                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var date = (DateTime)reader["date"];
                    var categoryId = (int)reader["categoryId"];
                    var desc = (string)reader["description"];
                    var usr = (string)reader["username"];
                    var amount = (decimal)reader["amount"];
                    var appr = (bool)reader["approved"];
                    string approvedString = appr ? "Approvato" : "Non Approvato";

                    var expenseString = $"{id} - {date} - {categoryId} - {desc} - {usr} - {amount} - {approvedString}";
                    expenses.Add(expenseString);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return expenses;
        }
        public static List<string> ShowExpensesByCategory(int catId)
        {
            List<string> expenses = new List<string>();
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();

                string query = $"select * from Expenses where categoryId = {catId}";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();
                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var date = (DateTime)reader["date"];
                    var categoryId = (int)reader["categoryId"];
                    var desc = (string)reader["description"];
                    var usr = (string)reader["username"];
                    var amount = (decimal)reader["amount"];
                    var appr = (bool)reader["approved"];
                    string approvedString = appr ? "Approvato" : "Non Approvato";

                    var expenseString = $"{id} - {date} - {categoryId} - {desc} - {usr} - {amount} - {approvedString}";
                    expenses.Add(expenseString);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return expenses;
        }
        #endregion

        public static List<string> ShowCategories()
        {
            List<string> categories = new List<string>();
            using SqlConnection connection = new SqlConnection(connectionStringSQL);
            try
            {
                connection.Open();

                string query = $"select * from Categories";

                SqlCommand commandShowNotApproved = new SqlCommand(query, connection);
                SqlDataReader reader = commandShowNotApproved.ExecuteReader();

                while (reader.Read())
                {
                    var id = (int)reader["id"];
                    var category = (string)reader["category"];
                    

                    var expenseString = $"{id} - {category}";
                    categories.Add(expenseString);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return categories;
        }
        public static void DeleteExpenseById(int deleteId)
        {
            DataSet ExpManagerDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                foreach (var exp in ShowExpenses())
                {
                    Console.WriteLine(exp);
                }

                SqlDataAdapter adapter = InitializeAdapter(conn);
                adapter.Fill(ExpManagerDS, "Expenses");
                conn.Close();
                Console.WriteLine("connessione chiusa");


                DataRow deletingRow = ExpManagerDS.Tables["Expenses"].Rows.Find(deleteId);
                if (deletingRow != null)
                {
                    deletingRow.Delete();
                }

                adapter.Update(ExpManagerDS, "Expenses");


            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private static SqlDataAdapter InitializeAdapter(SqlConnection conn)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            //Fill
            adapter.SelectCommand = new SqlCommand("Select * from Expenses", conn);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //UPDATE
            adapter.UpdateCommand = GenerateUpdateCommand(conn);

            //DELETE
            adapter.DeleteCommand = GenerateDeleteCommand(conn);


            return adapter;
        }

        private static SqlCommand GenerateDeleteCommand(SqlConnection conn)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = "Delete from Expenses where id = @id";

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "id"));
            return command;
        }

        private static SqlCommand GenerateUpdateCommand(SqlConnection conn)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = "Update Expenses set date = @date, categoryId = @cat, description = @descr, username = @usr, amount = @amount, approved = @approved  where id = @id";

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "id"));
            command.Parameters.Add(new SqlParameter("@cat", SqlDbType.Int, 0, "categoryId"));
            command.Parameters.Add(new SqlParameter("@descr", SqlDbType.VarChar, 500, "description"));
            command.Parameters.Add(new SqlParameter("@usr", SqlDbType.VarChar, 100, "username"));
            command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Decimal, 0, "amount"));
            command.Parameters.Add(new SqlParameter("@approved", SqlDbType.Bit, 0, "approved"));

            return command;
        }
    }
}
