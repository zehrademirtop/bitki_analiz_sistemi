using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite; 
using System;
using System.Threading.Tasks;


namespace BitkiMenseiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenseiController : ControllerBase
    {
        private readonly string connectionString = @"Data Source=C:\Users\HP\Desktop\bitkimensei.db";

        public MenseiController()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS BitkiMensei (
                        BitkiAdi TEXT PRIMARY KEY,
                        Mensei TEXT NOT NULL
                    );
                    INSERT OR IGNORE INTO BitkiMensei (BitkiAdi, Mensei) VALUES
                        ('Ankyropetalum arsusianum', 'Hatay'),
                        ('Ankyropetalum reuteri', 'Maraş'),
                        ('Ankyropetalum gypsophiloides', 'Mardin');
                ";
                using var command = new SqliteCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
            }
            catch (SqliteException sqlEx)
            {
                // Hata mesajını loglayabiliriz, ama burada sadece konsola yazalım
                System.Diagnostics.Debug.WriteLine($"Veritabanı başlatma hatası: {sqlEx.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMensei([FromQuery] string bitki)
        {
            if (string.IsNullOrEmpty(bitki))
            {
                return BadRequest("Bitki adı belirtilmelidir.");
            }

            try
            {
                using var connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();
                string query = "SELECT Mensei FROM BitkiMensei WHERE BitkiAdi = @BitkiAdi";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@BitkiAdi", bitki.Trim());
                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return Ok(new { Bitki = bitki, Mensei = "Bilinmiyor" });
                }
                return Ok(new { Bitki = bitki, Mensei = result.ToString() });
            }
            catch (SqliteException sqlEx)
            {
                return StatusCode(500, $"SQLite Hatası: {sqlEx.Message}, Hata Kodu: {sqlEx.SqliteErrorCode}, İç Hata: {sqlEx.InnerException?.Message}, StackTrace: {sqlEx.StackTrace}, Bitki: {bitki}, ConnectionString: {connectionString}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Genel Hata: {ex.Message}, İç Hata: {ex.InnerException?.Message}, StackTrace: {ex.StackTrace}, Bitki: {bitki}, ConnectionString: {connectionString}");
            }
        }
    }
}
