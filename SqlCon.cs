using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace JaneczkoSklep
{
    public class SqlCon
    {
        SQLiteConnection m_dbConnection;
        int id = 0;

        public void IfDatabaseExist(string baza)
        {
            if(File.Exists(baza))
            {
                ConnectToDatabase(baza);
            }
            else
            {
                SQLiteConnection.CreateFile(baza);
                ConnectToDatabase(baza);
                CreateTable();
            }
        }

        public void ConnectToDatabase(string baza)
        {
            m_dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", baza));
            m_dbConnection.Open();
        }

        public void CreateTable()
        {
            string sql = "create table produkty (Id int, Nazwa varchar(90), Ilosc varchar(90), Cena varchar(90), Image varchar(4000))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void CheckLast()
        {                    
            string selectMaxId = "Select Max(Id) From produkty";
            SQLiteCommand selectMaxCmd = new SQLiteCommand(selectMaxId, m_dbConnection);
            object val = selectMaxCmd.ExecuteScalar();
            int.TryParse(val.ToString(), out id);
        }

        public void AddToSql(string _nazwa, string _ilosc, string _cena, string _convertedImage)
        {
            CheckLast();
            id++;
            string sql = string.Format("INSERT INTO produkty(Id, Nazwa, Ilosc, Cena, Image) VALUES('{0}','{1}','{2}','{3}','{4}')", id, _nazwa, _ilosc, _cena, _convertedImage);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void CloseConnection()
        {
            m_dbConnection.Close();
        }

        public List<Produkt> getFromTable(string baza)
        {
            List<Produkt> produkty = new List<Produkt>();
            ImageDecoder imageDecoder = new ImageDecoder();
            ConnectToDatabase(baza);
            string sql = "SELECT * FROM produkty";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Produkt element = new Produkt
                    {
                        Name = (string)reader["Nazwa"],
                        Price = (string)reader["Cena"],
                        Quantity = (string)reader["Ilosc"],
                        BitmapSource = imageDecoder.ImageRead((string)reader["Image"])
                    };

                    produkty.Add(element);
                }
            }

            return produkty;
        }
    }
}