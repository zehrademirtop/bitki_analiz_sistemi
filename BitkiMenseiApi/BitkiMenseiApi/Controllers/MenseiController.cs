using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitkiMenseiApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace BitkiMenseiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenseiController : ControllerBase
    {
        private static readonly Dictionary<string, string> BitkiMenseiEslestirme = new()
        {
            { "Ankyropetalum arsusianum", "Hatay" },
            { "Ankyropetalum reuteri", "Maraş" },
            { "Ankyropetalum gypsophiloides", "Mardin" }
        };

        [HttpGet]
        public ActionResult<BitkiMensei> GetMensei([FromQuery] string bitki)
        {
            if (string.IsNullOrEmpty(bitki))
            {
                return BadRequest(new { Message = "Bitki adı belirtilmedi." });
            }

            var normalizedBitki = bitki.Trim();
            var mensei = BitkiMenseiEslestirme.ContainsKey(normalizedBitki)
                ? BitkiMenseiEslestirme[normalizedBitki]
                : null;

            if (mensei == null)
            {
                return NotFound(new { Message = $"Bitki '{bitki}' için menşei bulunamadı." });
            }

            return Ok(new BitkiMensei { Bitki = normalizedBitki, Mensei = mensei });
        }
    }
}
