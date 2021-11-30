using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace Jobs.Models
{

    public class ClassificacaoTecnica
    {
        public int ID { get; set; }
        public string Coloracao { get; set; }
        public string Corte { get; set; }
        public string Setor { get; set; }
        public string Usuario { get; set; }
        public string Observacao { get; set; }
        public string DataLamina { get; set; }
        public string DataAvaliacao { get; set; }

        public List<ClassificacaoTecnica> List()
        {
            try
            {
                List<ClassificacaoTecnica> lista = new();

                using SqlConnection connection = SqlHelper.GetConnection();

                SqlCommand sql = new("SELECT * FROM TECNICA", connection);

                connection.Open();

                SqlDataReader reader = sql.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new ClassificacaoTecnica
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Coloracao = Convert.ToString(reader["Coloracao"]),
                        Corte = Convert.ToString(reader["Corte"]),
                        Setor = Convert.ToString(reader["Setor"]),
                        Usuario = Convert.ToString(reader["Usuario"]),
                        Observacao = Convert.ToString(reader["Observacao"]),
                        DataLamina = Convert.ToDateTime(reader["DataLamina"]).ToString("dd/MM/yyyy"),
                        DataAvaliacao = Convert.ToDateTime(reader["DataAvaliacao"]).ToString("dd/MM/yyyy")
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
