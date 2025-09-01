using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcademiaMvc.Models;

namespace AcademiaMvc.Controllers
{
    public class ProfessoresController : Controller
    {
        private readonly AcademiaDbContext _context;

        public ProfessoresController(AcademiaDbContext context)
        {
            _context = context;
        }

        // GET: Professores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Professores.ToListAsync());
        }

        // GET: Professores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // Lista interna de especialidades
        private readonly List<string> _especialidades = new List<string>
        {
             "Musculação",
             "Natação",
             "Yoga",
             "Pilates",
             "Crossfit"
        };

        // GET: Professores/Create
        public IActionResult Create()
        {
            ViewBag.Especialidades = _especialidades;
            return View();
        }

        // POST: Professores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Especialidade")] Professor professor)
        {
            // Verifica se já existe um professor com a mesma especialidade
            bool existe = await _context.Professores
                .AnyAsync(p => p.Especialidade == professor.Especialidade);

            if (existe)
            {
                ModelState.AddModelError("Especialidade", "Já existe um professor cadastrado com esta especialidade.");
                ViewBag.Especialidades = _especialidades;
                return View(professor);
            }

            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Especialidades = _especialidades;
            return View(professor);
        }
        // GET: Professores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professores.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            // Passa a lista de especialidades para o combo-box
            ViewBag.Especialidades = _especialidades;
            return View(professor);
        }
        // POST: Professores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Especialidade")] Professor professor)
        {
            if (id != professor.Id)
            {
                return NotFound();
            }

            // Valida se já existe outro professor com a mesma especialidade
            bool existe = await _context.Professores
                .AnyAsync(p => p.Especialidade == professor.Especialidade && p.Id != professor.Id);

            if (existe)
            {
                ModelState.AddModelError("Especialidade", "Já existe outro professor cadastrado com esta especialidade.");
                ViewBag.Especialidades = _especialidades;
                return View(professor);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Professores.Any(e => e.Id == professor.Id))
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
            ViewBag.Especialidades = _especialidades;
            return View(professor);
        }

        // GET: Professores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professor = await _context.Professores.FindAsync(id);
            if (professor != null)
            {
                _context.Professores.Remove(professor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professores.Any(e => e.Id == id);
        }
    }
}
