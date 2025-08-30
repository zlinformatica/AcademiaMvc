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
    public class AlunosController : Controller
    {
        private readonly AcademiaDbContext _context;

        public AlunosController(AcademiaDbContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public async Task<IActionResult> Index()
        {
            var alunos = _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria);
            return View(await alunos.ToListAsync());
        }

        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos.Where(e => e.Quantidade > 0), "Id", "Nome");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Alunos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Idade,Email,Cpf,EquipamentoId,CategoriaId")] Aluno aluno)
        {
            // Verifica duplicidade CPF
            if (_context.Alunos.Any(a => a.Cpf == aluno.Cpf))
            {
                ModelState.AddModelError("Cpf", "Já existe um aluno com este CPF.");
            }

            var equipamento = await _context.Equipamentos.FindAsync(aluno.EquipamentoId);
            if (equipamento == null || equipamento.Quantidade <= 0)
            {
                ModelState.AddModelError("EquipamentoId", "Este equipamento não está disponível.");
            }

            if (ModelState.IsValid)
            {
                equipamento!.Quantidade -= 1;
                _context.Update(equipamento);

                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recarrega combos em caso de erro
            ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos.Where(e => e.Quantidade > 0), "Id", "Nome", aluno.EquipamentoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", aluno.CategoriaId);

            return View(aluno);
        }

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos, "Id", "Nome", aluno.EquipamentoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", aluno.CategoriaId);

            return View(aluno);
        }

        // POST: Alunos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Idade,Email,Cpf,EquipamentoId,CategoriaId")] Aluno aluno)
        {
            if (id != aluno.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos, "Id", "Nome", aluno.EquipamentoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", aluno.CategoriaId);

            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.Include(a => a.Equipamento).FirstOrDefaultAsync(a => a.Id == id);
            if (aluno != null)
            {
                if (aluno.Equipamento != null)
                {
                    aluno.Equipamento.Quantidade += 1; // devolve equipamento
                    _context.Update(aluno.Equipamento);
                }

                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }
    }
}
