#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using teste_WebMotors.Models;
using teste_WebMotors.Models.Context;

namespace teste_WebMotors.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly MyContext _context;

        public AnunciosController(MyContext context)
        {
            _context = context;
        }

        // GET: Anuncios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Anuncios.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string txtAnuncio)
        {
            if (!String.IsNullOrEmpty(txtAnuncio))
            {
                return View(await _context.Anuncios.Where(c => c.Modelo.ToUpper().Contains(txtAnuncio.ToUpper())).ToListAsync());
            }

            return View(await _context.Anuncios.ToListAsync());
        }

        // GET: Anuncios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anunciosDTO = await _context.Anuncios.FindAsync(id);

            if (anunciosDTO == null)
            {
                return NotFound();
            }
            return View(anunciosDTO);
        }

        // GET: Anuncios/Create
        public async Task<IActionResult> Create()
        {

            return View();
        }

        // POST: Anuncios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Versao,Ano,Quilometragem,Observacao")] AnunciosDTO anunciosDTO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anunciosDTO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anunciosDTO);
        }

        // GET: Anuncios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anunciosDTO = await MapAPIModel(await _context.Anuncios.FindAsync(id));

            if (anunciosDTO == null)
            {
                return NotFound();
            }
            return View(anunciosDTO);
        }

        // POST: Anuncios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,Versao,Ano,Quilometragem,Observacao")] AnunciosDTO anunciosDTO)
        {
            if (id != anunciosDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anunciosDTO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnunciosDTOExists(anunciosDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(anunciosDTO);
        }

        // GET: Anuncios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anunciosDTO = await _context.Anuncios.FindAsync(id);
            if (anunciosDTO == null)
            {
                return NotFound();
            }
            return View(anunciosDTO);
        }

        // POST: Anuncios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anunciosDTO = await _context.Anuncios.FindAsync(id);
            _context.Anuncios.Remove(anunciosDTO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnunciosDTOExists(int id)
        {
            return _context.Anuncios.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<string> GetMakes()
        {
            string apiMethod = "Make";

            return await GetAPIData(apiMethod);
        }

        [HttpGet]
        public async Task<string> GetModelByMakeID(int makeID)
        {
            string apiMethod = $"Model?MakeID={makeID}";

            return await GetAPIData(apiMethod);
        }

        [HttpGet]
        public async Task<string> GetVersionByModelID(int modelID)
        {
            string apiMethod = $"Version?ModelID={modelID}";

            return await GetAPIData(apiMethod);
        }

        private async Task<AnunciosDTO> MapAPIModel(AnunciosDTO anuncio)
        {
            IEnumerable<MarcaDTO> objMarca = new Stack<MarcaDTO>();
            IEnumerable<ModeloDTO> objModelo = new Stack<ModeloDTO>();
            IEnumerable<VersaoDTO> objVersao = new Stack<VersaoDTO>();

            if (anuncio != null)
            {
                objMarca = JsonConvert.DeserializeObject<IEnumerable<MarcaDTO>>(await GetMakes());
                foreach (var item in objMarca)
                {
                    if (item.Name == anuncio.Marca)
                    {
                        anuncio.MakeID = item.ID;
                        break;
                    }
                }
                if (anuncio.MakeID > 0)
                {
                    objModelo = JsonConvert.DeserializeObject<IEnumerable<ModeloDTO>>(await GetModelByMakeID(anuncio.MakeID));
                    foreach (var item in objModelo)
                    {
                        if (item.Name == anuncio.Modelo)
                        {
                            anuncio.ModelID = item.ID;
                            break;
                        }
                    }
                    if (anuncio.ModelID > 0)
                    {
                        objVersao = JsonConvert.DeserializeObject<IEnumerable<VersaoDTO>>(await GetVersionByModelID(anuncio.ModelID));
                        foreach (var item in objVersao)
                        {
                            if (item.Name == anuncio.Versao)
                            {
                                anuncio.VersionID = item.ID;
                                break;
                            }
                        }
                    }
                }
            }

            return anuncio;
        }

        private async Task<string> GetAPIData(string apiMethod)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://desafioonline.webmotors.com.br/api/OnlineChallenge/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(apiMethod);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
