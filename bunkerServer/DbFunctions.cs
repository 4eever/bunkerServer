using bunkerServer.DTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bunkerServer
{
    public static class DbFunctions
    {
        public const string connectionString = "";

        public static void AddToUsers(string connectionString, string uid_User, string uid_Lobby, string user_Name, int avatar, string vote, int choice)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO users (Uid_user, Uid_lobby, User_name, Avatar, Vote, Choice) VALUES (@Uid_User, @Uid_Lobby, @User_Name, @Avatar, @Vote, @Choice)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", uid_User);
                    command.Parameters.AddWithValue("@Uid_Lobby", uid_Lobby);
                    command.Parameters.AddWithValue("@User_Name", user_Name);
                    command.Parameters.AddWithValue("@Avatar", avatar);
                    command.Parameters.AddWithValue("@Vote", vote);
                    command.Parameters.AddWithValue("@Choice", choice);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User data inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert data.");
                    }
                }
            }
        }

        public static void AddToLobbies(string connectionString, string uid_Lobby)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO lobbies (Uid_Lobby) VALUES (@Uid_Lobby)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_Lobby", uid_Lobby);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Lobby data inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert data.");
                    }
                }
            }
        }

        public static void AddToCards(string connectionString, string uid_User, int card1, int card2, int card3, int card4, int card5, int card6)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO cards (Uid_user, card1, card2, card3, card4, card5, card6) VALUES (@Uid_User, @Card1, @Card2, @Card3, @Card4, @Card5, @Card6)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", uid_User);
                    command.Parameters.AddWithValue("@Card1", card1);
                    command.Parameters.AddWithValue("@Card2", card2);
                    command.Parameters.AddWithValue("@Card3", card3);
                    command.Parameters.AddWithValue("@Card4", card4);
                    command.Parameters.AddWithValue("@Card5", card5);
                    command.Parameters.AddWithValue("@Card6", card6);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Cards data inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert data.");
                    }
                }
            }
        }

        public static void AddToIsOpen(string connectionString, string uid_User, bool card1, bool card2, bool card3, bool card4, bool card5, bool card6)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO IsOpen (Uid_user, card11, card22, card33, card44, card55, card66) VALUES (@Uid_User, @Card1, @Card2, @Card3, @Card4, @Card5, @Card6)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", uid_User);
                    command.Parameters.AddWithValue("@Card1", card1);
                    command.Parameters.AddWithValue("@Card2", card2);
                    command.Parameters.AddWithValue("@Card3", card3);
                    command.Parameters.AddWithValue("@Card4", card4);
                    command.Parameters.AddWithValue("@Card5", card5);
                    command.Parameters.AddWithValue("@Card6", card6);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data inserted into IsOpen table successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert data into IsOpen table.");
                    }
                }
            }
        }



        public static User GetUserByUidUser(string connectionString, string uid_User)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM users WHERE Uid_User = @Uid_User";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", uid_User);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Uid_user = (string)reader["Uid_User"],
                                Uid_lobby = (string)reader["Uid_Lobby"],
                                User_name = (string)reader["User_Name"],
                                Avatar = (int)reader["Avatar"],
                                Vote = (string)reader["Vote"],
                                Choice = (int)reader["Choice"],
                                Card1 = 0,
                                Card2 = 0,
                                Card3 = 0,
                                Card4 = 0,
                                Card5 = 0,
                                Card6 = 0,
                            };
                        }
                        else
                        {
                            return null; // Пользователь с указанным uid_User не найден.
                        }
                    }
                }
            }
        }

        public static async Task UpdateIsOpen(IsOpenDTO isOpenDTO)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            UPDATE IsOpen
            SET card1 = @Card1,
                card2 = @Card2,
                card3 = @Card3,
                card4 = @Card4,
                card5 = @Card5,
                card6 = @Card6
            WHERE uid_user = @Uid_User";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", isOpenDTO.Uid_User);
                    command.Parameters.AddWithValue("@Card1", isOpenDTO.Card11);
                    command.Parameters.AddWithValue("@Card2", isOpenDTO.Card22);
                    command.Parameters.AddWithValue("@Card3", isOpenDTO.Cars33);
                    command.Parameters.AddWithValue("@Card4", isOpenDTO.Cars44);
                    command.Parameters.AddWithValue("@Card5", isOpenDTO.Cars55);
                    command.Parameters.AddWithValue("@Card6", isOpenDTO.Cars66);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data updated in is_open table successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update data in is_open table.");
                    }
                }
            }
        }

    }
}
